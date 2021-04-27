using System;
using System.Linq;
using System.Text;

namespace WareHouseSys.DBModels
{
    public class ChemicalData
    {
        
        public string MaterialNo {get;set;}

        public string SDSMaterialName { get;set;}

        public string BottleName { get; set; }

        public float weight { get; set; }

        public string PhysicalStatus {get;set;}

        public string MaterialMix {get;set;}

        public string HarmDesc {get;set;}

        public string HarmLevel {get;set;}

        public string CasNo { get;set;}

        public string HarmGroup1 {get;set;}

        public string HarmGroup2 {get;set;}

        public string Maker {get;set;}

        public string MakerAddress {get;set;}

        public string MakerTel {get;set;}

        public int StoreIndex { get; set; }

        public bool IsPriority { get; set; }

    }
}
