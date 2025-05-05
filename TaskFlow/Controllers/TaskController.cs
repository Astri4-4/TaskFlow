using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Requests;
using TaskFlow.Models;
using TaskFlow.Responses;

namespace TaskFlow.Controllers
{
    [ApiController]
    [Route("/api/tasks")]
    public class TaskController : ControllerBase
    {

        private readonly AppDbContext _context;

        public TaskController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskFlow.Models.Task>>> Get()
        {
            try
            {
                var user = HttpContext.Items["User"] as User;
                if (user == null)
                {
                    return Unauthorized("You are not connected");
                }

                var tasks = await _context.Tasks.Select(Task => new TaskFlow.Responses.TaskResponse
                {
                    Id = Task.Id,
                    Title = Task.Title,
                    Status = Task.Status,
                    DueDate = Task.DueDate,
                    Commentaires = Task.Commentaires,
                    ProjectId = Task.ProjectId,
                }).ToListAsync();

                return Ok(tasks);
            } catch(Exception e)
            {
                return StatusCode(500, new { message = e });
            }

        }

        [HttpPost]
        public async Task<ActionResult<TaskFlow.Models.Task>> PostTask([FromBody] TaskRequest request)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;
                if (user == null) return Unauthorized("You're not connected");

                var task = new TaskFlow.Models.Task
                {
                    Title = request.Title,
                    Status = request.Status,
                    ProjectId = request.ProjectId,
                    DueDate = request.DueDate
                };

                await _context.Tasks.AddAsync(task);
                await _context.SaveChangesAsync();

                return Ok();
            } catch (Exception e)
            {
                return StatusCode(500, new { message = e });
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskFlow.Models.Task>> GetTaskById(int id)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;
                if (user == null) return Unauthorized("You're not connected");

                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

                if (task == null) return NotFound("There is no task with id '" + id + "'");

                TaskResponse response = new TaskResponse();

                response.Id = task.Id;
                response.Status = task.Status;
                response.DueDate = task.DueDate;
                response.Commentaires = task.Commentaires;
                response.Title = task.Title;
                response.ProjectId = task.ProjectId;

                return Ok(response);
            } catch (Exception e)
            {
                return StatusCode(500, new { message = e });
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskFlow.Models.Task>> UpdateTask([FromBody] TaskRequest request, int id)
        {

            try
            {
                var user = HttpContext.Items["User"] as User;
                if (user == null) return Unauthorized("You're not connected");

                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
                if (task == null) return NotFound("There is no task with id '" + id + "'");

                if (request.Title != null)
                {
                    task.Title = request.Title;
                }
                if (request.DueDate != null)
                {
                    task.DueDate = request.DueDate;
                }
                if (request.Status != task.Status)
                {
                    task.Status = request.Status;
                }
                if (request.Commentaires != null)
                {
                    task.Commentaires = request.Commentaires;
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Task updated succesfully" });
            } catch (Exception e)
            {
                return StatusCode(500, new { message = e });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskFlow.Models.Task>> DeleteTask(int id)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;
                if (user == null) return Unauthorized("You're not connected");

                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
                if (task == null) return NotFound("There is no task with id '" + id + "'");

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Task deleted succesfully" });
            } catch (Exception e)
            {
                return StatusCode(500, new { message = e });
            }
        }



    }
}
