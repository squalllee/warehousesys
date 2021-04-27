using System;

namespace WareHouseSys.DBModels
{
    public class PreventiveWorkContent
    {
        
        public string RepairNo {get;set;}

        public DateTime? RepairDateTime {get;set;}

        public DateTime? CompleteDateTime {get;set;}

        public string RepairResaon {get;set;}

        public string RepairClass {get;set;}

        public string SecurityEq {get;set;}

        public string DangerWork {get;set;}

        public string WorkRequirement {get;set;}

        public string CreateUnit {get;set;}

        public Boolean? IsFinish {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
