using System;

namespace Models
{
    public class ScrapHeader
    {
        
        public string OrderNo {get;set;}

        public string WorkNo { get;set;}

        public string ApplyMan { get;set;}

        public DateTime? ApplyDate {get;set;}

        public string ApplyUnit {get;set;}

        public string Reason { get; set; }

        public string ScrapType { get; set; }

        public string Status {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
