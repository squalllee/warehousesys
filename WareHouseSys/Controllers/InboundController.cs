using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.ComponentModel;
using System.Linq;


namespace WareHouseSys.Controllers
{
    public class InboundController : BaseController
    {
        [HttpPost]
        public ActionResult InboundSearch(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
 
            ISugarQueryable<InboundHeaderViewModel> sugarQueryable = InboundFactory.getInboundHeader(filter, ID);

            int Total = sugarQueryable.Count();

            string sortStr = "";
            
            if(sort != null)
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
        public ActionResult InboundRecordSearch()
        {
            return View();
        }

        public ActionResult InboundRecordSearchGrid([DataSourceRequest] DataSourceRequest request)
        {
            ISugarQueryable<InboundSearchViewModel> sugarQueryable = InboundFactory.getInboundSearchViewModel(request);

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
            List<InboundSearchViewModel> InboundSearchViewModels = null;
            if (request.PageSize == 0)
                InboundSearchViewModels = sugarQueryable.ToList();
            else
                InboundSearchViewModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            string filePath = Server.MapPath("~") + "\\Attatchment\\Inbound\\";

            foreach (InboundSearchViewModel InboundSearchViewModel in InboundSearchViewModels)
            {
                if (Directory.Exists(filePath + InboundSearchViewModel.OrderNo))
                {
                    foreach (string f in Directory.GetFiles(filePath + InboundSearchViewModel.OrderNo))
                    {
                        InboundSearchViewModel.AttUrl = Path.GetFileName(f);
                    }
                }
            }

            request.Page = 1;
            DataSourceResult dataSourceResult = InboundSearchViewModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        [Authorize]
        public ActionResult FillInboundData(InboundHeaderViewModel obj)
        {

            string filePath = Server.MapPath("~") + "\\Attatchment\\Inbound\\" + obj.OrderNo;
            List<Attachment> attachments = new List<Attachment>();

            if (Directory.Exists(filePath))
            {
                foreach (string f in Directory.GetFiles(filePath))
                {
                    attachments.Add(new Attachment
                    {
                        FileName = Path.GetFileName(f),
                    });
                }
            }

            InboundHeaderViewModel inboundHeaderViewModel = InboundFactory.getInboundViewHeader(obj.OrderNo); 
            ViewBag.Attachments = attachments;

            ViewBag.OrderNo = obj.OrderNo;
            return View(inboundHeaderViewModel);
        }

        [HttpPost]
        public ActionResult InboundBodyDetail(int skip, int take, int page, int pageSize,
        string OrderNo, List<SortCriteria> sort = null)
        {
            string ID = HttpContext.User.Identity.Name;
            ISugarQueryable<InboundBodyViewModel> sugarQueryable = InboundFactory.getInboundBody(OrderNo);

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

            List<InboundBodyViewModel> inboundBodyViewModels = sugarQueryable.Skip(skip).Take(take).ToList();
            foreach(InboundBodyViewModel inboundBodyViewModel in inboundBodyViewModels)
            {
                inboundBodyViewModel.Storage = StorageInfoFactory.getStorageSigleInfo(inboundBodyViewModel.StorageId);
                inboundBodyViewModel.OccupiedStorage = StorageInfoFactory.getStorageSigleInfo(inboundBodyViewModel.OccupiedStorageId);
                inboundBodyViewModel.warehouseInfo = WarehouseInfoFactory.getWarehouseInfo(inboundBodyViewModel.WarehouseId);
            }
            var retObj = new
            {
                data = inboundBodyViewModels,
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }

        public ActionResult InboundSearch()
        {
            return View();
        }

        public ActionResult InboundAdd()
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroups = wGroups;
            return View();
        }

        public ActionResult InboundDetail(InboundHeaderViewModel obj)
        {
          
            string filePath = Server.MapPath("~") + "\\Attatchment\\Inbound\\" + obj.OrderNo;
            List<Attachment> attachments = new List<Attachment>();

            if (Directory.Exists(filePath))
            {
                foreach (string f in Directory.GetFiles(filePath))
                {
                    attachments.Add(new Attachment
                    {
                        FileName = Path.GetFileName(f),
                    });
                }
            }

            ViewBag.Attachments = attachments;

            ViewBag.OrderNo = obj.OrderNo;
            return View(obj);
        }


   
        public ActionResult InboundClose(string OrderNo)
        {

            string filePath = Server.MapPath("~") + "\\Attatchment\\Inbound\\" + OrderNo;
            List<Attachment> attachments = new List<Attachment>();

            if (Directory.Exists(filePath))
            {
                foreach (string f in Directory.GetFiles(filePath))
                {
                    attachments.Add(new Attachment
                    {
                        FileName = Path.GetFileName(f),
                    });
                }
            }

            InboundHeaderViewModel inboundHeaderViewModel = InboundFactory.getInboundViewHeader(OrderNo);
            ViewBag.Attachments = attachments;

            ViewBag.OrderNo = OrderNo;
            return View(inboundHeaderViewModel);
        }

        public ActionResult getAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Inbound\\" + OrderNo + "\\" + FileName;

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