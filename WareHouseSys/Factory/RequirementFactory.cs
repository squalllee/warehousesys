using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Configuration;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class RequirementFactory
    {
        static public ISugarQueryable<RequirementHeader> getRequirementHeader(string RequireNo,string StartDateTime,string EndDateTime,string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<RequirementHeader> sugarQueryable = db.Queryable<RequirementHeader>();
            if (RequireNo != "")
            {
                sugarQueryable = db.Queryable<RequirementHeader>().Where(e => e.OrderNo == RequireNo);
            }

            if (StartDateTime != "")
            {
                sugarQueryable = sugarQueryable.Where(e => e.ApplicationDate >= DateTime.Parse(StartDateTime));
            }


            if (EndDateTime != "")
            {
                sugarQueryable = sugarQueryable.Where(e => e.ApplicationDate < DateTime.Parse(EndDateTime));
            }

            sugarQueryable = sugarQueryable.Where(e => e.Status != "-1");
            return sugarQueryable;
        }

        static public RequirementHeader getRequirementHeader(string RequireNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            RequirementHeader requirementHeader = db.Queryable<RequirementHeader>().Where(e => e.OrderNo == RequireNo).Single();
           
            return requirementHeader;
        }

        static public RequirementHeaderViewModel getRequirementHeaderViewModel(string RequireNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            RequirementHeaderViewModel requirementHeaderViewModel = db.SqlQueryable<RequirementHeaderViewModel>("SELECT OrderNo, ApplicationDate,TMNAME Applicant,Applicant ApplicantId,UNITNAME ApplicantUnit, SpecifyBrand, SpecifyReason, Emergency, EmergencyReason, " +
                "Temporary,TemporaryReason,AcceptanceStd, AcceptanceReason, Status, AddDateTime, UpdateDateTime,IsCreateForm " +
                "FROM RequirementHeader inner join Employee on KeyNo = Applicant " +
                "inner join UNIT on Employee.UNITNO = UNIT.UNITNO").Where(e=>e.OrderNo == RequireNo).Single();

            return requirementHeaderViewModel;
        }

        static public bool updateRequirementHeader(RequirementHeader requirementHeader)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            try
            {
                db.Updateable<RequirementHeader>(requirementHeader).ExecuteCommand();
            }
            catch
            {
                return false;
            }

            return true;
        }

        static public ISugarQueryable<RequirementBody> getRequirementBody(string RequireNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<RequirementBody> sugarQueryable = db.Queryable<RequirementBody>().Where(e => e.OrderNo == RequireNo);

            return sugarQueryable;
        }

        static public RequirementDetailUpdateModel getRequirementBody(string RequireNo,string SerialNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            RequirementDetailUpdateModel MaterialInfo = null;

            try
            {
                MaterialInfo = db.SqlQueryable<RequirementDetailUpdateModel>("SELECT OrderNo,MaterialInfo.MaterialNo, MaterialName, RequireReason,Spec, Unit, SystemId, SubSystemId, LineAbb, Length, rBody.SerialNo, FixClass," +
                                                                    "AffectClass, VendorId, Witdh, Height, weight, ReplaceNo, EqQuantity, IsFix," +
                                                                    "RequireUnit,RequirementQty,EstPrice,Inventory,RequireDate,PeriodStart1," +
                                                                    "PeriodEnd1,PeriodQty1,PeriodStart2,PeriodEnd2,PeriodQty2,RepairClass,RequireReason " +
                                                                    "IsDangerous, IsLimitTime, Expiration, SafetyStock, FailureRate, " +
                                                                    "(SELECT isnull(sum(Quantity),0) Quantity FROM Inventory where MaterialNo = rBody.MaterialNo group by MaterialNo) Quantity, " +
                                                                    "(SELECT isnull(OnOrderInventoryQty,'0') OnOrderInventoryQty FROM OnOrderInventory where MaterialNo = rBody.MaterialNo) OnOrderInventory, " +
                                                                    "(SELECT isnull(sum(Quantity),'0') Quantity FROM Inventory where MaterialNo =  ReplaceNo group by MaterialNo) ReplaceQuantity " +
                                                                    "FROM  MaterialInfo  inner join RequirementBody rBody on MaterialInfo.MaterialNo = rBody.MaterialNo")
                                                                    .Where(e =>e.OrderNo == RequireNo && e.SerialNo == SerialNo).Single();
            }
            catch (Exception ex)
            {

            }


            return MaterialInfo;
        }

        static public RequirementDetailUpdateModel getRequirementReport(string RequireNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            RequirementDetailUpdateModel MaterialInfo = null;

            try
            {
                MaterialInfo = db.SqlQueryable<RequirementDetailUpdateModel>("SELECT ROW_NUMBER() OVER(ORDER BY rHeader.ApplicationDate) AS ROWID,rHeader.ApplicationDate,rHeader.OrderNo,(SELECT TMNAME FROM Employee where KEYNO=rHeader.Applicant ) Applicant," +
                                                                    "(SELECT UNITNAME FROM Employee inner join UNIT on Employee.UNITNO = UNIT.UNITNO where KEYNO=rHeader.Applicant) ApplicantUnit," +
                                                                    "MaterialInfo.MaterialNo, MaterialName, RequireReason,Spec, Unit, IsDevelopment, ReplaceNo, EqQuantity, RepairPeriod, Simple, IsFix," +
                                                                    "(select UNITNAME from UNIT where UNITNO = RequireUnit) RequireUnit, " +
                                                                    "RequirementQty,EstPrice,Inventory,RequireDate,PeriodStart1,PeriodEnd1,PeriodQty1, " +
                                                                    "PeriodStart2,PeriodEnd2,PeriodQty2,DeliveryPeriod,RequireReason IsDangerous, IsLimitTime, Expiration, SafetyStock, FailureRate, (SELECT isnull(sum(Quantity),0) Quantity FROM Inventory  " +
                                                                    "where MaterialNo = rBody.MaterialNo group by MaterialNo) Quantity, (SELECT isnull(OnOrderInventoryQty,'0') OnOrderInventoryQty FROM OnOrderInventory where MaterialNo = rBody.MaterialNo) OnOrderInventory,  " +
                                                                    "(SELECT isnull(sum(Quantity),'0') Quantity FROM Inventory where MaterialNo =  ReplaceNo group by MaterialNo) ReplaceQuantity FROM  MaterialInfo   " +
                                                                    "inner join RequirementBody rBody on MaterialInfo.MaterialNo = rBody.MaterialNo " +
                                                                    "inner join RequirementHeader rHeader on rBody.OrderNo=rHeader.OrderNo").Where(e => e.OrderNo == RequireNo).Single();
            }
            catch (Exception ex)
            {

            }


            return MaterialInfo;
        }

        static public bool deleteRequiremenet(string OrderNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            bool retValue = true;

            db.Ado.BeginTran();
            try
            {
                db.Ado.ExecuteCommand("update RequirementHeader set Status = '-1' where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                //db.Ado.ExecuteCommand("delete from RequirementHeader where OrderNo=@OrderNo",new { OrderNo= OrderNo });
                //db.Ado.ExecuteCommand("delete from RequirementBody where OrderNo=@OrderNo", new { OrderNo = OrderNo });
                db.Ado.CommitTran();
            }
            catch
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;

        }

        static public bool updateRequiremenetHeader(RequirementHeader requirementHeader)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            RequirementHeader requirement = db.Queryable<RequirementHeader>().Where(e => e.OrderNo == requirementHeader.OrderNo).Single();

            requirement.UpdateDateTime = DateTime.Now;
            requirement.AcceptanceStd = requirementHeader.AcceptanceStd;
            requirement.AcceptanceReason = requirementHeader.AcceptanceReason;
            requirement.Emergency = requirementHeader.Emergency;
            requirement.EmergencyReason = requirementHeader.EmergencyReason;
            requirement.SpecifyBrand = requirementHeader.SpecifyBrand;
            requirement.SpecifyReason = requirementHeader.SpecifyReason;
            requirement.Temporary = requirementHeader.Temporary;
            requirement.TemporaryReason = requirementHeader.TemporaryReason;
           
            return db.Updateable(requirement).ExecuteCommand() > 0;

        }

        static public bool saveRequiremenet(RequirementSaveModel saveModel)
        {
            bool retVal = true;
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            db.Ado.BeginTran();

            try
            {
                db.Insertable<RequirementHeader>(saveModel.requirementHeader).ExecuteCommand();
                foreach(RequirementBody requirementBody in saveModel.requirementBodies)
                {
                    db.Insertable<RequirementBody>(requirementBody).ExecuteCommand();
                }
                db.Ado.CommitTran();
            }
            catch(Exception ex)
            {
                retVal = false;
                db.Ado.RollbackTran();
            }
            return retVal;
            
        }

        static public bool saveRequiremenetDetail(RequirementBody saveModel)
        {
            bool retVal = true;
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            saveModel.SerialNo = getDetailSerialNo(saveModel.OrderNo);

            db.Ado.BeginTran();

            try
            {
                db.Insertable<RequirementBody>(saveModel).ExecuteCommand();
               
                db.Ado.CommitTran();
            }
            catch
            {
                retVal = false;
                db.Ado.RollbackTran();
            }
            return retVal;

        }

        static public bool updateRequiremenetDetail(RequirementBody updateModel)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            if (db.Updateable<RequirementBody>(updateModel).ExecuteCommand() > 0) return true;
            else return false;

        }

        static public bool deleteRequiremenetDetail(string OrderNo,string MaterialNo,string SerialNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            RequirementBody  requirementBody = db.Queryable<RequirementBody>().Where(e => e.OrderNo == OrderNo && e.MaterialNo == MaterialNo && e.SerialNo == SerialNo).Single();

            if (db.Deleteable<RequirementBody>(requirementBody).ExecuteCommand() > 0) return true;
            else return false;

        }


        static public string getOrderNo()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'"+ OrderPrefix + "-0000') OrderNo FROM RequirementHeader where SUBSTRING(OrderNo,1,7) = @OrderPrefix";
            var OrderNo = "";
            try
            {
                OrderNo = db.Ado.SqlQuerySingle<string>(sql, new { OrderPrefix = OrderPrefix });
            }
            catch (Exception ex)
            {

            }

            return OrderNo;
        }

        static public string getDetailSerialNo(string OrderNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from RequirementBody where OrderNo=@OrderNo";
            
            try
            {
                OrderNo = db.Ado.SqlQuerySingle<string>(sql, new { OrderNo = OrderNo });
            }
            catch (Exception ex)
            {

            }

            OrderNo = (int.Parse(OrderNo) + 1).ToString("0000");

            return OrderNo;
        }

        static public bool IsClose(string RequireNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            RequirementHeader header = db.Queryable<RequirementHeader>().Where(e => e.OrderNo == RequireNo).Single();

            if (header == null) return false;

            return !header.IsCreateForm && (header.Status == "1" ? true : false);
        }

        static public List<TransToPurViewModel> getTransToPurInfo(string requireNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<TransToPurViewModel> transToPurViewModel = null;

            try
            {
                transToPurViewModel = db.SqlQueryable<TransToPurViewModel>("select RequirementBody.OrderNo,ROW_NUMBER() OVER(ORDER BY RequirementBody.MaterialNo) SerialNo,RequirementBody.SerialNo reqSerialNo,RequirementBody.MaterialNo,MaterialName,Spec,Unit,'' Price " +
                                                                    ",RequirementQty Quantity,'' DeliveryLot,'' DeliveryPlace,getdate() PerformancePeriod,RequireUnit ReqireUnit" +
                                                                    ",(select UNITNAME from Unit where UNITNO = RequirementBody.RequireUnit) RequireUnitName from RequirementBody  " +
                                                                    "inner join MaterialInfo on RequirementBody.MaterialNo = MaterialInfo.MaterialNo ").Where(e => e.OrderNo == requireNo).ToList();
            }
            catch (Exception ex)
            {

            }

            return transToPurViewModel;
        }

        static public bool TransToPur(TransToPurSaveModel SaveObj)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<PurchaseBody> purchaseBodies = new List<PurchaseBody>();

            RequirementHeader requirementHeader = getRequirementHeader(SaveObj.purchaseHeader.RequirementNo);

           // int count = PurchaseFactory.getPurcheaseHeaderSerialNo(SaveObj.purchaseHeader.RequirementNo);

            if (SaveObj.purchaseHeader.OpenContract)
            {
                requirementHeader.Status = "2"; //已轉採購單(開口契約)
            }
            else
            {
                requirementHeader.Status = "3"; //已轉採購單
            }
            

            SaveObj.purchaseHeader.Status = "0";
            

            int serialNo = 1;

            PurchaseHeader purchaseHeader = new PurchaseHeader
            {
                //PurchaseNo = SaveObj.purchaseHeader.PurchaseNo.Split('-')[0] + "-" + (count+1).ToString(),
                PurchaseNo = SaveObj.purchaseHeader.PurchaseNo,
                RequirementNo = SaveObj.purchaseHeader.RequirementNo,
                ContractPriceIncludeVAT = SaveObj.purchaseHeader.ContractPriceIncludeVAT,
                ContractPriceWithoutVAT = SaveObj.purchaseHeader.ContractPriceWithoutVAT,
                Mobile = SaveObj.purchaseHeader.Mobile,
                PurchaseDate = SaveObj.purchaseHeader.PurchaseDate,
                PurchaseMan = SaveObj.purchaseHeader.PurchaseMan,
                PurchaseMethod = SaveObj.purchaseHeader.PurchaseMethod,
                PurchaseName = SaveObj.purchaseHeader.PurchaseName,
                PurchaseUnit = SaveObj.purchaseHeader.PurchaseUnit,
                Status = "0",
                Tel = SaveObj.purchaseHeader.Tel,
                VendorContact = SaveObj.purchaseHeader.VendorContact,
                VendorName = SaveObj.purchaseHeader.VendorName,
                UpdateDateTime = DateTime.Now,
                OpenContract = SaveObj.purchaseHeader.OpenContract,
                PurClass = SaveObj.purchaseHeader.PurClass,
                
            };

            purchaseHeader.BudgetSource = string.Join("@", SaveObj.purchaseHeader.BudgetSource);

            List<OnOrderInventory> insertOnOrderInventories = new List<OnOrderInventory>();
            List<OnOrderInventory> updateOnOrderInventories = new List<OnOrderInventory>();

            foreach (TransToPurViewModel transToPurViewModel in SaveObj.purchaseBodies)
            {
                ISugarQueryable<OnOrderInventory> sugarQueryable = db.Queryable<OnOrderInventory>().Where(e => e.MaterialNo == transToPurViewModel.MaterialNo);
                if (sugarQueryable.Count() == 0)
                {
                    if(insertOnOrderInventories.Where(e => e.MaterialNo == transToPurViewModel.MaterialNo).Count() > 0)
                    {
                        OnOrderInventory onOrderInventory = insertOnOrderInventories.Where(e => e.MaterialNo == transToPurViewModel.MaterialNo).Single();
                        onOrderInventory.OnOrderInventoryQty += int.Parse(transToPurViewModel.Quantity.ToString());
                    }
                    else
                    {
                        insertOnOrderInventories.Add(new OnOrderInventory
                        {
                            MaterialNo = transToPurViewModel.MaterialNo,
                            OnOrderInventoryQty = int.Parse(transToPurViewModel.Quantity.ToString()),
                            AddDateTime = DateTime.Now,
                            UpdateDateTime = DateTime.Now
                        });
                    }
                   
                }
                else
                {
                    OnOrderInventory onOrderInventory = sugarQueryable.Count() == 0 ? insertOnOrderInventories.Where(e => e.MaterialNo == transToPurViewModel.MaterialNo).First() : sugarQueryable.Single();
                    onOrderInventory.OnOrderInventoryQty += int.Parse(transToPurViewModel.Quantity.ToString());
                    onOrderInventory.UpdateDateTime = DateTime.Now;
                    updateOnOrderInventories.Add(onOrderInventory);
                }

                purchaseBodies.Add(new PurchaseBody
                {
                    PurchaseNo = purchaseHeader.PurchaseNo,
                    DeliveryLot = transToPurViewModel.DeliveryLot,
                    DeliveryPlace = transToPurViewModel.DeliveryPlace,
                    MaterialNo = transToPurViewModel.MaterialNo,
                    PerformancePeriod = transToPurViewModel.PerformancePeriod ,
                    Price = decimal.Parse(transToPurViewModel.Price),
                    Quantity = int.Parse(transToPurViewModel.Quantity.ToString()),
                    RequireUnit = transToPurViewModel.ReqireUnit,
                    SerialNo = serialNo.ToString("000")

                });
                serialNo++;
            }

            db.Ado.BeginTran();
            try
            {

                if(insertOnOrderInventories.Count > 0)
                    db.Insertable(insertOnOrderInventories).ExecuteCommand();
                if(updateOnOrderInventories.Count > 0)
                    db.Updateable(updateOnOrderInventories).ExecuteCommand();

                db.Insertable(purchaseHeader).ExecuteCommand();
                db.Insertable(purchaseBodies).ExecuteCommand();

                db.Updateable(requirementHeader).ExecuteCommand();

                db.Ado.CommitTran();
                return true;
            }
            catch(Exception ex)
            {
                db.Ado.RollbackTran();
                return false;
            }
        }
    }
}