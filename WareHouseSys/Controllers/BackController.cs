using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;
using System.ComponentModel;
using System.Linq;

namespace WareHouseSys.Controllers
{
    public class BackController : BaseController
    {

        [Authorize]
        public ActionResult BackRecordSearch()
        {
            return View();
        }

        public ActionResult BackRecordSearchGrid([DataSourceRequest] DataSourceRequest request)
        {
            ISugarQueryable<BackSearchViewModel> sugarQueryable = BackFactory.getBackSearchViewModel(request);

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
            List<BackSearchViewModel> backSearchViewModels = null;
            if (request.PageSize == 0)
                backSearchViewModels = sugarQueryable.ToList();
            else
                backSearchViewModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            string filePath = Server.MapPath("~") + "\\Attatchment\\Back\\";

            foreach (BackSearchViewModel backSearchViewModel in backSearchViewModels)
            {
                if (Directory.Exists(filePath + backSearchViewModel.OrderNo))
                {
                    foreach (string f in Directory.GetFiles(filePath + backSearchViewModel.OrderNo))
                    {
                        backSearchViewModel.AttUrl = Path.GetFileName(f);
                    }
                }
            }

            request.Page = 1;
            DataSourceResult dataSourceResult = backSearchViewModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        public ActionResult BackLend(string OrderNo)
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroups = wGroups;
            LendHeaderViewModel lendHeaderViewModel = LendFactory.getLendHeaderViewModelByLendNo(OrderNo).Single();
            return View(lendHeaderViewModel);
        }

        public ActionResult BackSearch()
        {
            return View();
        }

        public ActionResult BackMaterial()
        {
            return View();
        }

        public ActionResult BackDetail(string OrderNo)
        {
            string filePath = Server.MapPath("~") + "\\Attatchment\\Back\\" + OrderNo;
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

            BackHeaderViewModel backHeaderView = BackFactory.getBackHeaderViewModel(OrderNo).Single();
            backHeaderView.attachments = attachments;

            ViewBag.OrderNo = OrderNo;
            return View(backHeaderView);
        }

        public ActionResult doBack(string OrderNo)
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroups = wGroups;
            ViewBag.OrderNo = OrderNo;
            BackHeaderViewModel backHeaderView = BackFactory.getBackHeaderViewModel(OrderNo).Single();

            return View(backHeaderView);
        }


        public ActionResult ModifyBack(string OrderNo)
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroups = wGroups;
            BackHeaderViewModel backHeaderViewModel = BackFactory.getBackHeaderViewModel(OrderNo).Single();
            ViewBag.OrderNo = OrderNo;
         
            return View(backHeaderViewModel);
        }

        public ActionResult BackBodyViewModel(string OrderNo, int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<BackBodyViewModel> sugarQueryable = BackFactory.getBackBodies(filter, OrderNo);

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
        
        public ActionResult LendBodyWithBackViewModel(string OrderNo,int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<LendBodiesWithBackViewModel> sugarQueryable = BackFactory.getBackBodiesWithLend(filter, OrderNo);

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

        public ActionResult getBackHaderViewModel(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<BackHeaderViewModel> sugarQueryable = BackFactory.getBackHeaderViewModel(ID,filter, wgroupIdList);

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

        public ActionResult getAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Back\\" + OrderNo + "\\" + FileName;

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