namespace SPO.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using SPO.Models;
    using System.Linq;

    [Authorize]
    public class SubjectsController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            Student student = await GetLoggedInStudent();
            IQueryable<Subject> subjects = db.Subjects.Where(x => x.StudentId == student.Id);
            return View(await subjects.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != subject.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(subject);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Credits,Enrolled,Semester,Professor")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                Student dbStudent = await GetLoggedInStudent();
                subject.StudentId = dbStudent.Id;
                db.Subjects.Add(subject);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            Student dbStudent = await GetLoggedInStudent();
            if(dbStudent.Id != subject.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Credits,Enrolled,Semester,Professor")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                Subject dbSubject = await db.Subjects.FindAsync(subject.Id);
                Student dbStudent = await GetLoggedInStudent();
                if (dbStudent.Id != dbSubject.StudentId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                dbSubject.Name = subject.Name;
                dbSubject.Professor = subject.Professor;
                dbSubject.Credits = subject.Credits;
                dbSubject.Enrolled = subject.Enrolled;
                dbSubject.Semester = subject.Semester;
                db.Entry(dbSubject).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != subject.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Subject subject = await db.Subjects.FindAsync(id);
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != subject.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.Subjects.Remove(subject);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
