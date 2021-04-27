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
    public class ExtendController : ApiController
    {
        [Route("api/Extend/SaveExtend")]
        [HttpPost]
        public IHttpActionResult SaveExtend(ExtendSaveModel extendSaveModel)
        {
            if(ExtendFactory.isExtend(extendSaveModel.extendHeaderViewModel.LendNo))
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("此借出單正在展延中，無法再次展延!")
               ));
            }

            if (ExtendFactory.saveExtend(extendSaveModel))
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

        [Route("api/Extend/UpdateExtend")]
        [HttpPost]
        public IHttpActionResult UpdateExtend(ExtendHeaderViewModel extendHeaderViewModel)
        {
           
            if (ExtendFactory.updateExtend(extendHeaderViewModel))
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

        [Route("api/Extend/deleteExtend/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult deleteExtend(string OrderNo)
        {
            if (ExtendFactory.deleteExtend(OrderNo))
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

        [Route("api/Extend/doExtend")]
        [HttpPost]
        public IHttpActionResult doExtend(ExtendSaveModel extendSaveModel)
        {
            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Extend\\" + extendSaveModel.extendHeaderViewModel.OrderNo;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (Attachment att in extendSaveModel.attachment)
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

            if (ExtendFactory.doExtend(extendSaveModel))
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
