using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class ExtendHeaderViewModel:ExtendHeader
    {
        public string ExtendManId { get; set; }

        public string ApprovedManId { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}