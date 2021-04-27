using Kendo.Mvc.UI;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Globalization;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class ReqMaterialFactory
    {
        public static ISugarQueryable<ReqMaterialHeaderViewModel> getReqMaterialHeaderViewModel(DataSourceRequest request, string ID, List<string> wgroupIdList)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ReqMaterialHeaderViewModel> sugarQueryable = db.SqlQueryable<ReqMaterialHeaderViewModel>("SELECT OrderNo, ReqUnit ReqUnitId,(select UnitName from Unit where UnitNo=ReqUnit) ReqUnit, ReqMan ReqManId," +
                "(select TMNAME from Employee where KeyNo = ReqMan) ReqMan ,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status, Status StatusId, ReqDateTime, UpdateDateTime, updateMan " +
                "FROM ReqMaterialHeader");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            if(wgroupIdList.Count == 0)
                sugarQueryable = sugarQueryable.Where(e => e.ReqManId == ID);

            return sugarQueryable;
        }

        public static ReqMaterialHeaderViewModel getReqMaterialHeaderViewModel(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ReqMaterialHeaderViewModel> sugarQueryable = db.SqlQueryable<ReqMaterialHeaderViewModel>("SELECT OrderNo, ReqUnit ReqUnitId,(select UnitName from Unit where UnitNo=ReqUnit) ReqUnit, ReqMan ReqManId," +
                "(select TMNAME from Employee where KeyNo = ReqMan) ReqMan ,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status, Status StatusId, ReqDateTime, UpdateDateTime, updateMan " +
                "FROM ReqMaterialHeader");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable.Single();
        }

        public static ISugarQueryable<ReqMaterialBodyViewModel> getReqMaterialBodyViewModel(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ReqMaterialBodyViewModel> sugarQueryable = db.SqlQueryable<ReqMaterialBodyViewModel>("SELECT OrderNo, SerialNo,MaterialNo, MaterialName, Spec,(select SystemName from MainSystem where SystemId = rb.SystemId) SystemName,SystemId," +
                " LineAbb,(select SubSystemName from SubSystem where SystemId = rb.SystemId and SubSystemId = rb.SubSystemId) SubSystemName,SubSystemId, " +
                "(select ClassName from FixClass where ClassId = rb.FixClass) FixClassName,FixClass," +
                "(select ClassName from AffectClass where ClassId=rb.AffectClass) AffectClassName,AffectClass, VendorId, Unit," +
                "(select UnitName from MeasurementUnit where UnitNo = rb.Unit) UnitName,'綠線' LineName," +
                "Length, Witdh, Height, weight, ReplaceNo, ROP, EqQuantity, EstPurPeriod, EstAnnConsumption, EstUnitPrice, IsFix, " +
                "IsDangerous, IsLimitTime, Expiration,SpecifyBrand, SafetyStock FROM  ReqMaterialBody rb");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static bool saveReqMaterial(ReqMaterialSaveModel reqMaterialSaveModel,string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            reqMaterialSaveModel.reqMaterialHeader.OrderNo = getOrderNo();
            reqMaterialSaveModel.reqMaterialHeader.ReqUnit = db.Queryable<Employee>().Where(e => e.KEYNO == reqMaterialSaveModel.reqMaterialHeader.ReqMan).Single().UNITNO;
            reqMaterialSaveModel.reqMaterialHeader.Status = "0";
            reqMaterialSaveModel.reqMaterialHeader.UpdateDateTime = DateTime.Now;
            reqMaterialSaveModel.reqMaterialHeader.updateMan = ID;

            int count = 1;
            foreach (ReqMaterialBody reqMaterialBody in reqMaterialSaveModel.reqMaterialBodies)
            {
                reqMaterialBody.SerialNo = count.ToString("0000");
                reqMaterialBody.OrderNo = reqMaterialSaveModel.reqMaterialHeader.OrderNo;

                count++;
            }

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(reqMaterialSaveModel.reqMaterialHeader).ExecuteCommand();
                db.Insertable(reqMaterialSaveModel.reqMaterialBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool updateReqMaterialHeader(ReqMaterialHeader reqMaterialHeader,string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ReqMaterialHeader reqMaterial = db.Queryable<ReqMaterialHeader>().Where(e => e.OrderNo == reqMaterialHeader.OrderNo).Single();

            reqMaterial.UpdateDateTime = DateTime.Now;
            reqMaterial.updateMan = ID;
            reqMaterial.ReqMan = reqMaterialHeader.ReqMan;
            reqMaterial.ReqDateTime = reqMaterialHeader.ReqDateTime;
            reqMaterial.ReqUnit = db.Queryable<Employee>().Where(e => e.KEYNO == reqMaterialHeader.ReqMan).Single().UNITNO;

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(reqMaterial).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool addReqMaterial(ReqMaterialBody reqMaterialBody)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            reqMaterialBody.SerialNo = getSerialNo(reqMaterialBody.OrderNo);

            bool retValue = true;
            try
            {
                db.Insertable(reqMaterialBody).ExecuteCommand();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }


        public static bool closeReqMaterial(ReqMaterialReviewModel reqMaterialReviewModel,string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
           

            ReqMaterialHeader reqMaterialHeader = db.Queryable<ReqMaterialHeader>().Where(e => e.OrderNo == reqMaterialReviewModel.OrderNo).Single();

            List<ReqMaterialBody> reqMaterialBodies = db.Queryable<ReqMaterialBody>().Where(e => e.OrderNo == reqMaterialReviewModel.OrderNo).ToList();

            reqMaterialHeader.Status = "1";

            string SerialNo = "";
            MaterialInfo materialInfo = null;

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(reqMaterialHeader).ExecuteCommand();
                
                foreach (ReqMaterialBody reqMaterialBody in reqMaterialBodies)
                {
                    SerialNo = GetSerialNo(reqMaterialBody, db);
                    reqMaterialBody.MaterialNo = reqMaterialBody.SystemId + reqMaterialBody.LineAbb + "." + reqMaterialBody.SubSystemId + "." + SerialNo + "." + reqMaterialBody.VendorId;
                    materialInfo = new MaterialInfo
                    {
                        MaterialNo = reqMaterialBody.SystemId + reqMaterialBody.LineAbb + "." + reqMaterialBody.SubSystemId + "." + SerialNo + "." + reqMaterialBody.VendorId,
                        AffectClass = reqMaterialBody.AffectClass,
                        SerialNo = SerialNo,
                        FixClass = reqMaterialBody.FixClass,
                        VendorId = reqMaterialBody.VendorId,
                        SystemId = reqMaterialBody.SystemId,
                        EqQuantity = reqMaterialBody.EqQuantity,
                        EstAnnConsumption = int.Parse(reqMaterialBody.EstAnnConsumption),
                        EstPurPeriod = reqMaterialBody.EstPurPeriod,
                        Expiration = reqMaterialBody.Expiration == null ? null : int.Parse(reqMaterialBody.Expiration.ToString()).ToString(),
                        Handtool = false,
                        IsDangerous = reqMaterialBody.IsDangerous,
                        IsFix = reqMaterialBody.IsFix,
                        IsLimitTime = reqMaterialBody.IsLimitTime,
                        Length = reqMaterialBody.Length,
                        Height = reqMaterialBody.Height,
                        LineAbb = reqMaterialBody.LineAbb,
                        MaterialName = reqMaterialBody.MaterialName,
                        ReplaceNo = reqMaterialBody.ReplaceNo,
                        ROP = reqMaterialBody.ROP,
                        SafetyStock = reqMaterialBody.SafetyStock,
                        Spec = reqMaterialBody.Spec,
                        Unit = reqMaterialBody.Unit,
                        SubSystemId = reqMaterialBody.SubSystemId,
                        weight = reqMaterialBody.weight,
                        Witdh = reqMaterialBody.Witdh,
                        UpdateDateTime = DateTime.Now,
                        UpdateMan = ID
                    };
                   
                    db.Insertable(materialInfo).ExecuteCommand();
                    db.Updateable(reqMaterialBodies).ExecuteCommand();
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

        public static bool updateReqMaterial(ReqMaterialBody reqMaterialBody)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(reqMaterialBody).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool deleteReqMaterial(ReqMaterialBody reqMaterialBody)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Deleteable(reqMaterialBody).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        static public string getSerialNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            //TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            //DateTime datetime = DateTime.Now;

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from ReqMaterialBody where OrderNo=@OrderNo";

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

        static public string getOrderNo()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = "N" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM ReqMaterialHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

        static public string GetSerialNo(ReqMaterialBody reqMaterialBody, SqlSugarClient db)
        {
            //SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            string sql = "SELECT max(MaterialNo) MaterialNo FROM MaterialInfo where MaterialNo like @Prefix + '%'";
            string SerialNo = "";
            string Prefix = reqMaterialBody.SystemId  + reqMaterialBody.LineAbb + "." + reqMaterialBody.SubSystemId + ".";

            string retVal = "";
            try
            {
                SerialNo = db.Ado.SqlQuerySingle<string>(sql, new { Prefix = Prefix});

                if (string.IsNullOrEmpty(SerialNo))
                {
                    retVal = "0001";
                }
                else
                {
                    retVal = (int.Parse(SerialNo.Split('.')[2]) + 1).ToString("0000");
                }
            }
            catch (Exception ex)
            {

            }

            return retVal;
        }



    }
}