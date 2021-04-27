using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.Models
{
    public class RecvHeaderViewModel
    {
        public string OrderNo { get; set; }

        public string PurchaseNo { get; set; }

        public string DeliveryLot { get; set; }

        public DateTime? ReceiveDate { get; set; }

        public string ReceiveMan { get; set; }

        public string ReceiveStatus { get; set; }

        public bool IsDocument { get; set; }
    }
}