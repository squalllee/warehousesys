using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using WareHouseSys.DBModels;

namespace WareHouseSys.Factory
{
    public class StorageInfoFactory
    {
        static public List<StorageInfo> getStorageInfo(string WarehouseId)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<StorageInfo> storageInfos = db.Queryable<StorageInfo>().Where(e=>e.WarehouseId == WarehouseId).ToList();

            return storageInfos;
        }

        static public StorageInfo getStorageSigleInfo(string StorageId)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            StorageInfo storageInfo = db.Queryable<StorageInfo>().Where(e => e.StorageId == StorageId).Single();

            return storageInfo;
        }
    }
}