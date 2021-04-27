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
    public class LendController : ApiController
    {
        [Route("api/Lend/SaveLend")]
        [HttpPost]
        public IHttpActionResult SaveLend(LendSaveModel lendSaveModel)
        {
            if (LendFactory.saveLend(lendSaveModel))
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

        [Route("api/Lend/getLimiteDate/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult getLimiteDate(string OrderNo)
        {
            try
            {
                return Ok(LendFactory.getLimiteDate(OrderNo));
            }
            catch
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }

        }

        [Route("api/Lend/doLend")]
        [HttpPost]
        public IHttpActionResult doLend(LendSaveModel lendSaveModel)
        {
            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Lend\\" + lendSaveModel.lendHeaderViewModel.OrderNo;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (Attachment att in lendSaveModel.attachment)
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

            if (LendFactory.doLend(lendSaveModel))
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

        [Route("api/Lend/LendBodyUpdate")]
        [HttpPost]
        public IHttpActionResult LendBodyUpdate(LendBodyViewModel lendBodyViewModel)
        {
            if (LendFactory.LendBodyUpdate(lendBodyViewModel))
            {
                return Ok(lendBodyViewModel);
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

        [Route("api/Lend/doLendBodyUpdate")]
        [HttpPost]
        public IHttpActionResult doLendBodyUpdate(LendBodyViewModel lendBodyViewModel)
        {
            if (LendFactory.doLendBodyUpdate(lendBodyViewModel))
            {
                return Ok(lendBodyViewModel);
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
        

        [Route("api/Lend/LendBodyAdd")]
        [HttpPost]
        public IHttpActionResult LendBodyAdd(LendBodyViewModel lendBodyViewModel)
        {
            if (LendFactory.LendBodyAdd(lendBodyViewModel))
            {
                return Ok(lendBodyViewModel);
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

        [Route("api/Lend/LendBodyDelete")]
        [HttpPost]
        public IHttpActionResult LendBodyDelete(LendBodyViewModel lendBodyViewModel)
        {
            if (LendFactory.LendBodyDelete(lendBodyViewModel))
            {
                return Ok(lendBodyViewModel);
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

        [Route("api/Lend/UpdateLendHeader")]
        [HttpPost]
        public IHttpActionResult UpdateLendHeader(LendSaveModel lendSaveModel)
        {
            if (LendFactory.UpdateLendHeader(lendSaveModel.lendHeaderViewModel))
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

        [Route("api/Lend/deleteLend/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult deleteLend(string OrderNo)
        {
            if (LendFactory.deleteLend(OrderNo))
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
