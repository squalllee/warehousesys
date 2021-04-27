using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class sysdiagrams
    {
        
        public string name {get;set;}

        public int principal_id {get;set;}

        public int diagram_id {get;set;}

        public int? version {get;set;}

        public Byte[] definition {get;set;}

    }
}
