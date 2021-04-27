using System;

namespace WareHouseSys.DBModels
{
    public class MaterialUpdateHeader
    {
        
        public string OrderNo {get;set;}

        public string ReqUnit {get;set;}

        public string ReqMan {get;set;}

        public string Status {get;set;}

        public DateTime ReqDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

        public string updateMan {get;set;}

        public string Reason { get; set; }

    }
}
