using System;

namespace WareHouseSys.ReportModels
{
    public class RequireReportModel
    {
        public int ROWID { set; get; }

        public string OrderNo { get; set; }

        public DateTime? ApplicationDate { get; set; }

        public string Applicant { get; set; }

        public string ApplicantUnit { get; set; }

        public string RequireUnit { get; set; }

        public string MaterialNo { get; set; }

        public string MaterialName { get; set; }

        public string Spec { get; set; }

        public string Unit { get; set; }

        public string IsDevelopment { get; set; } = "";

        public string ReplaceNo { get; set; } = "";

        public string EqQuantity { get; set; } = "";

        public string RepairPeriod { get; set; } = "";

        public string Simple { get; set; }

        public bool IsFix { get; set; }

        public bool IsDangerous { get; set; }

        public bool IsLimitTime { get; set; }

        public string Expiration { get; set; } = "";

        public int SafetyStock { get; set; }

        public float? FailureRate { get; set; }

        public float Quantity { get; set; }

        public string ReplaceQuantity { get; set; } = "";

        public int OnOrderInventory { get; set; }

        public DateTime? PeriodStart1 { get; set; }

        public DateTime? PeriodEnd1 { get; set; }

        public int? PeriodQty1 { get; set; }

        public DateTime? PeriodStart2 { get; set; }

        public DateTime? PeriodEnd2 { get; set; }

        public int? PeriodQty2 { get; set; }

        public string DeliveryPeriod { get; set; }

        public string RequireReason { get; set; }
    }
}