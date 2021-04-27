using System;
using System.Linq;
using System.Text;

namespace WareHouseSys.DBModels
{
    public class StockInventoryHeader
    {
        
        public string OrderNo {get;set;}

        public string InventoryMan {get;set;}

        public DateTime? InventoryDate {get;set;}

        public string InventoryWarHouse { get;set;}

        public string InventoryUnit {get;set;}

        public string Period {get;set;}

        public string InventoryAttr {get;set;}

        public string Status {get;set;}

        public string WGroupId { get; set; }

        public string WareHouseMgr { get; set; }

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
