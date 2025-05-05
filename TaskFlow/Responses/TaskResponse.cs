using TaskFlow.Models;

namespace TaskFlow.Responses
{
    public class TaskResponse
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public Status.StatusType? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public ICollection<string>? Commentaires { get; set; }
        public int? ProjectId { get; set; }

    }
}
