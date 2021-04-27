using System.Collections.Generic;
using System.Web.Mvc;
using WareHouseSys.Factory;
using WareHouseSys.Models;

namespace WareHouseSys.Controllers
{
    public class FormCreateController : BaseController
    {
        public ActionResult ReceiveCreate()
        {
            return View();
        }

        public ActionResult PurCreate()
        {
            return View();
        }

        public ActionResult AcceptanceCreate()
        {
            return View();
        }

        [HttpGet]
        [Route("FormCreate/PurDetail/{RequireNo}")]
        public ActionResult PurDetail(string RequireNo)
        {
            ViewBag.RequireNo = RequireNo;
            ViewBag.IsClose = RequirementFactory.IsClose(RequireNo);

            return View();
        }

        [HttpGet]
        [Route("FormCreate/ReceiveDetail/{PurchaseNo}")]
        public ActionResult ReceiveDetail(string PurchaseNo)
        {
            //string PurchaseNo = form.Get("PurchaseNo");
            ViewBag.PurchaseNo = PurchaseNo;
            ViewBag.IsClose = PurchaseFactory.IsClose(PurchaseNo);
            List<PurLotClass> Lots = PurchaseFactory.getRecvLots(PurchaseNo);

            return View(Lots);
        }
    }
}