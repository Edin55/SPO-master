namespace SPO.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ToDoList : DatabaseEntity
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Started { get; set; }

        public DateTime? Finished { get; set; }
    }
}