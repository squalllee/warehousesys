using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class InboundSearchViewModel: InboundBody
    {

        public string PurNo { get; set; }

        public string InboundMan { get; set; }

        public string DeliveryLot { get; set; }

        public DateTime? InboundDate { get; set; }

        public string Status { get; set; }

        public string UNITNAME { get; set; }

        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string WareHouseName { get; set; }

        public string AttUrl { get; set; }
    }
}