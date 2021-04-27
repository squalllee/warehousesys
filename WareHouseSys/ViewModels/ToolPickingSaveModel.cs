using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class ToolPickingSaveModel
    {
        public PickingToolHeader pickingToolHeader { get; set; }

        public List<PickingToolBody> pickingToolBodies { get; set; }
    }
}