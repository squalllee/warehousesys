using System;
using System.Linq;
using System.Text;

namespace WareHouseSys.DBModels
{
    public class Employee
    {
        
        public string KEYNO {get;set;}

        public string TMNAME {get;set;}

        public string USERPWD {get;set;}

        public string EMAIL {get;set;}

        public string TelExtension {get;set;}

        public string UNITNO {get;set;}

        public string JOBName {get;set;}

        public string OFFJOBDATE {get;set;}

        public DateTime? UpdatedTime {get;set;}

        public DateTime? CreatedTime {get;set;}

    }
}
