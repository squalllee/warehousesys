using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;

namespace WareHouseSys.ViewModels
{
    public class RequirementHeaderViewModel: RequirementHeader
    {

        public string ApplicantId { get; set; }

        public string ApplicantUnit { get; set; }
    }
}