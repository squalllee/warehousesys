using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class MaterialPickHeaderViewModel
    {
        public string OrderNo { get; set; }

        public string WorkNo { get; set; }

        public string PickingMan { get; set; }

        public string PickingManId { get; set; }

        public string PickingUnit { get; set; }

        public string PickingUnitId { get; set; }

        public string OutBoundMan { get; set; }

        public string OutBoundManId { get; set; }

        public DateTime? OutBoundDate { get; set; }

        public Boolean EmergencyPicking { get; set; }

        public string PickingReason { get; set; }

        public DateTime ApplyDateTime { get; set; }

        public string Status { get; set; }

        public string WGroupId { get; set; }
    }
}