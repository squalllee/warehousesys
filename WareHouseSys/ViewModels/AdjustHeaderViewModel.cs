using System.Collections.Generic;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class AdjustHeaderViewModel: AdjustHeader
    {
        public string ApplyManId { get; set; }

        public string ApplyUnitId { get; set; }

        public string StatusId { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}