using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class InboundBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public string Expiration { set; get; }

        public int? Quantity {get;set;}

        public int InboundQty { get; set; }

        public string WarehouseId {get;set;}

        public string StorageId {get;set;}

        public string OccupiedStorageId {get;set;}

        public string Note {get;set;}

        public string Lot {get;set;}

        public float Price { get; set; }

        public bool SaveStockAlert { get; set; }

    }
}
