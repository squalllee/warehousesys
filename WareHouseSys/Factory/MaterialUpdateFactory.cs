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
    public class MaterialUpdateFactory
    {
        public static bool updateMaterialUpdateHeader(MaterialUpdateHeader MaterialUpdateHeader, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            MaterialUpdateHeader materialUpdateHeader = db.Queryable<MaterialUpdateHeader>().Where(e => e.OrderNo == MaterialUpdateHeader.OrderNo).Single();

            materialUpdateHeader.UpdateDateTime = DateTime.Now;
            materialUpdateHeader.updateMan = ID;
            materialUpdateHeader.ReqMan = MaterialUpdateHeader.ReqMan;
            materialUpdateHeader.ReqDateTime = MaterialUpdateHeader.ReqDateTime;
            materialUpdateHeader.ReqUnit = db.Queryable<Employee>().Where(e => e.KEYNO == MaterialUpdateHeader.ReqMan).Single().UNITNO;
            materialUpdateHeader.Reason = MaterialUpdateHeader.Reason;

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(materialUpdateHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool addMaterialUpdate(MaterialUpdateBody MaterialUpdateBody)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            

            MaterialInfo materialInfo = db.Queryable<MaterialInfo>().Where(e => e.MaterialNo == MaterialUpdateBody.MaterialNo).Single();

            MaterialUpdateBody.SerialNo = getSerialNo(MaterialUpdateBody.OrderNo);
            MaterialUpdateBody.OriMaterialName = materialInfo.MaterialName;
            MaterialUpdateBody.OriSpec = materialInfo.Spec;
            MaterialUpdateBody.OriAffectClass = materialInfo.AffectClass;
            MaterialUpdateBody.OriEqQuantity = materialInfo.EqQuantity;
            MaterialUpdateBody.OriEstAnnConsumption = materialInfo.EstAnnConsumption.ToString();
            MaterialUpdateBody.OriEstPurPeriod = materialInfo.EstPurPeriod;
            MaterialUpdateBody.OriExpiration = String.IsNullOrEmpty(materialInfo.Expiration) ? 0 : int.Parse(materialInfo.Expiration);
            MaterialUpdateBody.OriFixClass = materialInfo.FixClass;
            MaterialUpdateBody.OriHeight = materialInfo.Height;
            MaterialUpdateBody.OriIsDangerous = materialInfo.IsDangerous;
            MaterialUpdateBody.OriIsFix = materialInfo.IsFix;
            MaterialUpdateBody.OriLength = materialInfo.Length;
            MaterialUpdateBody.OriIsLimitTime = materialInfo.IsLimitTime;
            MaterialUpdateBody.OriReplaceNo = materialInfo.ReplaceNo;
            MaterialUpdateBody.OriROP = materialInfo.ROP;
            MaterialUpdateBody.OriSafetyStock = materialInfo.SafetyStock;
            MaterialUpdateBody.OriSpec = materialInfo.Spec;
            MaterialUpdateBody.OriUnit = materialInfo.Unit;
            MaterialUpdateBody.OriVendorId = materialInfo.VendorId;
            MaterialUpdateBody.Oriweight = materialInfo.weight;
            MaterialUpdateBody.OriWitdh = materialInfo.Witdh;

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(MaterialUpdateBody).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool updateMaterialUpdate(MaterialUpdateBody materialUpdateBody)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            MaterialUpdateBody materialUpdate = db.Queryable<MaterialUpdateBody>().Where(e => e.MaterialNo == materialUpdateBody.MaterialNo && e.SerialNo == materialUpdateBody.SerialNo).Single();

            materialUpdate.AffectClass = materialUpdateBody.AffectClass;
            materialUpdate.EqQuantity = materialUpdateBody.EqQuantity;
            materialUpdate.MaterialName = materialUpdateBody.MaterialName;
            materialUpdate.EstAnnConsumption = materialUpdateBody.EstAnnConsumption.ToString();
            materialUpdate.EstPurPeriod = materialUpdateBody.EstPurPeriod;
            materialUpdate.Expiration = materialUpdateBody.Expiration;
            materialUpdate.FixClass = materialUpdateBody.FixClass;
            materialUpdate.Height = materialUpdateBody.Height;
            materialUpdate.IsDangerous = materialUpdateBody.IsDangerous;
            materialUpdate.IsFix = materialUpdateBody.IsFix;
            materialUpdate.Length = materialUpdateBody.Length;
            materialUpdate.IsLimitTime = materialUpdateBody.IsLimitTime;
            materialUpdate.ReplaceNo = materialUpdateBody.ReplaceNo;
            materialUpdate.ROP = materialUpdateBody.ROP;
            materialUpdate.SafetyStock = materialUpdateBody.SafetyStock;
            materialUpdate.Spec = materialUpdateBody.Spec;
            materialUpdate.Unit = materialUpdateBody.Unit;
            materialUpdate.VendorId = materialUpdateBody.VendorId;
            materialUpdate.weight = materialUpdateBody.weight;
            materialUpdate.Witdh = materialUpdateBody.Witdh;

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(materialUpdate).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static bool deleteMaterialUpdate(MaterialUpdateBody MaterialUpdateBody)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Deleteable(MaterialUpdateBody).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;
        }

        public static ISugarQueryable<MaterialUpdateHeaderViewModel> getMaterialUpdateHeaderViewModel(DataSourceRequest request, string ID, List<string> wgroupIdList)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<MaterialUpdateHeaderViewModel> sugarQueryable = db.SqlQueryable<MaterialUpdateHeaderViewModel>("SELECT OrderNo, ReqUnit ReqUnitId,(select UnitName from Unit where UnitNo=ReqUnit) ReqUnit, ReqMan ReqManId," +
                "(select TMNAME from Employee where KeyNo = ReqMan) ReqMan ,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status, Status StatusId, ReqDateTime, UpdateDateTime, updateMan " +
                "FROM MaterialUpdateHeader");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            if (wgroupIdList.Count == 0)
                sugarQueryable = sugarQueryable.Where(e => e.ReqManId == ID);

            return sugarQueryable;
        }

        public static ISugarQueryable<MaterialUpdateBodyViewModel> getMaterialUpdateBodyViewModel(FilterCriteria filter,string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<MaterialUpdateBodyViewModel> sugarQueryable = db.SqlQueryable<MaterialUpdateBodyViewModel>("SELECT OrderNo, SerialNo,MaterialNo, MaterialName, Spec,(select ClassName from FixClass where ClassId = rb.FixClass) FixClassName,FixClass," +
                "(select ClassName from AffectClass where ClassId=rb.AffectClass) AffectClassName,AffectClass, VendorId, Unit," +
                "(select UnitName from MeasurementUnit where UnitNo = rb.Unit) UnitName," +
                "Length, Witdh, Height, weight, ReplaceNo, ROP, EqQuantity, EstPurPeriod, EstAnnConsumption, IsFix," +
                " OriSpec, OriFixClass, OriAffectClass, OriVendorId, OriUnit, OriLength, OriWitdh, OriHeight, " +
                "Oriweight, OriReplaceNo, OriROP, OriEqQuantity, OriEstPurPeriod, OriEstAnnConsumption, OriIsFix, OriIsDangerous,  " +
                "OriIsLimitTime, OriExpiration, OriSafetyStock, " +
                "IsDangerous, IsLimitTime, Expiration, SafetyStock FROM  MaterialUpdateBody rb");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static MaterialUpdateHeaderViewModel getMaterialUpdateHeaderViewModel(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<MaterialUpdateHeaderViewModel> sugarQueryable = db.SqlQueryable<MaterialUpdateHeaderViewModel>("SELECT OrderNo, ReqUnit ReqUnitId,(select UnitName from Unit where UnitNo=ReqUnit) ReqUnit, ReqMan ReqManId," +
                "(select TMNAME from Employee where KeyNo = ReqMan) ReqMan ,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status, Status StatusId, ReqDateTime, UpdateDateTime, updateMan,Reason " +
                "FROM MaterialUpdateHeader");

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable.Single();
        }

        public static bool saveMaterialUpdate(MaterialUpdateSaveModel materialUpdateSaveModel, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            materialUpdateSaveModel.materialUpdateHeader.OrderNo = getOrderNo();
            materialUpdateSaveModel.materialUpdateHeader.ReqUnit = db.Queryable<Employee>().Where(e => e.KEYNO == materialUpdateSaveModel.materialUpdateHeader.ReqMan).Single().UNITNO;
            materialUpdateSaveModel.materialUpdateHeader.Status = "0";
            materialUpdateSaveModel.materialUpdateHeader.UpdateDateTime = DateTime.Now;
            materialUpdateSaveModel.materialUpdateHeader.updateMan = ID;

            MaterialInfo materialInfo = null;
            int count = 1;
            foreach (MaterialUpdateBody materialUpdateBody in materialUpdateSaveModel.materialUpdateBodies)
            {
                materialInfo = db.Queryable<MaterialInfo>().Where(e => e.MaterialNo == materialUpdateBody.MaterialNo).Single();
                
                materialUpdateBody.SerialNo = count.ToString("0000");
                materialUpdateBody.OrderNo = materialUpdateSaveModel.materialUpdateHeader.OrderNo;
                materialUpdateBody.OriMaterialName = materialInfo.MaterialName;
                materialUpdateBody.OriSpec = materialInfo.Spec;
                materialUpdateBody.OriAffectClass = materialInfo.AffectClass;
                materialUpdateBody.OriEqQuantity = materialInfo.EqQuantity;
                materialUpdateBody.OriEstAnnConsumption = materialInfo.EstAnnConsumption.ToString();
                materialUpdateBody.OriEstPurPeriod = materialInfo.EstPurPeriod;
                materialUpdateBody.OriExpiration = string.IsNullOrEmpty(materialInfo.Expiration) ?0: int.Parse(materialInfo.Expiration);
                materialUpdateBody.OriFixClass = materialInfo.FixClass;
                materialUpdateBody.OriHeight = materialInfo.Height;
                materialUpdateBody.OriIsDangerous = materialInfo.IsDangerous;
                materialUpdateBody.OriIsFix = materialInfo.IsFix;
                materialUpdateBody.OriLength = materialInfo.Length;
                materialUpdateBody.OriIsLimitTime = materialInfo.IsLimitTime;
                materialUpdateBody.OriReplaceNo = materialInfo.ReplaceNo;
                materialUpdateBody.OriROP = materialInfo.ROP;
                materialUpdateBody.OriSafetyStock = materialInfo.SafetyStock;
                materialUpdateBody.OriSpec = materialInfo.Spec;
                materialUpdateBody.OriUnit = materialInfo.Unit;
                materialUpdateBody.OriVendorId = materialInfo.VendorId;
                materialUpdateBody.Oriweight = materialInfo.weight;
                materialUpdateBody.OriWitdh = materialInfo.Witdh;

                count++;
            }

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(materialUpdateSaveModel.materialUpdateHeader).ExecuteCommand();
                db.Insertable(materialUpdateSaveModel.materialUpdateBodies).ExecuteCommand();
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

            string OrderPrefix = "M" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM MaterialUpdateHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from MaterialUpdateBody where OrderNo=@OrderNo";

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


        public static bool closeMaterialUpdate(MaterialUpdateReviewModel materialUpdateReviewModel, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");


            MaterialUpdateHeader materialUpdateHeader = db.Queryable<MaterialUpdateHeader>().Where(e => e.OrderNo == materialUpdateReviewModel.OrderNo).Single();

            List<MaterialUpdateBody> materialUpdateBodies = db.Queryable<MaterialUpdateBody>().Where(e => e.OrderNo == materialUpdateReviewModel.OrderNo).ToList();

            materialUpdateHeader.Status = "1";

            MaterialInfo materialInfo = null;

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(materialUpdateHeader).ExecuteCommand();

                foreach (MaterialUpdateBody materialUpdateBody in materialUpdateBodies)
                {
                    materialInfo = db.Queryable<MaterialInfo>().Where(e => e.MaterialNo == materialUpdateBody.MaterialNo).Single();

                    materialInfo.AffectClass = materialUpdateBody.AffectClass;
                    materialInfo.FixClass = materialUpdateBody.FixClass;
                    materialInfo.VendorId = materialUpdateBody.VendorId;
                    materialInfo.EqQuantity = materialUpdateBody.EqQuantity;
                    materialInfo.EstAnnConsumption = int.Parse(materialUpdateBody.EstAnnConsumption);
                    materialInfo.EstPurPeriod = materialUpdateBody.EstPurPeriod;
                    materialInfo.Expiration = materialUpdateBody.Expiration == null ? null : int.Parse(materialUpdateBody.Expiration.ToString()).ToString();
                    materialInfo.Handtool = false;
                    materialInfo.IsDangerous = materialUpdateBody.IsDangerous;
                    materialInfo.IsFix = materialUpdateBody.IsFix;
                    materialInfo.IsLimitTime = materialUpdateBody.IsLimitTime;
                    materialInfo.Length = materialUpdateBody.Length;
                    materialInfo.Height = materialUpdateBody.Height;
                    materialInfo.MaterialName = materialUpdateBody.MaterialName;
                    materialInfo.ReplaceNo = materialUpdateBody.ReplaceNo;
                    materialInfo.ROP = materialUpdateBody.ROP;
                    materialInfo.SafetyStock = materialUpdateBody.SafetyStock;
                    materialInfo.Spec = materialUpdateBody.Spec;
                    materialInfo.Unit = materialUpdateBody.Unit;
                    materialInfo.weight = materialUpdateBody.weight;
                    materialInfo.Witdh = materialUpdateBody.Witdh;
                    materialInfo.UpdateDateTime = DateTime.Now;
                    materialInfo.UpdateMan = ID;
                    
                    db.Updateable(materialInfo).ExecuteCommand();
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
    }
}