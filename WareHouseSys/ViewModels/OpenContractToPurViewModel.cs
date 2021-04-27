using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.ViewModel;

namespace WareHouseSys.ViewModels
{
    public class OpenContractToPurViewModel
    {
        public string PurchaseNo { get; set; }
        public string Lot { get; set; }
        public List<PurchaseBodyViewModel> PurBodies { get; set; }
    }
}