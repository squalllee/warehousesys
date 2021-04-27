using System.Collections.Generic;
using System.Web.Mvc;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;

namespace WareHouseSys.Models
{
    [Authorize]
    public class BaseController : Controller
    {
        public BaseController()
        {
            var userFromAuthCookie = System.Threading.Thread.CurrentPrincipal;
            if (userFromAuthCookie != null && userFromAuthCookie.Identity.IsAuthenticated)
            {
                string ID = userFromAuthCookie.Identity.Name;
                Employee emp = EmployeeFactory.getEmployee(ID);
                UNIT unit = UnitFactory.getUint(emp.UNITNO);
                List<string> wareHouseList = WareHouseGroupFactory.getWareHouseIdByUser(ID);

                if (emp != null)
                {
                    ViewBag.UserName = emp.TMNAME;
                    ViewBag.UnitName = unit.UNITNAME;
                    ViewBag.JOBName = emp.JOBName;
                    ViewBag.Ext = emp.TelExtension;
                    ViewBag.Email = emp.EMAIL;
                    ViewBag.UserId = emp.KEYNO.Trim();
                    ViewBag.UnitId = unit.UNITNO.Trim();
                    ViewBag.WareHouseList = wareHouseList;
                }

            }
        }

        
        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    string ID = HttpContext.User.Identity.Name;
        //    Employee emp = EmployeeFactory.getEmployee(ID);
        //    UNIT unit = UnitFactory.getUint(emp.UNITNO);
        //    List<string> wareHouseList = WareHouseGroupFactory.getWareHouseIdByUser(ID);

        //    if (emp != null)
        //    {
        //        ViewBag.UserName = emp.TMNAME;
        //        ViewBag.UnitName = unit.UNITNAME;
        //        ViewBag.JOBName = emp.JOBName;
        //        ViewBag.Ext = emp.TelExtension;
        //        ViewBag.Email = emp.EMAIL;
        //        ViewBag.UserId = emp.KEYNO.Trim();
        //        ViewBag.UnitId = unit.UNITNO.Trim();
        //        ViewBag.WareHouseList = wareHouseList;
        //    }
        //}
      
    }
}