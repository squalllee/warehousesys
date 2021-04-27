using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class BackHeader
    {
        
        public string OrderNo {get;set;}

        public string LendNo {get;set;}

        public string BackMan {get;set;}

        public string BackUnit { get; set; }

        public string InBoundMan {get;set;}

        public DateTime? InBoundDate {get;set;}

        public Boolean? Overdue {get;set;}

        public string WGroupId { get; set; }

        public string Note { get; set; }

        public string Status {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
