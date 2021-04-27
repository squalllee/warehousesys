using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class StockInventoryHeaderViewModel:StockInventoryHeader
    {
        public string InventoryManId { get; set; }

        public string WGroupName { get; set; }

        public string InventoryWarHouseId { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}