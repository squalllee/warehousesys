using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class TransToInboundViewModel
    {
        public string PurchaseNo { get; set; }
        
        public string MaterialNo { get; set; }

        public string MaterialName { get; set; }

        public string DeliveryLot { get; set; }

        public string DeliveryPlace { get; set; }

        public int Quantity { get; set; }

        public int ReceivedQty { get; set; }

        public int UnReceivedQty { get; set; }

        public float Price { get; set; }
    }
}