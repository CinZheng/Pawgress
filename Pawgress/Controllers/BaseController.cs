using Microsoft.AspNetCore.Mvc;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<T> : Controller where T : class
    {
        private readonly BaseService<T> _service;

        public BaseController(BaseService<T> service)
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
            var result = _service.GetById(id);
            if (result == null) return NotFound("Niet gevonden.");
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] T entity)
        {
            return Ok(_service.Create(entity));
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] T updatedEntity)
        {
            var result = _service.Update(id, updatedEntity);
            if (result == null) return NotFound("Niet gevonden.");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _service.Delete(id);
            if (!result) return NotFound("Niet gevonden.");
            return Ok("Succesvol verwijderd.");
        }
    }
}
