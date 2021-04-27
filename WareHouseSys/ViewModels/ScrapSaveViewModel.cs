using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class ScrapSaveViewModel
    {
        public string OrderNo { get; set; }

        public ScrapHeaderViewModel scrapHeaderViewModel { get; set; }

        public List<PickingToScrapViewModel> ScrapBodies { get; set; }
    }
}