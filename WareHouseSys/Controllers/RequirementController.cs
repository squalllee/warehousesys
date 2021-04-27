using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers
{
    public class RequirementController : BaseController
    {
        public ActionResult RequirementSearch()
        {
            return View();
        }

        public ActionResult RequirementDetail(string OrderNo)
        {
            RequirementHeaderViewModel requirementHeaderViewModel = RequirementFactory.getRequirementHeaderViewModel(OrderNo);
            string filePath = Server.MapPath("~") + "\\Attatchment\\Requirement\\" + OrderNo;
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

            ViewBag.Attachments = attachments;

            ViewBag.OrderNo = OrderNo;
            return View(requirementHeaderViewModel);
        }

        public ActionResult RequirementAdd()
        {
            List<Employee> employees = EmployeeFactory.getAllEmployee();
            List<UNIT> uNITs = UnitFactory.getAllUint();

            RequireAddViewModel requireAddViewModel = new RequireAddViewModel
            {
                Users = employees,
                Units = uNITs
            };
            return View(requireAddViewModel);
        }

        public ActionResult RequirementAddDetail()
        {
            List<UNIT> uNITs = UnitFactory.getWareHouseUint();
            return View(uNITs);
        }

        public ActionResult RequirementUpdate(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            RequirementHeader requirementHeader = RequirementFactory.getRequirementHeader(OrderNo);
            Employee employee = EmployeeFactory.getEmployee(requirementHeader.Applicant);
            UNIT uNIT = UnitFactory.getUint(employee.UNITNO.Trim());

            ViewBag.Applicant = employee.TMNAME.Trim() + "[" + employee.KEYNO.Trim() + "]";
            ViewBag.ApplicantUnit = uNIT.UNITNAME.Trim() + "["+uNIT.UNITNO+"]";
            ViewBag.FillDate = DateTime.Parse(requirementHeader.AddDateTime.ToString()).ToString("yyyy/MM/dd");
            return View(requirementHeader);
        }

        public ActionResult RequirementDetailUpdate(string OrderNo,string SerialNo)
        {
            ViewBag.OrderNo = OrderNo;

            RequirementDetailUpdateModel requirementDetailUpdateModel = RequirementFactory.getRequirementBody(OrderNo, SerialNo);

            requirementDetailUpdateModel.uNIT = UnitFactory.getWareHouseUint();

            return View(requirementDetailUpdateModel);
        }

        public ActionResult RequirementClose(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            RequirementHeader requirementHeader = RequirementFactory.getRequirementHeader(OrderNo);
            Employee employee = EmployeeFactory.getEmployee(requirementHeader.Applicant);
            UNIT uNIT = UnitFactory.getUint(employee.UNITNO.Trim());

            ViewBag.Applicant = employee.TMNAME.Trim() + "[" + employee.KEYNO.Trim() + "]";
            ViewBag.ApplicantUnit = uNIT.UNITNAME.Trim() + "[" + uNIT.UNITNO + "]";
            ViewBag.FillDate = DateTime.Parse(requirementHeader.AddDateTime.ToString()).ToString("yyyy/MM/dd");
            return View();
        }

        public ActionResult getTransToPurInfo(string requireNo)
        {
            List<TransToPurViewModel> transToPurViewModels =  RequirementFactory.getTransToPurInfo(requireNo);

            var retObj = new
            {
                data = transToPurViewModels,
                Total = transToPurViewModels.Count,
                Errors = ""
            };

            return Json(retObj);
        }

        public ActionResult getAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Requirement\\" + OrderNo + "\\" + FileName;

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