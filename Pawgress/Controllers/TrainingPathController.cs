using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingPathController : Controller
    {
        private readonly TrainingPathService _service;

        public TrainingPathController(TrainingPathService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var path = _service.GetById(id);
            return path == null ? NotFound("Niet gevonden.") : Ok(path);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TrainingPath path)
        {
            return Ok(_service.Create(path));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] TrainingPath path)
        {
            var result = _service.Update(id, path);
            return result == null ? NotFound("Niet gevonden.") : Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return _service.Delete(id) ? Ok("Succesvol verwijderd.") : NotFound("Niet gevonden.");
        }
    }
}
