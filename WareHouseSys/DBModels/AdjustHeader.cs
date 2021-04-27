using System;

namespace WareHouseSys.DBModels
{
    public class AdjustHeader
    {
        
        public string OrderNo {get;set;}

        public string ApplyMan {get;set;}

        public DateTime ApplyDate {get;set;}

        public string ApplyUnit {get;set;}

        public string Status {get;set;}

        public DateTime AddDateTime {get;set;}

        public DateTime UpdateDateTime {get;set;}

    }
}
