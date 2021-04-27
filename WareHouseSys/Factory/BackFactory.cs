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
    public class BackFactory
    {
        static public ISugarQueryable<BackSearchViewModel> getBackSearchViewModel(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<BackSearchViewModel> sugarQueryable = db.SqlQueryable<BackSearchViewModel>("SELECT BackBody.OrderNo,BackBody.MaterialNo, BackBody.Quantity, BackBody.BackQty, BackBody.WareHouseId, BackBody.StorageId, BackBody.Note, BackBody.Lot, " +
                "LendNO, TMNAME BackMan, InBoundDate, WGroupId, BackHeader.Note Note1,  MaterialInfo.MaterialName, Spec, WarehouseInfo.WareHouseName, " +
                "(select TMNAME from Employee where BackHeader.InBoundMan = KEYNO) InBoundMan, " +
                "(select UNITNAME from UNIT where BackHeader.BackUnit = UNITNO) UNITNAME, " +
                "case when BackHeader.Status = '1' then '已陳核' when BackHeader.Status = '0' then '辦理中' else '已作廢' end Status, case when Overdue = 1 then '是' else '否' end Overdue " +
                "FROM  BackBody inner join BackHeader on BackBody.OrderNo = BackHeader.OrderNo " +
                "inner join WarehouseInfo on WarehouseInfo.WarehouseId = BackBody.WareHouseId " +
                "inner join MaterialInfo on BackBody.MaterialNo = MaterialInfo.MaterialNo " +
                "inner join Employee on BackHeader.BackMan = Employee.KEYNO ");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }
        public static ISugarQueryable<LendBodiesWithBackViewModel> getBackBodiesWithLend(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<LendBodiesWithBackViewModel> sugarQueryable = db.SqlQueryable<LendBodiesWithBackViewModel>("select OrderNo, LendBody.SerialNo, LendBody.MaterialNo,MaterialName,Spec,Unit," +
                "LendQty,isnull(BackQty,0) BackQty,LendQty - isnull(BackQty,0) NotReturnQty," +
                "LendBody.WareHouseId,WareHouseName, StorageId, Lot from LendBody " +
                "inner join WarehouseInfo on LendBody.WareHouseId = WarehouseInfo.WarehouseId " +
                "inner join MaterialInfo on LendBody.MaterialNo = MaterialInfo.MaterialNo");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo );

            return sugarQueryable;
        }

        public static ISugarQueryable<BackBodyViewModel> getBackBodies(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<BackBodyViewModel> sugarQueryable = db.SqlQueryable<BackBodyViewModel>("select BackHeader.OrderNo, BackBody.SerialNo, BackBody.MaterialNo,MaterialName,Spec," +
                 "  LendQty,BackBody.Quantity,  BackBody.BackQty,BackBody.WareHouseId,WareHouseName,  " +
                 " BackBody.StorageId, OccupiedStorageId, BackBody.Note, BackBody.Lot FROM BackBody " +
                 " inner join BackHeader on BackBody.OrderNo = BackHeader.OrderNo " +
                 "inner join MaterialInfo on BackBody.MaterialNo = MaterialInfo.MaterialNo " +
                 "inner join WarehouseInfo on WarehouseInfo.WareHouseId = BackBody.WareHouseId " +
                 "inner join LendHeader on BackHeader.LendNo = LendHeader.OrderNo " +
                 "inner join LendBody on LendHeader.OrderNo = LendBody.OrderNo and LendBody.MaterialNo = BackBody.MaterialNo and LendBody.WareHouseId = BackBody.WareHouseId and LendBody.StorageId = BackBody.StorageId");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static ISugarQueryable<BackHeaderViewModel> getBackHeaderViewModel(string ID,FilterCriteria filter, List<string> wgroupIdList)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<BackHeaderViewModel> sugarQueryable = db.SqlQueryable<BackHeaderViewModel>("SELECT OrderNo, LendNo,(SELECT TMNAME FROM Employee WHERE KEYNO = BackHeader.BackMan) BackMan,BackMan BackManId," +
                 " (SELECT TMNAME FROM Employee WHERE KEYNO = InBoundMan) InBoundMan,InBoundMan InBoundManId, InBoundDate, Overdue,  " +
                 " WGroupId,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status, AddDateTime, UpdateDateTime FROM  BackHeader ");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            if(wgroupIdList.Count>0)
                sugarQueryable = sugarQueryable.Where(e => wgroupIdList.Contains(e.WGroupId));
            else
                sugarQueryable = sugarQueryable.Where(e => e.BackManId == ID);

            sugarQueryable = sugarQueryable.Where(e => e.Status != "-1");

            return sugarQueryable;
        }

        public static ISugarQueryable<BackHeaderViewModel> getBackHeaderViewModel(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<BackHeaderViewModel> sugarQueryable = db.SqlQueryable<BackHeaderViewModel>("SELECT BackHeader.OrderNo, LendNo,(SELECT TMNAME FROM Employee WHERE KEYNO = BackHeader.BackMan) BackMan,BackMan BackManId, Deadline,ExtendDate," +
                 "(SELECT TMNAME FROM Employee WHERE KEYNO = InBoundMan) InBoundMan,InBoundMan InBoundManId, InBoundDate, Overdue,BackHeader.WGroupId," +
                 "CASE WHEN BackHeader.Status = '1' THEN '結案' WHEN BackHeader.Status = '0' THEN '辦理中' END Status, BackHeader.AddDateTime, BackHeader.UpdateDateTime,Note " +
                 "FROM  BackHeader inner join LendHeader on BackHeader.LendNo = LendHeader.OrderNo");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            sugarQueryable = sugarQueryable.Where(e => e.Status != "-1");

            return sugarQueryable;
        }

        public static bool deleteBack(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            bool retValue = true;

            db.Ado.BeginTran();
            try
            {
                db.Ado.ExecuteCommand("update  BackHeader set Status='-1' where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                //db.Ado.ExecuteCommand("delete from BackBody where OrderNo=@OrderNo",new { OrderNo=OrderNo});
                //db.Ado.ExecuteCommand("delete from BackHeader where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                db.Ado.CommitTran();
            }
            catch
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;

        }

        public static bool updateBackQty(BackBodyViewModel backBodyView)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            BackBody backBody = db.Queryable<BackBody>().Where(e => e.OrderNo == backBodyView.OrderNo && e.SerialNo == backBodyView.SerialNo).Single();

            backBody.BackQty = backBodyView.BackQty;

            return db.Updateable(backBody).ExecuteCommand() > 0;
        }

        public static bool doBack(BackHeaderViewModel backHeaderView)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            BackHeader backHeader = db.Queryable<BackHeader>().Where(e => e.OrderNo == backHeaderView.OrderNo).Single();
            List<BackBody> backBodies = db.Queryable<BackBody>().Where(e => e.OrderNo == backHeaderView.OrderNo).ToList();

            backHeader.InBoundDate = backHeaderView.InBoundDate;
            backHeader.InBoundMan = backHeaderView.InBoundMan;
            backHeader.Status = "1";
            backHeader.Overdue = backHeaderView.Overdue;
            backHeader.Note = backHeaderView.Note;

            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                db.Updateable(backHeader).ExecuteCommand();
                foreach(BackBody backBody in backBodies)
                {
                    db.Ado.ExecuteCommand("update LendBody set BackQty=BackQty +" + backBody.BackQty + " where OrderNo=@OrderNo and MaterialNo=@MaterialNo",
                        new { OrderNo = backHeader.LendNo, MaterialNo = backBody.MaterialNo });

                    db.Ado.ExecuteCommand("update Inventory set LendQty = LendQty - " + backBody.BackQty + " where " +
                        "MaterialNo=@MaterialNo and WarehouseId=@WarehouseId and StorageId=@StorageId and Lot=@Lot",
                        new { MaterialNo = backBody.MaterialNo, WarehouseId = backBody.WareHouseId, StorageId = backBody.StorageId, Lot = backBody.Lot });
                   
                }
                db.Ado.CommitTran();
            }
            catch
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        public static bool saveBack(BackSaveModel backSaveModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            BackHeader backHeader = new BackHeader
            {
                OrderNo = getOrderNo(),
                AddDateTime = DateTime.Now,
                BackMan = backSaveModel.backHeaderViewModel.BackMan,
                BackUnit = backSaveModel.backHeaderViewModel.BackUnit,
                UpdateDateTime = DateTime.Now,
                WGroupId = backSaveModel.backHeaderViewModel.WGroupId,
                LendNo = backSaveModel.backHeaderViewModel.LendNo,
                Status = "0",
                Overdue = backSaveModel.backHeaderViewModel.Overdue,
                Note = backSaveModel.backHeaderViewModel.Note
            };

            List<BackBody> BackBodies = new List<BackBody>();

            int count = 1;
            foreach (BackBodyViewModel backBodyViewModel in backSaveModel.backBodies)
            {
                BackBodies.Add(new BackBody
                {
                    OrderNo = backHeader.OrderNo,
                    MaterialNo = backBodyViewModel.MaterialNo,
                    WareHouseId = backBodyViewModel.WareHouseId,
                    Quantity = backBodyViewModel.NotReturnQty,
                    Lot = backBodyViewModel.Lot,
                    Note = backBodyViewModel.Note,
                    StorageId = backBodyViewModel.StorageId,
                    SerialNo = count.ToString("0000")
                });
                count++;
            }

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(backHeader).ExecuteCommand();
                db.Insertable(BackBodies).ExecuteCommand();
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

            string OrderPrefix = "F" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM BackHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
            var OrderNo = "";
            try
            {
                OrderNo = db.Ado.SqlQuerySingle<string>(sql, new { OrderPrefix = OrderPrefix });
            }
            catch
            {

            }

            return OrderPrefix + "-" + (int.Parse(OrderNo.Split('-')[1]) + 1).ToString("0000");
        }
    }
}