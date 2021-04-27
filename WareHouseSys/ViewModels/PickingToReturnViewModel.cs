using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class PickingToReturnViewModel: MaterialBase
    {
        public string OrderNo { get; set; }

        public string SerialNo { get; set; }

        public int? PickedQty { get; set; }

        public string WareHouseId { get; set; }

        public string WareHouseName { get; set; }

        public string StorageId { get; set; }

        public string Note { get; set; }

        public string Lot { get; set; }

        public int ReturnedQty { get; set; }

        public int CanReturnQty { get; set; }
    }
}