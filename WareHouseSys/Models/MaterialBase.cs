using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.Models
{
    public class MaterialBase
    {
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}