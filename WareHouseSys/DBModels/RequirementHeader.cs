using System;

namespace WareHouseSys.DBModels
{
    public class RequirementHeader
    {
        
        public string OrderNo {get;set;}

        public DateTime? ApplicationDate {get;set;}

        public string Applicant {get;set;}

        public Boolean? SpecifyBrand {get;set;}

        public string SpecifyReason {get;set;}

        public Boolean? Emergency {get;set;}

        public string EmergencyReason {get;set;}

        public Boolean? Temporary {get;set;}

        public string TemporaryReason {get;set;}

        public string AcceptanceStd {get;set;}

        public string AcceptanceReason {get;set;}

        public string Status {get;set;}

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

        public Boolean IsCreateForm {get;set;}

    }
}
