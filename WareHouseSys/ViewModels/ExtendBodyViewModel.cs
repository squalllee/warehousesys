using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class ExtendBodyViewModel:MaterialBase
    {
        public string OrderNo { get; set; }

        public string SerialNo { get; set; }

        public int ExtendQty { get; set; }

        public string Lot { get; set; }
    }
}