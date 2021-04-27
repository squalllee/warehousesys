using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class InventorySaveModel
    {
        public StockInventoryHeaderViewModel StockInventoryHeaderViewModel { set; get; }
        public List<StockInventoryBodyViewModel> stockInventoryBodyViewModels { set; get; }
    }
}