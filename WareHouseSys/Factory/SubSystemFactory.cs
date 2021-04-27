using SqlSugar;
using System.Collections.Generic;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.Factory
{
    public class SubSystemFactory
    {
        static public List<SubSystem> GetSubSystems()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            List<SubSystem> subSystems = db.Queryable<SubSystem>().ToList();

            return subSystems;
        }
        
    }
}