using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;
using Pawgress.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DogProfileController : ControllerBase
    {
        private readonly DogProfileService _service;
        private readonly ApplicationDbContext _context;

        public DogProfileController(DogProfileService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var dogProfiles = _service.GetAll();
            var dtos = dogProfiles.Select(dp => new DogProfileDto
            {
                DogProfileId = dp.DogProfileId,
                Name = dp.Name,
                Breed = dp.Breed,
                Image = dp.Image,
                DateOfBirth = dp.DateOfBirth,
                Notes = dp.Notes?.Select(n => n.Description).ToList(),
                CreationDate = dp.CreationDate,
                UpdateDate = dp.UpdateDate
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var dogProfile = _service.GetById(id);
            if (dogProfile == null) return NotFound();

            var dto = new DogProfileDto
            {
                DogProfileId = dogProfile.DogProfileId,
                Name = dogProfile.Name,
                Breed = dogProfile.Breed,
                Image = dogProfile.Image,
                DateOfBirth = dogProfile.DateOfBirth,
                Notes = dogProfile.Notes?.Select(n => n.Description).ToList(),
                CreationDate = dogProfile.CreationDate,
                UpdateDate = dogProfile.UpdateDate


            };
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DogProfileDto dogProfileDto)
        {
            Console.WriteLine($"Image received: {dogProfileDto.Image}");
            var dogProfile = new DogProfile
            {
                DogProfileId = Guid.NewGuid(),
                Name = dogProfileDto.Name,
                Breed = dogProfileDto.Breed,
                DateOfBirth = dogProfileDto.DateOfBirth,
                Image = dogProfileDto.Image,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };

            var created = _service.Create(dogProfile);
            return Ok(new DogProfileDto
            {
                DogProfileId = created.DogProfileId,
                Name = created.Name,
                Breed = created.Breed,
                DateOfBirth = created.DateOfBirth,
                Image = created.Image,
                CreationDate = created.CreationDate,
                UpdateDate = created.UpdateDate
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] DogProfileDto dogProfileDto)
        {
            var dogProfile = _service.GetById(id);
            if (dogProfile == null) return NotFound();

            dogProfile.Name = dogProfileDto.Name;
            dogProfile.Breed = dogProfileDto.Breed;
            dogProfile.DateOfBirth = dogProfileDto.DateOfBirth;
            dogProfile.Image = dogProfileDto.Image;
            dogProfile.UpdateDate = DateTime.Now;
            _service.Update(id, dogProfile);
            return Ok(dogProfileDto);
        }

        [HttpPost("{dogId}/favorite/{userId}")]
        public async Task<IActionResult> ToggleFavorite(Guid dogId, Guid userId)
        {
            try
            {
                var userDogProfile = await _context.UserDogProfiles
                    .FirstOrDefaultAsync(udp => udp.DogProfileId == dogId && udp.UserId == userId);

                if (userDogProfile == null)
                {
                    // Create new relationship if it doesn't exist
                    userDogProfile = new User_DogProfile
                    {
                        UserId = userId,
                        DogProfileId = dogId,
                        IsFavorite = true,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.MaxValue,
                        CreationDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow
                    };
                    _context.UserDogProfiles.Add(userDogProfile);
                }
                else
                {
                    // Toggle existing favorite status
                    userDogProfile.IsFavorite = !userDogProfile.IsFavorite;
                    userDogProfile.UpdateDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return Ok(new { isFavorite = userDogProfile.IsFavorite });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{dogId}/favorite/{userId}")]
        public async Task<IActionResult> GetFavoriteStatus(Guid dogId, Guid userId)
        {
            try
            {
                var userDogProfile = await _context.UserDogProfiles
                    .FirstOrDefaultAsync(udp => udp.DogProfileId == dogId && udp.UserId == userId);

                return Ok(new { isFavorite = userDogProfile?.IsFavorite ?? false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var dogProfile = _service.GetById(id);
            if (dogProfile == null) return NotFound("Hondenprofiel niet gevonden.");

            // Verwijder gekoppelde notities indien nodig
            if (dogProfile.Notes != null && dogProfile.Notes.Any())
            {
                dogProfile.Notes.Clear();
            }

            // Verwijder het hondenprofiel
            var result = _service.Delete(id);
            if (!result) return BadRequest("Er is iets misgegaan bij het verwijderen.");

            return Ok("Hondenprofiel succesvol verwijderd.");
        }

        
    }
}
