using System;
using System.Linq;
using System.Text;

namespace WareHouseSys.DBModels
{
    public class DemandBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public int? Quantity {get;set;}

        public Double? EstPriceWithOutTax {get;set;}

        public Double? EstTotalPriceWithOutTax {get;set;}

        public string VendorId { get; set; }

        public string Vendor1 {get;set;}

        public string Vendor2 {get;set;}

        public string Vendor3 {get;set;}

        public string PurchaseName {get;set;}

        public DateTime? DemanDate {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
