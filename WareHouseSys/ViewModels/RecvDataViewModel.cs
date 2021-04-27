using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class RecvDataViewModel:MaterialBase
    {
        public string OrderNo { get; set; }
        public string DeliveryLot { get; set; }
        public string SerialNo { get; set; }
        public string WarehouseId { get; set; }
        public string WareHouseName { get; set; }
        public string StorageId { get; set; }
        public string receivedQty { get; set; }
        public string UnreceivedQty { get; set; }
        public string Note { get; set; }
    }
}