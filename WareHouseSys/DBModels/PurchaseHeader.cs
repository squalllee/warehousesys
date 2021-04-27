using System;
using System.Collections.Generic;

namespace WareHouseSys.DBModels
{
    public class PurchaseHeader
    {
        public string PurchaseNo {get;set;}

        public string RequirementNo { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.Parse("1911/01/01");

        public string PurchaseName {get;set;}

        public Decimal ContractPriceWithoutVAT {get;set;}

        public Decimal ContractPriceIncludeVAT {get;set;}

        public string PurchaseMethod {get;set;}

        public string BudgetSource {get;set;}

        public string PurchaseMan { get; set; }

        public string PurchaseUnit { get;set;}

        public string VendorName {get;set;}

        public string VendorContact {get;set;}

        public string Tel {get;set;}

        public string Mobile {get;set;}

        public string PurClass { get; set; }

        public string Status {get;set;}

        public DateTime UpdateDateTime {get;set;} = DateTime.Parse("1911/01/01");

        //public string RequireNo {get;set;}

        public Boolean IsCreateRecv { get; set; } = false;

        public bool OpenContract { get; set; } = false;

    }
}
