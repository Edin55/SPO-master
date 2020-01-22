using System.ComponentModel.DataAnnotations;

namespace SPO.Models
{
    public class DatabaseEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }
}