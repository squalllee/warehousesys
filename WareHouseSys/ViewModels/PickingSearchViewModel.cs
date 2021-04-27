using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class PickingSearchViewModel:PickingBody
    {
        public string PickingMan { get; set; }
        public string  WorkNo { get; set; }
        public string UNITNAME { get; set; }
        public string TMNAME { get; set; }

        public string OutBoundMan { get; set; }
        public DateTime OutBoundDate { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string WareHouseName { get; set; }
        public string Status { get; set; }
        public string EmergencyPicking { get; set; }

        public string AttUrl { get; set; }

    }
}