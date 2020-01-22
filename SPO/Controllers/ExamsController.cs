namespace SPO.Controllers
{
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using SPO.Models;
    using SPO.Enums;

    [Authorize]
    public class ExamsController : ControllerBase
    {
        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            Student student = await GetLoggedInStudent();
            var exams = db.Exams.Where(x => x.StudentId == student.Id);
            return View(await exams.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            Student student = await GetLoggedInStudent();
            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.StudentId == student.Id), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SubjectId,ExamDate,Grade")] Exam exam)
        {
            Student student = await GetLoggedInStudent();
            if (ModelState.IsValid)
            {
                exam.StudentId = student.Id;
                if(exam.Grade == 0)
                {
                    exam.Grade = Grade.Pet;
                }
                db.Exams.Add(exam);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.StudentId == student.Id), "Id", "Name", exam.SubjectId);
            return View(exam);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = await db.Exams.FindAsync(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != exam.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.StudentId == dbStudent.Id), "Id", "Name", exam.SubjectId);
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SubjectId,ExamDate,Grade")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                Exam dbExam = await db.Exams.FindAsync(exam.Id);
                dbExam.SubjectId = exam.SubjectId;
                dbExam.Subject = exam.Subject;
                dbExam.ExamDate = exam.ExamDate;
                dbExam.Grade = exam.Grade;
                db.Entry(dbExam).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            Student dbStudent = await GetLoggedInStudent();
            ViewBag.SubjectId = new SelectList(db.Subjects.Where(x => x.StudentId == dbStudent.Id), "Id", "Name", exam.SubjectId);
            return View(exam);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = await db.Exams.FindAsync(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != exam.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(exam);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Exam exam = await db.Exams.FindAsync(id);
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != exam.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.Exams.Remove(exam);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
