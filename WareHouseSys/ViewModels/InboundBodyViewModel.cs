using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class InboundBodyViewModel:MaterialBase
    {
        public string OrderNo { get; set; }

        public string SerialNo { get; set; }

        public string Expiration { get; set; }

        public int InboundQty { get; set; }

        public StorageInfo Storage { get; set; }

        public StorageInfo OccupiedStorage { get; set; }

        public string StorageId { get; set; }

        public string OccupiedStorageId { get; set; }

        public string WarehouseId { get; set; }

        public string Note { get; set; }

        public string Lot { get; set; }

        public bool SaveStockAlert { get; set; }

        public WarehouseInfo warehouseInfo { get; set; }

    }
}