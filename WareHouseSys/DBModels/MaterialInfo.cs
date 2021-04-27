using System;
using System.ComponentModel.DataAnnotations;

namespace WareHouseSys.DBModels
{
    public class MaterialInfo
    {
        [Required(ErrorMessage ="料號必需輸入")]
        public string MaterialNo {get;set;}

        [Required(ErrorMessage = "品名必需輸入")]
        public string MaterialName {get;set;}

        [Required(ErrorMessage = "規格必需輸入")]
        public string Spec {get;set;}

        [Required(ErrorMessage = "系統別必需輸入")]
        public string SystemId {get;set;}

        public string LineAbb {get;set;}

        [Required(ErrorMessage = "子系統別必需輸入")]
        public string SubSystemId {get;set;}

        public string SerialNo {get;set;}

        public string FixClass {get;set;}

        public string AffectClass {get;set;}

        public string VendorId {get;set;}

        //public string IsDevelopment {get;set;}

        public string Unit {get;set;}

        public Double? Length {get;set;}

        public Double? Witdh {get;set;}

        public Double? Height {get;set;}

        public Double? weight {get;set;}

        public string ReplaceNo {get;set;}

        public string ROP {get;set;}

        public string EqQuantity {get;set;}

        public string EstPurPeriod {get;set;}

        public int EstAnnConsumption {get;set;}

        public Boolean IsFix {get;set;}

        public Boolean IsDangerous {get;set;}

        public Boolean IsLimitTime {get;set;}

        public Boolean SpecifyBrand { get; set; }

        public string Expiration {get;set;}

        public int? SafetyStock {get;set;}

        public Double? FailureRate {get;set;}

        public Boolean Handtool {get;set;}

        public Boolean Freeze { get; set; }

        public DateTime? UpdateDateTime {get;set;}

        public string UpdateMan {get;set;}

    }
}
