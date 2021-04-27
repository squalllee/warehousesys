using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WareHouseSys.Factory;

namespace WareHouseSys.Controllers.api
{
    public class VendorInfoController : ApiController
    {
        [Route("api/VendorInfo/getVendorInfo")]
        [HttpGet]
        public IHttpActionResult getVendorInfo()
        {
            try
            {
                return Ok(VendorInfoFactory.getVendorInfo());
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
