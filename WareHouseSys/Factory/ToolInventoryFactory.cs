using SqlSugar;
using System;
using System.Collections.Generic;
using System.Globalization;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class ToolInventoryFactory
    {
        public static ISugarQueryable<ToolInventoryHeaderViewModel> getToolInventoryHeaderViewModel(FilterCriteria filter, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ToolInventoryHeaderViewModel> sugarQueryable = db.SqlQueryable<ToolInventoryHeaderViewModel>("SELECT OrderNo,(select TMNAME from Employee where KeyNo = InventoryMan) InventoryMan,InventoryMan InventoryManId," +
                 " InventoryDate,(select UNITNAME from UNIT where UNITNO=KeepUnit)  KeepUnit,KeepUnit KeepUnitId, Period, InventoryAttr,(select TMNAME from Employee where KeyNo = ToolMgr) ToolMgr,ToolMgr ToolMgrId,  " +
                 "CASE WHEN Status = '1' THEN '完成初盤' WHEN Status = '2' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status, AddDateTime, UpdateDateTime FROM ToolInventoryHeader ");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            List<string> units = db.Queryable<ToolManager>().Where(e => e.ToolMgr == ID).Select(e => e.UNITNO).ToList();

            sugarQueryable = sugarQueryable.Where(e => units.Contains(e.KeepUnitId));

            return sugarQueryable;
        }

        public static ToolInventoryHeaderViewModel getToolInventoryHeaderViewModelByOrderNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ToolInventoryHeaderViewModel> sugarQueryable = db.SqlQueryable<ToolInventoryHeaderViewModel>("SELECT OrderNo,(select TMNAME from Employee where KeyNo = InventoryMan) InventoryMan,InventoryMan InventoryManId," +
                 " InventoryDate,(select UNITNAME from UNIT where UNITNO=KeepUnit)  KeepUnit,KeepUnit KeepUnitId, Period, InventoryAttr,(select TMNAME from Employee where KeyNo = ToolMgr) ToolMgr,ToolMgr ToolMgrId,  " +
                 "CASE WHEN Status = '1' THEN '完成初盤' WHEN Status = '2' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status, AddDateTime, UpdateDateTime FROM ToolInventoryHeader ");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable.Single();
        }

        public static ISugarQueryable<ToolInventoryViewModel> getToolInventoryViewModel(FilterCriteria filter, string UnitId)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

   
            ISugarQueryable<ToolInventoryViewModel> sugarQueryable = db.SqlQueryable<ToolInventoryViewModel>("select ToolInventory.MaterialNo,MaterialName,Spec,Unit, Lot, Quantity,TMNAME KeepMan, " +
                 " KeepMan KeepManId,UNITNAME KeepUnit, KeepUnit KeepUnitId  " +
                 "from ToolInventory inner join Employee on Employee.KEYNO = KeepMan " +
                 "inner join UNIT on UNIT.UNITNO = KeepUnit" +
                 " inner join MaterialInfo on ToolInventory.MaterialNo = MaterialInfo.MaterialNo where Handtool='1'");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.KeepUnitId.StartsWith(UnitId.TrimEnd('0')));

            return sugarQueryable;
        }

        public static bool saveToolInventory(ToolInventorySaveModel toolInventorySaveModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ToolInventoryHeader toolInventoryHeader = new ToolInventoryHeader
            {
                OrderNo = getOrderNo(),
                AddDateTime = DateTime.Now,
                InventoryMan = toolInventorySaveModel.toolInventoryHeader.InventoryMan,
                UpdateDateTime = DateTime.Now,
                InventoryAttr = toolInventorySaveModel.toolInventoryHeader.InventoryAttr,
                InventoryDate = toolInventorySaveModel.toolInventoryHeader.InventoryDate,
                KeepUnit = toolInventorySaveModel.toolInventoryHeader.KeepUnit,
                ToolMgr = toolInventorySaveModel.toolInventoryHeader.ToolMgr,
                Period = toolInventorySaveModel.toolInventoryHeader.Period,
                Status = "0"
            };

            List<ToolInventoryBody> toolInventoryBodies = new List<ToolInventoryBody>();

            int count = 1;
            foreach (ToolInventoryViewModel toolInventoryViewModel in toolInventorySaveModel.toolInventoryViewModels)
            {
                toolInventoryBodies.Add(new ToolInventoryBody
                {
                    OrderNo = toolInventoryHeader.OrderNo,
                    MaterialNo = toolInventoryViewModel.MaterialNo,
                    Quantity = toolInventoryViewModel.Quantity,
                    SerialNo = count.ToString("0000"),
                    FirstCheckQty = toolInventoryViewModel.Quantity,
                    SecondCheckQty = toolInventoryViewModel.Quantity,
                    KeepMan = toolInventoryViewModel.KeepManId
                });
                count++;
            }

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(toolInventoryHeader).ExecuteCommand();
                db.Insertable(toolInventoryBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
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

            string OrderPrefix = "K" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM ToolInventoryHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

        public static bool deleteToolInventory(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            bool retValue = true;

            db.Ado.BeginTran();
            try
            {
                db.Ado.ExecuteCommand("delete from ToolInventoryBody where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                db.Ado.ExecuteCommand("delete from ToolInventoryHeader where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                db.Ado.CommitTran();
            }
            catch
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;

        }

        public static ISugarQueryable<ToolInventoryBodyViewModel> getToolInventoryBodyViewModelByOrderNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ToolInventoryBodyViewModel> sugarQueryable = db.SqlQueryable<ToolInventoryBodyViewModel>("SELECT  OrderNo,ToolInventoryBody.SerialNo,ToolInventoryBody.MaterialNo,MaterialName,MaterialInfo.Spec,Note,Quantity, FirstCheckQty,  " +
                " SecondCheckQty,Unit,(select TMNAME from Employee where KEYNO=KeepMan) KeepMan,Quantity-SecondCheckQty diffQty  FROM ToolInventoryBody " +
                "inner join MaterialInfo on ToolInventoryBody.MaterialNo = MaterialInfo.MaterialNo  ");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static ISugarQueryable<ToolInventoryBodyViewModel> getToolDiffInventoryBodyViewModelByOrderNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ToolInventoryBodyViewModel> sugarQueryable = db.SqlQueryable<ToolInventoryBodyViewModel>("SELECT  OrderNo,ToolInventoryBody.SerialNo,ToolInventoryBody.MaterialNo,MaterialName,MaterialInfo.Spec,Note,Quantity, FirstCheckQty,  " +
                " SecondCheckQty,Unit,(select TMNAME from Employee where KEYNO=KeepMan) KeepMan,Quantity-SecondCheckQty diffQty  FROM ToolInventoryBody " +
                "inner join MaterialInfo on ToolInventoryBody.MaterialNo = MaterialInfo.MaterialNo  ");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo && (e.diffQty != 0 || e.Note != ""));

            return sugarQueryable;
        }

        public static bool updateFirstCheckQty(List<ToolInventoryBodyViewModel> toolInventoryBodyViewModels)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ToolInventoryBody toolInventoryBody = null;
            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                foreach (ToolInventoryBodyViewModel toolInventoryBodyViewModel in toolInventoryBodyViewModels)
                {
                    toolInventoryBody = db.Queryable<ToolInventoryBody>().Where(e => e.OrderNo == toolInventoryBodyViewModel.OrderNo && e.SerialNo == toolInventoryBodyViewModel.SerialNo).Single();

                    toolInventoryBody.FirstCheckQty = toolInventoryBodyViewModel.FirstCheckQty;
                    toolInventoryBody.SecondCheckQty = toolInventoryBodyViewModel.FirstCheckQty;
                    toolInventoryBody.Note = toolInventoryBodyViewModel.Note;
                    db.Updateable(toolInventoryBody).ExecuteCommand();
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

        public static bool setToolFirstCheckComplete(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ToolInventoryHeader toolInventoryHeader = db.Queryable<ToolInventoryHeader>().Where(e => e.OrderNo == OrderNo).Single();

            toolInventoryHeader.Status = "1";

            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                db.Updateable(toolInventoryHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;
        }

        public static bool updateToolSecondCheckQty(List<ToolInventoryBodyViewModel> toolInventoryBodyViewModels)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            ToolInventoryBody toolInventoryBody = null;
            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                foreach (ToolInventoryBodyViewModel toolInventoryBodyViewModel in toolInventoryBodyViewModels)
                {
                    toolInventoryBody = db.Queryable<ToolInventoryBody>().Where(e => e.OrderNo == toolInventoryBodyViewModel.OrderNo && e.SerialNo == toolInventoryBodyViewModel.SerialNo).Single();

                    toolInventoryBody.SecondCheckQty = toolInventoryBodyViewModel.SecondCheckQty;
                    toolInventoryBody.Note = toolInventoryBodyViewModel.Note;
                    db.Updateable(toolInventoryBody).ExecuteCommand();
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

        public static bool setToolSecondCheckComplete(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ToolInventoryHeader toolInventoryHeader = db.Queryable<ToolInventoryHeader>().Where(e => e.OrderNo == OrderNo).Single();

            toolInventoryHeader.Status = "2";

            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                db.Updateable(toolInventoryHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;
        }

        public static bool ToolInventoryClose(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ToolInventoryHeader toolInventoryHeader = db.Queryable<ToolInventoryHeader>().Where(e => e.OrderNo == OrderNo).Single();

            toolInventoryHeader.Status = "2";

            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                db.Updateable(toolInventoryHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;
        }
    }
}