using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class WareHouseViewModel:WarehouseInfo
    {
        public string StorageId { get; set; }
    }
}