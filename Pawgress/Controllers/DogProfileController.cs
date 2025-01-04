using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Services;
using System;
using System.Linq;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DogProfileController : ControllerBase
    {
        private readonly DogProfileService _service;

        public DogProfileController(DogProfileService service)
        {
            _service = service;
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
                Notes = dp.Notes?.Select(n => n.Description).ToList()
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
                Notes = dogProfile.Notes?.Select(n => n.Description).ToList()
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
                Image = dogProfileDto.Image
            };

            var created = _service.Create(dogProfile);
            return Ok(new DogProfileDto
            {
                DogProfileId = created.DogProfileId,
                Name = created.Name,
                Breed = created.Breed,
                DateOfBirth = created.DateOfBirth,
                Image = created.Image
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

            _service.Update(id, dogProfile);
            return Ok(dogProfileDto);
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
