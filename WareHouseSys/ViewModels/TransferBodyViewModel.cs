using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class TransferBodyViewModel:MaterialBase
    {
        public string OrderNo { get; set; }

        public string SerialNo { get; set; }

        public int TransferOutQty { get; set; }

        public int TransferInQty { get; set; }

        public string OutWareHouseId { get; set; }

        public string OutWareHouseName { get; set; }

        public string OutStorageId { get; set; }

        public string InWareHouseId { get; set; }

        public string InWareHouseName { get; set; }

        public string InStorageId { get; set; }

        public string OccupiedStorageId { get; set; }

        public string Note { get; set; }

        public string Lot { get; set; }
    }
}