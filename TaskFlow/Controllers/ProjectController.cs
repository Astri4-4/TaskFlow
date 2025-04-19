using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;
using TaskFlow.Requests;

namespace TaskFlow.Controllers
{
    [Route("/api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects()
        {
            // ADDING THE PROJECT TO DB
            var user = HttpContext.Items["User"] as User;

            if (user == null)
            {
                return Unauthorized("You are not connected");
            }

            var projects = await _context.Projects.Select(p => new Project
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedDate = p.CreatedDate,
                Tasks = p.Tasks,
            }).ToListAsync();

            

            return Ok(projects);
        }


        // A RETIRER
        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] ProjectRequest request)
        {

            var user = HttpContext.Items["User"] as User;

            if (user == null) { return Unauthorized("You are not connected"); }

            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                CreatedDate = DateTime.Now,
                UserId = user.Id,
                User = user
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return Ok("Project added successfully");
        }

    }
}
