using System;
using System.Linq;
using System.Text;
namespace WareHouseSys.DBModels
{
    public class JobList
    {
        
        public int JobId {get;set;}

        public string JobName {get;set;}

        public string OrderNo {get;set;}

        public string CaseOfficer {get;set;}

        public string ExcuteResult {get;set;}

        public string ErrorMssage {get;set;}

    }
}
