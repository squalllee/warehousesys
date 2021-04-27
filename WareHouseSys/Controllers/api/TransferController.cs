using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class TransferController : ApiController
    {
        [Route("api/Transfer/SaveTransfer")]
        [HttpPost]
        public IHttpActionResult SaveTransfer(TransferSaveModel transferSaveModel)
        {
            if(TransferFactory.saveTransfer(transferSaveModel))
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

        [Route("api/Transfer/UpdateTransferHeader")]
        [HttpPost]
        public IHttpActionResult UpdateTransferHeader(TransferSaveModel transferSaveModel)
        {
            if (TransferFactory.updateTransferHeader(transferSaveModel))
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

        [Route("api/Transfer/UpdateOutTransferHeader")]
        [HttpPost]
        public IHttpActionResult UpdateOutTransferHeader(TransferSaveModel transferSaveModel)
        {
            if (TransferFactory.updateTransferOutHeader(transferSaveModel))
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

        [Route("api/Transfer/UpdateInTransferHeader")]
        [HttpPost]
        public IHttpActionResult UpdateInTransferHeader(TransferSaveModel transferSaveModel)
        {
            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Transfer\\" + transferSaveModel.OrderNo;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (Attachment att in transferSaveModel.attachment)
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

            if (TransferFactory.updateTransferInHeader(transferSaveModel))
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


        [Route("api/Transfer/TransferEstablishUpdate")]
        [HttpPost]
        public IHttpActionResult TransferEstablishUpdate(TransferBodyViewModel transferBodyViewModel)
        {
            if (TransferFactory.updateTransfer(transferBodyViewModel))
            {
                return Ok(transferBodyViewModel);
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

        [Route("api/Transfer/TransferEstablishDelete")]
        [HttpPost]
        public IHttpActionResult TransferEstablishDelete(TransferBodyViewModel transferBodyViewModel)
        {
            if (TransferFactory.deleteTransfer(transferBodyViewModel))
            {
                return Ok(transferBodyViewModel);
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

        [Route("api/Transfer/TransferEstablishAdd")]
        [HttpPost]
        public IHttpActionResult TransferEstablishAdd(TransferBodyViewModel transferBodyViewModel)
        {
            if (TransferFactory.addTransfer(transferBodyViewModel))
            {
                return Ok(transferBodyViewModel);
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
        
        [Route("api/Transfer/TransferOutAdd")]
        [HttpPost]
        public IHttpActionResult TransferOutAdd(TransferBodyViewModel transferBodyViewModel)
        {
            if (TransferFactory.addTransferOut(transferBodyViewModel))
            {
                return Ok(transferBodyViewModel);
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

        [Route("api/Transfer/TransferInAdd")]
        [HttpPost]
        public IHttpActionResult TransferInAdd(TransferBodyViewModel transferBodyViewModel)
        {
            if (TransferFactory.addTransferIn(transferBodyViewModel))
            {
                return Ok(transferBodyViewModel);
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

        [Route("api/Transfer/TransferOutUpdate")]
        [HttpPost]
        public IHttpActionResult TransferOutUpdate(TransferBodyViewModel transferBodyViewModel)
        {
            if (TransferFactory.TransferOutUpdate(transferBodyViewModel))
            {
                return Ok(transferBodyViewModel);
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

        [Route("api/Transfer/TransferInUpdate")]
        [HttpPost]
        public IHttpActionResult TransferInUpdate(TransferBodyViewModel transferBodyViewModel)
        {
            if (TransferFactory.TransferInUpdate(transferBodyViewModel))
            {
                return Ok(transferBodyViewModel);
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

        [Route("api/Transfer/deleteTransferBody")]
        [HttpPost]
        public IHttpActionResult deleteTransferBody(TransferBodyViewModel transferBodyViewModel)
        {
            if (TransferFactory.deleteTransferBody(transferBodyViewModel))
            {
                return Ok(transferBodyViewModel);
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

        [Route("api/Transfer/deleteTransfer/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult deleteTransfer(string OrderNo)
        {
            string ID = User.Identity.Name;

            if (TransferFactory.scrapTransfer(OrderNo))
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


    }
}
