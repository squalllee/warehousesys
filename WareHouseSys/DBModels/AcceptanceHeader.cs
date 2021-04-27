using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class AcceptanceHeader
    {
        
        public string OrderNo {get;set;}

        public string ReceiveNo {get;set;}

        public DateTime? AcceptanceDate {get;set;}

        public string AcceptanceMan {get;set;}

        public string Note {get;set;}

        public int? Status {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

        public string updateMan {get;set;}

    }
}
