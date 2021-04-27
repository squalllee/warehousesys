using System.Collections.Generic;

namespace WareHouseSys.ViewModels
{
    public class NewScrapViewModel
    {
        public ScrapHeaderViewModel scrapHeaderViewModel { get; set; }
        public List<ScrapBodyViewModel> scrapBodyViewModels { get; set; }
    }
}