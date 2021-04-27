using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class EstDemandController : ApiController
    {
        [Route("api/EstDemand/SaveDemand")]
        [HttpPost]
        public IHttpActionResult SaveReqMaterial(EstDemandSaveModel estDemandSaveModel)
        {
            string ID = User.Identity.Name;

            if (DemandFactory.saveDemand(estDemandSaveModel, ID))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }

        }

        [Route("api/EstDemand/ReviewDemand")]
        [HttpPost]
        public IHttpActionResult ReviewDemand(ReqMaterialReviewModel reqMaterialReviewModel)
        {
            string ID = User.Identity.Name;

            if (DemandFactory.closeDemand(reqMaterialReviewModel.OrderNo))
            {
                string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\EstDemand\\" + reqMaterialReviewModel.OrderNo;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                foreach (Attachment att in reqMaterialReviewModel.attachments)
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
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }

        }

        [Route("api/EstDemand/ExcelData")]
        [HttpPost]
        public IHttpActionResult ExcelData(List<string> Orders)
        {
            string ID = User.Identity.Name;

            try
            {
                return Ok(DemandFactory.ExcelData(Orders));
            }
            catch
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(
                  (HttpStatusCode)422,
                  new HttpError("失敗")
              ));
            }

        }


        [Route("api/EstDemand/updateDemandHeader")]
        [HttpPost]
        public IHttpActionResult updateDemandHeader(DemandHeaderViewModel demandHeaderViewModel)
        {
            if (DemandFactory.updateDemandHeader(demandHeaderViewModel))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }

        }

        [Route("api/EstDemand/addDemandBody")]
        [HttpPost]
        public IHttpActionResult addDemandBody(DemandBody demandBody)
        {
            if (DemandFactory.addDemandBody(demandBody))
            {
                return Ok(demandBody);
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }

        }

        [Route("api/EstDemand/updateDemandBody")]
        [HttpPost]
        public IHttpActionResult updateDemandBody(EstDemandBodyViewModel estDemandBodyViewModel)
        {
            if (DemandFactory.updateDemandBody(estDemandBodyViewModel))
            {
                return Ok(estDemandBodyViewModel);
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }

        }

        [Route("api/EstDemand/deleteDemandBody")]
        [HttpPost]
        public IHttpActionResult deleteDemand(EstDemandBodyViewModel estDemandBodyViewModel)
        {
            if (DemandFactory.deleteDemandBody(estDemandBodyViewModel))
            {
                return Ok(estDemandBodyViewModel);
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }

        }

        [Route("api/EstDemand/deleteDemand")]
        [HttpGet]
        public IHttpActionResult deleteDemand(string OrderNo)
        {
            if (DemandFactory.deleteDemand(OrderNo))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }

        }
    }
}
