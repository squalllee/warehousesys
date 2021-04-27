using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class Inventory
    {
        
        public string MaterialNo {get;set;}

        public string WarehouseId {get;set;}

        public string StorageId {get;set;}

        public Double? Quantity {get;set;}

        public string OccupiedStorageId {get;set;}

        public string Lot {get;set;}

        public bool SaveStockAlert { get; set; }

    }
}
