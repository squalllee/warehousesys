using System.Collections.Generic;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class ReqMaterialReviewModel
    {
        public string OrderNo { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}