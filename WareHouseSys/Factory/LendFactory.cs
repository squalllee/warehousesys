using SqlSugar;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;
using Kendo.Mvc.UI;

namespace WareHouseSys.Factory
{
    public class LendFactory
    {
        static public ISugarQueryable<LendSearchViewModel> getLendSearchViewModel(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<LendSearchViewModel> sugarQueryable = db.SqlQueryable<LendSearchViewModel>("SELECT LendBody.OrderNo, LendBody.SerialNo, LendBody.MaterialNo, LendBody.BackQty ,LendBody.Quantity, LendBody.LendQty, LendBody.WarehouseId, LendBody.StorageId, LendBody.ExtendCount, LendBody.Lot, " +
                "TMNAME LendMan, WGroupId, OutBoundDate, ExtendDate, MaterialInfo.MaterialName, Spec, WarehouseInfo.WareHouseName, Reason, OtherReason, " +
                "case when LendHeader.Status = '1' then '已陳核' when LendHeader.Status = '0' then '辦理中' else '已作廢' end Status, (select TMNAME from Employee where LendHeader.OutBoundMan = KEYNO) OutBoundMan, " +
                "(select UNITNAME from UNIT where LendHeader.LendUnit = UNITNO) UNITNAME " +
                "FROM LendBody inner join LendHeader on LendBody.OrderNo = LendHeader.OrderNo " +
                "inner join WarehouseInfo on WarehouseInfo.WarehouseId = LendBody.WareHouseId " +
                "inner join MaterialInfo on MaterialInfo.MaterialNo = LendBody.MaterialNo " +
                "inner join Employee on Employee.KEYNO = LendHeader.LendMan " +
                "inner join UNIT on Employee.UNITNO = UNIT.UNITNO ");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }
        public static ISugarQueryable<LendHeaderViewModel> getLendHeaderViewModel(FilterCriteria filter, List<string> wgroupIdList,string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<LendHeaderViewModel> sugarQueryable = db.SqlQueryable<LendHeaderViewModel>("select OrderNo," +
                "(select TMNAME from Employee where KEYNO=LendMan) LendMan,LendMan LendManId," +
                " (select TMNAME from Employee where KEYNO=OutBoundMan) OutBoundMan, OutBoundMan OutBoundManId, " +
                " OutBoundDate,ExtendDate, CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END  Status, Deadline, Reason, WGroupId, AddDateTime, UpdateDateTime from LendHeader");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.Status != "-1");

            if (wgroupIdList.Count>0)
            {
                sugarQueryable = sugarQueryable.Where(e => wgroupIdList.Contains(e.WGroupId));
            }
            else
            {
                sugarQueryable = sugarQueryable.Where(e => e.LendManId == ID);
            }
            

            return sugarQueryable;
        }

