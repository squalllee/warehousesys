using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;

namespace WareHouseSys.Controllers
{
    public class MaterialBasicInfomationController : BaseController
    {
        // GET: MaterialBasicInfomation
        public ActionResult MaterialMaintain()
        {
            return View();
        }

        public ActionResult getMaterialInfo([DataSourceRequest]DataSourceRequest request)
        {
            ISugarQueryable<MaterialInfo> sugarQueryable =  MaterialFactory.getMaterialBasicInfo(request);


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

            List<MaterialInfo> materialInfos = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            return Json(materialInfos.ToDataSourceResult(request));
        }

        public ActionResult addMaterialInfo(MaterialInfo materialInfo, [DataSourceRequest] DataSourceRequest request)
        {
            if (MaterialFactory.addMaterialBasicInfo(materialInfo))
            {
                return Json(new[] { materialInfo }.ToDataSourceResult(request));
            }
            else
            {
                Response.StatusCode = 400;
                //設定TrySkipIisCustomErrors，停用IIS自訂錯誤頁面
                Response.TrySkipIisCustomErrors = true;
                return Content(
                    "{ \"errors\": \"發生錯誤！\" }", "application/json");
            }
        }

        public ActionResult updateMaterialInfo(MaterialInfo materialInfo, [DataSourceRequest] DataSourceRequest request)
        {
            if (MaterialFactory.updateMaterialBasicInfo(materialInfo))
            {
                return Json(new[] { materialInfo }.ToDataSourceResult(request));
            }
            else
            {
                Response.StatusCode = 400;
                //設定TrySkipIisCustomErrors，停用IIS自訂錯誤頁面
                Response.TrySkipIisCustomErrors = true;
                return Content(
                    "{ \"errors\": \"發生錯誤！\" }", "application/json");
            }
        }

        public ActionResult deleteMaterialInfo(MaterialInfo materialInfo, [DataSourceRequest] DataSourceRequest request)
        {
            if (MaterialFactory.deleteMaterialBasicInfo(materialInfo))
            {
                return Json(new[] { materialInfo }.ToDataSourceResult(request));
            }
            else
            {
                Response.StatusCode = 400;
                //設定TrySkipIisCustomErrors，停用IIS自訂錯誤頁面
                Response.TrySkipIisCustomErrors = true;
                return Content(
                    "{ \"errors\": \"發生錯誤！\" }", "application/json");
            }
        }

    }
}