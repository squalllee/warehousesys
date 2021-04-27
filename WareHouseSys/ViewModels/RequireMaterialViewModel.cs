using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.Models
{
    public class RequireMaterialViewModel
    {
        public string MaterialNo { get; set; }

        public string MaterialName { get; set; }

        public string Spec { get; set; }

        public string Unit { get; set; }

        public string SystemId { get; set; }

        public string SubSystemId { get; set; }

        public string LineAbb { get; set; }

        public Double? Length { get; set; }

        public string SerialNo { get; set; }

        public string FixClass { get; set; }

        public string AffectClass { get; set; }

        public string VendorId { get; set; }

        public Double? Witdh { get; set; }

        public Double? Height { get; set; }

        public Double? weight { get; set; }

        public string ReplaceNo { get; set; } = "";

        public string EqQuantity { get; set; } = "";

        public string RepairPeriod { get; set; } = "";

        public Boolean IsFix { get; set; }

        public Boolean IsDangerous { get; set; }

        public Boolean IsLimitTime { get; set; }

        public string Expiration { get; set; } = "";

        public int? SafetyStock { get; set; }

        public Double? FailureRate { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public string UpdateMan { get; set; } = "";

        public float Quantity { get; set; }

        public string ReplaceQuantity { get; set; } = "";

        public int OnOrderInventory { get; set; }
    }
}