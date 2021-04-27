using System;

namespace WareHouseSys.ViewModels
{
    public class TransferHeaderViewModel
    {
        public string OrderNo { get; set; }

        public DateTime? TransferDate { get; set; }

        public string Status { get; set; }

        public string ApplicantMan { get; set; }

        public string ApplicantId { get; set; }

        public string TransferOutMan { get; set; }

        public string TransferOutManId { get; set; }

        public string TransferInMan { get; set; }

        public string TransferInManId { get; set; }

        public string WGroupId { get; set; }

        public DateTime? AddDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }
    }
}