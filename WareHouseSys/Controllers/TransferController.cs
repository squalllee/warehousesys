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
    public class TransferController : BaseController
    {
      
        public ActionResult TransferEstablish()
        {
            return View();
        }

        [Authorize]
        public ActionResult TransferRecordSearch()
        {
            return View();
        }

        public ActionResult TransferRecordSearchGrid([DataSourceRequest] DataSourceRequest request)
        {
            ISugarQueryable<TransferSearchViewModel> sugarQueryable = TransferFactory.getTransferSearchViewModel(request);

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
            List<TransferSearchViewModel> transferSearchViewModels = null;
            if (request.PageSize == 0)
                transferSearchViewModels = sugarQueryable.ToList();
            else
                transferSearchViewModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            string filePath = Server.MapPath("~") + "\\Attatchment\\Transfer\\";

            foreach (TransferSearchViewModel transferSearchViewModel in transferSearchViewModels)
            {
                if (Directory.Exists(filePath + transferSearchViewModel.OrderNo))
                {
                    foreach (string f in Directory.GetFiles(filePath + transferSearchViewModel.OrderNo))
                    {
                        transferSearchViewModel.AttUrl = Path.GetFileName(f);
                    }
                }
            }

            request.Page = 1;
            DataSourceResult dataSourceResult = transferSearchViewModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }
        public ActionResult TransferEstablishAdd(string OrderNo)
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroup = wGroups;

            TransferHeaderViewModel transferHeaderViewModel = TransferFactory.getTransferHeaderViewModel(OrderNo);

            return View(transferHeaderViewModel);
        }

        public ActionResult TransferSearch(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<TransferHeaderViewModel> sugarQueryable = TransferFactory.getTransferHeaderViewModel(filter, ID);

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

        public ActionResult TransferInSearchGrid(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<TransferHeaderViewModel> sugarQueryable = TransferFactory.getTransferInHeaderViewModel(filter, ID);

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

        public ActionResult TransferBodyViewModel(string OrderNo,int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<TransferBodyViewModel> sugarQueryable = TransferFactory.getTransferBodyViewModel(filter,OrderNo);

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

            List<TransferBodyViewModel> transferBodyViewModels = sugarQueryable.Skip(skip).Take(take).ToList();


            var retObj = new
            {
                data = sugarQueryable.Skip(skip).Take(take).ToList(),
                Total = Total,
                Errors = ""

            };

            return Json(retObj);

        }

        public ActionResult TransferOutSearch()
        {
            return View();

        }

        public ActionResult TransferInSearch()
        {
            return View();

        }

        

        public ActionResult TransferOut(string OrderNo)
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroup = wGroups;

            TransferHeaderViewModel transferHeaderViewModel = TransferFactory.getTransferHeaderViewModel(OrderNo);

            return View(transferHeaderViewModel);

        }

        public ActionResult TransferIn(string OrderNo)
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroup = wGroups;

            TransferHeaderViewModel transferHeaderViewModel = TransferFactory.getTransferHeaderViewModel(OrderNo);

            return View(transferHeaderViewModel);

        }

        public ActionResult TransferDetail(string OrderNo)
        {
            string filePath = Server.MapPath("~") + "\\Attatchment\\Transfer\\" + OrderNo;
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
            TransferHeaderViewModel transferHeaderViewModel = TransferFactory.getTransferHeaderViewModel(OrderNo);

            return View(transferHeaderViewModel);
        }

        public ActionResult getAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Transfer\\" + OrderNo + "\\" + FileName;

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