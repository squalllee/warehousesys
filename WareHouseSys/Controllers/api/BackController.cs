using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class BackController : ApiController
    {
        [Route("api/Back/SaveBack")]
        [HttpPost]
        public IHttpActionResult SaveTransfer(BackSaveModel backSaveModel)
        {
            if (BackFactory.saveBack(backSaveModel))
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

        [Route("api/Back/doBack")]
        [HttpPost]
        public IHttpActionResult doBack(BackSaveModel backSaveModel)
        {
            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Back\\" + backSaveModel.backHeaderViewModel.OrderNo;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (Attachment att in backSaveModel.attachment)
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

            if (BackFactory.doBack(backSaveModel.backHeaderViewModel))
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
        
        [Route("api/Back/updateBackQty")]
        [HttpPost]
        public IHttpActionResult updateBackQty(BackBodyViewModel backBodyView)
        {
            if (BackFactory.updateBackQty(backBodyView))
            {
                return Ok(backBodyView);
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
        
        [Route("api/Back/deleteBack/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult deleteBack(string OrderNo)
        {
            if (BackFactory.deleteBack(OrderNo))
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
