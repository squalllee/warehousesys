using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers
{
    public class ToolInventoryController : BaseController
    {

        public ActionResult InventoryBodies(string UnitId,int skip, int take, int page, int pageSize,
         List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            ISugarQueryable<ToolInventoryViewModel> sugarQueryable = ToolInventoryFactory.getToolInventoryViewModel(filter, UnitId);

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


    }
}