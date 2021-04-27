using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class ReturnBodyViewModel:ReturnBody
    {
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string Unit { get; set; }
        public string WareHouseName { get; set; }
    }
}