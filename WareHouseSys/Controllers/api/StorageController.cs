using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;

namespace WareHouseSys.Controllers.api
{
    public class StorageController : ApiController
    {
        [Route("api/getStorageInfo/{WarehouseId}")]
        [HttpGet]
        public IHttpActionResult getStorageInfo(string WarehouseId)
        {
            try
            {
                List<StorageInfo> storageInfos = StorageInfoFactory.getStorageInfo(WarehouseId);
                return Ok(storageInfos);
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
