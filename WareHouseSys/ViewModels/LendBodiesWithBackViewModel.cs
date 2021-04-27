using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class LendBodiesWithBackViewModel:LendBodyViewModel
    {
        public int BackQty { get; set; }

        public int NotReturnQty { get; set; }
    }
}