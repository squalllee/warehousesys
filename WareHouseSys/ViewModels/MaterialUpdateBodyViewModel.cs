using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class MaterialUpdateBodyViewModel: MaterialUpdateBody
    {
    
        public string UnitName { get; set; }

        public string FixClassName { get; set; }

        public string AffectClassName { get; set; }

        public string OriUnitName { get; set; }

        public string OriFixClassName { get; set; }

        public string OriAffectClassName { get; set; }
    }
}