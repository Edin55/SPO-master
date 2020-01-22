namespace SPO.Controllers
{
    using Microsoft.AspNet.Identity;
    using SPO.Models;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class ControllerBase : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        protected async Task<Student> GetLoggedInStudent()
        {
            string userFk = User.Identity.GetUserId();
            Student dbStudent = await db.Students.Where(x => x.UserFk.Equals(userFk)).SingleOrDefaultAsync();
            return dbStudent;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}