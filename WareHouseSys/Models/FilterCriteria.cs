using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseSys.Models
{
    public class FilterCriteria
    {
        public List<GridFilter> Filters { get; set; }
        public string Logic { get; set; }
    }

    public class GridFilter
    {
        public string Operator { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }
}