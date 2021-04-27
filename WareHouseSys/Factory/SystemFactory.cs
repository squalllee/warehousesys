using SqlSugar;
using System.Collections.Generic;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.Factory
{
    public class SystemFactory
    {
        static public List<MainSystem> GetSystems()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            List<MainSystem> mainSystems = db.Queryable<MainSystem>().ToList();

            return mainSystems;
        }

        static public List<SubSystem> GetSubSystems()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            List<SubSystem> subSystems = db.Queryable<SubSystem>().ToList();

            return subSystems;
        }

        static public List<SubSystem> GetSubSystems(string SystemId)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            List<SubSystem> subSystems = db.Queryable<SubSystem>().Where(e=>e.SystemId == SystemId).ToList();

            return subSystems;
        }
    }
}