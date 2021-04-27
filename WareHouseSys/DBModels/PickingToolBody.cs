using System;
using System.Linq;
using System.Text;

namespace Models
{
    public class PickingToolBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public int? Quantity {get;set;}

        public int? PickedQty {get;set;}

        public string WareHouseId {get;set;}

        public string StorageId {get;set;}

        public string Note {get;set;}

        public string Lot {get;set;}

        public string KeepMan {get;set;}

        public string KeepUnit {get;set;}

    }
}
