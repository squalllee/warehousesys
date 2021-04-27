using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class LendSearchViewModel:LendBody
    {        

        public string LendMan { get; set; }

        public string OutBoundMan { get; set; }

        public DateTime? OutBoundDate { get; set; }

        public string Status { get; set; }

        public DateTime? ExtendDate { get; set; }

        public string Reason { get; set; }

        public string OtherReason { get; set; }

        public string WGroupId { get; set; }

        public string UNITNAME { get; set; }

        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string WareHouseName { get; set; }

        public string AttUrl { get; set; }

    }
}