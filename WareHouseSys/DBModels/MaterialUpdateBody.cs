using System;

namespace WareHouseSys.DBModels
{
    public class MaterialUpdateBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public string MaterialName {get;set;}

        public string Spec {get;set;}

        public string FixClass {get;set;}

        public string AffectClass {get;set;}

        public string VendorId {get;set;}

        public string Unit {get;set;}

        public Double? Length {get;set;}

        public Double? Witdh {get;set;}

        public Double? Height {get;set;}

        public int? weight {get;set;}

        public string ReplaceNo {get;set;}

        public string ROP {get;set;}

        public string EqQuantity {get;set;}

        public string EstPurPeriod {get;set;}

        public string EstAnnConsumption {get;set;}

        public Boolean IsFix {get;set;}

        public Boolean IsDangerous {get;set;}

        public Boolean IsLimitTime {get;set;}

        public int? Expiration {get;set;}

        public int? SafetyStock {get;set;}

        public string OriMaterialName { get; set; }

        public string OriSpec {get;set;}

        public string OriFixClass {get;set;}

        public string OriAffectClass {get;set;}

        public string OriVendorId {get;set;}

        public string OriUnit {get;set;}

        public Double? OriLength {get;set;}

        public Double? OriWitdh {get;set;}

        public Double? OriHeight {get;set;}

        public Double? Oriweight {get;set;}

        public string OriReplaceNo {get;set;}

        public string OriROP {get;set;}

        public string OriEqQuantity {get;set;}

        public string OriEstPurPeriod {get;set;}

        public string OriEstAnnConsumption {get;set;}

        public Boolean? OriIsFix {get;set;}

        public Boolean? OriIsDangerous {get;set;}

        public Boolean? OriIsLimitTime {get;set;}

        public int? OriExpiration {get;set;}

        public int? OriSafetyStock {get;set;}

    }
}
