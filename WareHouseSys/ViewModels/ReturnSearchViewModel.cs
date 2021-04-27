using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class ReturnSearchViewModel: ReturnBody
    {

        public DateTime? InBoundDate { get; set; }

        public string PickingNo { get; set; }

        public string WorkNo { get; set; }

        public string ReturnMan { get; set; }

        public string ReturnUnit { get; set; }

        public string InBoundMan { get; set; }

        public string Status { get; set; }

        public string WGroupId { get; set; }

        public string ReturnReason { get; set; }

        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string WareHouseName { get; set; }

        public string AttUrl { get; set; }
    }
}