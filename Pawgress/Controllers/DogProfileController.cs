using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pawgress.Data;
using Pawgress.Models;
using Pawgress.Services;
using System;
using System.ComponentModel.DataAnnotations;
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
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var dogProfiles = _service.GetAll();
                if (dogProfiles == null || !dogProfiles.Any())
                {
                    return NotFound("Geen hond profielen gevonden.");
                }

                var dtos = dogProfiles.Select(dp => new DogProfileDto
                {
                    DogProfileId = dp.DogProfileId,
                    Name = string.IsNullOrEmpty(dp.Name) ? "Naamloze hond" : dp.Name,
                    Breed = dp.Breed,
                    Image = dp.Image,
                    DateOfBirth = dp.DateOfBirth,
                    Notes = dp.Notes?.Select(n => n.Description).ToList() ?? new List<string>(),
                    CreationDate = dp.CreationDate,
                    UpdateDate = dp.UpdateDate
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden bij het ophalen van de hond profielen: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Ongeldige hond profiel ID.");
                }

                var dogProfile = _service.GetById(id);
                if (dogProfile == null)
                {
                    return NotFound($"Hond profiel met ID {id} niet gevonden.");
                }

                var dto = new DogProfileDto
                {
                    DogProfileId = dogProfile.DogProfileId,
                    Name = string.IsNullOrEmpty(dogProfile.Name) ? "Naamloze hond" : dogProfile.Name,
                    Breed = dogProfile.Breed,
                    Image = dogProfile.Image,
                    DateOfBirth = dogProfile.DateOfBirth,
                    Notes = dogProfile.Notes?.Select(n => n.Description).ToList() ?? new List<string>(),
                    CreationDate = dogProfile.CreationDate,
                    UpdateDate = dogProfile.UpdateDate
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden bij het ophalen van het hond profiel: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] DogProfileDto dogProfileDto)
        {
            try
            {
                if (dogProfileDto == null)
                {
                    return BadRequest("Geen hond profiel data ontvangen.");
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(dogProfileDto.Name))
                {
                    return BadRequest("Hond naam is verplicht.");
                }

                // Validate field lengths
                if (dogProfileDto.Name.Length < 2 || dogProfileDto.Name.Length > 100)
                {
                    return BadRequest("Hond naam moet tussen 2 en 100 karakters zijn.");
                }

                if (!string.IsNullOrWhiteSpace(dogProfileDto.Breed) && (dogProfileDto.Breed.Length < 2 || dogProfileDto.Breed.Length > 100))
                {
                    return BadRequest("Ras moet tussen 2 en 100 karakters zijn.");
                }

                // Validate date of birth
                if (dogProfileDto.DateOfBirth > DateTime.UtcNow)
                {
                    return BadRequest("Geboortedatum kan niet in de toekomst liggen.");
                }

                var minDate = new DateTime(1990, 1, 1);
                if (dogProfileDto.DateOfBirth < minDate)
                {
                    return BadRequest("Geboortedatum kan niet voor 1 januari 1990 liggen.");
                }

                // Validate image URL if provided
                if (!string.IsNullOrWhiteSpace(dogProfileDto.Image) && !Uri.TryCreate(dogProfileDto.Image, UriKind.Absolute, out _))
                {
                    return BadRequest("Afbeelding URL is niet geldig.");
                }

                var dogProfile = new DogProfile
                {
                    DogProfileId = Guid.NewGuid(),
                    Name = dogProfileDto.Name.Trim(),
                    Breed = dogProfileDto.Breed?.Trim(),
                    Image = dogProfileDto.Image?.Trim(),
                    DateOfBirth = dogProfileDto.DateOfBirth,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                var created = _service.Create(dogProfile);
                return CreatedAtAction(nameof(GetById), new { id = created.DogProfileId }, new DogProfileDto
                {
                    DogProfileId = created.DogProfileId,
                    Name = created.Name,
                    Breed = created.Breed,
                    Image = created.Image,
                    DateOfBirth = created.DateOfBirth,
                    Notes = created.Notes?.Select(n => n.Description).ToList() ?? new List<string>(),
                    CreationDate = created.CreationDate,
                    UpdateDate = created.UpdateDate
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database fout bij het aanmaken van het hond profiel: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden bij het aanmaken van het hond profiel: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] DogProfileDto dogProfileDto)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Ongeldige hond profiel ID.");
                }

                if (dogProfileDto == null)
                {
                    return BadRequest("Geen hond profiel data ontvangen.");
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(dogProfileDto.Name))
                {
                    return BadRequest("Hond naam is verplicht.");
                }

                // Validate field lengths
                if (dogProfileDto.Name.Length < 2 || dogProfileDto.Name.Length > 100)
                {
                    return BadRequest("Hond naam moet tussen 2 en 100 karakters zijn.");
                }

                if (!string.IsNullOrWhiteSpace(dogProfileDto.Breed) && (dogProfileDto.Breed.Length < 2 || dogProfileDto.Breed.Length > 100))
                {
                    return BadRequest("Ras moet tussen 2 en 100 karakters zijn.");
                }

                // Validate date of birth
                if (dogProfileDto.DateOfBirth > DateTime.UtcNow)
                {
                    return BadRequest("Geboortedatum kan niet in de toekomst liggen.");
                }

                var minDate = new DateTime(1990, 1, 1);
                if (dogProfileDto.DateOfBirth < minDate)
                {
                    return BadRequest("Geboortedatum kan niet voor 1 januari 1990 liggen.");
                }

                // Validate image URL if provided
                if (!string.IsNullOrWhiteSpace(dogProfileDto.Image) && !Uri.TryCreate(dogProfileDto.Image, UriKind.Absolute, out _))
                {
                    return BadRequest("Afbeelding URL is niet geldig.");
                }

                var existingDogProfile = _service.GetById(id);
                if (existingDogProfile == null)
                {
                    return NotFound($"Hond profiel met ID {id} niet gevonden.");
                }

                existingDogProfile.Name = dogProfileDto.Name.Trim();
                existingDogProfile.Breed = dogProfileDto.Breed?.Trim();
                existingDogProfile.Image = dogProfileDto.Image?.Trim();
                existingDogProfile.DateOfBirth = dogProfileDto.DateOfBirth;
                existingDogProfile.UpdateDate = DateTime.UtcNow;

                var updated = _service.Update(id, existingDogProfile);
                return Ok(new DogProfileDto
                {
                    DogProfileId = updated.DogProfileId,
                    Name = updated.Name,
                    Breed = updated.Breed,
                    Image = updated.Image,
                    DateOfBirth = updated.DateOfBirth,
                    Notes = updated.Notes?.Select(n => n.Description).ToList() ?? new List<string>(),
                    CreationDate = updated.CreationDate,
                    UpdateDate = updated.UpdateDate
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database fout bij het bijwerken van het hond profiel: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden bij het bijwerken van het hond profiel: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Ongeldige hond profiel ID.");
                }

                var result = _service.Delete(id);
                if (!result)
                {
                    return NotFound($"Hond profiel met ID {id} niet gevonden.");
                }

                return Ok("Hond profiel succesvol verwijderd.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database fout bij het verwijderen van het hond profiel: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden bij het verwijderen van het hond profiel: {ex.Message}");
            }
        }

        [HttpGet("{id}/favorite/{userId}")]
        public async Task<IActionResult> GetFavoriteStatus(Guid id, Guid userId)
        {
            try
            {
                var favorite = await _context.UserDogProfiles
                    .FirstOrDefaultAsync(udp => udp.DogProfileId == id && udp.UserId == userId);
                
                return Ok(new { isFavorite = favorite != null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden bij het ophalen van de favoriet status: {ex.Message}");
            }
        }

        [HttpPost("{id}/favorite/{userId}")]
        public async Task<IActionResult> ToggleFavorite(Guid id, Guid userId)
        {
            try
            {
                var favorite = await _context.UserDogProfiles
                    .FirstOrDefaultAsync(udp => udp.DogProfileId == id && udp.UserId == userId);

                bool newFavoriteStatus;
                if (favorite != null)
                {
                    _context.UserDogProfiles.Remove(favorite);
                    newFavoriteStatus = false;
                }
                else
                {
                    favorite = new User_DogProfile
                    {
                        UserId = userId,
                        DogProfileId = id
                    };
                    await _context.UserDogProfiles.AddAsync(favorite);
                    newFavoriteStatus = true;
                }

                await _context.SaveChangesAsync();
                return Ok(new { isFavorite = newFavoriteStatus });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden bij het wijzigen van de favoriet status: {ex.Message}");
            }
        }
    }
}
