using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WareHouseSys.Factory;

namespace WareHouseSys.Controllers.api
{
    public class MaintenanceController : ApiController
    {
        [Route("api/Maintenance/GetWorkNo")]
        [HttpGet]
        public IHttpActionResult GetWorkNo(string text)
        {
            try
            {
                List<string> returnObj = MaintenanceFactory.getWorkNos(text);
                return Ok(returnObj);
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

        [Route("api/Maintenance/GetUnCloseWorkNo")]
        [HttpGet]
        public IHttpActionResult GetUnCloseWorkNo(string text)
        {
            try
            {
                List<string> returnObj = MaintenanceFactory.getUnCloseWorkNos(text);
                return Ok(returnObj);
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
    }
}
