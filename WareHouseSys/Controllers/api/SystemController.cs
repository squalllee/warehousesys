using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;

namespace WareHouseSys.Controllers.api
{
    public class SystemController : ApiController
    {
        [Route("api/System/getSystemInfo")]
        [HttpGet]
        public IHttpActionResult getSystemInfo()
        {
            try
            {
                List<MainSystem> mainSystems = SystemFactory.GetSystems();
                return Ok(mainSystems);
            }
            catch (Exception ex)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗:" + ex.Message)
               ));
            }
        }

        [Route("api/System/getSubSystemInfo")]
        [HttpGet]
        public IHttpActionResult getSubSystemInfo(string SystemId)
        {
            try
            {
                List<SubSystem> subSystems = SystemFactory.GetSubSystems(SystemId);
                return Ok(subSystems);
            }
            catch (Exception ex)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗:" + ex.Message)
               ));
            }


        }
    }
}
