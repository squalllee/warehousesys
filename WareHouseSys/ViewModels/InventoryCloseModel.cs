using System.Collections.Generic;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class InventoryCloseModel
    {
        public string OrderNo { get; set; }
        public List<Attachment> attchments { get; set; }
    }
}