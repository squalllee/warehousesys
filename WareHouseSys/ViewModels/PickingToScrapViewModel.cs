using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class PickingToScrapViewModel : MaterialBase
    {
        public string OrderNo { get; set; }

        public string SerialNo { get; set; }

        public int? PickedQty { get; set; }

        public string WareHouseId { get; set; }

        public string WareHouseName { get; set; }

        public string StorageId { get; set; }

        public string Reason { get; set; }

        public string Lot { get; set; }

        public int ScrapedQty { get; set; }

        public int CanScrapQty { get; set; }
    }
}