using System;

namespace WareHouseSys.DBModels
{
    public class WorkOrder
    {
        
        public string RepairNo {get;set;}

        public string ItemId {get;set;}

        public string LocationLayer1 {get;set;}

        public string LocationLayer2 {get;set;}

        public string LocationLayer3 {get;set;}

        public string LocationLayer4 {get;set;}

        public string SystemId {get;set;}

        public string SubSystemId {get;set;}

        public DateTime? RepairDateTime {get;set;}

        public string ModuleId {get;set;}

        public string ContractNo {get;set;}

        public DateTime? CompleteDateTime {get;set;}

        public string BrokenReason {get;set;}

        public string RepairResult {get;set;}

        public Boolean? IsAvailability {get;set;}

        public string CreateUnit {get;set;}

        public string DispatchingMan {get;set;}

        public string ClassType {get;set;}

        public Boolean? IsFinish {get;set;}

        public DateTime? CreateDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
