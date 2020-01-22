namespace SPO.Models.Dtos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DashboardDto
    {

        public decimal GradeAverage { get; set; }

        public List<Subject> UnpassedSubjects { get; set; }

        public List<Subject> PassedSubjects { get; set; }

        public List<Exam> UnpassedExams { get; set; }

        public List<Exam> PassedExams { get; set; }

        public List<Subject> Subjects { get; set; }
        public DashboardDto()
        {
            Subjects = new List<Subject>();
            UnpassedSubjects = new List<Subject>();
            PassedSubjects = new List<Subject>();
            UnpassedExams = new List<Exam>();
            PassedExams = new List<Exam>();
        }

        public int SubjectsNum { get => Subjects.Count; }

        public int PassedSubjectsNum { get => PassedSubjects.Count; }

        public int UnpassedExamsNum { get => UnpassedExams.Count; }

        public DateTime? NextExam { get => GetNextExam(); }

        private DateTime? GetNextExam()
        {
            try
            {
                return UnpassedExams.Where(x => x.ExamDate > DateTime.Now).Min(x => x.ExamDate);
            }
            catch
            {
                return null;
            }
        }

        
    }
}