using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class ReceiveSearchViewModel : ReceiveBody
    {

        public string PurchaseNo { get; set; }

        public string DeliveryLot { get; set; }

        public DateTime? ReceiveDate { get; set; }

        public string ReceiveMan { get; set; }

        public string IsDocument { get; set; }

        public string Status1 { get; set; }

        public string ReceiveStatus { get; set; }

        public string IsTransToInbound { get; set; }

        public string IsRecved1 { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public string UpdateMan { get; set; }

        public string UNITNAME { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string WareHouseName { get; set; }

        public string AttUrl { get; set; }
    }
}