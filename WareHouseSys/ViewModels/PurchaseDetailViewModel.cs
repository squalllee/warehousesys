using System;

namespace WareHouseSys.ViewModels
{
    public class PurchaseDetailViewModel
    {
        public string PurchaseNo { get; set; }

        public string RequirementNo { get; set; }

        public string SerialNo { get; set; }

        public string MaterialNo { get; set; }

        public string MaterialName { get; set; }

        public string Spec { get; set; }

        public string Unit { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public string DeliveryLot { get; set; }

        public string DeliveryPlace { get; set; }

        public DateTime? PerformancePeriod { get; set; }

        public string ReqireUnit { get; set; }

    }
}