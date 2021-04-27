using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class ExtendHeader
    {
        
        public string OrderNo {get;set;}

        public string LendNo {get;set;}

        public string ExtendMan { get; set; }

        public string ApprovedMan { get; set; }

        public string ExtendReason {get;set;}

        public int Days {get;set;}

        public string WGroupId { get; set; }
        
        public string Status {get;set;}

        public DateTime ExtendDate { get; set; }

        public DateTime CloseDate { get; set; }

        public DateTime AddDateTime {get;set;}

        public DateTime UpdateDateTime {get;set;}

    }
}
