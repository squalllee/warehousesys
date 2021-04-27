using System;
using System.Collections.Generic;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class RequirementDetailUpdateModel: RequireMaterialViewModel
    {
        public string OrderNo { get; set; }

        public string RequireUnit { get; set; }

        public float RequirementQty { get; set; }

        public double EstPrice { get; set; }

        public int Inventory { get; set; }

        public DateTime RequireDate { get; set; }

        public DateTime PeriodStart1 { get; set; }

        public DateTime PeriodEnd1 { get; set; }

        public int PeriodQty1 { get; set; }

        public DateTime? PeriodStart2 { get; set; }

        public DateTime? PeriodEnd2 { get; set; }

        public int? PeriodQty2 { get; set; }

        public string RepairClass { get; set; }

        public string RequireReason { get; set; }

        public List<UNIT> uNIT { get; set; }

    }
}