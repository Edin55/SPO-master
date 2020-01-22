namespace SPO.Controllers
{
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using SPO.Models;

    [Authorize]
    public class FilesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            Student student = await GetLoggedInStudent();
            IQueryable<File> files = db.Files.Where(x => x.StudentId == student.Id);
            return View(await files.ToListAsync());
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(HttpPostedFileBase file1)
        {
            if (ModelState.IsValid)
            {
                Student dbStudent = await GetLoggedInStudent();
                foreach (string upload in Request.Files)
                {
                    File file;
                    HttpPostedFileBase uploadedFile = Request.Files[upload];
                    if (uploadedFile != null && uploadedFile.ContentLength > 0)
                    {
                        file = new File
                        {
                            FileName = System.IO.Path.GetFileName(uploadedFile.FileName),
                            Type = uploadedFile.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(uploadedFile.InputStream))
                        {
                            file.Content = reader.ReadBytes(uploadedFile.ContentLength);
                        }
                    }
                    else
                    {
                        return View();
                    }
                    file.StudentId = dbStudent.Id;
                    db.Files.Add(file);
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Download(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = await db.Files.FindAsync(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != file.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return File(file.Content, file.Type, file.FileName);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = await db.Files.FindAsync(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != file.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(file);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            File file = await db.Files.FindAsync(id);
            Student dbStudent = await GetLoggedInStudent();
            if (dbStudent.Id != file.StudentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.Files.Remove(file);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
