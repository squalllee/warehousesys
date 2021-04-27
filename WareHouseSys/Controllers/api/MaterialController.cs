using SqlSugar;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class MaterialController : ApiController
    {
        [Route("api/Material/getMaterialbyTerm/")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult getMaterialbyTerm(string term)
        {
            List<MaterialInfo> materialInfos = MaterialFactory.getMaterialByTerm(term);
            return Ok(materialInfos);
        }

        [Route("api/Material/getMaterialPrice/{MaterialNo}/{Lot}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult getMaterialPrice(string MaterialNo,string Lot)
        {
            decimal price = MaterialFactory.getMaterialPrice(MaterialNo,Lot);

            return Ok(price);

        }

        [Route("api/Material/getMaterialInfo/")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult getMaterialInfo()
        {
            List<MaterialInfo> materialInfos = MaterialFactory.getMaterialInfo();
            return Ok(materialInfos);
        }

        [Route("api/Material/getHarmMaterialInfo/")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult getHarmMaterialInfo()
        {
            List<MaterialInfo> materialInfos = MaterialFactory.getHarmMaterialInfo();
            return Ok(materialInfos);
        }

        [Route("api/Material/IsPurchased")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult IsPurchased(string MaterialNo)
        {
            return Ok(MaterialFactory.IsPurchased(MaterialNo));
        }

        [Route("api/Material/getMaterialInfoByNo")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult getMaterialInfo(string MaterialNo)
        {
            List<RequireMaterialViewModel> materialInfos = MaterialFactory.getMaterialInfo(MaterialNo);
            return Ok(materialInfos);
        }

        [Route("api/Material/getMaterialInfo/{MaterialNo}/{WGroupId}")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult getMaterialInfo(string MaterialNo,string WGroupId)
        {
            List<MaterialComboViewModel> materialInfos = MaterialFactory.getMaterialInfo(MaterialNo, WGroupId);
            return Ok(materialInfos);
            //return Json(materialInfos);
        }

        [Route("api/Material/getMaterialInfoAll/{WGroupId}")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult getMaterialInfoAll(string WGroupId)
        {
            List<MaterialComboViewModel> materialInfos = MaterialFactory.getMaterialInfoAll(WGroupId);
            return Ok(materialInfos);
        }
        /*
        [Route("api/Material/getMaterialInfoByNo/{MaterialNo}")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult getMaterialInfoByNo(string MaterialNo, string WGroupId)
        {
            List<MaterialComboViewModel> materialInfos = MaterialFactory.getMaterialInfoByNo(MaterialNo);
            return Ok(materialInfos);
        }
        */
        [Route("api/Material/getMaterialInfoKendo")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult getMaterialInfoKendo(FilterCriteria filter = null)
        {
            ISugarQueryable<MaterialComboViewModel> sugarQueryable = MaterialFactory.getMaterialInfoAll();

            //sugarQueryable.Where(e => e.MaterialNo.Contains(text) || e.MaterialName.Contains(text));
            return Ok(sugarQueryable.ToList());
        }

        [Route("api/Material/getMaterialInfoByWareHouse/{WGroupId}")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult getMaterialInfoByWareHouse(string WGroupId)
        {
            List<MaterialComboViewModel> materialInfos = MaterialFactory.getMaterialInfoByWareHouse(WGroupId);
            return Ok(materialInfos);
        }

        [Route("api/Material/updateMaterialInfo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult updateMaterialInfo(MaterialInfo materialInfo)
        {
           if(MaterialFactory.updateMaterialInfo(materialInfo))
            {
                return Ok(materialInfo);
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