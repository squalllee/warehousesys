using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class ChemicalController : ApiController
    {
        [Route("api/Chemical/getChemicalHarm")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult getChemicalHarm()
        {
            List<chemicalHarm> chemicalHarms = ChemicalFactory.getChemicalHarm();
            return Ok(chemicalHarms);
        }

        

        [Route("api/Chemical/ChemicalCreate")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult ChemicalCreate(ChemicalData chemicalData)
        {
            if (ChemicalFactory.ChemicalCreate(chemicalData))
            {
                return Ok(chemicalData);
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

        [Route("api/Chemical/ChemicalUpdate")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult ChemicalUpdate(ChemicalData chemicalData)
        {
            if (ChemicalFactory.ChemicalUpdate(chemicalData))
            {
                return Ok(chemicalData);
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


        [Route("api/Chemical/ChemicalDelete")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult ChemicalDelete(ChemicalData chemicalData)
        {
            if (ChemicalFactory.ChemicalDelete(chemicalData))
            {
                return Ok(chemicalData);
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
