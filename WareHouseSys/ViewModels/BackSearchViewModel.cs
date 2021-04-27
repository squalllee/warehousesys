using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class BackSearchViewModel:BackBody
    {

        public string LendNo { get; set; }

        public string BackMan { get; set; }

        public string InBoundMan { get; set; }

        public DateTime? InBoundDate { get; set; }

        public string Overdue { get; set; }

        public string WGroupId { get; set; }

        public string Note1 { get; set; }

        public string Status { get; set; }

        public string UNITNAME { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string WareHouseName { get; set; }

        public string AttUrl { get; set; }

    }
}