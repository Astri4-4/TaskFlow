using TaskFlow.Models;

namespace TaskFlow.Responses
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }

        public ICollection<TaskFlow.Models.Task> Tasks { get; set; }

    }
}
