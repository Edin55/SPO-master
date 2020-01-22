namespace SPO.Models
{
    using SPO.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Exam : DatabaseEntity
    {
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }

        [Required]
        public Boolean isPassed { get; set; }
        public Grade Grade { get; set; }
        public int StudentId { get; internal set; }
    }
}