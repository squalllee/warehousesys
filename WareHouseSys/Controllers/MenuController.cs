using System.Web.Mvc;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;

namespace WareHouseSys.Controllers
{
    public class MenuController : Controller
    {
        public ActionResult menu(string controller,string action)
        {
            ViewBag.Controller = controller;
            ViewBag.Action = action;

            string ID = HttpContext.User.Identity.Name;
            Employee emp = EmployeeFactory.getEmployee(ID);
            if (emp != null) ViewBag.UserName = emp.TMNAME;
            return PartialView("_Menu");
        }  
    }
}