using System.Collections.Generic;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class InboundSaveModel
    {
        public InboundHeader InboundHeader { get; set; }

        public List<InboundBody> inboundBodies { get; set; }
    }
}