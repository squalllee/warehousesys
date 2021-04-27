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
    public class ScrapFactory
    {
        public static ISugarQueryable<ScrapHeaderViewModel> getScrapHeaderViewModel(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ScrapHeaderViewModel> sugarQueryable = db.SqlQueryable<ScrapHeaderViewModel>("SELECT OrderNo, WorkNo,TMNAME ApplyMan,ApplyMan ApplyManId, ApplyDate,UNITNAME ApplyUnit," +
                "ApplyUnit ApplyUnitId, ScrapType, Reason, AddDateTime, UpdateDateTime," +
                 "CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status,Status StatusId " +
                "FROM ScrapHeader left join Employee on ScrapHeader.ApplyMan = Employee.KEYNO " +
               
                "left join UNIT on ScrapHeader.ApplyUnit = UNIT.UNITNO");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

        public static ScrapHeaderViewModel getScrapHeaderViewModel(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ScrapHeaderViewModel> sugarQueryable = db.SqlQueryable<ScrapHeaderViewModel>("SELECT OrderNo, WorkNo,TMNAME ApplyMan,ApplyMan ApplyManId, ApplyDate,UNITNAME ApplyUnit," +
                "ApplyUnit ApplyUnitId, ScrapType, Reason, AddDateTime, UpdateDateTime," +
                 "CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status,Status StatusId " +
                "FROM ScrapHeader left join Employee on ScrapHeader.ApplyMan = Employee.KEYNO " +

                "left join UNIT on ScrapHeader.ApplyUnit = UNIT.UNITNO");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable.Single();
        }

        public static ISugarQueryable<ScrapBodyViewModel> getScrapBodyViewModel(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ScrapBodyViewModel> sugarQueryable = db.SqlQueryable<ScrapBodyViewModel>("SELECT OrderNo, ScrapBody.SerialNo, ScrapBody.Unit, ScrapBody.MaterialNo,MaterialName,Spec, Quantity, " +
                "ScrapBody.WareHouseId,WarehouseInfo.WareHouseName, StorageId, UnitPrice,MaterialClass, TotalPrice, Lot FROM ScrapBody " +
                 "inner join MaterialInfo on ScrapBody.MaterialNo = MaterialInfo.MaterialNo " +
                "left join WarehouseInfo on ScrapBody.WareHouseId = WarehouseInfo.WarehouseId");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        static public string getOrderNo()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = "I" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM ScrapHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

        static public bool AddScrap(NewScrapViewModel scrapObj, string ID)
        {
            bool retValue = true;
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            Employee employee = EmployeeFactory.getEmployee(scrapObj.scrapHeaderViewModel.ApplyMan);

            scrapObj.scrapHeaderViewModel.WorkNo = scrapObj.scrapHeaderViewModel.WorkNo.Where(e => e != "").ToList();

            ScrapHeader scrapHeader = new ScrapHeader
            {
                OrderNo = ScrapFactory.getOrderNo(),
                AddDateTime = DateTime.Now,
                WorkNo = String.Join(",", scrapObj.scrapHeaderViewModel.WorkNo.Select(e=>e.Trim()).ToList()),
                Status = "0",
                UpdateDateTime = DateTime.Now,
                ScrapType = "0", //廢品報廢單
                ApplyMan = scrapObj.scrapHeaderViewModel.ApplyMan,
                ApplyDate = scrapObj.scrapHeaderViewModel.ApplyDate,
                Reason = scrapObj.scrapHeaderViewModel.Reason,
                ApplyUnit = employee.UNITNO.Trim()
            };

            List<ScrapBody> scrapBodies = new List<ScrapBody>();
            int serialNo = 1;
            foreach (ScrapBodyViewModel scrapBody in scrapObj.scrapBodyViewModels)
            {
                scrapBodies.Add(new ScrapBody
                {
                    OrderNo = scrapHeader.OrderNo,
                    SerialNo = serialNo.ToString("0000"),
                    MaterialNo = scrapBody.MaterialNo,
                    Unit = scrapBody.Unit,
                    Quantity = scrapBody.Quantity,
                    MaterialClass = String.Join(",", scrapBody.MaterialClass.Select(e => e.Trim()).ToList())
                });
                serialNo++;
            }

            db.Ado.BeginTran();
            try
            {
                db.Insertable(scrapHeader).ExecuteCommand();
                db.Insertable(scrapBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        static public bool AddScrapBody(ScrapBodyViewModel scrapObj,out string SerialNo)
        {
            SerialNo = "";
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ScrapBody scrapBody = new ScrapBody
            {
                OrderNo = scrapObj.OrderNo,
                SerialNo = getSerialNo(scrapObj.OrderNo),
                MaterialClass = String.Join(",", scrapObj.MaterialClass.Select(e => e.Trim()).ToList()),
                MaterialNo = scrapObj.MaterialNo,
                Quantity = scrapObj.Quantity,
                Unit = scrapObj.Unit
        };
            SerialNo = scrapBody.SerialNo;

            return db.Insertable(scrapBody).ExecuteCommand() > 0;
        }

        static public bool UpdateScrapBody(ScrapBodyViewModel scrapObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ScrapBody scrapBody = db.Queryable<ScrapBody>().Where(e => e.OrderNo == scrapObj.OrderNo && e.SerialNo == scrapObj.SerialNo).Single();

            scrapBody.MaterialClass = String.Join(",", scrapObj.MaterialClass.Select(e=> e.Trim()).ToList());
            scrapBody.Quantity = scrapObj.Quantity;
            scrapBody.Unit = scrapObj.Unit;
           

            return db.Updateable(scrapBody).ExecuteCommand() > 0;
        }

        static public bool SaveScrapHeader(ScrapHeaderViewModel scrapObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ScrapHeader scrapHeader = db.Queryable<ScrapHeader>().Where(e => e.OrderNo == scrapObj.OrderNo).Single();

            Employee employee = EmployeeFactory.getEmployee(scrapObj.ApplyMan);
            scrapObj.WorkNo = scrapObj.WorkNo.Where(e => e != "").ToList();

            scrapHeader.Reason = scrapObj.Reason;
            scrapHeader.ApplyMan = scrapObj.ApplyMan;
            scrapHeader.ApplyDate = scrapObj.ApplyDate;
            scrapHeader.ApplyUnit = employee.UNITNO.Trim();
            scrapHeader.WorkNo = String.Join(",",scrapObj.WorkNo);
            

            return db.Updateable(scrapHeader).ExecuteCommand() > 0;
        }

        static public bool CloseScrap(ScrapHeaderViewModel scrapObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ScrapHeader scrapHeader = db.Queryable<ScrapHeader>().Where(e => e.OrderNo == scrapObj.OrderNo).Single();

            scrapHeader.Status = "1";

            return db.Updateable(scrapHeader).ExecuteCommand() > 0;
        }


        static public bool DeleteScrapBody(ScrapBodyViewModel scrapObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ScrapBody scrapBody = db.Queryable<ScrapBody>().Where(e => e.OrderNo == scrapObj.OrderNo && e.SerialNo == scrapObj.SerialNo).Single();


            return db.Deleteable(scrapBody).ExecuteCommand() > 0;
        }

        static public string getSerialNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from ScrapBody where OrderNo=@OrderNo";

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