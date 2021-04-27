using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class LendSaveModel
    {
        public LendHeaderViewModel lendHeaderViewModel { get; set; }

        public List<LendBodyViewModel> LendBodies { get; set; }

        public List<Attachment> attachment { set; get; }
    }
}