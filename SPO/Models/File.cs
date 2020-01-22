namespace SPO.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class File : DatabaseEntity
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public byte[] Content { get; set; }

        [Required]
        public string Type { get; set; }
    }
}