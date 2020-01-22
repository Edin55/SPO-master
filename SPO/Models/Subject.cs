namespace SPO.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Subject : DatabaseEntity
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Credits { get; set; }

        [Required]
        public DateTime Enrolled { get; set; }

        public int? Semester { get; set; }

        [Required]
        public string Professor { get; set; }
    }
}