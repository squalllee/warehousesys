using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class BackSaveModel
    {
        public BackHeaderViewModel backHeaderViewModel { get; set; }

        public List<BackBodyViewModel> backBodies { get; set; }

        public List<Attachment> attachment { set; get; }
    }
}