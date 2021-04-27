namespace WareHouseSys.DBModels
{
    public class ScrapBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string Unit { get; set; }

        public string MaterialNo {get;set;}

        public int? Quantity {get;set;}

        public string MaterialClass { get; set; }

        public string WareHouseId { get; set; }

        public string StorageId {get;set;}

        public decimal? UnitPrice {get;set;}

        public decimal? TotalPrice {get;set;}

        public string Lot {get;set;}

    }
}
