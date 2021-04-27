using Kendo.Mvc.UI;
using SqlSugar;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class StockFactory
    {
        public static ISugarQueryable<TotalStockViewModel> getTotalStock(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<TotalStockViewModel> sugarQueryable = db.SqlQueryable<TotalStockViewModel>("select Inventory.MaterialNo, MaterialInfo.MaterialName,MaterialInfo.Spec," +
                 "MaterialInfo.Unit, sum(Quantity) Quantity, sum(LendQty) LendQty,(sum(Quantity) - sum(LendQty)) Qty  " +
                 "from Inventory inner join MaterialInfo " +
                 "on Inventory.MaterialNo = MaterialInfo.MaterialNo where MaterialInfo.Freeze = 0 group by Inventory.MaterialNo,MaterialName,Spec,Unit");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

        public static ISugarQueryable<TotalStockByLotViewModel> getTotalStockByLot(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<TotalStockByLotViewModel> sugarQueryable = db.SqlQueryable<TotalStockByLotViewModel>("select Inventory.MaterialNo, MaterialInfo.MaterialName,MaterialInfo.Spec," +
                 "MaterialInfo.Unit,Lot,sum(Quantity) Quantity, sum(LendQty) LendQty,(sum(Quantity) - sum(LendQty)) Qty  " +
                 "from Inventory inner join MaterialInfo " +
                 "on Inventory.MaterialNo = MaterialInfo.MaterialNo where MaterialInfo.Freeze = 0 group by Inventory.MaterialNo,MaterialName,Spec,Unit,Lot");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

        public static ISugarQueryable<TotalStockViewByWareHouseModel> getTotalStockByWareHouse(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<TotalStockViewByWareHouseModel> sugarQueryable = db.SqlQueryable<TotalStockViewByWareHouseModel>("select Inventory.MaterialNo, MaterialInfo.MaterialName,MaterialInfo.Spec," +
                 "MaterialInfo.Unit,WarehouseName,StorageId,sum(Quantity) Quantity, sum(LendQty) LendQty,(sum(Quantity) - sum(LendQty)) Qty  " +
                 "from Inventory inner join MaterialInfo " +
                 "on Inventory.MaterialNo = MaterialInfo.MaterialNo " +
                 "inner join WarehouseInfo on WarehouseInfo.WarehouseId = Inventory.WarehouseId  where MaterialInfo.Freeze = 0 " +
                 "group by Inventory.MaterialNo,MaterialName,Spec,Unit,WarehouseName,StorageId");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

        public static ISugarQueryable<TotalStockByWareHouseAndLotViewModel> getTotalStockByWareHouseAndLot(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<TotalStockByWareHouseAndLotViewModel> sugarQueryable = db.SqlQueryable<TotalStockByWareHouseAndLotViewModel>("select Inventory.MaterialNo, MaterialInfo.MaterialName,MaterialInfo.Spec," +
                 "MaterialInfo.Unit,WarehouseName,StorageId,Lot,sum(Quantity) Quantity, sum(LendQty) LendQty,(sum(Quantity) - sum(LendQty)) Qty  " +
                 "from Inventory inner join MaterialInfo " +
                 "on Inventory.MaterialNo = MaterialInfo.MaterialNo " +
                 "inner join WarehouseInfo on WarehouseInfo.WarehouseId = Inventory.WarehouseId  where MaterialInfo.Freeze = 0 " +
                 "group by Inventory.MaterialNo,MaterialName,Spec,Unit,WarehouseName,StorageId,Lot");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

        


    }
}