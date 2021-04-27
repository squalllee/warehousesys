using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers
{
    public class ChemicalController : BaseController
    {
        public ActionResult ChemicalMaintain()
        {
            return View();
        }

        public ActionResult SDSUpload(HttpPostedFileBase SDSfile,string MaterialNo)
        {
            if (MaterialNo == "") return Content("error");
            string filePath = Server.MapPath("~") + "\\Attatchment\\SDS\\" + MaterialNo + ".pdf";

            SDSfile.SaveAs(filePath);

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult SDSRemove( string MaterialNo)
        {

            string filePath = Server.MapPath("~") + "\\Attatchment\\SDS\\" + MaterialNo + ".pdf";

            System.IO.File.Delete(filePath);

            // Return an empty string to signify success
            return Content("");
        }

        

        public ActionResult ChemicalGrid(int skip, int take, int page, int pageSize,
        List<SortCriteria> sort = null, FilterCriteria filter = null)
        {

            ISugarQueryable<ChemicalDataViewModel> sugarQueryable = ChemicalFactory.getChemicalDataViewModel(filter);

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

            List<ChemicalDataViewModel> chemicalDataViewModels = sugarQueryable.Skip(skip).Take(take).ToList();

            string filePath = Server.MapPath("~") + "\\Attatchment\\SDS\\";
            foreach (ChemicalDataViewModel chemicalDataViewModel in chemicalDataViewModels)
            {
                if(System.IO.File.Exists(filePath + chemicalDataViewModel.MaterialNo + ".pdf"))
                {
                    chemicalDataViewModel.SDSFile = chemicalDataViewModel.MaterialNo + ".pdf";
                }
                else
                {
                    chemicalDataViewModel.SDSFile = "";
                }
            }

            var retObj = new
            {
                data = chemicalDataViewModels,
                Total = Total,
                Errors = ""

            };

            return Json(retObj);
        }

        public ActionResult getSDSAttatchment(string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\SDS\\"  + FileName;

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