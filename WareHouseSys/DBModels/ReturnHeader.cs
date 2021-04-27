using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class ReturnHeader
    {
        
        public string OrderNo {get;set;}

        public DateTime? InBoundDate {get;set;}

        public string PickingNo {get;set;}

        public string WorkNo {get;set;}

        public string ReturnMan {get;set;}

        public string ReturnUnit { get; set; }

        public string InBoundMan {get;set;}

        public string Status {get;set;}

        public string WGroupId { get; set; }

        public string ReturnReason { get; set; }

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
