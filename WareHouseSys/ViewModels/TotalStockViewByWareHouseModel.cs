using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class TotalStockViewByWareHouseModel: TotalStockViewModel
    {
        public string WareHouseName { get; set; }

        public string StorageId { get; set; }
    }
}