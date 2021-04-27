using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class ToolInventorySaveModel
    {
        public ToolInventoryHeaderViewModel toolInventoryHeader { get; set; }
        public List<ToolInventoryViewModel> toolInventoryViewModels { get; set; }
    }
}