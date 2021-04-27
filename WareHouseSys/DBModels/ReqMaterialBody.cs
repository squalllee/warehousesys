using System;

namespace WareHouseSys.DBModels
{
    public class ReqMaterialBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo { get; set; }

        public string MaterialName {get;set;}

        public string Spec {get;set;}

        public string SystemId {get;set;}

        public string LineAbb {get;set;}

        public string SubSystemId {get;set;}

        public string FixClass {get;set;}

        public string AffectClass {get;set;}

        public string VendorId {get;set;}

        public string Unit {get;set;}

        public Double? Length {get;set;}

        public Double? Witdh {get;set;}

        public Double? Height {get;set;}

        public Double? weight {get;set;}

        public string ReplaceNo {get;set;}

        public string ROP {get;set;}

        public string EqQuantity {get;set;}

        public string EstPurPeriod {get;set;}

        public string EstAnnConsumption {get;set;}

        public int? EstUnitPrice {get;set;}

        public Boolean IsFix {get;set;}

        public Boolean IsDangerous {get;set;}

        public Boolean IsLimitTime {get;set;}

        public Boolean SpecifyBrand { get; set; }

        public int? Expiration {get;set;}

        public int? SafetyStock {get;set;}

    }
}
