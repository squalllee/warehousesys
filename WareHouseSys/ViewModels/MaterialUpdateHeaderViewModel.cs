using System.Collections.Generic;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class MaterialUpdateHeaderViewModel: MaterialUpdateHeader
    {
        public string ReqManId { get; set; }

        public string ReqUnitId { get; set; }

        public string StatusId { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}