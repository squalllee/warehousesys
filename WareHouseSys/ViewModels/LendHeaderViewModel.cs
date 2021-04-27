using System.Collections.Generic;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class LendHeaderViewModel:LendHeader
    {

        public string LendManId { get; set; }
        public string LendUnitId { get; set; }
        public string OutBoundManId { get; set; }
        
        public List<Attachment> attachments { get; set; }
    }
}