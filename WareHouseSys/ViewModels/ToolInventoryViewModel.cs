using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class ToolInventoryViewModel: MaterialBase
    {
        public string Lot { get; set; }

        public string KeepMan { get; set; }

        public string KeepManId { get; set; }

        public string KeepUnit { get; set; }

        public string KeepUnitId { get; set; }
    }
}