
using Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Hosting;
using System.Web.Http;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class RequirementController : ApiController
    {
        [Route("api/Requirement/getRequirementHeader")]
        [HttpPost]
        public IHttpActionResult Post(FormDataCollection form)
        {

            string ID = User.Identity.Name;

            string RequireNo = form.Get("RequireNo");
            string RequireDateStart = form.Get("RequireDateStart");
            string RequireDateEnd = form.Get("RequireDateEnd");

            int draw = int.Parse(form.Get("draw"));
            int start = int.Parse(form.Get("start"));
            int length = int.Parse(form.Get("length"));

            string col_index = form.Get("order[0][column]");
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : form.Get($@"columns[{col_index}][data]");
            string asc_desc = string.IsNullOrEmpty(form.Get("order[0][dir]")) ? "desc" : form.Get("order[0][dir]");//防呆

            List<Employee> employees = EmployeeFactory.getAllEmployee();
            List<UNIT> Units = UnitFactory.getAllUint();

            ISugarQueryable<RequirementHeader> sugarQueryable = RequirementFactory.getRequirementHeader(RequireNo, RequireDateStart, RequireDateEnd,ID);

           
            sugarQueryable = sugarQueryable.OrderBy($@"{sortColName} {asc_desc}");

            List<RequirementHeader> requirementHeaders = sugarQueryable.Skip(start).Take(length).ToList();

            foreach (RequirementHeader requirement in requirementHeaders)
            {
                requirement.Applicant = employees.Where(e => e.KEYNO == requirement.Applicant).Select(e => e.TMNAME).SingleOrDefault();


                if (requirement.Status == "0") requirement.Status = "辦理中";
                else if (requirement.Status == "1") requirement.Status = "已結案";
                else if (requirement.Status == "2") requirement.Status = "已轉採購單(開口契約)";
                else if (requirement.Status == "3") requirement.Status = "已轉採購單";
            }

            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = sugarQueryable.Count(),
                  recordsFiltered = sugarQueryable.Count(),
                  data = requirementHeaders
              };

            return Ok(returnObj);

        }

        [Route("api/Requirement/getReqBody/{OrderNo}")]
        [HttpPost]
        public IHttpActionResult getReqBody(string OrderNo, FormDataCollection form)
        {
            int draw = int.Parse(form.Get("draw"));
            int start = int.Parse(form.Get("start"));
            int length = int.Parse(form.Get("length"));

            string col_index = form.Get("order[0][column]");
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : form.Get($@"columns[{col_index}][data]");
            string asc_desc = string.IsNullOrEmpty(form.Get("order[0][dir]")) ? "desc" : form.Get("order[0][dir]");//防呆

            ISugarQueryable<RequirementBody> sugarQueryable = RequirementFactory.getRequirementBody(OrderNo);
            sugarQueryable.OrderBy(String.Format("{0} {1}", sortColName, asc_desc));
            List<RequirementBody> reqs = sugarQueryable.Skip(start).Take(length).ToList();

            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = sugarQueryable.Count(),
                  recordsFiltered = sugarQueryable.Count(),
                  data = reqs
              };

           
            return Ok(returnObj);
        }

        [Route("api/Requirement/CreatePur/{RequireNo}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult CreatePur(string RequireNo)
        {
            string ID = User.Identity.Name;

            return Ok(UtilityFactory.CreatePUR(RequireNo, ID));
        }
        
        [Route("api/Requirement/UpdateRequirementDetail/")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateRequirementDetail(RequirementBody updateModel)
        {
                    

            if (RequirementFactory.updateRequiremenetDetail(updateModel))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(
                  (HttpStatusCode)422,
                  new HttpError("新增失敗!")

              ));
            }

        }
        
        [Route("api/Requirement/DeleteRequirement")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult DeleteRequirement(string OrderNo)
        {
            if (RequirementFactory.deleteRequiremenet(OrderNo))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(
                  (HttpStatusCode)422,
                  new HttpError("新增失敗!")

              ));
            }

        }

        [Route("api/Requirement/updateRequiremenetHeader/")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult updateRequiremenetHeader(RequirementHeader requirementHeader)
        {
            if (RequirementFactory.updateRequiremenetHeader(requirementHeader))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(
                  (HttpStatusCode)422,
                  new HttpError("新增失敗!")

              ));
            }

        }

        [Route("api/Requirement/SaveRequirement/")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult SaveRequirement(RequirementSaveModel saveModel)
        {
            string OrderNo = RequirementFactory.getOrderNo();
            int serialNo = 1;
            OrderNo = OrderNo.Split('-')[0] + "-" + (int.Parse(OrderNo.Split('-')[1]) + 1).ToString("0000");
            saveModel.requirementHeader.OrderNo = OrderNo;
            saveModel.requirementHeader.UpdateDateTime = DateTime.Now;
            saveModel.requirementHeader.Status = "0";
            saveModel.requirementHeader.AddDateTime = DateTime.Now;
           

            foreach (RequirementBody requirementBody in saveModel.requirementBodies)
            {
                requirementBody.OrderNo = OrderNo;
                requirementBody.SerialNo = serialNo.ToString("0000");
                requirementBody.RequireDate = requirementBody.PeriodStart1;
                serialNo++;
            }

            if (RequirementFactory.saveRequiremenet(saveModel))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(
                  (HttpStatusCode)422,
                  new HttpError("新增失敗!")

              ));
            }
        }

        [Route("api/Requirement/uploadRequireAtt/")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult uploadRequireAtt(List<Attachment> atts,string OrderNo)
        {
            RequirementHeader requirementHeader = RequirementFactory.getRequirementHeader(OrderNo);
            requirementHeader.Status = "1";
            if(!RequirementFactory.updateRequirementHeader(requirementHeader))
            {
                return NotFound();
            }

            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Requirement\\" + OrderNo;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (Attachment att in atts)
            {
                try
                {
                    File.WriteAllBytes(filePath + "\\" + att.FileName, Convert.FromBase64String(att.Content));
                }
                catch
                {
                    return NotFound();
                }
            }
            return Ok();
        }

        [Route("api/Requirement/SaveRequirementDetail/")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult SaveRequirementDetail(RequirementBody saveModel)
        {
            if (RequirementFactory.saveRequiremenetDetail(saveModel))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(
                  (HttpStatusCode)422,
                  new HttpError("新增失敗!")

              ));
            }

        }

        [Route("api/Requirement/DeleteRequirementDetail/")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult DeleteRequirementDetail(string OrderNo,string MaterialNo,string SerialNo)
        {
            if (RequirementFactory.deleteRequiremenetDetail(OrderNo, MaterialNo, SerialNo))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(
                  (HttpStatusCode)422,
                  new HttpError("新增失敗!")

              ));
            }

        }


        [Route("api/Requirement/getTransToPurInfo/{requireNo}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult getTransToPurInfo(string requireNo)
        {
            try
            {
                return Ok(RequirementFactory.getTransToPurInfo(requireNo));
            }
            catch
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(
                  (HttpStatusCode)422,
                  new HttpError("新增失敗!")

              ));
            }

        }


        [Route("api/Requirement/TransToPur")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult TransToPur(TransToPurSaveModel saveObj)
        {
            
            try
            {
                if (RequirementFactory.TransToPur(saveObj))
                {
                    return Ok();
                }
                else
                {
                    return new System.Web.Http.Results.ResponseMessageResult(
                      Request.CreateErrorResponse(
                          (HttpStatusCode)422,
                          new HttpError("新增失敗!")

                      ));
                }
                //return Ok(RequirementFactory.TransToPur(saveObj));
            }
            catch(Exception ex)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(
                  (HttpStatusCode)422,
                  new HttpError(ex.Message)

              ));
            }

        }

    }
}
