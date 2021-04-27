using System.Web.Mvc;

namespace WareHouseSys.Controllers
{
    public class testController : Controller
    {
        // GET: test
        public ActionResult Index(string Code,string Status)
        {
            string ID = HttpContext.User.Identity.Name;

            return View();
        }
    }
}