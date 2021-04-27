using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class EstDemandBodyViewModel:DemandBody
    {
        public string MaterialName { get; set; }

        public string Spec { get; set; }

        public string VendorName { get; set; }
    }
}