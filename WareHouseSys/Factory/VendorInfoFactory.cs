using SqlSugar;
using System.Collections.Generic;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.Factory
{
    public class VendorInfoFactory
    {
        public static List<VendorInfo> getVendorInfo()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            List<VendorInfo> vendorInfos = db.Queryable<VendorInfo>().ToList(); ;

            return vendorInfos;
        }
    }
}