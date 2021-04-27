using Models;
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
    public class AdjustController : ApiController
    {
        [Route("api/Adjust/AddAdjust")]
        [HttpPost]
        public IHttpActionResult AddAdjust(AdjustSaveModel obj)
        {
            string ID = User.Identity.Name;

            if (AdjustFactory.AddAdjust(obj, ID))
                return Ok();
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }
        }

        [Route("api/Adjust/addAdjustBody")]
        [HttpPost]
        public IHttpActionResult addAdjustBody(AdjustBody obj)
        {
            string SerialNo = "";

            if (AdjustFactory.AddAdjustBody(obj))
            {
                obj.SerialNo = SerialNo;
                return Ok(obj);
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

        [Route("api/Adjust/updateAdjustBody")]
        [HttpPost]
        public IHttpActionResult updateAdjustBody(AdjustBody obj)
        {
            string ID = User.Identity.Name;

            if (AdjustFactory.UpdateAdjustBody(obj))
                return Ok(obj);
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }
        }

        [Route("api/Adjust/deleteAdjustBody")]
        [HttpPost]
        public IHttpActionResult deleteAdjustBody(AdjustBody obj)
        {
            string ID = User.Identity.Name;

            if (AdjustFactory.DeleteadjustBody(obj))
                return Ok(obj);
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }
        }

        [Route("api/Adjust/SaveAdjustHeader")]
        [HttpPost]
        public IHttpActionResult SaveAdjustHeader(AdjustHeader obj)
        {
            string ID = User.Identity.Name;

            if (AdjustFactory.SaveAdjustHeader(obj))
                return Ok(obj);
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }
        }

        [Route("api/Adjust/CloseAdjust")]
        [HttpPost]
        public IHttpActionResult CloseAdjust(AdjustHeaderViewModel obj)
        {
            string ID = User.Identity.Name;

            if (AdjustFactory.CloseAdjust(obj))
            {
                string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Adjust\\" + obj.OrderNo;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                foreach (Attachment att in obj.attachments)
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
    }
}
