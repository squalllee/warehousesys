using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class ReceiveHeader
    {
        
        public string OrderNo {get;set;}

        public string PurchaseNo {get;set;}

        public string DeliveryLot {get;set;}

        public DateTime? ReceiveDate {get;set;}

        public string ReceiveMan {get;set;}

        public Boolean IsDocument {get;set;}

        public string Status {get;set;}

        public string ReceiveStatus { get; set; }

        public bool IsTransToInbound { get; set; }

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

        public string updateMan {get;set;}

    }
}
