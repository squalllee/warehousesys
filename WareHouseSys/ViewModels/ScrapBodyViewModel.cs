using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class ScrapBodyViewModel : ScrapBody
    {
        public string MaterialName { get; set; }

        public string Spec { get; set; }

        public string WareHouseName { get; set; }

        public new List<string> MaterialClass
        {
            get
            {
                if (base.MaterialClass != null)
                    return new List<string>(base.MaterialClass.Split(','));
                else
                    return null;
            }
            set
            {
                base.MaterialClass = String.Join(", ", value.ToArray());
            }
        }
    }
}