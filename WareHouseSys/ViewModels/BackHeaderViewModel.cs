using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class BackHeaderViewModel:BackHeader
    {
        public string BackManId { get; set; }

        public string BackUnitId { get; set; }

        public string InBoundManId { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime ExtendDate { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}