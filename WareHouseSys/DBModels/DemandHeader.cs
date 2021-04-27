using System;
using System.Linq;
using System.Text;

namespace WareHouseSys.DBModels
{
    public class DemandHeader
    {
        
        public string OrderNo {get;set;}

        public string Annual {get;set;}

        public string Season {get;set;}

        public string ApplyUnit {get;set;}

        public string ApplyMan {get;set;}

        public string Status { get; set; }

        public DateTime ApplyDate { get; set; }

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
