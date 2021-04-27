using Kendo.Mvc.UI;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class MaterialFactory
    {
        static public List<MaterialInfo> getMaterialByTerm(string term)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<MaterialInfo> materialInfos = db.Queryable<MaterialInfo>().Where(e => e.MaterialNo.Contains(term) || e.MaterialName.Contains(term)).ToList();

            return materialInfos;
        }

        static public decimal getMaterialPrice(string MaterialNo,string Lot)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<decimal> prices = db.Ado.SqlQuery<decimal>("select InboundBody.Price from InboundBody " +
                "inner join InboundHeader on InboundBody.OrderNo = InboundHeader.OrderNo " +
                "inner join PurchaseHeader on InboundHeader.PurNo = PurchaseHeader.PurchaseNo " +
                "inner join PurchaseBody on PurchaseHeader.PurchaseNo = PurchaseBody.PurchaseNo " +
                "where InboundBody.MaterialNo = @MaterialNo and InboundBody.Lot = @Lot",new { MaterialNo= MaterialNo, Lot = Lot });

            if(prices.Count == 0)
            {
                return 0;
            }
            else
            {
                return prices[0];
            }
        }

        public static List<MaterialInfo> getHarmMaterialInfo()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<MaterialInfo> MaterialInfo = null;

            try
            {
                MaterialInfo = db.Queryable<MaterialInfo>().Where(e => !e.Freeze && e.IsDangerous).ToList();
            }
            catch (Exception ex)
            {

            }


            return MaterialInfo;
        }
        

        public static List<MaterialInfo> getMaterialInfo()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<MaterialInfo> MaterialInfo = null;

            try
            {
                MaterialInfo = db.Queryable<MaterialInfo>().Where(e=>!e.Freeze).ToList();
            }
            catch (Exception ex)
            {

            }


            return MaterialInfo;
        }

        public static bool IsPurchased(string MaterialNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            int count = db.Queryable<PurchaseBody>().Where(e => e.MaterialNo == MaterialNo).Count();

            return count != 0;
        }

        public static MaterialInfoViewModel getMaterialViewModelInfo(string MaterialNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            MaterialInfoViewModel materialInfoViewModels = null;

            try
            {
                materialInfoViewModels = db.SqlQueryable<MaterialInfoViewModel>("select MaterialNo, MaterialName, Spec, MaterialInfo.SystemId,MainSystem.SystemName, LineAbb, MaterialInfo.SubSystemId,SubSystem.SubSystemName," +
                    "SerialNo, FixClass,FixClass.ClassName FixClassName, MaterialInfo.AffectClass,AffectClass.ClassName AffectClassName," +
                    "MaterialInfo.VendorId,VendorInfo.VendorName, Unit, Length, Witdh, Height, weight, ReplaceNo, ROP, EqQuantity, EstPurPeriod, EstAnnConsumption," +
                    "SpecifyBrand, IsFix, IsDangerous, IsLimitTime, Expiration, SafetyStock, FailureRate " +
                    "from MaterialInfo inner join MainSystem on MaterialInfo.SystemId = MainSystem.SystemId " +
                    "left join SubSystem on MainSystem.SystemId = MaterialInfo.SystemId and MaterialInfo.SubSystemId = SubSystem.SubSystemId " +
                    "left join VendorInfo on MaterialInfo.VendorId = VendorInfo.VendorId " +
                    "left join FixClass on MaterialInfo.FixClass = FixClass.ClassId " +
                    "left join AffectClass on MaterialInfo.AffectClass = AffectClass.ClassId" +
                      "where Freeze='0' ").Where(e=>e.MaterialNo == MaterialNo).Single();
            }
            catch (Exception ex)
            {

            }


            return materialInfoViewModels;
        }


        public static ISugarQueryable<MaterialInfoViewModel> getMaterialViewModelInfo(FilterCriteria filter)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<MaterialInfoViewModel> sugarQueryable = null;

            try
            {
                sugarQueryable = db.SqlQueryable<MaterialInfoViewModel>("select MaterialNo, MaterialName, Spec, MaterialInfo.SystemId,MainSystem.SystemName, LineAbb, MaterialInfo.SubSystemId,SubSystem.SubSystemName," +
                    "SerialNo, FixClass,FixClass.ClassName FixClassName, MaterialInfo.AffectClass,AffectClass.ClassName AffectClassName," +
                    "MaterialInfo.VendorId,VendorInfo.VendorName, Unit, Length, Witdh, Height, weight, ReplaceNo, ROP, EqQuantity, EstPurPeriod, EstAnnConsumption," +
                    "SpecifyBrand, IsFix, IsDangerous, IsLimitTime, Expiration, SafetyStock, FailureRate,Freeze " +
                    "from MaterialInfo inner join MainSystem on MaterialInfo.SystemId = MainSystem.SystemId " +
                    "left join SubSystem on SubSystem.SystemId = MaterialInfo.SystemId and MaterialInfo.SubSystemId = SubSystem.SubSystemId " +
                    "left join VendorInfo on MaterialInfo.VendorId = VendorInfo.VendorId " +
                    "left join FixClass on MaterialInfo.FixClass = FixClass.ClassId " +
                    "left join AffectClass on MaterialInfo.AffectClass = AffectClass.ClassId " );

                sugarQueryable = DBUtility.Query(sugarQueryable, filter);
            }
            catch (Exception ex)
            {

            }


            return sugarQueryable;
        }

        public static ISugarQueryable<MaterialInfo> getMaterialBasicInfo(DataSourceRequest request)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<MaterialInfo> sugarQueryable = null;

            try
            {
                sugarQueryable = db.Queryable<MaterialInfo>();

                sugarQueryable = DBUtility.Query(sugarQueryable, request);
            }
            catch (Exception ex)
            {

            }


            return sugarQueryable;
        }

        public static bool addMaterialBasicInfo(MaterialInfo materialInfo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);


            return db.Insertable(materialInfo).ExecuteCommand() > 0;
        }

        public static bool updateMaterialBasicInfo( MaterialInfo materialInfo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);


            return db.Updateable(materialInfo).ExecuteCommand() > 0;
        }

        public static bool deleteMaterialBasicInfo(MaterialInfo materialInfo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);


            return db.Deleteable(materialInfo).ExecuteCommand() > 0;
        }

        public static List<RequireMaterialViewModel> getMaterialInfo(string MaterialNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<RequireMaterialViewModel> MaterialInfo = null;

            try
            {
                MaterialInfo = db.SqlQueryable<RequireMaterialViewModel>("SELECT MaterialNo, MaterialName, Spec, Unit, SystemId, SubSystemId, LineAbb, Length, SerialNo, FixClass," +
                                                                    "AffectClass, VendorId, Witdh, Height, weight, ReplaceNo, EqQuantity, IsFix," +
                                                                    "IsDangerous, IsLimitTime, Expiration, SafetyStock, FailureRate, UpdateDateTime, UpdateMan, " +
                                                                    "(SELECT isnull(sum(Quantity),0) Quantity FROM Inventory where MaterialNo = MaterialInfo.MaterialNo group by MaterialNo) Quantity, " +
                                                                    "(SELECT isnull(OnOrderInventoryQty,'0') OnOrderInventoryQty FROM OnOrderInventory where MaterialNo = MaterialInfo.MaterialNo) OnOrderInventory, " +
                                                                    "(SELECT isnull(sum(Quantity),'0') Quantity FROM Inventory where MaterialNo =  ReplaceNo group by MaterialNo) ReplaceQuantity " +
                                                                    "FROM  MaterialInfo where Freeze = '0' ").Where(e=>e.MaterialNo == MaterialNo).ToList();
            }
            catch (Exception ex)
            {

            }


            return MaterialInfo;
        }

        public static List<MaterialComboViewModel> getMaterialInfo(string MaterialNo,string WGroupId)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<string> WareHouseIdList = WareHouseGroupFactory.getWareHouseIdByGroup(WGroupId);

            ISugarQueryable<MaterialComboViewModel> sugarQueryable = db.SqlQueryable<MaterialComboViewModel>("select Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,Inventory.Lot,Inventory.StorageId,sum(Quantity) Qty from Inventory " +
                " inner join WarehouseInfo on Inventory.WarehouseId = WarehouseInfo.WarehouseId " +
                "inner join MaterialInfo on Inventory.MaterialNo = MaterialInfo.MaterialNo  " +
                  "where Freeze='0' " +
                " Group by Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,Inventory.Lot,Inventory.StorageId");

            sugarQueryable = sugarQueryable.Where(e => e.MaterialNo == MaterialNo && WareHouseIdList.Contains(e.WarehouseId));

            return sugarQueryable.ToList();
        }

        public static List<MaterialComboViewModel> getMaterialInfoAll(string WGroupId)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<string> WareHouseIdList = WareHouseGroupFactory.getWareHouseIdByGroup(WGroupId);

            ISugarQueryable<MaterialComboViewModel> sugarQueryable = db.SqlQueryable<MaterialComboViewModel>("select Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,Inventory.Lot,Inventory.StorageId,sum(Quantity) Qty from Inventory " +
                " inner join WarehouseInfo on Inventory.WarehouseId = WarehouseInfo.WarehouseId " +
                "inner join MaterialInfo on Inventory.MaterialNo = MaterialInfo.MaterialNo  " +
                 "where Freeze='0' " +
                " Group by Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,Inventory.Lot,Inventory.StorageId");

            sugarQueryable = sugarQueryable.Where(e =>  WareHouseIdList.Contains(e.WarehouseId));

            return sugarQueryable.ToList();
        }
        /*
        public static List<MaterialComboViewModel> getMaterialInfoByNo(string MaterialNo, string WGroupId)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<string> WareHouseIdList = WareHouseGroupFactory.getWareHouseIdByGroup(WGroupId);

            ISugarQueryable<MaterialComboViewModel> sugarQueryable = db.SqlQueryable<MaterialComboViewModel>("select Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,Inventory.Lot,Inventory.StorageId,sum(Quantity) Qty from Inventory " +
                " inner join WarehouseInfo on Inventory.WarehouseId = WarehouseInfo.WarehouseId " +
                "inner join MaterialInfo on Inventory.MaterialNo = MaterialInfo.MaterialNo  " +
                 "where Freeze='0' " +
                " Group by Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,Inventory.Lot,Inventory.StorageId");

            sugarQueryable = sugarQueryable.Where(e => WareHouseIdList.Contains(e.WarehouseId));

            return sugarQueryable.ToList();

        }

        */
        public static ISugarQueryable<MaterialComboViewModel> getMaterialInfoAll()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<MaterialComboViewModel> sugarQueryable = db.SqlQueryable<MaterialComboViewModel>("select Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,MaterialInfo.Unit,Inventory.Lot,Inventory.StorageId,sum(Quantity)-sum(LendQty) Qty from Inventory " +
                " inner join WarehouseInfo on Inventory.WarehouseId = WarehouseInfo.WarehouseId " +
                "inner join MaterialInfo on Inventory.MaterialNo = MaterialInfo.MaterialNo  " +
                "where Freeze='0' " +
                " Group by Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,Inventory.Lot,Inventory.StorageId,MaterialInfo.Unit");

            return sugarQueryable;
        }

        public static ISugarQueryable<MaterialComboViewModel> getMaterialInfoByWareHouseId(string WareHouseId)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<MaterialComboViewModel> sugarQueryable = db.SqlQueryable<MaterialComboViewModel>("select Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,Inventory.StorageId,sum(Quantity) Qty from Inventory " +
                " inner join WarehouseInfo on Inventory.WarehouseId = WarehouseInfo.WarehouseId " +
                "inner join MaterialInfo on Inventory.MaterialNo = MaterialInfo.MaterialNo  " +
                " Group by Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,Inventory.StorageId");

            sugarQueryable = sugarQueryable.Where(e => e.WarehouseId == WareHouseId);

            return sugarQueryable;
        }

        public static List<MaterialComboViewModel> getMaterialInfoByWareHouse(string WGroupId)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<string> WareHouseIdList = WareHouseGroupFactory.getWareHouseIdByGroup(WGroupId);

            ISugarQueryable<MaterialComboViewModel> sugarQueryable = db.SqlQueryable<MaterialComboViewModel>("select Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.Lot,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,sum(Quantity) Qty from Inventory " +
                " inner join WarehouseInfo on Inventory.WarehouseId = WarehouseInfo.WarehouseId " +
                "inner join MaterialInfo on Inventory.MaterialNo = MaterialInfo.MaterialNo  " +
                "where Freeze='0' " +
                " Group by Inventory.MaterialNo,MaterialInfo.MaterialName,Inventory.WarehouseId,WarehouseInfo.WareHouseName,MaterialInfo.Spec,Inventory.Lot");

            sugarQueryable = sugarQueryable.Where(e => WareHouseIdList.Contains(e.WarehouseId));

            return sugarQueryable.ToList();
        }

        public static bool updateMaterialInfo(MaterialInfo materialInfo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            return db.Updateable(materialInfo).ExecuteCommand() > 0;
        }

        public static ISugarQueryable<MaterialInfoViewModel> getMaterialViewModelInfo(DataSourceRequest request)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<MaterialInfoViewModel> sugarQueryable = null;

            try
            {
                sugarQueryable = db.SqlQueryable<MaterialInfoViewModel>("select distinct MaterialNo, MaterialName, Spec, MaterialInfo.SystemId,MainSystem.SystemName, LineAbb, MaterialInfo.SubSystemId,SubSystem.SubSystemName," +
                    "SerialNo, FixClass,FixClass.ClassName FixClassName, MaterialInfo.AffectClass,AffectClass.ClassName AffectClassName," +
                    "MaterialInfo.VendorId,VendorInfo.VendorName, Unit, Length, Witdh, Height, weight, ReplaceNo, ROP, EqQuantity, EstPurPeriod, EstAnnConsumption," +
                    "SpecifyBrand, IsFix, IsDangerous, IsLimitTime, Expiration, SafetyStock, FailureRate,Freeze " +
                    "from MaterialInfo inner join MainSystem on MaterialInfo.SystemId = MainSystem.SystemId " +
                    "left join SubSystem on SubSystem.SystemId = MaterialInfo.SystemId and MaterialInfo.SubSystemId = SubSystem.SubSystemId " +
                    "left join VendorInfo on MaterialInfo.VendorId = VendorInfo.VendorId " +
                    "left join FixClass on MaterialInfo.FixClass = FixClass.ClassId " +
                    "left join AffectClass on MaterialInfo.AffectClass = AffectClass.ClassId where Freeze = 0");
            }
            catch (Exception ex)
            {

            }
            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

    }
}