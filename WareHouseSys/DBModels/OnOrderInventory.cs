using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class OnOrderInventory
    {
        
        public string MaterialNo {get;set;}

        public int? OnOrderInventoryQty {get;set;}

        public DateTime AddDateTime {get;set;}

        public DateTime UpdateDateTime {get;set;}

    }
}
