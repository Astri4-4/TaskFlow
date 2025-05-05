namespace TaskFlow.Requests
{
    public class ProjectRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public ICollection<TaskFlow.Models.Task>? Tasks { get; set; }
    }
}
