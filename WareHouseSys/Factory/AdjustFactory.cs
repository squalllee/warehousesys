using Kendo.Mvc.UI;
using Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class AdjustFactory
    {
        public static ISugarQueryable<AdjustHeaderViewModel> getAdjustHeaderViewModel(DataSourceRequest request, List<string> wgroupIdList, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<AdjustHeaderViewModel> sugarQueryable = db.SqlQueryable<AdjustHeaderViewModel>("SELECT  OrderNo,TMNAME ApplyMan,ApplyMan ApplyManId, ApplyDate," +
                "UNITNAME ApplyUnit,ApplyUnit ApplyUnitId," +
                "CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END  Status, Status StatusId," +
                "AddDateTime, UpdateDateTime FROM  AdjustHeader INNER JOIN Employee ON AdjustHeader.ApplyMan = Employee.KEYNO " +
                "inner join UNIT on ApplyUnit = UNIT.UNITNO");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            if (wgroupIdList.Count == 0)
            {
                sugarQueryable = sugarQueryable.Where(e => e.ApplyManId == ID);
            }
          


            return sugarQueryable;
        }

        public static AdjustHeaderViewModel getAdjustHeaderViewModel(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<AdjustHeaderViewModel> sugarQueryable = db.SqlQueryable<AdjustHeaderViewModel>("SELECT  OrderNo,TMNAME ApplyMan,ApplyMan ApplyManId, ApplyDate," +
                "UNITNAME ApplyUnit,ApplyUnit ApplyUnitId," +
                "CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END  Status, Status StatusId," +
                "AddDateTime, UpdateDateTime FROM  AdjustHeader INNER JOIN Employee ON AdjustHeader.ApplyMan = Employee.KEYNO " +
                "inner join UNIT on ApplyUnit = UNIT.UNITNO");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable.Single();
        }

        public static ISugarQueryable<AdjustBodyViewModel> getAdjustBodyViewModel(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<AdjustBodyViewModel> sugarQueryable = db.SqlQueryable<AdjustBodyViewModel>("SELECT OrderNo, AdjustBody.SerialNo, AdjustBody.MaterialNo, Quantity,  " +
                "MaterialName,Spec,AdjustBody.WareHouseId,WareHouseName,StorageId,Unit,Lot, Reason, StockQty FROM AdjustBody " +
                 "inner join WarehouseInfo on WarehouseInfo.WarehouseId = AdjustBody.WareHouseId " +
                "inner join MaterialInfo on MaterialInfo.MaterialNo = AdjustBody.MaterialNo");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

       
        static public bool AddAdjust(AdjustSaveModel AdjustObj, string ID)
        {
            bool retValue = true;
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            Employee employee = EmployeeFactory.getEmployee(AdjustObj.adjustHeader.ApplyMan);

            AdjustObj.adjustHeader.OrderNo = getOrderNo();
            AdjustObj.adjustHeader.Status = "0";
            AdjustObj.adjustHeader.UpdateDateTime = DateTime.Now;
            AdjustObj.adjustHeader.ApplyUnit = employee.UNITNO;
            AdjustObj.adjustHeader.AddDateTime = DateTime.Now;

            int serialNo = 0;
            foreach (AdjustBody adjustBody in AdjustObj.adjustBodies)
            {
                adjustBody.SerialNo = serialNo.ToString("0000");
                adjustBody.OrderNo = AdjustObj.adjustHeader.OrderNo;
                serialNo++;
            }

            db.Ado.BeginTran();
            try
            {
                db.Insertable(AdjustObj.adjustHeader).ExecuteCommand();
                db.Insertable(AdjustObj.adjustBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
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

            string OrderPrefix = "K" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM AdjustHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from AdjustBody where OrderNo=@OrderNo";

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

        static public bool CloseAdjust(AdjustHeaderViewModel adjustObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            AdjustHeader adjustHeader = db.Queryable<AdjustHeader>().Where(e => e.OrderNo == adjustObj.OrderNo).Single();

            List<AdjustBody> adjustBodies = db.Queryable<AdjustBody>().Where(e => e.OrderNo == adjustObj.OrderNo).ToList();

            adjustHeader.Status = "1";

            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                db.Updateable(adjustHeader).ExecuteCommand();
                foreach(AdjustBody adjustBody in adjustBodies)
                {
                    db.Ado.ExecuteCommand("update Inventory set Quantity=@Quantity where MaterialNo=@MaterialNo and " +
                        "WarehouseId = @WarehouseId and StorageId=@StorageId and Lot=@Lot",new { Quantity = adjustBody.Quantity.ToString(),
                            MaterialNo = adjustBody.MaterialNo,
                            WarehouseId = adjustBody.WareHouseId,
                            StorageId = adjustBody.StorageId,
                            Lot = adjustBody.Lot
                        });
                }
                
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        static public bool AddAdjustBody(AdjustBody adjustObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            adjustObj.SerialNo = getSerialNo(adjustObj.OrderNo);

            return db.Insertable(adjustObj).ExecuteCommand() > 0;
        }

        static public bool UpdateAdjustBody(AdjustBody AdjustObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            return db.Updateable(AdjustObj).ExecuteCommand() > 0;
        }

        static public bool SaveAdjustHeader(AdjustHeader adjustObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            AdjustHeader adjustHeader = db.Queryable<AdjustHeader>().Where(e => e.OrderNo == adjustObj.OrderNo).Single();

            Employee employee = EmployeeFactory.getEmployee(adjustHeader.ApplyMan);

            adjustHeader.ApplyUnit = employee.UNITNO.Trim();
            adjustHeader.ApplyMan = adjustObj.ApplyMan;
            adjustHeader.ApplyDate = adjustObj.ApplyDate;

            return db.Updateable(adjustHeader).ExecuteCommand() > 0;
        }

       


        static public bool DeleteadjustBody(AdjustBody adjustObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            return db.Deleteable(adjustObj).ExecuteCommand() > 0;
        }
    }
}