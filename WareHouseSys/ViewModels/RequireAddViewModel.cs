using System;
using System.Collections.Generic;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class RequireAddViewModel
    {
        public List<Employee> Users { get; set; }

        public List<UNIT> Units { get; set; }

        public PurchaseHeaderViewModel purchaseHeader { get; set; }


    }
}