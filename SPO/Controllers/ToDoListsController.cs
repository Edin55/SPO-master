namespace SPO.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using SPO.Models;
    using System.Linq;

    [Authorize]
    public class ToDoListsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            Student student = await GetLoggedInStudent();
            return View(await db.ToDoLists.Where(x => x.StudentId == student.Id).ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoList toDoList = await db.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != toDoList.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(toDoList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StudentId,Title,Description,Started,Finished")] ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                Student dbStudent = await GetLoggedInStudent();
                toDoList.StudentId = dbStudent.Id;
                db.ToDoLists.Add(toDoList);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(toDoList);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoList toDoList = await db.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != toDoList.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(toDoList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Started,Finished")] ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                ToDoList dbTodoList = await db.ToDoLists.FindAsync(toDoList.Id);
                Student dbStudent = await GetLoggedInStudent();
                if (dbStudent.Id != dbTodoList.StudentId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                dbTodoList.Title = toDoList.Title;
                dbTodoList.Description = toDoList.Description;
                dbTodoList.Started = toDoList.Started;
                dbTodoList.Finished = toDoList.Finished;
                db.Entry(dbTodoList).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(toDoList);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoList toDoList = await db.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != toDoList.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(toDoList);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ToDoList toDoList = await db.ToDoLists.FindAsync(id);
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != toDoList.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.ToDoLists.Remove(toDoList);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
