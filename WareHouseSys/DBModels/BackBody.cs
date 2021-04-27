using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class BackBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public int Quantity {get;set;}

        public int BackQty { get; set; }

        public string WareHouseId {get;set;}

        public string StorageId {get;set;}

        public string OccupiedStorageId {get;set;}

        public string Note {get;set;}

        public string Lot {get;set;}

    }
}
