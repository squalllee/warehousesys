using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class ExtendSaveModel
    {
        public ExtendHeaderViewModel extendHeaderViewModel { get; set; }

        public List<ExtendBodyViewModel> extendBodies { get; set; }

        public List<Attachment> attachment { set; get; }
    }
}