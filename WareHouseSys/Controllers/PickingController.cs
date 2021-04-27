using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers
{
    public class PickingController : BaseController
    {       
        [Authorize]
        public ActionResult MaterialPicking()
        {
            return View();
        }

        [Authorize]
        public ActionResult PickingSearch() {
            return View();
        }

        [Authorize]
        public ActionResult PickingRecordSearch()
        {
            return View();
        }

        public ActionResult PickingRecordSearchGrid([DataSourceRequest] DataSourceRequest request)
        {
            ISugarQueryable<PickingSearchViewModel> sugarQueryable = PickingFactory.getPickingSearchViewModel(request);

            string sortStr = "";
            if (request.Sorts.Any())
            {
                foreach (SortDescriptor sortDescriptor in request.Sorts)
                {
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        sortStr += String.Format("{0} {1}", sortDescriptor.Member, "asc") + ",";

                    }
                    else
                    {
                        sortStr += String.Format("{0} {1}", sortDescriptor.Member, "desc") + ",";
                    }
                }
            }

            if (sortStr != "") sugarQueryable = sugarQueryable.OrderBy(sortStr.TrimEnd(','));
            List<PickingSearchViewModel> pickingSearchViewModels = null;
            if (request.PageSize == 0)
              pickingSearchViewModels = sugarQueryable.ToList();
            else
              pickingSearchViewModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            string filePath = Server.MapPath("~") + "\\Attatchment\\Picking\\";

            foreach(PickingSearchViewModel pickingSearchViewModel in pickingSearchViewModels)
            {
                if (Directory.Exists(filePath + pickingSearchViewModel.OrderNo))
                {
                    foreach (string f in Directory.GetFiles(filePath + pickingSearchViewModel.OrderNo))
                    {
                        pickingSearchViewModel.AttUrl = Path.GetFileName(f);
                    }
                }
            }
       
            request.Page = 1;
            DataSourceResult dataSourceResult = pickingSearchViewModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        [Authorize]
        public ActionResult AddMaterialPicking(string OrderNo)
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroups = wGroups;
            ViewBag.OrderNo = OrderNo;
            MaterialPickHeaderSaveModel materialPickHeaderViewModel = PickingFactory.getMaterialPickingHeader(OrderNo);
            return View(materialPickHeaderViewModel);
        }

        public ActionResult AddToolPicking()
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroups = wGroups;
            return View();
        }
        
        public ActionResult ToolPicking()
        {
            return View();
        }

        public ActionResult TransferReturn(string OrderNo,string WGroupId)
        {
            ViewBag.OrderNo = OrderNo;
            ViewBag.WGroupId = WGroupId;
            return View();
        }

        public ActionResult TransferScrap(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;

            return View();
        }

        public ActionResult ToolPickingDetail(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;

            string filePath = Server.MapPath("~") + "\\Attatchment\\ToolPicking\\" + OrderNo ;

            ToolPickHeaderSaveModel materialPickHeaderViewModel = (ToolPickHeaderSaveModel)PickingFactory.getToolPickingHeader(OrderNo);

            materialPickHeaderViewModel.attachments = new List<Attachment>();

            if (Directory.Exists(filePath))
            {
                foreach (string f in Directory.GetFiles(filePath))
                {
                    materialPickHeaderViewModel.attachments.Add(new Attachment
                    {
                        FileName = Path.GetFileName(f),
                    });
                }
            }

            return View(materialPickHeaderViewModel);
        }

        public ActionResult PickingDetail(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;

            string filePath = Server.MapPath("~") + "\\Attatchment\\Picking\\" + OrderNo;

            MaterialPickHeaderSaveModel materialPickHeaderViewModel = PickingFactory.getMaterialPickingHeader(OrderNo);

            materialPickHeaderViewModel.attachments = new List<Attachment>();

            if (Directory.Exists(filePath))
            {
                foreach (string f in Directory.GetFiles(filePath))
                {
                    materialPickHeaderViewModel.attachments.Add(new Attachment
                    {
                        FileName = Path.GetFileName(f),
                    });
                }
            }

            return View(materialPickHeaderViewModel);
        }

        

        public ActionResult AddPickingMaterial(string OrderNo, string SerialNo)
        {
            ViewBag.OrderNo = OrderNo;
            MaterialPickBodyViewModel materialPickBodyViewModel = PickingFactory.getMaterialPickingBodyDetail(OrderNo, SerialNo);
            return View(materialPickBodyViewModel);
        }

        [Authorize]
        public ActionResult MaterialPickingSearch(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList =  WareHouseGroupFactory.getWGroupIdByUser(ID); 
            ISugarQueryable<MaterialPickHeaderViewModel> sugarQueryable = PickingFactory.getMaterialPickingHeader(filter, wgroupIdList, ID);

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

        [Authorize]
        public ActionResult ModifyMaterialPicking(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;

            MaterialPickHeaderViewModel materialPickHeaderViewModel = PickingFactory.getMaterialPickingHeader(OrderNo);
            return View(materialPickHeaderViewModel);
        }

        
        public ActionResult MaterialPickingBodyDetail(string OrderNo,int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<MaterialPickBodyViewModel> sugarQueryable = PickingFactory.getMaterialPickingBodyDetail(filter, OrderNo);

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

        public ActionResult MaterialPickingBodyDetailByOrder(string OrderNo)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<MaterialPickBodyViewModel> sugarQueryable = PickingFactory.getMaterialPickingBodyDetail( OrderNo);

            int Total = sugarQueryable.Count();

            var retObj = new
            {
                data = sugarQueryable.ToList(),
                Total = Total,
                Errors = ""

            };

            return Json(retObj);

        }

        public ActionResult PickingToScrapBodies(string OrderNo)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<PickingToScrapViewModel> sugarQueryable = PickingFactory.PickingToScrapBodies(OrderNo);

            int Total = sugarQueryable.Count();

            var retObj = new
            {
                data = sugarQueryable.ToList(),
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }

        public ActionResult PickingToReturnBodies(string OrderNo)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<PickingToReturnViewModel> sugarQueryable = PickingFactory.PickingToReturnBodies(OrderNo);

            int Total = sugarQueryable.Count();

            var retObj = new
            {
                data = sugarQueryable.ToList(),
                Total = Total,
                Errors = ""

            };

            return Json(retObj);

        }


        [Authorize]
        public ActionResult ToolPickingSearch(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);
            ISugarQueryable<ToolPickHeaderViewModel> sugarQueryable = PickingFactory.getToolPickingHeader(filter, wgroupIdList);

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

        [Authorize]
        public ActionResult ModifyToolPicking(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;

            ToolPickHeaderViewModel materialPickHeaderViewModel = PickingFactory.getToolPickingHeader(OrderNo);
            return View(materialPickHeaderViewModel);
        }

        public ActionResult ToolPickingBodyDetail(string OrderNo, int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<ToolPickBodyViewModel> sugarQueryable = PickingFactory.getToolPickingBodyDetail(filter, OrderNo);

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

        public ActionResult ToolPickingBodyDetailByOrderNo(string OrderNo)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<ToolPickBodyViewModel> sugarQueryable = PickingFactory.getToolPickingBodyDetail( OrderNo);

            int Total = sugarQueryable.Count();

            var retObj = new
            {
                data = sugarQueryable.ToList(),
                Total = Total,
                Errors = ""

            };

            return Json(retObj);

        }

        public ActionResult AddPickingTool(string OrderNo, string SerialNo)
        {
            ViewBag.OrderNo = OrderNo;
            ToolPickBodyViewModel toolPickBodyViewModel = PickingFactory.getToolPickingBodyDetail(OrderNo, SerialNo).Single();
            return View(toolPickBodyViewModel);
        }

        public ActionResult getToolAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\ToolPicking\\" + OrderNo + "\\" + FileName;

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

        public ActionResult getAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Picking\\" + OrderNo + "\\" + FileName;

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