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
    public class ReturnController : ApiController
    {
        [Route("api/Return/updateReturnHeader")]
        [HttpPost]
        public IHttpActionResult updateReturnHeader(ReturnHeader returnHeader)
        {
            if (ReturnFactory.updateReturnHeader(returnHeader))
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

        [Route("api/Return/updateBody")]
        [HttpPost]
        public IHttpActionResult updateBody(ReturnBodyViewModel returnBodyView)
        {
            if(ReturnFactory.updateReturnBody(returnBodyView))
            {
                return Ok(returnBodyView);
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

        [Route("api/Return/updateReturnBodyReturnQty")]
        [HttpPost]
        public IHttpActionResult updateReturnBodyReturnQty(ReturnBodyViewModel returnBodyView)
        {
            if (ReturnFactory.updateReturnBodyReturnQty(returnBodyView))
            {
                return Ok(returnBodyView);
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

        
        [Route("api/Return/deleteBody")]
        [HttpPost]
        public IHttpActionResult deleteBody(ReturnBodyViewModel returnBodyView)
        {
            if (ReturnFactory.deleteReturnBody(returnBodyView))
            {
                return Ok(returnBodyView);
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

        [Route("api/Return/DeleteReturn/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult DeleteReturn(string OrderNo)
        {
            if (ReturnFactory.DeleteReturn(OrderNo))
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

        [Route("api/Return/doReturn")]
        [HttpPost]
        public IHttpActionResult doReturn(ReturnHeaderViewModel returnHeader)
        {
            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Return\\" + returnHeader.OrderNo;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (Attachment att in returnHeader.attachments)
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

            if (ReturnFactory.doReturn(returnHeader))
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
