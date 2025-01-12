using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        //[Authorize(Roles = "Admin")] // alleen beheerders kunnen alle gebruikers zien, voor testing nu gecomment
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        //[Authorize] // toegang voor ingelogde gebruikers
        public IActionResult GetUserById(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound("Gebruiker niet gevonden.");
            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        //[Authorize(Roles = "Admin")] // alleen adminds
        public IActionResult CreateUser([FromBody] User newUser)
        {
            var createdUser = _userService.CreateUser(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        //[Authorize] // alleen ingelogde gebruiekrs kunnen eigen profiel bekijken
        public IActionResult UpdateUser(Guid id, [FromBody] User updatedUser)
        {
            var user = _userService.UpdateUser(id, updatedUser);
            if (user == null)
                return NotFound("Gebruiker niet gevonden of geen toestemming.");
            return Ok(user);
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        //[Authorize] // eigenprofiel verwijderen
        public IActionResult DeleteUser(Guid id)
        {
            var result = _userService.DeleteUser(id);
            if (!result)
                return NotFound("Gebruiker niet gevonden of geen toestemming.");
            return Ok("Gebruiker sucesvol verwijderd.");
        }
    }
}
