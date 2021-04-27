using System.Collections.Generic;
using WareHouseSys.DBModels;

namespace WareHouseSys.Models
{
    public class PurchaseSaveHeader:PurchaseHeader
    {
        public new List<string> BudgetSource { get; set; }
    }
}