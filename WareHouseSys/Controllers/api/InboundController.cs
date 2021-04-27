using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Hosting;
using System.Web.Http;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class InboundController : ApiController
    {
        [Route("api/Inbound/getInboundData")]
        [HttpPost]
        public IHttpActionResult getRecvData(FormDataCollection form, string OrderNo, string Lot)
        {
            string ID = User.Identity.Name;

            int draw = int.Parse(form.Get("draw"));
            int start = int.Parse(form.Get("start"));
            int length = int.Parse(form.Get("length"));

            string col_index = form.Get("order[0][column]");
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : form.Get($@"columns[{col_index}][data]");
            string asc_desc = string.IsNullOrEmpty(form.Get("order[0][dir]")) ? "desc" : form.Get("order[0][dir]");//防呆
            ISugarQueryable<RecvDataViewModel> sugarQueryable = RecvFactory.getRecvData(OrderNo, Lot);

            sugarQueryable.OrderBy(String.Format("{0} {1}", sortColName, asc_desc));
            List<RecvDataViewModel> receives = sugarQueryable.Skip(start).Take(length).ToList();

            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = sugarQueryable.Count(),
                  recordsFiltered = sugarQueryable.Count(),
                  data = receives
              };

            return Ok(returnObj);
        }

        [Route("api/Inbound/addInboundBody")]
        [HttpPost]
        public IHttpActionResult addInboundBody(InboundBodyViewModel obj)
        {
            string ID = User.Identity.Name;

            if (InboundFactory.addInboundData(obj, ID))
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

        [Route("api/Inbound/updateInboundBody")]
        [HttpPost]
        public IHttpActionResult updateInboundBody(InboundBodyViewModel obj)
        {
            string ID = User.Identity.Name;

            if (InboundFactory.updateInboundData(obj, ID))
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

        [Route("api/Inbound/destroyInboundBody")]
        [HttpPost]
        public IHttpActionResult destroyInboundBody(InboundBodyViewModel obj)
        {
            if (InboundFactory.deleteInboundData(obj))
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

        [Route("api/Inbound/updateInboundheader")]
        [HttpPost]
        public IHttpActionResult updateInboundheader(InboundHeaderViewModel obj)
        {
            string ID = User.Identity.Name;

            if (InboundFactory.updateInboundHeader(obj, ID))
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

        [Route("api/Inbound/SaveDirectInoundData")]
        [HttpPost]
        public IHttpActionResult SaveInoundData(InboundSaveModel obj)
        {
            string ID = User.Identity.Name;

            if(InboundFactory.SaveDirectInboundData(obj))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                     Request.CreateErrorResponse(
                         (HttpStatusCode)422,
                         new HttpError("新增失敗!")));
            }

            
        }

        [Route("api/Inbound/InboundClose/{OrderNo}")]
        [HttpPost]
        public IHttpActionResult InboundClose(List<Attachment> atts, string OrderNo)
        {
            string ID = User.Identity.Name;

            if (InboundFactory.InboundClose(OrderNo))
            {
                try
                {
                    string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Inbound\\" + OrderNo;
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    foreach (Attachment att in atts)
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
                catch
                {
                    return new System.Web.Http.Results.ResponseMessageResult(
                     Request.CreateErrorResponse(
                         (HttpStatusCode)422,
                         new HttpError("新增失敗!")));
                }
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                     Request.CreateErrorResponse(
                         (HttpStatusCode)422,
                         new HttpError("新增失敗!")));
            }
        }
    }
}
