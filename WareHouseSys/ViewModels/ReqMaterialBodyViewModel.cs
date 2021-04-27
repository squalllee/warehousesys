using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class ReqMaterialBodyViewModel:ReqMaterialBody
    {
        public string SystemName { get; set; }

        public string SubSystemName { get; set; }

        public string UnitName { get; set; }

        public string LineName { get; set; }

        public string FixClassName { get; set; }

        public string AffectClassName { get; set; }

    }
}