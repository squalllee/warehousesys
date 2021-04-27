using WareHouseSys.Factory;
using WareHouseSys.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WareHouseSys.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string ID)
        {
        
            //Employee emp = EmployeeFactory.getEmployee(ID);
            //UNIT unit = UnitFactory.getUint(emp.UNITNO);

            //if (emp != null)
            //{
            //    ViewBag.UserName = emp.TMNAME;
            //    ViewBag.UnitName = unit.UNITNAME;
            //    ViewBag.JOBName = emp.JOBName;
            //    ViewBag.Ext = emp.TelExtension;
            //    ViewBag.Email = emp.EMAIL;
            //}
            

            return View();
        }
        
    }
}