using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class MaterialUpdateController : ApiController
    {
        [Route("api/MaterialUpdate/SaveReqMaterial")]
        [HttpPost]
        public IHttpActionResult SaveReqMaterial(MaterialUpdateSaveModel materialUpdateSaveModel)
        {
            string ID = User.Identity.Name;

            if (MaterialUpdateFactory.saveMaterialUpdate(materialUpdateSaveModel, ID))
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

        [Route("api/MaterialUpdate/closeMaterialUpdate")]
        [HttpPost]
        public IHttpActionResult closeMaterialUpdate(MaterialUpdateReviewModel materialUpdateReviewModel)
        {
            string ID = User.Identity.Name;

            if (MaterialUpdateFactory.closeMaterialUpdate(materialUpdateReviewModel, ID))
            {
                string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\MaterialUpdate\\" + materialUpdateReviewModel.OrderNo;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                foreach (Attachment att in materialUpdateReviewModel.attachments)
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


        [Route("api/MaterialUpdate/updateMaterialUpdateHeader")]
        [HttpPost]
        public IHttpActionResult updateMaterialUpdateHeader(MaterialUpdateHeader materialUpdateHeader)
        {
            string ID = User.Identity.Name;

            if (MaterialUpdateFactory.updateMaterialUpdateHeader(materialUpdateHeader, ID))
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

        [Route("api/MaterialUpdate/closeReqMaterial")]
        [HttpPost]
        public IHttpActionResult closeReqMaterial(ReqMaterialReviewModel reqMaterialReviewModel)
        {
            string ID = User.Identity.Name;

            if (ReqMaterialFactory.closeReqMaterial(reqMaterialReviewModel, ID))
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

        [Route("api/MaterialUpdate/addMaterialUpdate")]
        [HttpPost]
        public IHttpActionResult addMaterialUpdate(MaterialUpdateBody MaterialUpdateBody)
        {
            string ID = User.Identity.Name;

            if (MaterialUpdateFactory.addMaterialUpdate(MaterialUpdateBody))
            {
                return Ok(MaterialUpdateBody);
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

        [Route("api/MaterialUpdate/updateMaterialUpdate")]
        [HttpPost]
        public IHttpActionResult updateMaterialUpdate(MaterialUpdateBody materialUpdateBody)
        {

            if (MaterialUpdateFactory.updateMaterialUpdate(materialUpdateBody))
            {
                return Ok(materialUpdateBody);
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

        [Route("api/MaterialUpdate/deleteMaterialUpdate")]
        [HttpPost]
        public IHttpActionResult deleteMaterialUpdate(MaterialUpdateBody materialUpdateBody)
        {

            if (MaterialUpdateFactory.deleteMaterialUpdate(materialUpdateBody))
            {
                return Ok(materialUpdateBody);
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
