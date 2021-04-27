using System.Collections.Generic;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class ToolInventoryHeaderViewModel:ToolInventoryHeader
    {
        public string InventoryManId { get; set; }

        public string KeepUnitId { get; set; }

        public string ToolMgrId { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}