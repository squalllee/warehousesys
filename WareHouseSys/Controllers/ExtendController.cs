using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Linq;
using System.ComponentModel;

namespace WareHouseSys.Controllers
{
    public class ExtendController : BaseController
    {

        [Authorize]
        public ActionResult ExtendRecordSearch()
        {
            return View();
        }

        public ActionResult ExtendRecordSearchGrid([DataSourceRequest] DataSourceRequest request)
        {
            ISugarQueryable<ExtendSearchViewModel> sugarQueryable = ExtendFactory.getExtendSearchViewModel(request);

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
            List<ExtendSearchViewModel> extendSearchViewModels = null;
            if (request.PageSize == 0)
                extendSearchViewModels = sugarQueryable.ToList();
            else
                extendSearchViewModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            string filePath = Server.MapPath("~") + "\\Attatchment\\Extend\\";

            foreach (ExtendSearchViewModel extendSearchViewModel in extendSearchViewModels)
            {
                if (Directory.Exists(filePath + extendSearchViewModel.OrderNo))
                {
                    foreach (string f in Directory.GetFiles(filePath + extendSearchViewModel.OrderNo))
                    {
                        extendSearchViewModel.AttUrl = Path.GetFileName(f);
                    }
                }
            }

            request.Page = 1;
            DataSourceResult dataSourceResult = extendSearchViewModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        public ActionResult ExtendLend(string OrderNo)
        {
            List<WGroup> wGroups = WareHouseGroupFactory.getWareHouseGroup();
            ViewBag.WGroups = wGroups;
            LendHeaderViewModel extendHeaderViewModel = LendFactory.getLendHeaderViewModelByLendNo(OrderNo).Single();
            return View(extendHeaderViewModel);
        }

        public ActionResult doExtend(string OrderNo)
        {
            ExtendHeaderViewModel extendHeaderViewModel = ExtendFactory.getExtendHeaderViewModel(OrderNo).Single();
            return View(extendHeaderViewModel);
        }

        public ActionResult ExtendSearch()
        {
            return View();
        }

        public ActionResult ExtendMaterial()
        {
            return View();
        }

        public ActionResult ExtendUpdate(string OrderNo)
        {
            ExtendHeaderViewModel extendHeaderViewModel = ExtendFactory.getExtendHeaderViewModel(OrderNo).Single();
            return View(extendHeaderViewModel);
        }

        public ActionResult ExtendDetail(string OrderNo)
        {
            string filePath = Server.MapPath("~") + "\\Attatchment\\Extend\\" + OrderNo;
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

            ExtendHeaderViewModel extendHeaderView = ExtendFactory.getExtendHeaderViewModel(OrderNo).Single();
            extendHeaderView.attachments = attachments;
            return View(extendHeaderView);
        }

        public ActionResult getExtendHaderViewModel(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<ExtendHeaderViewModel> sugarQueryable = ExtendFactory.getExtendHeaderViewModel(filter, wgroupIdList, ID);

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

        public ActionResult LendBodyWithExtendViewModel(string OrderNo, int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<LendBodiesWithExtendViewModel> sugarQueryable = ExtendFactory.getExtendBodiesWithLend(filter, OrderNo);

            int Total = sugarQueryable.Count();
            if (Total == 0)
                throw new HttpException(911, "此借出單都已歸還無法展延!");

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

        public ActionResult LendBodyWithExtendDetailViewModel(string OrderNo, int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            string ID = HttpContext.User.Identity.Name;
            List<string> wgroupIdList = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<LendBodiesWithExtendViewModel> sugarQueryable = ExtendFactory.getExtendDetailBodiesWithLend(filter, OrderNo);

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
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Extend\\" + OrderNo + "\\" + FileName;

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