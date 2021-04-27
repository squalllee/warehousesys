using Models;
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
    public class ScrapController : ApiController
    {
        [Route("api/Scrap/AddScrap")]
        [HttpPost]
        public IHttpActionResult AddScrap(NewScrapViewModel obj)
        {
            string ID = User.Identity.Name;

            if (ScrapFactory.AddScrap(obj, ID))
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

        [Route("api/Scrap/addScrapBody")]
        [HttpPost]
        public IHttpActionResult addScrapBody(ScrapBodyViewModel obj)
        {
            string SerialNo = "";

            if (ScrapFactory.AddScrapBody(obj,out SerialNo))
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

        [Route("api/Scrap/updateScrapBody")]
        [HttpPost]
        public IHttpActionResult updateScrapBody(ScrapBodyViewModel obj)
        {
            string ID = User.Identity.Name;

            if (ScrapFactory.UpdateScrapBody(obj))
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

        [Route("api/Scrap/deleteScrapBody")]
        [HttpPost]
        public IHttpActionResult deleteScrapBody(ScrapBodyViewModel obj)
        {
            string ID = User.Identity.Name;

            if (ScrapFactory.DeleteScrapBody(obj))
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

        [Route("api/Scrap/SaveScrapHeader")]
        [HttpPost]
        public IHttpActionResult SaveScrapHeader(ScrapHeaderViewModel obj)
        {
            string ID = User.Identity.Name;

            if (ScrapFactory.SaveScrapHeader(obj))
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

        [Route("api/Scrap/CloseScrap")]
        [HttpPost]
        public IHttpActionResult CloseScrap(ScrapHeaderViewModel obj)
        {
            string ID = User.Identity.Name;

            if (ScrapFactory.CloseScrap(obj))
            {
                string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Scrap\\" + obj.OrderNo;
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
