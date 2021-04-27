using System.Collections.Generic;
using WareHouseSys.DBModels;

namespace WareHouseSys.Models
{
    public class RequirementSaveModel
    {
        public RequirementHeader requirementHeader { get; set; }

        public List<RequirementBody> requirementBodies { get; set; }
    }
}