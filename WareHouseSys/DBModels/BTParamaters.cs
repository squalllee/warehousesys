using WareHouseSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.DBModels
{
    public class BTParameters
    {
        public int page { set; get; }
        public int rows { set; get; }
        public string sort { get; set; }
        public string order { get; set; }
        public string txtLocationId { get; set; }
        public string txtLocationDescription { get; set; }
    }
}