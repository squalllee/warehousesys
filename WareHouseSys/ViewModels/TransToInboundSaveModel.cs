using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class TransToInboundSaveModel
    {
        public string PurchaseNo { get; set; }
        public List<string> Lots { get; set; }
    }
}