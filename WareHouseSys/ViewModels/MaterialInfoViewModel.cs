using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class MaterialInfoViewModel:MaterialInfo
    {
        public string SystemName { get; set; }

        public string SubSystemName { get; set; }

        public string FixClassName { get; set; }

        public string AffectClassName { get; set; }

        public string VendorName { get; set; }
    }
}