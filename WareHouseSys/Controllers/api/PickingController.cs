using System;
using System.Collections.Generic;
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
    public class PickingController : ApiController
    {
        [Route("api/Picking/UpdateMaterialPickingBody")]
        [HttpPost]
        public IHttpActionResult UpdateMaterialPickingBody(MaterialPickBodyViewModel obj)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.updatePickingBody(obj, ID))
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

        [Route("api/Picking/deleteMaterialPickingBody")]
        [HttpPost]
        public IHttpActionResult deleteMaterialPickingBody(MaterialPickBodyViewModel obj)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.deletePickingBody(obj, ID))
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

        [Route("api/Picking/TransferToReturn")]
        [HttpPost]
        public IHttpActionResult TransferToReturn(CreateReturnViewModel obj)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.TransferToReturn(obj, ID))
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

        [Route("api/Picking/TransferToScrap")]
        [HttpPost]
        public IHttpActionResult TransferToScrap(ScrapSaveViewModel obj)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.TransferToScrap(obj, ID))
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

        [Route("api/Picking/AddPickingBody")]
        [HttpPost]
        public IHttpActionResult AddPickingBody(MaterialPickBodyViewModel obj)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.addPickingBody(obj, ID))
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

        [Route("api/Picking/SavePickingData")]
        [HttpPost]
        public IHttpActionResult SavePickingData(PickingSaveModel obj)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.SavePickingData(obj))
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

        [Route("api/Picking/SaveToolPickingData")]
        [HttpPost]
        public IHttpActionResult SaveToolPickingData(ToolPickingSaveModel obj)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.SaveToolPickingData(obj))
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

        [Route("api/Picking/AddPickingToolBody")]
        [HttpPost]
        public IHttpActionResult AddPickingToolBody(ToolPickBodyViewModel obj)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.addPickingToolBody(obj, ID))
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

        [Route("api/Picking/SavePickingHeader")]
        [HttpPost]
        public IHttpActionResult SavePickingHeader(MaterialPickHeaderSaveModel obj)
        {
            string ID = User.Identity.Name;

            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Picking\\" + obj.OrderNo;
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


            if (PickingFactory.savePickingHeader(obj, ID))
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

        [Route("api/Picking/updatePickingHeader")]
        [HttpPost]
        public IHttpActionResult updatePickingHeader(MaterialPickHeaderSaveModel obj)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.updatePickingHeader(obj, ID))
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

        [Route("api/Picking/deletePicking/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult deletePicking(string OrderNo)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.deletePicking(OrderNo))
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

        [Route("api/Picking/SaveToolPickingHeader")]
        [HttpPost]
        public IHttpActionResult SaveToolPickingHeader(ToolPickHeaderSaveModel obj)
        {
            string ID = User.Identity.Name;

            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\ToolPicking\\" + obj.OrderNo;
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


            if (PickingFactory.saveToolPickingHeader(obj, ID))
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

        [Route("api/Picking/UpdateToolPickingBody")]
        [HttpPost]
        public IHttpActionResult UpdateToolPickingBody(ToolPickBodyViewModel obj)
        {
            string ID = User.Identity.Name;

            if (PickingFactory.updateToolPickingBody(obj, ID))
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
        
    }
}
