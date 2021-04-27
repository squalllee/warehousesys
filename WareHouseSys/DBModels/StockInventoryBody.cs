namespace WareHouseSys.DBModels
{
    public class StockInventoryBody
    {
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public string WarehouseId {get;set;}

        public string StorageId {get;set;}

        public int Quantity {get;set;}

        public int? FirstCheckQty {get;set;}

        public int? SecondCheckQty {get;set;}

        public string Note { get; set; }

    }
}
