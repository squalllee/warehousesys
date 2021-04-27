namespace WareHouseSys.ViewModel
{
    public class PurchaseBodyViewModel
    {
        public string PurchaseNo { get; set; }

        public string MaterialNo { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public string DeliveryLot { get; set; }

        public string DeliveryPlace { get; set; }

        public string PerformancePeriod { get; set; }

        public string RequireUnit { get; set; }

        public bool IsCreateRecv { get; set; }

        public int receivedQty { get; set; }

        public int UnreceivedQty { get; set; }
    }
}