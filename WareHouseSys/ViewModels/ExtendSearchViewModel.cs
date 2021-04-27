using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class ExtendSearchViewModel: ExtendBody
    {

        public string LendNo { get; set; }

        public string ExtendMan { get; set; }

        public string ApprovedMan { get; set; }

        public string ExtendReason { get; set; }

        public int Days { get; set; }

        public string WGroupId { get; set; }

        public string Status { get; set; }

        public DateTime? ExtendDate { get; set; }

        public DateTime? AddDateTime { get; set; }

        public string UNITNAME { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }

        public string AttUrl { get; set; }
    }
}