using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class PurchaseHeaderViewModel:PurchaseHeader
    {
        public string PurchaseUnitId { get; set; }

        public string PurchaseManId { get; set; }
    }
}