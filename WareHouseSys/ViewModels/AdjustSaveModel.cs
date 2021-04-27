using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class AdjustSaveModel
    {
        public AdjustHeader adjustHeader { get; set; }

        public List<AdjustBody> adjustBodies { get; set; }
    }
}