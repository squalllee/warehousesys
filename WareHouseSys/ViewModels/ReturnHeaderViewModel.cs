using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class ReturnHeaderViewModel:ReturnHeader
    {
        public string ReturnManId { get; set; }

        public string ReturnUnitId { get; set; }

        public string InboundManId { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}