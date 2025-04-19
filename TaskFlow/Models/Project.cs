using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TaskFlow.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Column("Name")]
        [Required]
        public string Name { get; set; }
        
        [Column("Description")]
        public string Description { get; set; }
        
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("Tasks")]
        public ICollection<Task> Tasks { get; set; }
    }
}
