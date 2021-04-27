using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class TransferSaveModel
    {
        public string OrderNo { get; set; }

        public DateTime? TransferDate { get; set; }

        public string ApplicantMan { get; set; }

        public string ApplicantId { get; set; }

        public string TransferOutMan { get; set; }

        public string TransferOutId { get; set; }

        public string TransferInMan { get; set; }

        public string TransferInManId { get; set; }

        public string WGroupId { get; set; }

        public List<TransferBodyViewModel> TransferBodies { get; set; }

        public List<Attachment> attachment { set; get; }
    }
}