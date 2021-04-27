using System;

namespace WareHouseSys.DBModels
{
    public class InfoWork: object
    {
        
        public string WorkNo {get;set;}

        public string ReportMan {get;set;}

        public string ReportUnit {get;set;}

        public string Tel {get;set;}

        public DateTime? BrokenDateTime {get;set;}

        public DateTime? InfoDateTime {get;set;}

        public DateTime? CloseDateTime { get; set; }

        //public DateTime? CompleteDateTime { get; set; }

        public DateTime? ArrivalDateTime { get; set; }

        public string LocationLayer1 {get;set;}

        public string LocationLayer2 {get;set;}

        public string LocationLayer3 {get;set;}

        public string LocationLayer4 {get;set;}

        public string ResponsibleUnit {get;set;}

        public string ReportLevel { get; set; }

        public bool IsTransferOutSourcing { get; set; }

        public string OutSourcingName { get; set; }

        public string SystemId {get;set;}

        public string SubSystemId {get;set;}

        public string ReportReason {get;set;}

        public string Weather {get;set;}

        public int? Mile {get;set;}

        public string abbr { get; set; }

        public string SerialNo { get; set; }

        public string DispatchingMan { get; set; }

        public DateTime? DispatchingDateTime { get; set; }

        public string ExpirationDate { get; set; }

        public string EstCompleteDateTime { get; set; }

        public string InfoOutSourcingDateTime { get; set; }

        public string OriUnit { get; set; }

        public string OriUser { get; set; }

        public string OriStatus { get; set; }

        public string CurrentUnit {get;set;}

        public string CurrentUser {get;set;}

        public string Status {get;set;}

        public DateTime? CreateDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
