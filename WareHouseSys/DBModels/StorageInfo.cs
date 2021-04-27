using System;
using System.Linq;
using System.Text;

namespace WareHouseSys.DBModels
{
    public class StorageInfo
    {
        
        public string StorageId {get;set;}

        public string WarehouseId {get;set;}

        public string Area {get;set;}

        public int? Col {get;set;}

        public int? Layer {get;set;}

        public int? Grid {get;set;}

        public Double? Width {get;set;}

        public Double? Depth {get;set;}

        public Double? Height {get;set;}

        public Double? Volume {get;set;}

        public Double? Load {get;set;}

    }
}
