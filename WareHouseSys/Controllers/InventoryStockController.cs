using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers
{
    public class InventoryStockController : BaseController
    {
        public ActionResult InventoryDraft()
        {
            return View();
        }

        public ActionResult InventoryAdd()
        {
            return View();
        }

        public ActionResult FirstCheck(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            return View();
        }

        public ActionResult ToolFirstCheck(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            return View();
        }

        public ActionResult ToolSecondCheck(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            return View();
        }

        public ActionResult InventoryRecord(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            return View();
        }

        public ActionResult ToolInventoryRecord(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            return View();
        }


        public ActionResult ToolInventorySearch(string OrderNo)
        {
            return View();
        }

        public ActionResult ToolInventoryAdd()
        {
            return View();
        }

        public ActionResult InventoryHeader(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<ToolInventoryHeaderViewModel> sugarQueryable = ToolInventoryFactory.getToolInventoryHeaderViewModel(filter, ID);

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
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }

        public ActionResult SecondCheck(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            return View();
        }

        public ActionResult InventoryClose(string OrderNo)
        {
            StockInventoryHeaderViewModel stockInventoryHeaderViewModel = StockInventoryFactory.getStockInventoryHeaderViewModelByOrderNo(OrderNo).Single();
            ViewBag.OrderNo = OrderNo;
            return View(stockInventoryHeaderViewModel);
        }

        public ActionResult ToolInventoryClose(string OrderNo)
        {
            ToolInventoryHeaderViewModel toolInventoryHeaderViewModel = ToolInventoryFactory.getToolInventoryHeaderViewModelByOrderNo(OrderNo);
            ViewBag.OrderNo = OrderNo;
            return View(toolInventoryHeaderViewModel);
        }
        
        public ActionResult InventoryDetail(string OrderNo)
        {
            List<Attachment> attachments = new List<Attachment>();
            string FirstCheckfilePath = Server.MapPath("~") + "\\Attatchment\\Inventory\\" + OrderNo + "\\FirstCheck";

            if (Directory.Exists(FirstCheckfilePath))
            {
                foreach (string f in Directory.GetFiles(FirstCheckfilePath))
                {
                    attachments.Add(new Attachment
                    {
                        NoteText = "First",
                        FileName = Path.GetFileName(f)
                    });
                }
            }

            string SecondCheckfilePath = Server.MapPath("~") + "\\Attatchment\\Inventory\\" + OrderNo + "\\SecondCheck";

            if (Directory.Exists(SecondCheckfilePath))
            {
                foreach (string f in Directory.GetFiles(SecondCheckfilePath))
                {
                    attachments.Add(new Attachment
                    {
                        NoteText = "Second",
                        FileName = Path.GetFileName(f)
                    });
                }
            }

            StockInventoryHeaderViewModel stockInventoryHeaderViewModel = StockInventoryFactory.getStockInventoryHeaderViewModelByOrderNo(OrderNo).Single();
            stockInventoryHeaderViewModel.attachments = attachments;
            return View(stockInventoryHeaderViewModel);
        }

        public ActionResult ToolInventoryDetail(string OrderNo)
        {
            List<Attachment> attachments = new List<Attachment>();
            string FirstCheckfilePath = Server.MapPath("~") + "\\Attatchment\\ToolInventory\\" + OrderNo + "\\FirstCheck";

            if (Directory.Exists(FirstCheckfilePath))
            {
                foreach (string f in Directory.GetFiles(FirstCheckfilePath))
                {
                    attachments.Add(new Attachment
                    {
                        NoteText = "First",
                        FileName = Path.GetFileName(f)
                    });
                }
            }

            string SecondCheckfilePath = Server.MapPath("~") + "\\Attatchment\\ToolInventory\\" + OrderNo + "\\SecondCheck";

            if (Directory.Exists(SecondCheckfilePath))
            {
                foreach (string f in Directory.GetFiles(SecondCheckfilePath))
                {
                    attachments.Add(new Attachment
                    {
                        NoteText = "Second",
                        FileName = Path.GetFileName(f)
                    });
                }
            }

            ToolInventoryHeaderViewModel toolInventoryHeaderViewModel = ToolInventoryFactory.getToolInventoryHeaderViewModelByOrderNo(OrderNo);
            toolInventoryHeaderViewModel.attachments = attachments;
            return View(toolInventoryHeaderViewModel);
        }

        public ActionResult InventorySearch(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<StockInventoryHeaderViewModel> sugarQueryable = StockInventoryFactory.getStockInventoryHeaderViewModel(filter, wgroupIdList);

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
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }

        public ActionResult Inventorydiff(string OrderNo, int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<StockInventoryBodyViewModel> sugarQueryable = StockInventoryFactory.getStockInventoryDiffBodyViewModel(OrderNo);

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
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }

        public ActionResult ToolInventorydiff(string OrderNo, int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<ToolInventoryBodyViewModel> sugarQueryable = ToolInventoryFactory.getToolDiffInventoryBodyViewModelByOrderNo(OrderNo);

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
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }

        public ActionResult InventoryBodies(string OrderNo,int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<StockInventoryBodyViewModel> sugarQueryable = StockInventoryFactory.getStockInventoryBodyViewModelByOrderNo(OrderNo);

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
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }
        

        public ActionResult FirstSecondCheck(string OrderNo)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<StockInventoryBodyViewModel> sugarQueryable = StockInventoryFactory.getStockInventoryBodyViewModelByOrderNo(OrderNo);

            int Total = sugarQueryable.Count();

            var retObj = new
            {
                data = sugarQueryable.ToList(),
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }

        public ActionResult ToolFirstSecondCheck(string OrderNo)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<ToolInventoryBodyViewModel> sugarQueryable = ToolInventoryFactory.getToolInventoryBodyViewModelByOrderNo(OrderNo);

            int Total = sugarQueryable.Count();

            var retObj = new
            {
                data = sugarQueryable.ToList(),
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }

        public ActionResult getFirstCheckAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Inventory\\" + OrderNo + "\\FirstCheck\\" + FileName;

            string Extension = Path.GetExtension(FileName);
            string contentType = "";

            switch (Extension.ToUpper())
            {
                case ".PNG":
                    contentType = "image/png";
                    break;
                case ".JPG":
                    contentType = "image/jpeg";
                    break;
                case ".PDF":
                    contentType = "application/pdf";
                    break;
            }
            FileStream fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);

            return new FileStreamResult(fsSource, contentType);
        }

        public ActionResult getSecondCheckAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Inventory\\" + OrderNo + "\\SecondCheck\\" + FileName;

            string Extension = Path.GetExtension(FileName);
            string contentType = "";

            switch (Extension.ToUpper())
            {
                case ".PNG":
                    contentType = "image/png";
                    break;
                case ".JPG":
                    contentType = "image/jpeg";
                    break;
                case ".PDF":
                    contentType = "application/pdf";
                    break;
            }
            FileStream fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);

            return new FileStreamResult(fsSource, contentType);
        }

        public ActionResult getToolFirstCheckAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\ToolInventory\\" + OrderNo + "\\FirstCheck\\" + FileName;

            string Extension = Path.GetExtension(FileName);
            string contentType = "";

            switch (Extension.ToUpper())
            {
                case ".PNG":
                    contentType = "image/png";
                    break;
                case ".JPG":
                    contentType = "image/jpeg";
                    break;
                case ".PDF":
                    contentType = "application/pdf";
                    break;
            }
            FileStream fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);

            return new FileStreamResult(fsSource, contentType);
        }

        public ActionResult getToolSecondCheckAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\ToolInventory\\" + OrderNo + "\\SecondCheck\\" + FileName;

            string Extension = Path.GetExtension(FileName);
            string contentType = "";

            switch (Extension.ToUpper())
            {
                case ".PNG":
                    contentType = "image/png";
                    break;
                case ".JPG":
                    contentType = "image/jpeg";
                    break;
                case ".PDF":
                    contentType = "application/pdf";
                    break;
            }
            FileStream fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);

            return new FileStreamResult(fsSource, contentType);
        }
    }
}