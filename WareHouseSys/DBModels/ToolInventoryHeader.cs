using System;

namespace WareHouseSys.DBModels
{
    public class ToolInventoryHeader
    {
        
        public string OrderNo {get;set;}

        public string InventoryMan {get;set;}

        public DateTime? InventoryDate {get;set;}

        public string KeepUnit {get;set;}

        public string Period {get;set;}

        public string InventoryAttr {get;set;}

        public string ToolMgr {get;set;}

        public string Status {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
