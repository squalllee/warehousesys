using SqlSugar;
using System;
using System.Collections.Generic;
using System.Globalization;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class StockInventoryFactory
    {
        public static ISugarQueryable<StockInventoryHeaderViewModel> getStockInventoryHeaderViewModel(FilterCriteria filter, List<string> wgroupIdList)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<StockInventoryHeaderViewModel> sugarQueryable = db.SqlQueryable<StockInventoryHeaderViewModel>("SELECT OrderNo,(select KeyNo from Employee where KeyNo = InventoryMan) InventoryMan,InventoryMan InventoryManId," +
                 " InventoryDate, InventoryWarHouse InventoryWarHouseId, InventoryUnit, Period, InventoryAttr, WGroupId,  " +
                 "(select WareHouseName from WareHouseInfo where WarehouseId = InventoryWarHouse) InventoryWarHouse, " +
                 " CASE WHEN Status = '1' THEN '完成初盤' WHEN Status = '2' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status, AddDateTime, UpdateDateTime FROM  StockInventoryHeader ");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => wgroupIdList.Contains(e.WGroupId));

            return sugarQueryable;
        }

        public static ISugarQueryable<StockInventoryHeaderViewModel> getStockInventoryHeaderViewModelByOrderNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<StockInventoryHeaderViewModel> sugarQueryable = db.SqlQueryable<StockInventoryHeaderViewModel>("SELECT OrderNo,(select KeyNo from Employee where KeyNo = InventoryMan) InventoryMan,InventoryMan InventoryManId," +
                 " InventoryDate, InventoryWarHouse InventoryWarHouseId, InventoryUnit, Period, InventoryAttr, WGroupId,  " +
                 "(select WareHouseName from WareHouseInfo where WarehouseId = InventoryWarHouse) InventoryWarHouse, " +
                 " CASE  WHEN Status = '1' THEN '完成初盤' WHEN Status = '2' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status, AddDateTime, UpdateDateTime FROM  StockInventoryHeader ");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static ISugarQueryable<StockInventoryBodyViewModel> getStockInventoryBodyViewModelByOrderNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<StockInventoryBodyViewModel> sugarQueryable = db.SqlQueryable<StockInventoryBodyViewModel>("select OrderNo, stockInventoryBody.SerialNo, stockInventoryBody.MaterialNo,MaterialName,Spec,Unit, stockInventoryBody.WarehouseId,WarehouseInfo.WareHouseName,  " +
                " StorageId, Quantity, FirstCheckQty, SecondCheckQty,Quantity-SecondCheckQty diffQty,Note from stockInventoryBody " +
                "inner join WarehouseInfo on stockInventoryBody.WarehouseId = WarehouseInfo.WarehouseId  " +
                " inner join MaterialInfo on stockInventoryBody.MaterialNo= MaterialInfo.MaterialNo");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static ISugarQueryable<StockInventoryBodyViewModel> getStockInventoryDiffBodyViewModel(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<StockInventoryBodyViewModel> sugarQueryable = db.SqlQueryable<StockInventoryBodyViewModel>("select OrderNo, stockInventoryBody.SerialNo, stockInventoryBody.MaterialNo,MaterialName,Spec,Unit, stockInventoryBody.WarehouseId,WarehouseInfo.WareHouseName,  " +
                " StorageId, Quantity, FirstCheckQty, SecondCheckQty,Quantity-SecondCheckQty diffQty,Note from stockInventoryBody " +
                "inner join WarehouseInfo on stockInventoryBody.WarehouseId = WarehouseInfo.WarehouseId  " +
                " inner join MaterialInfo on stockInventoryBody.MaterialNo= MaterialInfo.MaterialNo");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo && (e.diffQty != 0 || e.Note != ""));

            return sugarQueryable;
        }

        public static bool saveInventory(InventorySaveModel inventorySaveModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            StockInventoryHeader stockInventoryHeader = new StockInventoryHeader
            {
                OrderNo = getOrderNo(),
                AddDateTime = DateTime.Now,
                InventoryMan = inventorySaveModel.StockInventoryHeaderViewModel.InventoryMan,
                UpdateDateTime = DateTime.Now,
                WGroupId = inventorySaveModel.StockInventoryHeaderViewModel.WGroupId,
                InventoryAttr = inventorySaveModel.StockInventoryHeaderViewModel.InventoryAttr,
                InventoryDate = inventorySaveModel.StockInventoryHeaderViewModel.InventoryDate,
                InventoryUnit = inventorySaveModel.StockInventoryHeaderViewModel.InventoryUnit,
                WareHouseMgr = inventorySaveModel.StockInventoryHeaderViewModel.WareHouseMgr,
                InventoryWarHouse = inventorySaveModel.StockInventoryHeaderViewModel.InventoryWarHouse,
                Period = inventorySaveModel.StockInventoryHeaderViewModel.Period,
                Status = "0"
            };

            List<StockInventoryBody> stockInventoryBodies = new List<StockInventoryBody>();

            int count = 1;
            foreach (StockInventoryBodyViewModel stockInventoryBodyViewModel in inventorySaveModel.stockInventoryBodyViewModels)
            {
                stockInventoryBodies.Add(new StockInventoryBody
                {
                    OrderNo = stockInventoryHeader.OrderNo,
                    MaterialNo = stockInventoryBodyViewModel.MaterialNo,
                    WarehouseId = stockInventoryBodyViewModel.WarehouseId,
                    Quantity = stockInventoryBodyViewModel.Quantity,
                    StorageId = stockInventoryBodyViewModel.StorageId,
                    SerialNo = count.ToString("0000"),
                    FirstCheckQty = stockInventoryBodyViewModel.Quantity,
                    SecondCheckQty = stockInventoryBodyViewModel.Quantity
                });
                count++;
            }

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(stockInventoryHeader).ExecuteCommand();
                db.Insertable(stockInventoryBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;

        }

        public static bool updateFirstCheckQty(List<StockInventoryBodyViewModel> stockInventoryBodyViewModels)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            StockInventoryBody stockInventoryBody = null;
            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                foreach(StockInventoryBodyViewModel stockInventoryBodyViewModel in stockInventoryBodyViewModels)
                {
                    stockInventoryBody = db.Queryable<StockInventoryBody>().Where(e => e.OrderNo == stockInventoryBodyViewModel.OrderNo && e.SerialNo == stockInventoryBodyViewModel.SerialNo).Single();

                    stockInventoryBody.FirstCheckQty = stockInventoryBodyViewModel.FirstCheckQty;
                    stockInventoryBody.SecondCheckQty = stockInventoryBodyViewModel.FirstCheckQty;
                    stockInventoryBody.Note = stockInventoryBodyViewModel.Note;
                    db.Updateable(stockInventoryBody).ExecuteCommand();
                }
                
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;
        }

        public static bool setFirstCheckComplete(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            StockInventoryHeader stockInventoryHeader = db.Queryable<StockInventoryHeader>().Where(e => e.OrderNo == OrderNo).Single();
           
            stockInventoryHeader.Status = "1";
            
            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                db.Updateable(stockInventoryHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;
        }

        public static bool updateSecondCheckQty(List<StockInventoryBodyViewModel> stockInventoryBodyViewModels)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            StockInventoryBody stockInventoryBody = null;
            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                foreach(StockInventoryBodyViewModel stockInventoryBodyViewModel in stockInventoryBodyViewModels)
                {
                    stockInventoryBody = db.Queryable<StockInventoryBody>().Where(e => e.OrderNo == stockInventoryBodyViewModel.OrderNo && e.SerialNo == stockInventoryBodyViewModel.SerialNo).Single();

                    stockInventoryBody.SecondCheckQty = stockInventoryBodyViewModel.SecondCheckQty;
                    stockInventoryBody.Note = stockInventoryBodyViewModel.Note;
                    db.Updateable(stockInventoryBody).ExecuteCommand();
                }
               
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;
        }

        public static bool setSecondCheckComplete(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            StockInventoryHeader stockInventoryHeader = db.Queryable<StockInventoryHeader>().Where(e => e.OrderNo == OrderNo).Single();

            stockInventoryHeader.Status = "2";

            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                db.Updateable(stockInventoryHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;
        }

        public static bool deleteInventory(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            bool retValue = true;

            db.Ado.BeginTran();
            try
            {
                db.Ado.ExecuteCommand("delete from StockInventoryBody where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                db.Ado.ExecuteCommand("delete from StockInventoryHeader where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                db.Ado.CommitTran();
            }
            catch
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;

        }

        public static bool InventoryClose(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            StockInventoryHeader stockInventoryHeader = db.Queryable<StockInventoryHeader>().Where(e => e.OrderNo == OrderNo).Single();

            stockInventoryHeader.Status = "2";

            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                db.Updateable(stockInventoryHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;
        }

        static public string getOrderNo()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = "J" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM StockInventoryHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
            var OrderNo = "";
            try
            {
                OrderNo = db.Ado.SqlQuerySingle<string>(sql, new { OrderPrefix = OrderPrefix });
            }
            catch (Exception ex)
            {

            }

            return OrderPrefix + "-" + (int.Parse(OrderNo.Split('-')[1]) + 1).ToString("0000");
        }

    }
}