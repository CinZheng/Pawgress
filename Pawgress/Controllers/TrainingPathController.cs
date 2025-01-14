using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Dtos;
using Pawgress.Models;
using Pawgress.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingPathController : ControllerBase
    {
        private readonly TrainingPathService _service;
        private readonly ApplicationDbContext _context;

        public TrainingPathController(TrainingPathService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var trainingPaths = _service.GetAll();
            var dtos = trainingPaths.Select(ToTrainingPathDto).ToList();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var trainingPath = _service.GetById(id);
            if (trainingPath == null) return NotFound("Niet gevonden.");
            return Ok(ToTrainingPathDto(trainingPath));
        }

        [HttpPost]
        public IActionResult Create([FromBody] TrainingPathDto trainingPathDto)
        {
            var trainingPath = new TrainingPath
            {
                TrainingPathId = Guid.NewGuid(),
                Name = trainingPathDto.Name,
                Description = trainingPathDto.Description,
                TrainingPathItems = trainingPathDto.LessonsQuizzes.Select((item, index) => new TrainingPathItemOrder
                {
                    TrainingPathItemId = item.Id,
                    Order = index + 1
                }).ToList(),
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            var created = _service.Create(trainingPath);
            return Ok(ToTrainingPathDto(created));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] TrainingPathDto trainingPathDto)
        {
            var trainingPath = _service.GetById(id);
            if (trainingPath == null) return NotFound("Niet gevonden.");

            trainingPath.Name = trainingPathDto.Name;
            trainingPath.Description = trainingPathDto.Description;
            trainingPath.TrainingPathItems = trainingPathDto.LessonsQuizzes.Select((item, index) => new TrainingPathItemOrder
            {
                TrainingPathItemId = item.Id,
                Order = index + 1
            }).ToList();
            trainingPath.UpdateDate = DateTime.Now;

            _service.Update(id, trainingPath);
            return Ok(ToTrainingPathDto(trainingPath));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return _service.Delete(id) ? Ok("Succesvol verwijderd.") : NotFound("Niet gevonden.");
        }

        // Helper to map a TrainingPath entity to a TrainingPathDto
        private static TrainingPathDto ToTrainingPathDto(TrainingPath tp)
        {
            return new TrainingPathDto
            {
                TrainingPathId = tp.TrainingPathId,
                Name = tp.Name,
                Description = tp.Description,
                LessonsQuizzes = tp.TrainingPathItems
                    .OrderBy(tpi => tpi.Order)
                    .Select(tpi => tpi.TrainingPathItem is Lesson
                        ? new LessonDto
                        {
                            Id = ((Lesson)tpi.TrainingPathItem).Id,
                            Name = ((Lesson)tpi.TrainingPathItem).Name,
                            CreationDate = ((Lesson)tpi.TrainingPathItem).CreationDate,
                            UpdateDate = ((Lesson)tpi.TrainingPathItem).UpdateDate,
                        } as TrainingPathItemDto
                        : new QuizDto
                        {
                            Id = ((Quiz)tpi.TrainingPathItem).Id,
                            CreationDate = ((Quiz)tpi.TrainingPathItem).CreationDate,
                            UpdateDate = ((Quiz)tpi.TrainingPathItem).UpdateDate,
                        }).ToList(),
                Users = tp.Users?.Select(u => new UserTrainingPathDto
                {
                    UserId = u.UserId,
                    TrainingPathId = u.TrainingPathId,
                    Progress = u.Progress,
                    Status = u.Status,
                    StartDate = u.StartDate,
                    CompletionDate = u.CompletionDate
                }).ToList(),
                CreationDate = tp.CreationDate,
                UpdateDate = tp.UpdateDate
            };
        }

        // Helper to map a TrainingPathItemDto to a TrainingPathItem entity
        private static TrainingPathItem ToTrainingPathItem(TrainingPathItemDto dto)
        {
            return dto switch
            {
                LessonDto lessonDto => new Lesson
                {
                    Id = lessonDto.Id,
                    Name = lessonDto.Name,
                    CreationDate = lessonDto.CreationDate,
                    UpdateDate = lessonDto.UpdateDate,
                },
                QuizDto quizDto => new Quiz
                {
                    Id = quizDto.Id,
                    CreationDate = quizDto.CreationDate,
                    UpdateDate = quizDto.UpdateDate,
                },
                _ => throw new InvalidOperationException("Unsupported TrainingPathItemDto type")
            };
        }

        [HttpPut("{trainingPathId}/progress/{userId}")]
        public async Task<IActionResult> UpdateProgress(Guid trainingPathId, Guid userId, [FromBody] int completedItems)
        {
            var userTrainingPath = await _context.UserTrainingPaths
                .FirstOrDefaultAsync(utp => utp.TrainingPathId == trainingPathId && utp.UserId == userId);

            if (userTrainingPath == null)
                return NotFound("User or Training Path not found.");

            var totalItems = await _context.TrainingPathItemOrders
                .CountAsync(tpio => tpio.TrainingPathId == trainingPathId);

            userTrainingPath.Progress = $"{completedItems}/{totalItems}";

            if (completedItems == totalItems)
            {
                userTrainingPath.Status = "Completed";
                userTrainingPath.CompletionDate = DateTime.UtcNow;
            }
            else
            {
                userTrainingPath.Status = "In Progress";
            }

            userTrainingPath.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(userTrainingPath);
        }

        [HttpGet("{trainingPathId}/progress/{userId}")]
        public async Task<ActionResult<ProgressDto>> GetProgress(Guid trainingPathId, Guid userId)
        {
            try
            {
                // Get the training path
                var trainingPath = await _context.TrainingPaths
                    .Include(tp => tp.TrainingPathItems)
                    .FirstOrDefaultAsync(tp => tp.TrainingPathId == trainingPathId);

                if (trainingPath == null)
                    return NotFound("Training path not found");

                // Get all progress records for this user and training path
                var progressRecords = await _context.UserProgress
                    .Where(up => up.TrainingPathId == trainingPathId && up.UserId == userId)
                    .ToListAsync();

                // Get all items in the training path
                var orderedItems = await _context.TrainingPathItemOrders
                    .Where(tpio => tpio.TrainingPathId == trainingPathId)
                    .OrderBy(tpio => tpio.Order)
                    .Include(tpio => tpio.TrainingPathItem)
                    .ToListAsync();

                // Load the items into memory first
                var itemIds = orderedItems.Select(x => x.TrainingPathItemId).ToList();
                
                // Then load the specific types
                var lessons = await _context.Set<Lesson>()
                    .Where(l => itemIds.Contains(l.Id))
                    .ToDictionaryAsync(l => l.Id, l => l);
                    
                var quizzes = await _context.Set<Quiz>()
                    .Where(q => itemIds.Contains(q.Id))
                    .ToDictionaryAsync(q => q.Id, q => q);

                var itemProgress = new List<ItemProgressDto>();
                foreach (var item in orderedItems)
                {
                    var progress = progressRecords.FirstOrDefault(p => p.ItemId == item.TrainingPathItemId);
                    string itemName;
                    string itemType;

                    if (lessons.TryGetValue(item.TrainingPathItemId, out var lesson))
                    {
                        itemName = lesson.Name;
                        itemType = "Lesson";
                    }
                    else if (quizzes.TryGetValue(item.TrainingPathItemId, out var quiz))
                    {
                        itemName = quiz.Name;
                        itemType = "Quiz";
                    }
                    else
                    {
                        continue; // Skip if item type is not recognized
                    }

                    itemProgress.Add(new ItemProgressDto
                    {
                        ItemId = item.TrainingPathItemId,
                        ItemType = itemType,
                        Name = itemName,
                        IsCompleted = progress?.IsCompleted ?? false,
                        CompletedDate = progress?.CompletedDate,
                        Score = progress?.Score
                    });
                }

                var completedCount = progressRecords.Count(p => p.IsCompleted);
                var totalItems = orderedItems.Count;
                var percentageComplete = totalItems > 0 
                    ? (double)completedCount / totalItems * 100 
                    : 0;

                var response = new ProgressDto
                {
                    ModuleName = trainingPath.Name,
                    CompletedItems = completedCount,
                    TotalItems = totalItems,
                    Status = completedCount == totalItems ? "Completed" : 
                             completedCount == 0 ? "Not Started" : "In Progress",
                    StartDate = progressRecords.Any() ? progressRecords.Min(p => p.CreatedAt) : null,
                    CompletionDate = completedCount == totalItems ? 
                        progressRecords.Max(p => p.CompletedDate) : null,
                    ItemProgress = itemProgress,
                    PercentageComplete = Math.Round(percentageComplete, 2)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("items/{itemId}/complete/{userId}")]
        public async Task<IActionResult> CompleteItem(Guid itemId, Guid userId, [FromBody] CompleteItemRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (itemId == Guid.Empty)
                {
                    return BadRequest("Invalid itemId");
                }

                if (userId == Guid.Empty)
                {
                    return BadRequest("Invalid userId");
                }

                if (request?.TrainingPathId == Guid.Empty)
                {
                    return BadRequest("Invalid trainingPathId in request");
                }

                Console.WriteLine($"CompleteItem called with:");
                Console.WriteLine($"- ItemId: {itemId}");
                Console.WriteLine($"- UserId: {userId}");
                Console.WriteLine($"- TrainingPathId: {request?.TrainingPathId}");

                // First verify the item exists and get its type
                var trainingPathItem = await _context.TrainingPathItems
                    .FirstOrDefaultAsync(tpi => tpi.Id == itemId);

                if (trainingPathItem == null)
                {
                    Console.WriteLine($"Training path item {itemId} not found");
                    return NotFound($"Training path item {itemId} not found");
                }

                Console.WriteLine($"Found training path item of type: {trainingPathItem.GetType().Name}");

                // Verify the training path exists
                var trainingPath = await _context.TrainingPaths
                    .FirstOrDefaultAsync(tp => tp.TrainingPathId == request.TrainingPathId);

                if (trainingPath == null)
                {
                    Console.WriteLine($"Training path {request.TrainingPathId} not found");
                    return NotFound($"Training path {request.TrainingPathId} not found");
                }

                // Check if progress already exists
                var progress = await _context.UserProgress
                    .FirstOrDefaultAsync(up => 
                        up.TrainingPathId == request.TrainingPathId && 
                        up.ItemId == itemId && 
                        up.UserId == userId);

                if (progress == null)
                {
                    // Create new progress entry
                    progress = new UserProgress
                    {
                        UserProgressId = Guid.NewGuid(),
                        UserId = userId,
                        TrainingPathId = request.TrainingPathId,
                        ItemId = itemId,
                        ItemType = trainingPathItem is Lesson ? "Lesson" : "Quiz", // Get type from actual item
                        IsCompleted = true,
                        CompletedDate = DateTime.UtcNow,
                        Score = request.Score ?? 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.UserProgress.Add(progress);
                }
                else
                {
                    // Update existing progress
                    progress.IsCompleted = true;
                    progress.CompletedDate = DateTime.UtcNow;
                    progress.Score = request.Score ?? progress.Score;
                    progress.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                // Update User_TrainingPath record
                var userTrainingPath = await _context.UserTrainingPaths
                    .FirstOrDefaultAsync(utp => utp.TrainingPathId == request.TrainingPathId && utp.UserId == userId);

                var totalItems = await _context.TrainingPathItemOrders
                    .CountAsync(tpio => tpio.TrainingPathId == request.TrainingPathId);

                var completedItems = await _context.UserProgress
                    .CountAsync(up => up.TrainingPathId == request.TrainingPathId && 
                                    up.UserId == userId && 
                                    up.IsCompleted);

                if (userTrainingPath == null)
                {
                    // Create new User_TrainingPath
                    userTrainingPath = new User_TrainingPath
                    {
                        UserId = userId,
                        TrainingPathId = request.TrainingPathId,
                        Progress = $"{completedItems}/{totalItems}",
                        Status = completedItems == totalItems ? "Completed" : "In Progress",
                        StartDate = DateTime.UtcNow,
                        CompletionDate = completedItems == totalItems ? DateTime.UtcNow : null,
                        CreationDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    };
                    _context.UserTrainingPaths.Add(userTrainingPath);
                }
                else
                {
                    // Update existing User_TrainingPath
                    userTrainingPath.Progress = $"{completedItems}/{totalItems}";
                    userTrainingPath.Status = completedItems == totalItems ? "Completed" : "In Progress";
                    if (completedItems == totalItems)
                        userTrainingPath.CompletionDate = DateTime.UtcNow;
                    userTrainingPath.UpdateDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                // Get and return updated progress
                var updatedProgress = await GetProgress(request.TrainingPathId, userId);
                if (updatedProgress.Result is OkObjectResult okResult)
                {
                    return Ok(okResult.Value);
                }
                
                return StatusCode(500, new { error = "Failed to get updated progress" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error completing item: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("items/{itemId}/complete/{userId}")]
        public async Task<IActionResult> UncompleteItem(Guid itemId, Guid userId, [FromBody] CompleteItemRequest request)
        {
            try
            {
                if (itemId == Guid.Empty || userId == Guid.Empty || request?.TrainingPathId == Guid.Empty)
                {
                    return BadRequest("Invalid itemId, userId, or trainingPathId");
                }

                // Find the progress record
                var progress = await _context.UserProgress
                    .FirstOrDefaultAsync(up => 
                        up.TrainingPathId == request.TrainingPathId && 
                        up.ItemId == itemId && 
                        up.UserId == userId);

                if (progress == null)
                {
                    return NotFound("Progress record not found");
                }

                // Delete the progress record
                _context.UserProgress.Remove(progress);
                await _context.SaveChangesAsync();

                // Update User_TrainingPath record
            var userTrainingPath = await _context.UserTrainingPaths
                    .FirstOrDefaultAsync(utp => utp.TrainingPathId == request.TrainingPathId && utp.UserId == userId);

                if (userTrainingPath != null)
                {
            var totalItems = await _context.TrainingPathItemOrders
                        .CountAsync(tpio => tpio.TrainingPathId == request.TrainingPathId);

                    var completedItems = await _context.UserProgress
                        .CountAsync(up => up.TrainingPathId == request.TrainingPathId && 
                                        up.UserId == userId && 
                                        up.IsCompleted);

            userTrainingPath.Progress = $"{completedItems}/{totalItems}";
                    userTrainingPath.Status = completedItems == 0 ? "Not Started" : 
                                            completedItems == totalItems ? "Completed" : "In Progress";
                    userTrainingPath.CompletionDate = completedItems == totalItems ? DateTime.UtcNow : null;
            userTrainingPath.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
                }

                // Get and return updated progress
                var updatedProgress = await GetProgress(request.TrainingPathId, userId);
                if (updatedProgress.Result is OkObjectResult okResult)
                {
                    return Ok(okResult.Value);
                }
                
                return StatusCode(500, new { error = "Failed to get updated progress" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uncompleting item: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        public class CompleteItemRequest
        {
            [Required]
            public Guid TrainingPathId { get; set; }
            public int? Score { get; set; }

            public override string ToString()
            {
                return $"CompleteItemRequest {{ TrainingPathId: {TrainingPathId}, Score: {Score} }}";
            }
        }

        [HttpPost("{trainingPathId}/progress/{userId}")]
        public async Task<IActionResult> CreateOrUpdateProgress(Guid trainingPathId, Guid userId, [FromBody] int completedItems)
        {
            // Check if User_TrainingPath exists
            var userTrainingPath = await _context.UserTrainingPaths
                .FirstOrDefaultAsync(utp => utp.TrainingPathId == trainingPathId && utp.UserId == userId);

            var totalItems = await _context.TrainingPathItemOrders
                .CountAsync(tpio => tpio.TrainingPathId == trainingPathId);

            if (userTrainingPath == null)
            {
                // Create a new User_TrainingPath
                userTrainingPath = new User_TrainingPath
                {
                    UserId = userId,
                    TrainingPathId = trainingPathId,
                    Progress = $"{completedItems}/{totalItems}",
                    Status = completedItems == totalItems ? "Completed" : "In Progress",
                    StartDate = DateTime.UtcNow,
                    CompletionDate = completedItems == totalItems ? DateTime.UtcNow : null,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _context.UserTrainingPaths.Add(userTrainingPath);
            }
            else
            {
                // Update existing User_TrainingPath
                userTrainingPath.Progress = $"{completedItems}/{totalItems}";
                userTrainingPath.Status = completedItems == totalItems ? "Completed" : "In Progress";
                if (completedItems == totalItems)
                    userTrainingPath.CompletionDate = DateTime.UtcNow;

                userTrainingPath.UpdateDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Ok(userTrainingPath);
        }

    }
}
