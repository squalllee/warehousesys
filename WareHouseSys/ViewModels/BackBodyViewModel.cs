using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class BackBodyViewModel:MaterialBase
    {
        public string OrderNo { get; set; }

        public string SerialNo { get; set; }

        public int BackQty { get; set; }

        public int LendQty { get; set; }

        public int NotReturnQty { get; set; }

        public string WareHouseId { get; set; }

        public string WareHouseName { get; set; }

        public string StorageId { get; set; }

        public string OccupiedStorageId { get; set; }

        public string Note { get; set; }

        public string Lot { get; set; }
    }
}