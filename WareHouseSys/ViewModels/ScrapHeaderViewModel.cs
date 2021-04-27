using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class ScrapHeaderViewModel:ScrapHeader
    {
        public string ApplyManId { get; set; }
        public string ApplyUnitId { get; set; }
        public string StatusId { get; set; }

        public new List<string> WorkNo
        {
            get
            {
                if (base.WorkNo != null)
                    return new List<string>(base.WorkNo.Split(','));
                else
                    return null;
            }
            set
            {
                base.WorkNo = String.Join(", ", value.ToArray());
            }
        }

        public List<Attachment> attachments { get; set; }
    }
}