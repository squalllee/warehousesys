namespace WareHouseSys.DBModels
{
    public class TransferBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public int Quantity {get;set;}

        public int TransferOutQty { get; set; }

        public int TransferInQty { get; set; }

        public string OutWareHouseId {get;set;}

        public string OutStorageId {get;set;}

        public string InWareHouseId {get;set;}

        public string InStorageId {get;set;}

        public string OccupiedStorageId {get;set;}

        public string Note {get;set;}

        public string Lot {get;set;}

    }
}
