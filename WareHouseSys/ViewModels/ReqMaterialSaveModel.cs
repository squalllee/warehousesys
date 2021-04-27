using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class ReqMaterialSaveModel
    {
        public ReqMaterialHeader reqMaterialHeader { get; set; }

        public List<ReqMaterialBody> reqMaterialBodies { get; set; }
    }
}