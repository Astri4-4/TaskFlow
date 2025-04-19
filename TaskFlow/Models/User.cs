using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column("Name")]
        [Required]
        public string Name { get; set; }
        
        [Column("Email")]
        [Required]
        public string Email { get; set; }
        
        [Column("PasswordHash")]
        [Required]
        public string PasswordHash { get; set; }

        [Column("Role")]
        public Role.RoleType Role { get; set; }

        [Column("Projects")]
        public ICollection<Project>? Projects { get; set; }

    }
}
