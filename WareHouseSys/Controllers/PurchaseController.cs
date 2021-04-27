using SqlSugar;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModel;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers
{
    public class PurchaseController : BaseController
    {
        
        public ActionResult PurchaseAdd(string requireNo)
        {
            ViewBag.requireNo = requireNo;
            TempData["requireNo"] = requireNo;
            List<Employee> employees = EmployeeFactory.getAllEmployee();
            List<UNIT> uNITs = UnitFactory.getAllUint();
            PurchaseHeaderViewModel purchaseHeaderViewModel = PurchaseFactory.getPurcheaseHeaderByReqNo(requireNo);

            RequireAddViewModel requireAddViewModel = new RequireAddViewModel
            {
                Users = employees,
                Units = uNITs,
                purchaseHeader = purchaseHeaderViewModel
            };

            RequirementHeader requirementHeader =  RequirementFactory.getRequirementHeader(requireNo);
            ViewBag.Status = requirementHeader.Status;

            if(requirementHeader.Status == "2")
            {
                TempData["TransToPurViewModel"] = requireAddViewModel;
                return RedirectToAction("PurchaseAddWithContract");
            }
            else
            {
                return View(requireAddViewModel);
            }
            
        }

        public ActionResult PurchaseLocalUpdate(TransToPurViewModel transToPurViewModel)
        {
            ViewBag.Units = UnitFactory.getAllUint();
            return View(transToPurViewModel);
        }

        public ActionResult PurchaseAddWithContract()
        {
            ViewBag.requireNo = TempData["requireNo"];
            RequireAddViewModel requireAddViewModel = (RequireAddViewModel)TempData["TransToPurViewModel"];
            return View(requireAddViewModel);
        }

        public ActionResult PurchaseDetailLocalAdd(string SerialNo)
        {
            ViewBag.SerialNo = SerialNo;

            return View(UnitFactory.getAllUint());
        }

        public ActionResult PurchaseDetailAdd(string PurchaseNo)
        {
            ViewBag.PurchaseNo = PurchaseNo;

            return View(UnitFactory.getAllUint());
        }

        public ActionResult PurchaseSearch()
        {
            return View();
        }

        public ActionResult PurDetail(string PurchaseNo)
        {
            ViewBag.PurchaseNo = PurchaseNo;
            PurchaseHeader sugarQueryable = PurchaseFactory.getPurcheaseHeader(PurchaseNo);
            ViewBag.ContractPriceWithoutVAT = sugarQueryable.ContractPriceWithoutVAT;
            ViewBag.ContractPriceIncludeVAT = sugarQueryable.ContractPriceIncludeVAT;
            return View();
        }

        public ActionResult getPurBodyByLot(string PurchaseNo,string DeliveryLot, int skip, int take, int page, int pageSize,
       List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<PurchaseBodyViewModel> sugarQueryable = PurchaseFactory.getPurcheaseBodyByLot(PurchaseNo, DeliveryLot);

            int Total = sugarQueryable.Count();

            string sortStr = "";

            if (sort != null)
            {
                foreach (SortCriteria sortCriteria in sort)
                {
                    sortStr += String.Format("{0} {1}", sortCriteria.Field, sortCriteria.Dir) + ",";
                }
                sortStr = sortStr.TrimEnd(',');

                sugarQueryable.OrderBy(sortStr);
            }


            var retObj = new
            {
                data = sugarQueryable.Skip(skip).Take(take).ToList(),
                //data = sugarQueryable.Skip((page-1)* pageSize).Take(pageSize).ToList(),
                //data = sugarQueryable.ToList(),
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }
/*
        public ActionResult getPurBodyByLot(int skip, int take, int page, int pageSize,
       List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<PurchaseBodyViewModel> sugarQueryable = PurchaseFactory.getPurcheaseBodyByLot("a10910089", "1");

            int Total = sugarQueryable.Count();

            string sortStr = "";

            if (sort != null)
            {
                foreach (SortCriteria sortCriteria in sort)
                {
                    sortStr += String.Format("{0} {1}", sortCriteria.Field, sortCriteria.Dir) + ",";
                }
                sortStr = sortStr.TrimEnd(',');

                sugarQueryable.OrderBy(sortStr);
            }


            var retObj = new
            {
                data = sugarQueryable.Skip(skip).Take(take).ToList(),
                //data = sugarQueryable.Skip((page-1)* pageSize).Take(pageSize).ToList(),
                //data = sugarQueryable.ToList(),
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }
*/
        public ActionResult PurchaseUpdate(string PurchaseNo)
        {
            PurchaseHeader purchaseHeader = PurchaseFactory.getPurcheaseHeader(PurchaseNo);
            ViewBag.Units = UnitFactory.getAllUint();
            ViewBag.Employees = EmployeeFactory.getAllEmployee();
            return View(purchaseHeader);
        }

        public ActionResult PurUpdateDetail(string PurchaseNo,string SerialNo)
        {
            ViewBag.PurchaseNo = PurchaseNo;
            PurchaseDetailViewModel purchaseDetailViewModel = PurchaseFactory.getPurcheaseBody(PurchaseNo, SerialNo);
            ViewBag.Units = UnitFactory.getAllUint();
            return View(purchaseDetailViewModel);
        }

        public ActionResult TransToRecv(string PurchaseNo)
        {
            ViewBag.PurchaseNo = PurchaseNo;
            ViewBag.OpenContract = PurchaseFactory.IsOpenContract(PurchaseNo);
            List<PurLotClass> Lots = PurchaseFactory.getTransRecvLots(PurchaseNo);

            for(int i=Lots.Count - 1;i>=0;i--)
            {
                if(!PurchaseFactory.canTransToRecv(Lots[i].PurchaseNo, Lots[i].DeliveryLot))
                {
                    Lots.Remove(Lots[i]);
                }
            }

            return View(Lots);
        }

        public ActionResult TransToInbound(string PurchaseNo)
        {
            ViewBag.PurchaseNo = PurchaseNo;
            List<PurLotClass> Lots = PurchaseFactory.getTransInboundLots(PurchaseNo);

            return View(Lots);
        }

        

    }
}