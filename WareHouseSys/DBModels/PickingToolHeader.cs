using System;
using System.Linq;
using System.Text;

namespace Models
{
    public class PickingToolHeader
    {
        
        public string OrderNo {get;set;}

        public string WorkNo {get;set;}

        public string PickingMan {get;set;}

        public string OutBoundMan {get;set;}

        public DateTime? OutBoundDate {get;set;}

        public Boolean? EmergencyPicking {get;set;}

        public string WGroupId { get; set; }

        public string Status {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
