using System;
using System.Linq;
using System.Text;

namespace WareHouseSys.DBModels
{
    public class PurchaseBody
    {
        
        public string PurchaseNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public decimal? Price {get;set;}

        public int? Quantity {get;set;}

        public string DeliveryLot {get;set;}

        public string DeliveryPlace {get;set;}

        public DateTime? PerformancePeriod {get;set;}

        public string RequireUnit { get; set; }

        public bool IsCreateRecv { get; set; }

    }
}
