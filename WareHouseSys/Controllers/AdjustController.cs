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
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers
{
    public class AdjustController : BaseController
    {

        public ActionResult AdjustApplySearch()
        {
            return View();
        }

        public ActionResult AddAdJust(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            AdjustHeaderViewModel scrapHeaderViewModel = AdjustFactory.getAdjustHeaderViewModel(OrderNo);
            return View(scrapHeaderViewModel);
        }

        public ActionResult doAdjustSearch()
        {
            return View();
        }

        public ActionResult AdjustClose(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            AdjustHeaderViewModel scrapHeaderViewModel = AdjustFactory.getAdjustHeaderViewModel(OrderNo);
            return View(scrapHeaderViewModel);
        }

        public ActionResult AdjustHeaderGridView([DataSourceRequest]DataSourceRequest request)
        {
            string ID = HttpContext.User.Identity.Name;

            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<AdjustHeaderViewModel> sugarQueryable = AdjustFactory.getAdjustHeaderViewModel(request, wgroupIdList,ID);

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

            List<AdjustHeaderViewModel> scrapHeaderViewModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            request.Page = 1;
            DataSourceResult dataSourceResult = scrapHeaderViewModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        public ActionResult AdjustBodyViewGrid(string OrderNo, int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<AdjustBodyViewModel> sugarQueryable = AdjustFactory.getAdjustBodyViewModel(filter, OrderNo);

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

        public ActionResult AdjustDetail(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;

            string filePath = Server.MapPath("~") + "\\Attatchment\\Adjust\\" + OrderNo;

            AdjustHeaderViewModel adjustHeaderViewModel = AdjustFactory.getAdjustHeaderViewModel(OrderNo);

            adjustHeaderViewModel.attachments = new List<Attachment>();

            if (Directory.Exists(filePath))
            {
                foreach (string f in Directory.GetFiles(filePath))
                {
                    adjustHeaderViewModel.attachments.Add(new Attachment
                    {
                        FileName = Path.GetFileName(f),
                    });
                }
            }

            return View(adjustHeaderViewModel);
        }

        public ActionResult getAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Adjust\\" + OrderNo + "\\" + FileName;

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