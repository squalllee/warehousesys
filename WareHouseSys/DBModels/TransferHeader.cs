using System;

namespace WareHouseSys.DBModels
{
    public class TransferHeader
    {
        
        public string OrderNo {get;set;}

        public DateTime? ApplicantDate { get;set;}

        public DateTime? TransferOutDate { get; set; }

        public DateTime? TransferInDate { get; set; }

        public string Status {get;set;}

        public string ApplicantMan { get;set;}

        public string TransferOutMan { get; set; }

        public string TransferInMan { get; set; }

        public string WGroupId { get; set; }

        public DateTime? AddDateTime {get;set;}

        public DateTime? UpdateDateTime {get;set;}

    }
}
