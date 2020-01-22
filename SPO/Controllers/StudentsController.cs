namespace SPO.Controllers
{
    using SPO.Enums;
    using SPO.Models;
    using SPO.Models.Dtos;
    using SPO.Utilities;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [Authorize]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Dashboard()
        {
            Student dbStudent = await GetLoggedInStudent();
            List<Exam> grades = await(from dbSubjects in db.Subjects.Where(x => x.StudentId == dbStudent.Id)
                                      join dbExams in db.Exams.Where(x => x.Grade != Grade.Pet)
                                      on dbSubjects.Id equals dbExams.SubjectId
                                      select dbExams).ToListAsync();

            decimal averageGrades = 0;
            if (grades.Any())
            {
                averageGrades = (decimal)grades.GroupBy(x => x.SubjectId).Select(x => x.Max(y => y.Grade.ToNumber())).Average();
            }
           

            List<Subject> subjectsPassed = await (from dbSubjects in db.Subjects.Where(x => x.StudentId == dbStudent.Id)
                                                  join dbExams in db.Exams.Where(x => x.Grade != Grade.Pet)
                                                  on dbSubjects.Id equals dbExams.SubjectId
                                                  select dbSubjects).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToListAsync();

            List<Subject> subjectsUnpassed = await (from dbSubjects in db.Subjects.Where(x => x.StudentId == dbStudent.Id)
                                                    join dbExams in db.Exams.Where(x => x.Grade == Grade.Pet) on dbSubjects.Id equals dbExams.SubjectId into se
                                                    from dbExams in se.DefaultIfEmpty()
                                                    select dbSubjects).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToListAsync();

            subjectsUnpassed = subjectsUnpassed.DistinctDisunionBy(subjectsPassed, x => x.Id).ToList();

            List<Subject> subjects = new List<Subject>();
            subjects.AddRange(subjectsPassed);
            subjects.AddRange(subjectsUnpassed);

            List<Exam> examPassed = await db.Exams.Where(x => x.Grade != Grade.Pet && x.StudentId == dbStudent.Id).ToListAsync();

            List<Exam> examUnpassed = await db.Exams.Where(x => x.Grade == Grade.Pet && x.StudentId == dbStudent.Id).ToListAsync();

            DashboardDto dashboardDto = new DashboardDto()
            {
                Subjects =  subjects,
                GradeAverage = averageGrades,
                UnpassedSubjects = subjectsUnpassed,
                UnpassedExams = examUnpassed,
                PassedSubjects = subjectsPassed,
                PassedExams = examPassed
            };

            return View(dashboardDto);
        }

        [HttpGet]
        public new async Task<ActionResult> Profile()
        {
            Student dbStudent = await GetLoggedInStudent();

            int subjectsNum = await db.Subjects.Where(x => x.StudentId == dbStudent.Id).CountAsync();

            int subjectsPassedNum = await (from dbSubjects in db.Subjects.Where(x => x.StudentId == dbStudent.Id)
                                           join dbExams in db.Exams.Where(x => x.Grade != Grade.Pet)
                                           on dbSubjects.Id equals dbExams.SubjectId
                                           select dbSubjects).GroupBy(x => x.Id).Select(x => x.Key).CountAsync();

            List<Exam> grades = await (from dbSubjects in db.Subjects.Where(x => x.StudentId == dbStudent.Id)
                                      join dbExams in db.Exams.Where(x => x.Grade != Grade.Pet)
                                      on dbSubjects.Id equals dbExams.SubjectId
                                      select dbExams).ToListAsync();

            decimal averageGrades = 0;
            if (grades.Any())
            {
                averageGrades = (decimal)grades.GroupBy(x => x.SubjectId).Select(x => x.Max(y => y.Grade.ToNumber())).Average();
            }

            ProfileDto profileDto = new ProfileDto()
            {
                Index = dbStudent.Index,
                FirstName = dbStudent.FirstName,
                LastName = dbStudent.LastName,
                School = dbStudent.School,
                Email = dbStudent.User.Email,
                Street = dbStudent.Street,
                City = dbStudent.City,
                Country = dbStudent.Country,
                Average = averageGrades,
                Passed = subjectsPassedNum,
                Subjects = subjectsNum
            };
            return View(profileDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public new async Task<ActionResult> Profile([Bind(Include = "Id,Name,Index,FirstName,LastName,School,Street,City,Country,Average,Passed,Subjects")] ProfileDto profile)
        {
            if (ModelState.IsValid)
            {
                Student dbStudent = await GetLoggedInStudent();
                dbStudent.Index = profile.Index;
                dbStudent.FirstName = profile.FirstName;
                dbStudent.LastName = profile.LastName;
                dbStudent.School = profile.School;
                dbStudent.Street = profile.Street;
                dbStudent.City = profile.City;
                dbStudent.Country = profile.Country;
                db.Entry(dbStudent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Profile");
            }

            ProfileDto profileDto = new ProfileDto()
            {
                Index = profile.Index,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                School = profile.School,
                Street = profile.Street,
                City = profile.City,
                Country = profile.Country,
                Average = profile.Average,
                Passed = profile.Passed,
                Subjects = profile.Subjects
            };

            return View(profileDto);
        }
    }
}