using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class StockInventoryBodyViewModel: MaterialBase
    {
        public string OrderNo { get; set; }

        public string SerialNo { get; set; }

        public string WarehouseId { get; set; }

        public string WareHouseName { get; set; }

        public string StorageId { get; set; }

        public string Lot { get; set; }

        public int FirstCheckQty { get; set; }

        public int SecondCheckQty { get; set; }

        public int diffQty { get; set; }

        public string Note { get; set; }
    }
}