using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.Models
{
    public class EstDemandExcelModel
    {
        public string OrderNo { get; set; }

        public string Annual { get; set; }

        public string Season { get; set; }

        public string ApplyUnit { get; set; }

        public string ApplyMan { get; set; }

        public DateTime ApplyDate { get; set; }

        public string MaterialName { get; set; }

        public string Spec { get; set; }

        public string VendorName { get; set; }

        public string SerialNo { get; set; }

        public string MaterialNo { get; set; }

        public int? Quantity { get; set; }

        public Double? EstPriceWithOutTax { get; set; }

        public Double? EstTotalPriceWithOutTax { get; set; }

        public string Vendor1 { get; set; }

        public string Vendor2 { get; set; }

        public string Vendor3 { get; set; }

        public string PurchaseName { get; set; }

        public DateTime? DemanDate { get; set; }
    }
}