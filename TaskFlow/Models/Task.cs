using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace TaskFlow.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Column("Title")]
        [Required]
        public string Title { get; set; }

        [Column("Status")]
        public Status.StatusType Status {  get; set; }

        [Column("DueDate")]
        public DateTime DueDate { get; set; }

        [Column("Commentaires")]
        public ICollection<String> Commentaires { get; set; }

        [ForeignKey("Project")]
        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

    }

}
