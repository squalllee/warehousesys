using Kendo.Mvc.UI;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class DemandFactory
    {
        public static ISugarQueryable<DemandHeaderViewModel> getEstDemandHeaderViewModel(string ID, DataSourceRequest request, List<string> wgroupIdList)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<DemandHeaderViewModel> sugarQueryable = db.SqlQueryable<DemandHeaderViewModel>("SELECT OrderNo,Annual,Season," +
                 "(select TMNAME from Employee where KEYNO=ApplyMan) ApplyMan,ApplyDate,ApplyMan ApplyManId," +
                 "(select UNITNAME from UNIT where UNITNO = ApplyUnit) ApplyUnit,ApplyUnit ApplyUnitId," +
                 "CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status,Status StatusId, AddDateTime, UpdateDateTime FROM DemandHeader");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);
            sugarQueryable = sugarQueryable.Where(e => e.Status != "-1");
           

            if (wgroupIdList.Count == 0)
                sugarQueryable = sugarQueryable.Where(e => e.ApplyManId == ID);
           
            return sugarQueryable;
        }

        public static DemandHeaderViewModel getEstDemandHeaderViewModel(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<DemandHeaderViewModel> sugarQueryable = db.SqlQueryable<DemandHeaderViewModel>("SELECT OrderNo,Annual,Season," +
                 "(select TMNAME from Employee where KEYNO=ApplyMan) ApplyMan,ApplyDate,ApplyMan ApplyManId," +
                 "(select UNITNAME from UNIT where UNITNO = ApplyUnit) ApplyUnit,ApplyUnit ApplyUnitId," +
                 "CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status,Status StatusId, AddDateTime, UpdateDateTime FROM DemandHeader");


           
                sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo && e.Status != "-1");

            return sugarQueryable.Single();
        }

        public static ISugarQueryable<EstDemandBodyViewModel> getEstDemandBodyViewModel(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<EstDemandBodyViewModel> sugarQueryable = db.SqlQueryable<EstDemandBodyViewModel>("SELECT  ROW_NUMBER() OVER(ORDER BY OrderNo ASC) AS Row#,OrderNo, DemandBody.SerialNo, DemandBody.MaterialNo,MaterialName,Spec, Quantity, EstPriceWithOutTax, EstTotalPriceWithOutTax," +
                 "(select distinct VendorName from VendorInfo where VendorId=DemandBody.VendorId) VendorName, DemandBody.VendorId, Vendor1, " +
                 "Vendor2, Vendor3, PurchaseName, DemanDate, AddDateTime, DemandBody.UpdateDateTime FROM DemandBody " +
                 "inner join MaterialInfo on DemandBody.MaterialNo = MaterialInfo.MaterialNo");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static bool saveDemand(EstDemandSaveModel estDemandSaveModel, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            estDemandSaveModel.demandHeader.OrderNo = getOrderNo();
            estDemandSaveModel.demandHeader.ApplyUnit = db.Queryable<Employee>().Where(e => e.KEYNO == estDemandSaveModel.demandHeader.ApplyMan).Single().UNITNO;
            estDemandSaveModel.demandHeader.Status = "0";
            estDemandSaveModel.demandHeader.ApplyDate = estDemandSaveModel.demandHeader.ApplyDate;
            estDemandSaveModel.demandHeader.UpdateDateTime = DateTime.Now;
            estDemandSaveModel.demandHeader.AddDateTime = DateTime.Now;

            int count = 1;
            foreach (DemandBody demandBody in estDemandSaveModel.demandBodies)
            {
                demandBody.SerialNo = count.ToString("0000");
                demandBody.OrderNo = estDemandSaveModel.demandHeader.OrderNo;
                demandBody.AddDateTime = DateTime.Now;
                demandBody.UpdateDateTime = DateTime.Now;

                count++;
            }

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(estDemandSaveModel.demandHeader).ExecuteCommand();
                db.Insertable(estDemandSaveModel.demandBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool updateDemandHeader(DemandHeaderViewModel demandHeaderViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            DemandHeader demandHeader = db.Queryable<DemandHeader>().Where(e => e.OrderNo == demandHeaderViewModel.OrderNo).Single();

            demandHeader.UpdateDateTime = DateTime.Now;
            demandHeader.ApplyMan = demandHeaderViewModel.ApplyMan;
            demandHeader.ApplyDate = demandHeaderViewModel.ApplyDate;
            demandHeader.ApplyUnit = db.Queryable<Employee>().Where(e => e.KEYNO == demandHeaderViewModel.ApplyMan).Single().UNITNO;
            demandHeader.Annual = demandHeaderViewModel.Annual;
            demandHeader.Season = demandHeaderViewModel.Season;

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(demandHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool addDemandBody(DemandBody demandBody)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            

            demandBody.SerialNo = getSerialNo(demandBody.OrderNo);
            demandBody.AddDateTime = DateTime.Now;
            demandBody.UpdateDateTime = DateTime.Now;


            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(demandBody).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool updateDemandBody(EstDemandBodyViewModel demandBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            DemandBody demandBody = db.Queryable<DemandBody>().Where(e => e.OrderNo == demandBodyViewModel.OrderNo && e.SerialNo == demandBodyViewModel.SerialNo).Single();

            demandBody.UpdateDateTime = DateTime.Now;
            demandBody.MaterialNo = demandBodyViewModel.MaterialNo;
            demandBody.Quantity = demandBodyViewModel.Quantity;
            demandBody.EstPriceWithOutTax = demandBodyViewModel.EstPriceWithOutTax;
            demandBody.EstTotalPriceWithOutTax = demandBodyViewModel.EstTotalPriceWithOutTax;
            demandBody.VendorId = demandBodyViewModel.VendorId;
            demandBody.Vendor1 = demandBodyViewModel.Vendor1;
            demandBody.Vendor2 = demandBodyViewModel.Vendor2;
            demandBody.Vendor3 = demandBodyViewModel.Vendor3;
            demandBody.PurchaseName = demandBodyViewModel.PurchaseName;
            demandBody.DemanDate = demandBodyViewModel.DemanDate;
          

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(demandBody).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool deleteDemand(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            DemandHeader demandHeader = db.Queryable<DemandHeader>().Where(e => e.OrderNo == OrderNo).Single();
            demandHeader.Status = "-1";
            //List<DemandBody> demandBodies = db.Queryable<DemandBody>().Where(e => e.OrderNo == OrderNo).ToList();

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(demandHeader).ExecuteCommand();
                //db.Deleteable(demandHeader).ExecuteCommand();
                //db.Deleteable(demandBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool deleteDemandBody(EstDemandBodyViewModel estDemandBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

          
            DemandBody demandBody = db.Queryable<DemandBody>().Where(e => e.OrderNo == estDemandBodyViewModel.OrderNo && e.SerialNo == estDemandBodyViewModel.SerialNo).Single();

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Deleteable(demandBody).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool closeDemand(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            DemandHeader demandHeader = db.Queryable<DemandHeader>().Where(e => e.OrderNo == OrderNo).Single();

            demandHeader.Status = "1";

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(demandHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static List<EstDemandExcelModel> ExcelData(List<string> OrderNos)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            List< EstDemandExcelModel> estDemandExcelModels = db.SqlQueryable<EstDemandExcelModel>("select DemandHeader.OrderNo, Annual, Season,UNITNAME ApplyUnit, TMNAME ApplyMan, ApplyDate," +
                "DemandBody.SerialNo, DemandBody.MaterialNo,MaterialName,Spec, Quantity, EstPriceWithOutTax," +
                "EstTotalPriceWithOutTax, VendorName, Vendor1, Vendor2, Vendor3, PurchaseName, DemanDate " +
                "from DemandHeader inner join DemandBody on DemandHeader.OrderNo = DemandBody.OrderNo " +
                "inner join MaterialInfo on DemandBody.MaterialNo = MaterialInfo.MaterialNo " +
                "inner join VendorInfo on DemandBody.VendorId = VendorInfo.VendorId " +
                "inner join Employee on ApplyMan = KEYNO " +
                "inner join UNIT on ApplyUnit = UNIT.UNITNO").Where(e=>OrderNos.Contains(e.OrderNo)).ToList();

            return estDemandExcelModels;
        }

        static public string getOrderNo()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = "Z" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM DemandHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from DemandBody where OrderNo=@OrderNo";

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