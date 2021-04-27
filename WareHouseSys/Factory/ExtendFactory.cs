using SqlSugar;
using System;
using System.Collections.Generic;
using System.Globalization;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;
using Kendo.Mvc.UI;

namespace WareHouseSys.Factory
{
    public class ExtendFactory
    {

        static public ISugarQueryable<ExtendSearchViewModel> getExtendSearchViewModel(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ExtendSearchViewModel> sugarQueryable = db.SqlQueryable<ExtendSearchViewModel>("SELECT ExtendBody.OrderNo,ExtendBody.MaterialNo, ExtendBody.SerialNo, ExtendBody.Lot, ExtendBody.Quantity,  " +
                "LendNO, TMNAME ExtendMan, ExtendDate, CONVERT(date, AddDateTime, 23) AddDateTime, WGroupId, ExtendReason, MaterialInfo.MaterialName, Spec, UNITNAME, Days, " +
                "(select TMNAME from Employee where ExtendHeader.ApprovedMan = KEYNO) ApprovedMan, " +
                "case when ExtendHeader.Status = '1' then '已陳核' when ExtendHeader.Status = '0' then '辦理中' else '已作廢' end Status " +
                "FROM  ExtendBody inner join ExtendHeader on ExtendBody.OrderNo = ExtendHeader.OrderNo " +
                "inner join MaterialInfo on ExtendBody.MaterialNo = MaterialInfo.MaterialNo " +
                "inner join Employee on ExtendHeader.ExtendMan = Employee.KEYNO " +
                "inner join UNIT on Employee.UNITNO = UNIT.UNITNO ");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

        public static ISugarQueryable<LendBodiesWithExtendViewModel> getExtendBodiesWithLend(FilterCriteria filter,string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<LendBodiesWithExtendViewModel> sugarQueryable = db.SqlQueryable<LendBodiesWithExtendViewModel>("select OrderNo, LendBody.SerialNo, LendBody.MaterialNo,MaterialName,Spec,Unit," +
                "LendQty,isnull(BackQty,0) BackQty,LendQty - isnull(BackQty,0) NotReturnQty," +
                "LendQty -isnull(BackQty,0) ExtendQty,  LendBody.WareHouseId,WareHouseName, StorageId, Lot from LendBody " +
                "inner join WarehouseInfo on LendBody.WareHouseId = WarehouseInfo.WarehouseId " +
                "inner join MaterialInfo on LendBody.MaterialNo = MaterialInfo.MaterialNo");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);
            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo && e.NotReturnQty > 0);

            return sugarQueryable;
        }

        public static ISugarQueryable<LendBodiesWithExtendViewModel> getExtendDetailBodiesWithLend(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<LendBodiesWithExtendViewModel> sugarQueryable = db.SqlQueryable<LendBodiesWithExtendViewModel>("select OrderNo, LendBody.SerialNo, LendBody.MaterialNo,MaterialName,Spec,Unit," +
                "LendQty,isnull(BackQty,0) BackQty,LendQty - isnull(BackQty,0) NotReturnQty," +
                "LendQty -isnull(BackQty,0) ExtendQty,  LendBody.WareHouseId,WareHouseName, StorageId, Lot from LendBody " +
                "inner join WarehouseInfo on LendBody.WareHouseId = WarehouseInfo.WarehouseId " +
                "inner join MaterialInfo on LendBody.MaterialNo = MaterialInfo.MaterialNo");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);
            
