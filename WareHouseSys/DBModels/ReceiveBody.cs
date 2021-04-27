using System;
using System.Linq;
using System.Text;

namespace WareHouseSys.DBModels
{
    public class ReceiveBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public string WareHouseId {get;set;}

        public string StorageId {get;set;}

        public int? Quantity {get;set;}

        public int? ReceivedQty {get;set;}

        public string Note { get;set;}

        public bool IsRecved { get; set; }

        public string Status {get;set;}

        public Boolean? IsCreateInventory {get;set;}

    }
}
