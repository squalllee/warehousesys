using System;
using System.Linq;
using System.Text;

namespace WareHouseSys.DBModels
{
    public class RequirementBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string RequireUnit {get;set;}

        public string MaterialNo {get;set;}

        public Double? RequirementQty {get;set;}

        public decimal? EstPrice {get;set;}

        public int? Inventory {get;set;}

        public int? OnOrderInventory {get;set;}

        public DateTime? RequireDate {get;set;}

        public DateTime? PeriodStart1 {get;set;}

        public DateTime? PeriodEnd1 {get;set;}

        public int? PeriodQty1 {get;set;}

        public DateTime? PeriodStart2 {get;set;}

        public DateTime? PeriodEnd2 {get;set;}

        public int? PeriodQty2 {get;set;}

        public string RepairClass { get;set;}

        public string RequireReason {get;set;}

    }
}
