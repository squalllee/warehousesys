using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class UnitController : ApiController
    {
        [Route("api/Unit/getUnit")]
        [HttpGet]
        public IHttpActionResult getUnit()
        {
            string ID = User.Identity.Name;
            try
            {
                List<UNIT> uNITs = UnitFactory.getWareHouseUint();
                return Ok(uNITs);
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

        [Route("api/Unit/getAllUnit")]
        [HttpGet]
        public IHttpActionResult getAllUnit()
        {
            //string ID = User.Identity.Name;
            try
            {
                List<UNIT> uNITs = UnitFactory.getAllUint();
                return Ok(uNITs);
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

        [Route("api/Unit/getToolKeepUnit")]
        [HttpGet]
        public IHttpActionResult getToolKeepUnit()
        {
            try
            {
                List<ToolManagerViewModel> uNITs = UnitFactory.getToolKeepUnit();
                return Ok(uNITs);
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