        public static ISugarQueryable<LendHeaderViewModel> getLendHeaderViewModelByLendNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<LendHeaderViewModel> sugarQueryable = db.SqlQueryable<LendHeaderViewModel>("select OrderNo," +
                " (select TMNAME from Employee where KEYNO=LendMan) LendMan,LendMan LendManId," +
                " (select UNITNAME from UNIT where UNITNO=LendUnit) LendUnit,LendUnit LendUnitId," +
                " (select TMNAME from Employee where KEYNO=OutBoundMan) OutBoundMan, OutBoundMan OutBoundManId, " +
                " OutBoundDate, Status, Deadline, Reason, WGroupId, AddDateTime, UpdateDateTime,OtherReason from LendHeader");

           
            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static ISugarQueryable<LendBodyViewModel> LendBodyViewModel(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<LendBodyViewModel> sugarQueryable = db.SqlQueryable<LendBodyViewModel>("select OrderNo, LendBody.SerialNo, LendBody.MaterialNo,MaterialName,Spec,Unit, Quantity," +
                " LendQty, LendBody.WareHouseId,WareHouseName, StorageId, Lot from LendBody " +
                " inner join WarehouseInfo on LendBody.WareHouseId = WarehouseInfo.WarehouseId " +
                " inner join MaterialInfo on LendBody.MaterialNo = MaterialInfo.MaterialNo");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static bool saveLend(LendSaveModel lendSaveModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            LendHeader lendHeader = new LendHeader
            {
                OrderNo = getOrderNo(),
                AddDateTime = DateTime.Now,
                LendMan = lendSaveModel.lendHeaderViewModel.LendMan,
                LendUnit = lendSaveModel.lendHeaderViewModel.LendUnit,
                UpdateDateTime = DateTime.Now,
                WGroupId = lendSaveModel.lendHeaderViewModel.WGroupId,
                Deadline = lendSaveModel.lendHeaderViewModel.Deadline,
                ExtendDate = lendSaveModel.lendHeaderViewModel.Deadline,
                Reason = lendSaveModel.lendHeaderViewModel.Reason,
                Status = "0",
                OtherReason = lendSaveModel.lendHeaderViewModel.OtherReason
            };

            List<LendBody> LendBodies = new List<LendBody>();

            int count = 1;
            foreach (LendBodyViewModel lendBodyViewModel in lendSaveModel.LendBodies)
            {
                LendBodies.Add(new LendBody
                {
                    OrderNo = lendHeader.OrderNo,
                    MaterialNo = lendBodyViewModel.MaterialNo,
                    WareHouseId = lendBodyViewModel.WareHouseId,
                    Quantity = lendBodyViewModel.Quantity,
                    SerialNo = count.ToString("0000"),
                    BackQty = 0
                });
                count++;
            }

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(lendHeader).ExecuteCommand();
                db.Insertable(LendBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;

        }

        public static bool UpdateLendHeader(LendHeaderViewModel lendHeaderView)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            LendHeader lendHeader = db.Queryable<LendHeader>().Where(e => e.OrderNo == lendHeaderView.OrderNo).Single();

            lendHeader.LendMan = lendHeaderView.LendMan;
            lendHeader.LendUnit = lendHeaderView.LendUnit;
            lendHeader.UpdateDateTime = DateTime.Now;
            lendHeader.WGroupId = lendHeaderView.WGroupId;
            lendHeader.Deadline = lendHeaderView.Deadline;
            lendHeader.ExtendDate = lendHeaderView.Deadline;
            lendHeader.Reason = lendHeaderView.Reason;
            lendHeader.OtherReason = lendHeaderView.OtherReason;
            lendHeader.Status = "0";

            return db.Updateable(lendHeader).ExecuteCommand() > 0;
        }

        public static string getLimiteDate(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            LendHeader lendHeader = db.Queryable<LendHeader>().Where(e => e.OrderNo == OrderNo).Single();

            return DateTime.Parse(lendHeader.ExtendDate.ToString()).ToString("yyyy/MM/dd");
        }

        public static bool doLend(LendSaveModel lendSaveModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            LendHeader lendHeader = db.Queryable<LendHeader>().Where(e => e.OrderNo == lendSaveModel.lendHeaderViewModel.OrderNo).Single();
            List<LendBody> lendBodies = db.Queryable<LendBody>().Where(e => e.OrderNo == lendSaveModel.lendHeaderViewModel.OrderNo).ToList();

            lendHeader.UpdateDateTime = DateTime.Now;
            lendHeader.Deadline = lendSaveModel.lendHeaderViewModel.Deadline;
            lendHeader.ExtendDate = lendSaveModel.lendHeaderViewModel.Deadline;
            lendHeader.OutBoundDate = lendSaveModel.lendHeaderViewModel.OutBoundDate;
            lendHeader.OutBoundMan = lendSaveModel.lendHeaderViewModel.OutBoundMan;
            lendHeader.OtherReason = lendSaveModel.lendHeaderViewModel.OtherReason;
            lendHeader.Reason = lendSaveModel.lendHeaderViewModel.Reason;
            lendHeader.Status = "1";
            lendHeader.ExtendCount = 0;

            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                foreach (LendBody lendBody in lendBodies)
                {
                    db.Ado.ExecuteCommand("update Inventory set LendQty = LendQty + " + lendBody.LendQty + " where " +
                        "MaterialNo=@MaterialNo and WarehouseId=@WarehouseId and StorageId=@StorageId and Lot=@Lot",
                        new { MaterialNo = lendBody.MaterialNo, WarehouseId = lendBody.WareHouseId, StorageId = lendBody.StorageId, Lot = lendBody.Lot });
                }

                db.Updateable(lendHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception ex)
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        public static bool deleteLend(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Ado.ExecuteCommand("update LendHeader set Status = '-1' where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                
                //db.Ado.ExecuteCommand("delete from LendBody where OrderNo=@OrderNo",new { OrderNo=OrderNo});
                //db.Ado.ExecuteCommand("delete from LendHeader where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }


        public static bool LendBodyUpdate(LendBodyViewModel lendBodyView)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            LendBody lendBody = db.Queryable<LendBody>().Where(e => e.OrderNo == lendBodyView.OrderNo && e.SerialNo == lendBodyView.SerialNo).Single();

            lendBody.MaterialNo = lendBodyView.MaterialNo;
            lendBody.WareHouseId = lendBodyView.WareHouseId;
            lendBody.Quantity = lendBodyView.Quantity;

            return db.Updateable(lendBody).ExecuteCommand() > 0;
        }

        public static bool doLendBodyUpdate(LendBodyViewModel lendBodyView)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            LendBody lendBody = db.Queryable<LendBody>().Where(e => e.OrderNo == lendBodyView.OrderNo && e.SerialNo == lendBodyView.SerialNo).Single();

            lendBody.WareHouseId = lendBodyView.WareHouseId;
            lendBody.StorageId = lendBodyView.StorageId;
            lendBody.LendQty = lendBodyView.LendQty;
            lendBody.Lot = lendBodyView.Lot;

            return db.Updateable(lendBody).ExecuteCommand() > 0;
        }

        public static bool LendBodyAdd(LendBodyViewModel lendBodyView)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            LendBody lendBody = new LendBody
            {
                OrderNo = lendBodyView.OrderNo,
                SerialNo = getSerialNo(lendBodyView.OrderNo),
                MaterialNo = lendBodyView.MaterialNo,
                WareHouseId = lendBodyView.WareHouseId,
                Quantity = lendBodyView.Quantity
            };
            
            return db.Insertable(lendBody).ExecuteCommand() > 0;
        }

        public static bool LendBodyDelete(LendBodyViewModel lendBodyView)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            LendBody lendBody = db.Queryable<LendBody>().Where(e => e.OrderNo == lendBodyView.OrderNo && e.SerialNo == lendBodyView.SerialNo).Single();

            return db.Deleteable(lendBody).ExecuteCommand() > 0;
        }

        static public string getOrderNo()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = "E" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM LendHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

        static public string getSerialNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from LendBody where OrderNo=@OrderNo";

            string SerialNo = "";

            try
            {
                SerialNo = db.Ado.SqlQuerySingle<string>(sql, new { OrderNo = OrderNo });
            }
            catch (Exception ex)
            {

            }

            SerialNo = (int.Parse(SerialNo) + 1).ToString("0000");

            return SerialNo;
        }

    }
}