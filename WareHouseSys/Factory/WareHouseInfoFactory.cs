using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using WareHouseSys.DBModels;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class WarehouseInfoFactory
    {
        static public List<WarehouseInfo> getWarehouseInfo()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<WarehouseInfo> warehouseInfos =  db.Queryable<WarehouseInfo>().ToList();

            return warehouseInfos;
        }

        static public List<WareHouseViewModel> getWarehouseInfoWithStorage()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<WareHouseViewModel> warehouseInfos = db.Ado.SqlQuery<WareHouseViewModel>("select distinct WarehouseInfo.WarehouseId,WarehouseInfo.WareHouseName,StorageId from WarehouseInfo " +
                "inner join StorageInfo on WarehouseInfo.WarehouseId = StorageInfo.WarehouseId");

            return warehouseInfos;
        }

        static public WarehouseInfo getWarehouseInfo(string WarehouseId)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            WarehouseInfo warehouseInfo = db.Queryable<WarehouseInfo>().Where(e=>e.WarehouseId == WarehouseId).Single();

            return warehouseInfo;
        }

        static public List<WGroupViewModel> getWGroupInfo()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<WGroupViewModel> wGroupViewModels = db.Ado.SqlQuery<WGroupViewModel>("SELECT  WGroup.WGroupId,WGroupName,WareHouseGroup.WarehouseId,WarehouseInfo.WareHouseName,WHouseGroupUser.KEYNO,Employee.TMNAME " +
                "FROM WGroup inner join WareHouseGroup on WGroup.WGroupId = WareHouseGroup.WGroupId " +
                "inner join WHouseGroupUser on WGroup.WGroupId = WHouseGroupUser.WGroupId " +
                "inner join Employee on WHouseGroupUser.KEYNO = Employee.KEYNO " +
                "inner join WarehouseInfo on WareHouseGroup.WarehouseId = WarehouseInfo.WarehouseId");

            return wGroupViewModels;
        }


        
    }
}