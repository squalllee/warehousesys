using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class ToolPickHeaderSaveModel: ToolPickHeaderViewModel
    {
        public List<Attachment> attachments { get; set; }
    }
}