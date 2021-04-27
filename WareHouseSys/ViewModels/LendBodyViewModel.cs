using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class LendBodyViewModel:MaterialBase
    {
        public string OrderNo { get; set; }

        public string SerialNo { get; set; }

        public string MaterialNo { get; set; }

        public string WareHouseId { get; set; }

        public string StorageId { get; set; }

        public string Lot { get; set; }

        public int LendQty { get; set; }

        public int Quantity { get; set; }

        public string WareHouseName { get; set; }
    }
}