using TaskFlow.Models;

namespace TaskFlow.Requests
{
    public class TaskRequest
    {
        public string Title { get; set; }
        public Status.StatusType Status { get; set; }
        public int ProjectId { get; set; }
        public DateTime? DueDate { get; set; }
        
        public ICollection<string>? Commentaires { get; set; }
        
    }
}
