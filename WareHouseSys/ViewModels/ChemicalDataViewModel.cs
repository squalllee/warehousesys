using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class ChemicalDataViewModel:ChemicalData
    {
        public string MaterialName { get; set; }

        public string Spec { get; set; }
        public float harmUpperLimit { get; set; }

        public string SDSFile { get; set; }
    }
}