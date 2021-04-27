using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class AdjustInfo
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public DateTime AdjustDate {get;set;}

        public string MaterialNo {get;set;}

        public string WarehouseId {get;set;}

        public string StorageId {get;set;}

        public int? AccountQty {get;set;}

        public int? RealQty {get;set;}

        public int? StockDiff {get;set;}

        public string Applicant {get;set;}

        public string ProcessMan {get;set;}

        public string Reason {get;set;}

        public string Note {get;set;}

        public int? Status {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

        public DateTime? Lot {get;set;}

    }
}
