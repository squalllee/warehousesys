namespace WareHouseSys.DBModels
{
    public class AdjustBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public int Quantity {get;set;}

        public string WareHouseId {get;set;}

        public string StorageId {get;set;}

        public string Lot {get;set;}

        public string Reason { get; set; }

        public int StockQty { get; set; }

    }
}
