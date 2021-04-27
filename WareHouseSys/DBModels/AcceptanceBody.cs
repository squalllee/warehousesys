using System;
using System.Linq;
using System.Text;

namespace WareHouseSys.DBModels
{
    public class AcceptanceBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public int? OKQuantity {get;set;}

        public int? NGQuantity {get;set;}

    }
}
