using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class TotalStockViewModel:MaterialInfo
    {
        public float Quantity { get; set; }

        public int LendQty { get; set; }

        public float Qty { get; set; }
    }
}