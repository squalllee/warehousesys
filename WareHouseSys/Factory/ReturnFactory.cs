using SqlSugar;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Configuration;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class ReturnFactory
    {

        static public ISugarQueryable<ReturnSearchViewModel> getReturnSearchViewModel(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ReturnSearchViewModel> sugarQueryable = db.SqlQueryable<ReturnSearchViewModel>("SELECT ReturnBody.OrderNo,ReturnBody.MaterialNo, ReturnBody.SerialNo, ReturnBody.PickingSerialNo, ReturnBody.Quantity, ReturnBody.ReturnQty, ReturnBody.WareHouseId, ReturnBody.StorageId, ReturnBody.OccupiedStorageId,  ReturnBody.Note, ReturnBody.Lot, " +
                "TMNAME ReturnMan, InBoundDate, PickingNo, WorkNo, WGroupId, ReturnReason, MaterialInfo.MaterialName, Spec, WarehouseInfo.WareHouseName, " +
                "(select TMNAME from Employee where ReturnHeader.InBoundMan = KEYNO) InBoundMan, " +
                "(select UNITNAME from UNIT where ReturnHeader.ReturnUnit = UNITNO) ReturnUnit, " +
                "case when ReturnHeader.Status = '1' then '已陳核' when ReturnHeader.Status = '0' then '辦理中' else '已作廢' end Status " +
                "FROM  ReturnBody inner join ReturnHeader on ReturnBody.OrderNo = ReturnHeader.OrderNo " +
                "inner join MaterialInfo on ReturnBody.MaterialNo = MaterialInfo.MaterialNo " +
                "inner join Employee on ReturnHeader.ReturnMan = Employee.KEYNO " +
                "inner join WarehouseInfo on WarehouseInfo.WarehouseId = ReturnBody.WareHouseId ");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }
        public static ISugarQueryable<ReturnHeaderViewModel> getReturnHeaderViewModel(FilterCriteria filter, List<string> wgroupIdList,string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ReturnHeaderViewModel> sugarQueryable = db.SqlQueryable<ReturnHeaderViewModel>("SELECT OrderNo, InBoundDate, PickingNo, WorkNo,WGroupId," +
                " (SELECT  TMNAME FROM Employee WHERE KEYNO = ReturnMan) ReturnMan, ReturnMan ReturnManId, " +
                "(SELECT  TMNAME FROM Employee WHERE KEYNO = InBoundMan) InBoundMan, InBoundMan InBoundManId," +
                "CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status," +
                "AddDateTime, UpdateDateTime FROM ReturnHeader");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);
            sugarQueryable = sugarQueryable.Where(e => e.Status != "-1");

            if(wgroupIdList.Count > 0)
            {
                sugarQueryable = sugarQueryable.Where(e => wgroupIdList.Contains(e.WGroupId));
            }
            else
            {
                sugarQueryable = sugarQueryable.Where(e => e.ReturnManId == ID);
            }
            

            return sugarQueryable;
        }

        public static ISugarQueryable<ReturnHeaderViewModel> getReturnHeaderViewModelByOrderNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ReturnHeaderViewModel> sugarQueryable = db.SqlQueryable<ReturnHeaderViewModel>("SELECT OrderNo, InBoundDate, PickingNo, WorkNo,WGroupId," +
                " (SELECT  TMNAME FROM Employee WHERE KEYNO = ReturnMan) ReturnMan, ReturnMan ReturnManId, " +
                " (SELECT  UNITNAME FROM UNIT WHERE UNITNO = ReturnUnit) ReturnUnit, ReturnUnit ReturnUnitId, " +
                "(SELECT  TMNAME FROM Employee WHERE KEYNO = InBoundMan) InBoundMan, InBoundMan InBoundManId,ReturnReason," +
                "CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status," +
                "AddDateTime, UpdateDateTime FROM ReturnHeader");

            sugarQueryable = sugarQueryable.Where(e => e.Status != "-1");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static ISugarQueryable<ReturnBodyViewModel> getReturnBodyViewModel(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ReturnBodyViewModel> sugarQueryable = db.SqlQueryable<ReturnBodyViewModel>("SELECT  OrderNo, ReturnBody.SerialNo, PickingSerialNo,MaterialName,Spec,Unit,ReturnBody.MaterialNo, Quantity, ReturnQty, " +
                " ReturnBody.WareHouseId,WareHouseName,StorageId,OccupiedStorageId, Note, Lot FROM  ReturnBody " +
                "inner join WarehouseInfo on ReturnBody.WareHouseId = WarehouseInfo.WarehouseId " +
                "inner join MaterialInfo on ReturnBody.MaterialNo = MaterialInfo.MaterialNo");



            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static bool updateReturnBody(ReturnBodyViewModel returnBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ReturnBody returnBody = db.Queryable<ReturnBody>().Where(e => e.OrderNo == returnBodyViewModel.OrderNo && e.SerialNo == returnBodyViewModel.SerialNo).Single();

            returnBody.Note = returnBodyViewModel.Note;
            returnBody.Quantity = returnBodyViewModel.Quantity;

            return db.Updateable(returnBody).ExecuteCommand() > 0;

        }

        public static bool updateReturnHeader(ReturnHeader returnHeader)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ReturnHeader retHeader = db.Queryable<ReturnHeader>().Where(e => e.OrderNo == returnHeader.OrderNo).Single();

            retHeader.PickingNo = returnHeader.PickingNo;
            retHeader.ReturnMan = returnHeader.ReturnMan;
            retHeader.ReturnUnit = returnHeader.ReturnUnit;
            retHeader.ReturnReason = returnHeader.ReturnReason;
            retHeader.AddDateTime = returnHeader.AddDateTime;
         

            return db.Updateable(retHeader).ExecuteCommand() > 0;

        }

        public static bool updateReturnBodyReturnQty(ReturnBodyViewModel returnBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ReturnBody returnBody = db.Queryable<ReturnBody>().Where(e => e.OrderNo == returnBodyViewModel.OrderNo && e.SerialNo == returnBodyViewModel.SerialNo).Single();

            returnBody.ReturnQty = returnBodyViewModel.ReturnQty;

            return db.Updateable(returnBody).ExecuteCommand() > 0;
        }

        public static bool deleteReturnBody(ReturnBodyViewModel returnBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ReturnBody returnBody = db.Queryable<ReturnBody>().Where(e => e.OrderNo == returnBodyViewModel.OrderNo && e.SerialNo == returnBodyViewModel.SerialNo).Single();

            return db.Deleteable(returnBody).ExecuteCommand() > 0;

        }

        public static bool doReturn(ReturnHeaderViewModel returnHeaderViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ReturnHeader returnHeader = db.Queryable<ReturnHeader>().Where(e => e.OrderNo == returnHeaderViewModel.OrderNo).Single();

            List<ReturnBody> returnBodies = db.Queryable<ReturnBody>().Where(e => e.OrderNo == returnHeaderViewModel.OrderNo).ToList();

            returnHeader.InBoundMan = returnHeaderViewModel.InBoundMan;
            returnHeader.InBoundDate = returnHeaderViewModel.InBoundDate;
            returnHeader.Status = "1";

            bool retVal = true;
            db.Ado.BeginTran();
            try
            {
                db.Updateable(returnHeader).ExecuteCommand();
                foreach(ReturnBody returnBody in returnBodies)
                {
                    db.Ado.ExecuteCommand("update Inventory set Quantity=Quantity+" + returnBody.ReturnQty +
                        " where MaterialNo=@MaterialNo and WarehouseId=@WarehouseId and StorageId=@StorageId",new { MaterialNo =returnBody.MaterialNo, WarehouseId = returnBody.WareHouseId, StorageId =returnBody.StorageId});
                }
                
                db.Ado.CommitTran();
            }
            catch
            {
                retVal = false;
                db.Ado.RollbackTran();
            }

            return retVal;

        }

        

        public static bool DeleteReturn(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            bool retValue = true;

            db.Ado.BeginTran();
            try
            {
                db.Ado.ExecuteCommand("update ReturnHeader set Status = '-1' where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                //db.Ado.ExecuteCommand("delete from ReturnHeader where OrderNo=@OrderNo",new { OrderNo=OrderNo});
                //db.Ado.ExecuteCommand("delete from ReturnBody where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                db.Ado.CommitTran();
            }
            catch
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;

        }
        
        static public string getOrderNo()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = "D" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM ReturnHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

        

        static public string getSerialNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from ReturnBody where OrderNo=@OrderNo";

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