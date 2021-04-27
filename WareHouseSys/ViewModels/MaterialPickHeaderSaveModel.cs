using System;
using System.Collections.Generic;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class MaterialPickHeaderSaveModel: MaterialPickHeaderViewModel
    {
        public List<Attachment> attachments { get; set; }
    }
}