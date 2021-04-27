using System.Collections.Generic;
using WareHouseSys.DBModels;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Models
{
    public class TransToPurSaveModel
    {
        public PurchaseSaveHeader purchaseHeader { get; set; }
        public List<TransToPurViewModel> purchaseBodies { get; set; }
    }
}