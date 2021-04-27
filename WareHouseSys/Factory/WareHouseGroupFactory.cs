using SqlSugar;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using WareHouseSys.DBModels;

namespace WareHouseSys.Factory
{
    public class WareHouseGroupFactory
    {
        static public List<WGroup> getWareHouseGroup()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<WGroup> wGroups = db.Queryable<WGroup>().ToList();

            return wGroups;
        }
        static public List<string> getWareHouseIdByUser(string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<string> whouseIdList = db.Ado.SqlQuery<string>("select WareHouseId from WHouseGroupUser " +
                "inner join WareHouseGroup on WHouseGroupUser.WGroupId = WareHouseGroup.WGroupId " +
                "where KEYNO=@ID",new { ID=ID});

            return whouseIdList;
        }

        static public List<string> getWGroupIdByUser(string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<string> WGroupIdList = db.Ado.SqlQuery<string>("select distinct WGroupId from WHouseGroupUser where KEYNO=@ID", new { ID = ID });

            return WGroupIdList;
        }

        static public List<string> getWareHouseIdByGroup(string WGroupId)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<string> WareHouseIdList = db.Ado.SqlQuery<string>("select distinct WarehouseId from WareHouseGroup where WGroupId=@WGroupId", new { WGroupId = WGroupId });

            return WareHouseIdList;
        }
    }
}