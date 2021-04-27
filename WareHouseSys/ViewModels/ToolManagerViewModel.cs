using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class ToolManagerViewModel: ToolManager
    {
        public string UNITNAME { get; set; }
        public string ToolMgrId { get; set; }
    }
}