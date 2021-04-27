using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.ViewModels
{
    public class TransactionRecordDetailViewModel
    {
        public DateTime transactionDate { set; get; }
        public string className { set; get; }
        public string OrderNo { set; get; }

        public string MaterialNo { set; get; }

        public string MaterialName { set; get; }

        public string WareHouseName { set; get; }

        public Decimal InQty { set; get; }

        public Decimal InPrice { set; get; }

        public Decimal InTotalPrice { set; get; }

        public Decimal OutQty { set; get; }

        public Decimal OutPrice { set; get; }

        public Decimal OutTotalPrice { set; get; }

        public Decimal AdjustQty { set; get; }

        public Decimal AdjustPrice { set; get; }

        public Decimal AdjustTotalPrice { set; get; }
        public Decimal InventoryQty { set; get; }

        public Decimal InventoryPrice { set; get; }

        public Decimal InventoryTotalPrice { set; get; }

        public string Note { set; get; }



    }
}