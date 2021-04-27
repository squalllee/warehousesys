using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class PickingSaveModel
    {
        public PickingHeader pickingHeader { get; set; }

        public List<PickingBody> pickingBodies { get; set; }
    }
}