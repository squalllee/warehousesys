using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers
{
    public class MaterialController : Controller
    {

        public ActionResult MaterialInfo(string MaterialNo)
        {
            MaterialInfoViewModel materialInfoViewModel = MaterialFactory.getMaterialViewModelInfo(MaterialNo);
            return View(materialInfoViewModel);
        }

        public ActionResult getMaterialInfoByWareHouseId(string WareHouseId,int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {
            ISugarQueryable<MaterialComboViewModel> sugarQueryable = MaterialFactory.getMaterialInfoByWareHouseId(WareHouseId);

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


        public ActionResult getMaterialInfoKendo(string filter)
        {
            ISugarQueryable<MaterialComboViewModel> sugarQueryable = MaterialFactory.getMaterialInfoAll();

            sugarQueryable.Where(e => e.MaterialNo.Contains(filter) || e.MaterialName.Contains(filter));
            return Json(sugarQueryable.ToList(),JsonRequestBehavior.AllowGet);
        }

        public ActionResult MaterialImage(string MaterialNo)
        {
            string filePath = Server.MapPath("~") + "\\Picture\\"+ MaterialNo;

           string fileNames = "";

            if(Directory.Exists(filePath))
            {
                foreach (string file in Directory.GetFiles(filePath))
                {
                    fileNames += Path.GetFileName(file) + ",";

                }
            }    
           

            ViewBag.fileNames = fileNames.TrimEnd(',');
            return View();
        }

        public ActionResult MaterialImagUpload(List<HttpPostedFileBase> files, string MaterialNo)
        {
        
            string filePath = Server.MapPath("~") + "\\Picture\\"+ MaterialNo + "\\";

            foreach(HttpPostedFileBase httpPostedFileBase in files)
            {
                if(!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                httpPostedFileBase.SaveAs(filePath + httpPostedFileBase.FileName);
                // httpPostedFileBase.SaveAs(filePath + MaterialNo + "-" + (Directory.GetFiles(filePath).Length + 1).ToString() + Path.GetExtension(httpPostedFileBase.FileName));
            }
            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult MaterialImagDelete(string MaterialNo, string FileName)
        {

            string filePath = Server.MapPath("~") + "\\Picture\\" + MaterialNo + "\\";

            System.IO.File.Delete(filePath + FileName);

         
            return Content("");
        }
        
    }
}