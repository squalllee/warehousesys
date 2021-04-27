using WareHouseSys.DBModels;

namespace WareHouseSys.Models
{
    public class RequirementDetailViewModel
    {
        public RequireMaterialViewModel requireMaterialViewModel { get; set; }

        public UNIT unit { get; set; }

        public RequirementBody requirementBody { set; get; }
    }
}