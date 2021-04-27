using System;
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
    public class ReqMaterialInfoController : ApiController
    {
        [Route("api/ReqMaterialInfo/SaveReqMaterial")]
        [HttpPost]
        public IHttpActionResult SaveReqMaterial(ReqMaterialSaveModel reqMaterialSaveModel)
        {
            string ID = User.Identity.Name;

            if (ReqMaterialFactory.saveReqMaterial(reqMaterialSaveModel,ID))
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

        [Route("api/ReqMaterialInfo/updateReqMaterialHeader")]
        [HttpPost]
        public IHttpActionResult updateReqMaterialHeader(ReqMaterialHeader reqMaterialHeader)
        {
            string ID = User.Identity.Name;

            if (ReqMaterialFactory.updateReqMaterialHeader(reqMaterialHeader, ID))
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

        [Route("api/ReqMaterialInfo/closeReqMaterial")]
        [HttpPost]
        public IHttpActionResult closeReqMaterial(ReqMaterialReviewModel reqMaterialReviewModel)
        {
            string ID = User.Identity.Name;

            if (ReqMaterialFactory.closeReqMaterial(reqMaterialReviewModel,ID))
            {
                string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\ReqMaterial\\" + reqMaterialReviewModel.OrderNo;
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
        
        [Route("api/ReqMaterialInfo/addReqMaterial")]
        [HttpPost]
        public IHttpActionResult addReqMaterial(ReqMaterialBody reqMaterialBody)
        {
            string ID = User.Identity.Name;

            if (ReqMaterialFactory.addReqMaterial(reqMaterialBody))
            {
                return Ok(reqMaterialBody);
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

        [Route("api/ReqMaterialInfo/updateReqMaterial")]
        [HttpPost]
        public IHttpActionResult updateReqMaterial(ReqMaterialBody reqMaterialBody)
        {

            if (ReqMaterialFactory.updateReqMaterial(reqMaterialBody))
            {
                return Ok(reqMaterialBody);
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

        [Route("api/ReqMaterialInfo/deleteReqMaterial")]
        [HttpPost]
        public IHttpActionResult deleteReqMaterial(ReqMaterialBody reqMaterialBody)
        {

            if (ReqMaterialFactory.deleteReqMaterial(reqMaterialBody))
            {
                return Ok(reqMaterialBody);
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

        //[Route("api/ReqMaterialInfo/GetReqMaterialInfo")]
        //[HttpPost]
        //public IHttpActionResult GetReqMaterialInfo(string text)
        //{

        //    if (ReqMaterialFactory.deleteReqMaterial(reqMaterialBody))
        //    {
        //        return Ok(reqMaterialBody);
        //    }
        //    else
        //    {
        //        return new System.Web.Http.Results.ResponseMessageResult(
        //       Request.CreateErrorResponse(
        //           (HttpStatusCode)422,
        //           new HttpError("失敗")
        //       ));
        //    }

        //}
    }
}
