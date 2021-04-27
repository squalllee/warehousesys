using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class MaterialComboViewModel
    {
        public string MaterialNo { get; set; }

        public string MaterialName { get; set; }

        public string WarehouseId { get; set; }

        public string WareHouseName { get; set; }

        public string StorageId { get; set; }

        public string Spec { get; set; }

        public string Unit { get; set; }

        public string Lot { get; set; }

        public Double? Qty { get; set; }
    }
}