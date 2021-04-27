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
    public class MeasurementUnitController : ApiController
    {
        [Route("api/MeasurementUnit/getMeasurementUnit")]
        [HttpGet]
        public IHttpActionResult getMeasurementUnit()
        {
            List<MeasurementUnit> measurementUnits = MeasurementUnitFactory.getMeasurementUnit();

            return Ok(measurementUnits);

        }
    }
}
