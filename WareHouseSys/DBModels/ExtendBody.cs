using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class ExtendBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public string Lot { get; set; }

        public int? Quantity {get;set;}

    }
}
