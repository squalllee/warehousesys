using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class TransferSearchViewModel: TransferBody
    {

        public DateTime? ApplicantDate { get; set; }

        public DateTime? TransferOutDate { get; set; }

        public DateTime? TransferInDate { get; set; }

        public string Status { get; set; }

        public string ApplicantMan { get; set; }

        public string TransferOutMan { get; set; }

        public string TransferInMan { get; set; }

        public string WGroupId { get; set; }

        public string UNITNAME { get; set; }

        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string OutWareHouseName { get; set; }

        public string InWareHouseName { get; set; }

        public string AttUrl { get; set; }
    }
}