using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GandamarCloudAPI.Data;
using GandamarCloudAPI.Models;

namespace GandamarCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncUsers([FromBody] List<CloudUser> users)
        {
            foreach (var user in users)
            {
                var existing = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                if (existing != null)
                {
                    existing.PasswordHash = user.PasswordHash;
                    existing.Role = user.Role;
                }
                else
                {
                    _context.Users.Add(user);
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == req.Username && u.PasswordHash == req.PasswordHash);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
            return Ok(new { user.SyncId, user.Username, user.Role });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
    }
}
