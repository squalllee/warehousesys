using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
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
    public class ReturnController : BaseController
    {
        public ActionResult ReturnSearch()
        {
            
            return View();
        }

        public ActionResult ReturnMaterial()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReturnRecordSearch()
        {
            return View();
        }

        public ActionResult ReturnRecordSearchGrid([DataSourceRequest] DataSourceRequest request)
        {
            ISugarQueryable<ReturnSearchViewModel> sugarQueryable = ReturnFactory.getReturnSearchViewModel(request);

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
            List<ReturnSearchViewModel> returnSearchViewModels = null;
            if (request.PageSize == 0)
                returnSearchViewModels = sugarQueryable.ToList();
            else
                returnSearchViewModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            string filePath = Server.MapPath("~") + "\\Attatchment\\Return\\";

            foreach (ReturnSearchViewModel returnSearchViewModel in returnSearchViewModels)
            {
                if (Directory.Exists(filePath + returnSearchViewModel.OrderNo))
                {
                    foreach (string f in Directory.GetFiles(filePath + returnSearchViewModel.OrderNo))
                    {
                        returnSearchViewModel.AttUrl = Path.GetFileName(f);
                    }
                }
            }

            request.Page = 1;
            DataSourceResult dataSourceResult = returnSearchViewModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        public ActionResult ReturnDetail(string OrderNo)
        {
            string filePath = Server.MapPath("~") + "\\Attatchment\\Return\\" + OrderNo;
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

           
            ReturnHeaderViewModel returnHeader = ReturnFactory.getReturnHeaderViewModelByOrderNo(OrderNo).Single();
            returnHeader.attachments = attachments;
            return View(returnHeader);
        }

        public ActionResult doReturn(string OrderNo)
        {
            ReturnHeaderViewModel returnHeader = ReturnFactory.getReturnHeaderViewModelByOrderNo(OrderNo).Single();
            return View(returnHeader);
        }

        public ActionResult ReturnGrid(string OrderNo, int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<ReturnHeaderViewModel> sugarQueryable = ReturnFactory.getReturnHeaderViewModel(filter, wgroupIdList, ID);

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


        public ActionResult getReturnBodyViewBody(string OrderNo, int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<ReturnBodyViewModel> sugarQueryable = ReturnFactory.getReturnBodyViewModel(OrderNo);

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
        

        public ActionResult ModifyReturn(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            ReturnHeaderViewModel returnHeader = ReturnFactory.getReturnHeaderViewModelByOrderNo(OrderNo).Single();
            return View(returnHeader);
        }

        public ActionResult getAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Return\\" + OrderNo + "\\" + FileName;

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