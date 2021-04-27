using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class EstDemandSaveModel
    {
        public DemandHeader demandHeader { get; set; }

        public List<DemandBody> demandBodies { get; set; }
    }
}