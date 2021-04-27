using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class AdjustBodyViewModel:AdjustBody
    {
        public string WareHouseName { get; set; }

        public string MaterialName { get; set; }

        public string Spec { get; set; }

        public string Unit { get; set; }
    }
}