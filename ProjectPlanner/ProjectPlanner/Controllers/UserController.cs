
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Client.Models;
using ProjectPlanner.Data;
using ProjectPlanner.Data.Projects;

namespace ProjectPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<string>> GetUser(string uuid)
        {
            var user = await _context.Users.FindAsync(uuid);
            if (user == null) return NotFound();
            return Ok(user.UserName);
        }
    }
}