            return sugarQueryable;
        }

        public static ISugarQueryable<ExtendHeaderViewModel> getExtendHeaderViewModel(FilterCriteria filter, List<string> wgroupIdList,string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ExtendHeaderViewModel> sugarQueryable = db.SqlQueryable<ExtendHeaderViewModel>("select  OrderNo,(SELECT TMNAME FROM Employee WHERE KEYNO = ExtendMan) ExtendMan,ExtendMan ExtendManId,WGroupId," +
                 "  (SELECT TMNAME FROM Employee WHERE KEYNO = ApprovedMan) ApprovedMan,ApprovedMan ApprovedManId,  " +
                 " ExtendDate, LendNo, ExtendReason, Days, CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status," +
                 "  AddDateTime, UpdateDateTime from ExtendHeader ");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);
            sugarQueryable = sugarQueryable.Where(e => e.Status != "-1");
            if (wgroupIdList.Count == 0)
            {
                sugarQueryable = sugarQueryable.Where(e => e.ExtendManId == ID);
            }

            return sugarQueryable;
        }

        public static ISugarQueryable<ExtendHeaderViewModel> getExtendHeaderViewModel(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ExtendHeaderViewModel> sugarQueryable = db.SqlQueryable<ExtendHeaderViewModel>("select  OrderNo,(SELECT TMNAME FROM Employee WHERE KEYNO = ExtendMan) ExtendMan,ExtendMan ExtendManId," +
                 "  (SELECT TMNAME FROM Employee WHERE KEYNO = ApprovedMan) ApprovedMan,ApprovedMan ApprovedManId,CloseDate,  " +
                 " ExtendDate, LendNo, ExtendReason, Days, CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status," +
                 "  AddDateTime, UpdateDateTime from ExtendHeader ");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);
            sugarQueryable = sugarQueryable.Where(e => e.Status != "-1");
            return sugarQueryable;
        }

        public static bool doExtend(ExtendSaveModel extendSaveModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            LendHeader lendHeader = db.Queryable<LendHeader>().Where(e => e.OrderNo == extendSaveModel.extendHeaderViewModel.LendNo).Single();
            ExtendHeader extendHeader = db.Queryable<ExtendHeader>().Where(e => e.OrderNo == extendSaveModel.extendHeaderViewModel.OrderNo).Single();
            List<ExtendBody> extendBodies = db.Queryable<ExtendBody>().Where(e => e.OrderNo == extendSaveModel.extendHeaderViewModel.OrderNo).ToList();

            extendHeader.Days = extendSaveModel.extendHeaderViewModel.Days;
            extendHeader.ExtendDate = DateTime.Parse(lendHeader.ExtendDate.ToString()).AddDays(extendSaveModel.extendHeaderViewModel.Days);
            extendHeader.CloseDate = extendSaveModel.extendHeaderViewModel.CloseDate;
            extendHeader.ApprovedMan = extendSaveModel.extendHeaderViewModel.ApprovedMan;
            extendHeader.Status = "1";

            extendHeader.UpdateDateTime = DateTime.Now;
            lendHeader.ExtendDate = DateTime.Parse(lendHeader.ExtendDate.ToString()).AddDays(extendSaveModel.extendHeaderViewModel.Days);
            lendHeader.ExtendCount += 1;

            bool retValue = true;

            db.Ado.BeginTran();
            try
            {
                foreach(ExtendBody extendBody in extendBodies)
                {
                    LendBody lendBody = db.Queryable<LendBody>().Where(e => e.OrderNo == extendSaveModel.extendHeaderViewModel.LendNo && e.MaterialNo == extendBody.MaterialNo && e.SerialNo == extendBody.SerialNo).Single();
                    if(lendBody != null)
                    {
                        lendBody.ExtendCount += 1;
                        db.Updateable(lendBody).ExecuteCommand();
                    }
                }
                db.Updateable(extendHeader).ExecuteCommand();
                db.Updateable(lendHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        public static  bool isExtend (string LendNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            return db.Queryable<ExtendHeader>().Where(e => e.LendNo == LendNo && e.Status == "0").Count() > 0;
        }

        public static bool deleteExtend(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                
                db.Ado.ExecuteCommand("update ExtendHeader set Status='-1' where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                //db.Ado.ExecuteCommand("delete from ExtendBody where OrderNo=@OrderNo",new { OrderNo=OrderNo});
                //db.Ado.ExecuteCommand("delete from ExtendHeader where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                db.Ado.CommitTran();
            }
            catch
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        public static bool saveExtend(ExtendSaveModel extendSaveModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            LendHeader lendHeader = db.Queryable<LendHeader>().Where(e => e.OrderNo == extendSaveModel.extendHeaderViewModel.LendNo).Single();

            ExtendHeader extendHeader = new ExtendHeader
            {
                OrderNo = getOrderNo(),
                AddDateTime = DateTime.Now,
                Days = extendSaveModel.extendHeaderViewModel.Days,
                ExtendMan = extendSaveModel.extendHeaderViewModel.ExtendMan,
                UpdateDateTime = DateTime.Now,
                ExtendReason = extendSaveModel.extendHeaderViewModel.ExtendReason,
                WGroupId = extendSaveModel.extendHeaderViewModel.WGroupId,
                LendNo = extendSaveModel.extendHeaderViewModel.LendNo,
                Status = "0",
                ExtendDate = DateTime.Parse(lendHeader.ExtendDate.ToString()).AddDays(extendSaveModel.extendHeaderViewModel.Days)
            };

            List<ExtendBody> ExtendBodies = new List<ExtendBody>();

            int count = 1;
            foreach (ExtendBodyViewModel extendBodyViewModel in extendSaveModel.extendBodies)
            {
                ExtendBodies.Add(new ExtendBody
                {
                    OrderNo = extendHeader.OrderNo,
                    MaterialNo = extendBodyViewModel.MaterialNo,
                    Quantity = extendBodyViewModel.ExtendQty,
                    SerialNo = count.ToString("0000"),
                    Lot = extendBodyViewModel.Lot
                });
                count++;
            }

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(extendHeader).ExecuteCommand();
                db.Insertable(ExtendBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;

        }

        public static bool updateExtend(ExtendHeaderViewModel extendHeaderViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            ExtendHeader extendHeader = db.Queryable<ExtendHeader>().Where(e => e.OrderNo == extendHeaderViewModel.OrderNo).Single();
            LendHeader lendHeader = db.Queryable<LendHeader>().Where(e => e.OrderNo == extendHeaderViewModel.LendNo).Single();

            extendHeader.Days = extendHeaderViewModel.Days;
                extendHeader.ExtendMan = extendHeaderViewModel.ExtendMan;
            extendHeader.UpdateDateTime = DateTime.Now;
            extendHeader.ExtendReason = extendHeaderViewModel.ExtendReason;
            extendHeader.ExtendDate = DateTime.Parse(lendHeader.Deadline.ToString()).AddDays(extendHeaderViewModel.Days);
            

           

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(extendHeader).ExecuteCommand();
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

            string OrderPrefix = "T" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM ExtendHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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