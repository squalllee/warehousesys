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
    public class WareHouseController : ApiController
    {
        [Route("api/WareHouse/getWarehouseInfo")]
        [HttpGet]
        public IHttpActionResult getWarehouseInfo()
        {
            try
            {
                List<WarehouseInfo> warehouseInfo = WarehouseInfoFactory.getWarehouseInfo();
                return Ok(warehouseInfo);
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

        [Route("api/WareHouse/getWarehouseInfoWithStorage")]
        [HttpGet]
        public IHttpActionResult getWarehouseInfoWithStorage()
        {
            try
            {
                List<WareHouseViewModel> warehouseInfo = WarehouseInfoFactory.getWarehouseInfoWithStorage();
                return Ok(warehouseInfo);
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

        [Route("api/WareHouse/getWGroupInfo")]
        [HttpGet]
        public IHttpActionResult getWGroupInfo()
        {
            try
            {
                List<WGroupViewModel> wGroupViewModels = WarehouseInfoFactory.getWGroupInfo();

                return Ok(wGroupViewModels);
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
