using System.Collections.Generic;

using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class DemandHeaderViewModel: DemandHeader
    {
        public string ApplyUnitId { get; set; }

        public string ApplyManId { get; set; }

        public string StatusId { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}