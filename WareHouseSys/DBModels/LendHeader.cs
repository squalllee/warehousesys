using System;
namespace WareHouseSys.DBModels
{
    public class LendHeader
    {
        public string OrderNo {get;set;}

        public string LendMan {get;set;}

        public string LendUnit { get; set; }

        public string OutBoundMan {get;set;}

        public DateTime? OutBoundDate {get;set;}

        public string Status {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

        public DateTime? Deadline { get;set;}

        public DateTime? ExtendDate { get; set; }

        public string Reason {get;set;}

        public string OtherReason { get; set; }

        public string WGroupId { get; set; }

        public int ExtendCount { get; set; }

    }
}
