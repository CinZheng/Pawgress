using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Services;
using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(Guid userId)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                    return NotFound("User not found");

                return Ok(new
                {
                    userId = user.UserId,
                    username = user.Username,
                    email = user.Email,
                    creationDate = user.CreationDate,
                    updateDate = user.UpdateDate
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{userId}/favorites")]
        public async Task<IActionResult> GetFavoriteDogs(Guid userId)
        {
            try
            {
                var favoriteDogs = await _context.UserDogProfiles
                    .Where(udp => udp.UserId == userId && udp.IsFavorite)
                    .Include(udp => udp.DogProfile)
                    .Select(udp => new
                    {
                        udp.DogProfile.DogProfileId,
                        udp.DogProfile.Name,
                        udp.DogProfile.Breed,
                        udp.DogProfile.Image,
                        udp.DogProfile.DateOfBirth
                    })
                    .ToListAsync();

                return Ok(favoriteDogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{userId}/modules")]
        public async Task<IActionResult> GetUserModules(Guid userId)
        {
            try
            {
                var userModules = await _context.UserTrainingPaths
                    .Where(utp => utp.UserId == userId)
                    .Include(utp => utp.TrainingPath)
                    .Select(utp => new
                    {
                        trainingPathId = utp.TrainingPathId,
                        name = utp.TrainingPath.Name,
                        description = utp.TrainingPath.Description,
                        status = utp.Status,
                        startDate = utp.StartDate,
                        progress = utp.Progress
                    })
                    .ToListAsync();

                var result = userModules.Select(module => new
                {
                    trainingPathId = module.trainingPathId,
                    name = module.name,
                    description = module.description,
                    status = module.status,
                    startDate = module.startDate,
                    percentageComplete = module.progress != null ? 
                        (double.Parse(module.progress.Split('/')[0]) / double.Parse(module.progress.Split('/')[1])) * 100 : 0
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(Guid userId, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                    return NotFound("User not found");

                // Update user properties
                user.Username = request.Username;
                user.Email = request.Email;
                user.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    userId = user.UserId,
                    username = user.Username,
                    email = user.Email,
                    creationDate = user.CreationDate,
                    updateDate = user.UpdateDate
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        public class UpdateUserRequest
        {
            public string Username { get; set; }
            public string Email { get; set; }
        }
    }
}
