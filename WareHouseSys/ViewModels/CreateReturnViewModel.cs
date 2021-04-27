using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class CreateReturnViewModel
    {
        public string OrderNo { get; set; }

        public string WGroupId { get; set; }

        public List<PickingToReturnViewModel> ReturnBodies { get; set; }
    }
}