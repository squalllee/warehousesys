using System;
using System.ComponentModel.DataAnnotations;

namespace WareHouseSys.ViewModels
{
    public class TransToPurViewModel
    {
        public string OrderNo { get; set; }

        public int SerialNo { get; set; }

        public string reqSerialNo { get; set; }

        public string MaterialNo { get; set; }

        public string Price { get; set; }

        public float Quantity { get; set; }

        public string DeliveryLot { get; set; }

        public string DeliveryPlace { get; set; }

        public DateTime? PerformancePeriod { get; set; }
        
        public string MaterialName { get; set; }

        public string Spec { get; set; }

        public string Unit { get; set; }

        public string ReqireUnit { get; set; }

        public string RequireUnitName { get; set; }

    }
}