using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers
{
    public class SearchController : BaseController
    {
        public ActionResult TotalStock()
        {
            return View();
        }

        public ActionResult MaterialInfoSearch() {
            return View();
        }

        public ActionResult MaterialInfoGrid([DataSourceRequest]DataSourceRequest request)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<MaterialInfoViewModel> sugarQueryable = MaterialFactory.getMaterialViewModelInfo(request);

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

            List<MaterialInfoViewModel> materialInfoViewModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            request.Page = 1;
            DataSourceResult dataSourceResult = materialInfoViewModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        public ActionResult TotalStockGrid([DataSourceRequest]DataSourceRequest request)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<TotalStockViewModel> sugarQueryable = StockFactory.getTotalStock(request);

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

            List<TotalStockViewModel> totalStockViewByWareHouseModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            request.Page = 1;
            DataSourceResult dataSourceResult = totalStockViewByWareHouseModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        public ActionResult TotalStockByLotGrid([DataSourceRequest]DataSourceRequest request)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<TotalStockByLotViewModel> sugarQueryable = StockFactory.getTotalStockByLot(request);

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

            List<TotalStockByLotViewModel> totalStockViewByWareHouseModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            request.Page = 1;
            DataSourceResult dataSourceResult = totalStockViewByWareHouseModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        public ActionResult TotalStockByWareHouseGrid([DataSourceRequest]DataSourceRequest request)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<TotalStockViewByWareHouseModel> sugarQueryable = StockFactory.getTotalStockByWareHouse(request);

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

            List<TotalStockViewByWareHouseModel> totalStockViewByWareHouseModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            request.Page = 1;
            DataSourceResult dataSourceResult = totalStockViewByWareHouseModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        public ActionResult TotalStockByWareHouseAndLotGrid([DataSourceRequest]DataSourceRequest request)
        {
            string ID = HttpContext.User.Identity.Name;

            ISugarQueryable<TotalStockByWareHouseAndLotViewModel> sugarQueryable = StockFactory.getTotalStockByWareHouseAndLot(request);

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

            List<TotalStockByWareHouseAndLotViewModel> totalStockViewByWareHouseModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            request.Page = 1;
            DataSourceResult dataSourceResult = totalStockViewByWareHouseModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }


    }
}