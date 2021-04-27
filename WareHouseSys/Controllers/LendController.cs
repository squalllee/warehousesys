using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.ComponentModel;

namespace WareHouseSys.Controllers
{
    public class LendController : BaseController
    {
        public ActionResult LendSearch()
        {
            return View();
        }

        public ActionResult LendMaterial(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            return View();
        }

        public ActionResult LendDetail(string OrderNo)
        {
            string filePath = Server.MapPath("~") + "\\Attatchment\\Lend\\" + OrderNo;
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

            ViewBag.OrderNo = OrderNo;
            LendHeaderViewModel lendHeaderViewModel = LendFactory.getLendHeaderViewModelByLendNo(OrderNo).Single();
            lendHeaderViewModel.attachments = attachments;
            return View(lendHeaderViewModel);
        }

        [Authorize]
        public ActionResult LendRecordSearch()
        {
            return View();
        }
        public ActionResult LendRecordSearchGrid([DataSourceRequest] DataSourceRequest request)
        {
            ISugarQueryable<LendSearchViewModel> sugarQueryable = LendFactory.getLendSearchViewModel(request);

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
            List<LendSearchViewModel> lendSearchViewModels = null;
            if (request.PageSize == 0)
                lendSearchViewModels = sugarQueryable.ToList();
            else
                lendSearchViewModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            string filePath = Server.MapPath("~") + "\\Attatchment\\Lend\\";

            foreach (LendSearchViewModel lendSearchViewModel in lendSearchViewModels)
            {
                if (Directory.Exists(filePath + lendSearchViewModel.OrderNo))
                {
                    foreach (string f in Directory.GetFiles(filePath + lendSearchViewModel.OrderNo))
                    {
                        lendSearchViewModel.AttUrl = Path.GetFileName(f);
                    }
                }
            }

            request.Page = 1;
            DataSourceResult dataSourceResult = lendSearchViewModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }


        public ActionResult getAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Lend\\" + OrderNo + "\\" + FileName;

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

        public ActionResult ModifyLend(string OrderNo)
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroups = wGroups;
            ViewBag.OrderNo = OrderNo;
            LendHeaderViewModel lendHeaderViewModel = LendFactory.getLendHeaderViewModelByLendNo(OrderNo).Single();
            return View(lendHeaderViewModel);
        }

        public ActionResult LendAdd(string OrderNo)
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroups = wGroups;
            LendHeaderViewModel lendHeaderViewModel = LendFactory.getLendHeaderViewModelByLendNo(OrderNo).Single();
            return View(lendHeaderViewModel);
        }

        public ActionResult LendGrid(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<LendHeaderViewModel> sugarQueryable = LendFactory.getLendHeaderViewModel(filter, wgroupIdList, ID);

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

        public ActionResult LendBodyViewModel(string OrderNo, int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<LendBodyViewModel> sugarQueryable = LendFactory.LendBodyViewModel(filter, OrderNo);

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

            List<LendBodyViewModel> transferBodyViewModels = sugarQueryable.Skip(skip).Take(take).ToList();


            var retObj = new
            {
                data = sugarQueryable.Skip(skip).Take(take).ToList(),
                Total = Total,
                Errors = ""

            };

            return Json(retObj);

        }
    }
}