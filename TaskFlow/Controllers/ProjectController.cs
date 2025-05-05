using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;
using TaskFlow.Requests;
using TaskFlow.Responses;

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
            try
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
            } catch(Exception e)
            {
                return StatusCode(500, new {message = e});
            }
        }


        // A RETIRER
        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] ProjectRequest request)
        {

            try
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
            } catch (Exception e)
            {
                return StatusCode(500, new { message = e });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;

                if (user == null) { return Unauthorized("You are not connected"); }
                var project = await _context.Projects
                    .Include(p => p.Tasks)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (project == null) return NotFound("Project not found");

                var findedProject = new ProjectResponse
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    CreatedDate = project.CreatedDate,
                    UserId = project.UserId,
                    Tasks = project.Tasks
                };

                return Ok(findedProject);
            } catch (Exception e)
            {
                return StatusCode(500, new { message = e});
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Project>> UpdateProjectById(int id, [FromBody] ProjectRequest request)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;

                if (user == null) { return Unauthorized("You are not connected"); }

                var currentProject = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

                if (currentProject == null) return NotFound("Project with id '" + id + "' doesn't exists");

                if (request.Name != null)
                {
                    currentProject.Name = request.Name;
                }

                if (request.Description != null)
                {
                    currentProject.Description = request.Description;
                }

                if (request.Tasks != null)
                {
                    currentProject.Tasks = request.Tasks;
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Project updated succesfully" });
            } catch (Exception e)
            {
                return StatusCode(500, new { message = e });
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> DeleteById(int id)
        {

            try
            {
                var user = HttpContext.Items["User"] as User;
                if (user == null) { return Unauthorized("You are not connected"); }

                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

                if (project == null)
                {
                    return NotFound("Project with id '" + id + "' doesn't exists");
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Project delete successfully" });
            } catch (Exception e) {
                return StatusCode(500, new { message = e });
            }

        }

    }
}
