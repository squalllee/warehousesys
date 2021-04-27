using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class MaterialUpdateSaveModel
    {
        public MaterialUpdateHeader materialUpdateHeader { get; set; }
        public List<MaterialUpdateBody> materialUpdateBodies { get; set; }
    }
}