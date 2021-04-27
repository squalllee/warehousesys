using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class ToolPickBodyViewModel : MaterialBase
    {
        public string OrderNo { get; set; }

        public string SerialNo { get; set; }

        public int? PickedQty { get; set; }

        public string WareHouseId { get; set; }

        public WarehouseInfo warehouseInfo { get; set; }

        public StorageInfo Storage { get; set; }

        public string WareHouseName { get; set; }

        public string StorageId { get; set; }

        public string Note { get; set; }

        public string Lot { get; set; }

        public string WGroupId { get; set; }

        public string KeepMan { get; set; }

        public string KeepManId { get; set; }

        public string KeepUnit { get; set; }

        public string KeepUnitId { get; set; }
    }
}