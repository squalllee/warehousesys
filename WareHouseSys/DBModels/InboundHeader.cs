using System;
namespace WareHouseSys.DBModels
{
    public class InboundHeader
    {
        
        public string OrderNo {get;set;}

        public string PurNo { get;set;}

        public string InboundMan {get;set;}

        public string DeliveryLot { get; set; }

        public DateTime? InboundDate {get;set;}

        public string Status {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
