using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class MaterialUpdateReviewModel
    {
        public string OrderNo { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}