using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class ToolPickHeaderViewModel
    {
        public string OrderNo { get; set; }

        public string WorkNo { get; set; }

        public string PickingMan { get; set; }

        public string PickingManId { get; set; }

        public string OutBoundMan { get; set; }

        public string OutBoundManId { get; set; }

        public DateTime? OutBoundDate { get; set; }

        public Boolean EmergencyPicking { get; set; }

        public string Status { get; set; }

        public string WGroupId { get; set; }
    }
}