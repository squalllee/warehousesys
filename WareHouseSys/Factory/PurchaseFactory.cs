using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Configuration;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModel;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class PurchaseFactory
    {
        static public ISugarQueryable<PurchaseHeader> getPurcheaseHeader(PurchaseParameters parameters)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<PurchaseHeader> sugarQueryable = db.Queryable<PurchaseHeader>();
            if(parameters.PurchaseNo != null)
            {
                sugarQueryable = db.Queryable<PurchaseHeader>().Where(e => e.PurchaseNo == parameters.PurchaseNo);
            }

            if(parameters.CreateDateTimeStart != null)
            {
                sugarQueryable = db.Queryable<PurchaseHeader>().Where(e => e.PurchaseDate.ToString("yyyy-MM-dd").CompareTo(parameters.CreateDateTimeStart) >= 0);
            }

            if (parameters.CreateDateTimeEnd != null)
            {
                sugarQueryable = db.Queryable<PurchaseHeader>().Where(e => e.PurchaseDate.ToString("yyyy-MM-dd").CompareTo(parameters.CreateDateTimeEnd) <= 0);
            }

            if (parameters.sort != null)
            {
                sugarQueryable.OrderBy(String.Format("{0} {1}", parameters.sort, parameters.order));
            }

            return sugarQueryable;
        }

        static public ISugarQueryable<PurchaseHeader> getPurcheaseHeader(string PurchaseNo, string StartDateTime, string EndDateTime, string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<PurchaseHeader> sugarQueryable = db.SqlQueryable<PurchaseHeader>("SELECT PurchaseNo,PurchaseDate,PurchaseName,ContractPriceWithoutVAT,ContractPriceIncludeVAT,PurchaseMethod,BudgetSource," +
                                                                                       "(select TMNAME from Employee where KEYNO= p.PurchaseMan) PurchaseMan, " +
                                                                                       " (select UNITNAME from UNIT where UNITNO= p.PurchaseUnit) PurchaseUnit, " +
                                                                                       " VendorName,VendorContact,Tel,Mobile,trim(Status) Status,RequirementNo,IsCreateRecv,UpdateDateTime " +
                                                                                       "FROM PurchaseHeader p");
            if (PurchaseNo != "")
            {
                sugarQueryable = db.Queryable<PurchaseHeader>().Where(e => e.PurchaseNo == PurchaseNo);
            }

            if (StartDateTime != "")
            {
                sugarQueryable = sugarQueryable.Where(e => e.PurchaseDate >= DateTime.Parse(StartDateTime));
            }


            if (EndDateTime != "")
            {
                sugarQueryable = sugarQueryable.Where(e => e.PurchaseDate < DateTime.Parse(EndDateTime));
            }

            return sugarQueryable;
        }

        static public PurchaseHeader getPurcheaseHeader(string PurchaseNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            PurchaseHeader purchaseHeader = db.Queryable<PurchaseHeader>().Where(e => e.PurchaseNo == PurchaseNo).Single();

            return purchaseHeader;
        }

        static public PurchaseHeaderViewModel getPurcheaseHeaderByReqNo(string RequirementNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<PurchaseHeaderViewModel> purchaseHeaders = db.Ado.SqlQuery<PurchaseHeaderViewModel>("SELECT PurchaseNo,RequirementNo,PurchaseDate,PurchaseName,ContractPriceWithoutVAT,ContractPriceIncludeVAT,PurchaseMethod,BudgetSource," +
                                                                                      "(select TMNAME from Employee where KEYNO= p.PurchaseMan) PurchaseMan,PurchaseMan PurchaseManId, " +
                                                                                      " (select UNITNAME from UNIT where UNITNO= p.PurchaseUnit) PurchaseUnit,PurchaseUnit PurchaseUnitId, " +
                                                                                      " VendorName,VendorContact,Tel,Mobile,Status,RequirementNo,OpenContract,PurClass,IsCreateRecv,UpdateDateTime " +
                                                                                      "FROM PurchaseHeader p where p.RequirementNo = @RequirementNo",new { RequirementNo = RequirementNo });
            return purchaseHeaders.Count > 0 ? purchaseHeaders[0]:null;
       
        }

        static public int getPurcheaseHeaderSerialNo(string RequirementNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<PurchaseHeaderViewModel> sugarQueryable = db.SqlQueryable<PurchaseHeaderViewModel>("SELECT PurchaseNo,RequirementNo,PurchaseDate,PurchaseName,ContractPriceWithoutVAT,ContractPriceIncludeVAT,PurchaseMethod,BudgetSource," +
                                                                                      "(select TMNAME from Employee where KEYNO= p.PurchaseMan) PurchaseMan,PurchaseMan PurchaseManId, " +
                                                                                      " (select UNITNAME from UNIT where UNITNO= p.PurchaseUnit) PurchaseUnit,PurchaseUnit PurchaseUnitId, " +
                                                                                      " VendorName,VendorContact,Tel,Mobile,Status,OpenContract,PurClass,IsCreateRecv,UpdateDateTime " +
                                                                                      "FROM PurchaseHeader p ").Where(e=>e.RequirementNo == RequirementNo);
            return sugarQueryable.Count();

        }


        static public ISugarQueryable<PurchaseDetailViewModel> getPurcheaseBody(string PurchaseNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<PurchaseDetailViewModel> sugarQueryable = db.SqlQueryable<PurchaseDetailViewModel>("SELECT PurchaseNo, p.SerialNo, p.MaterialNo,MaterialInfo.MaterialName,MaterialInfo.Spec, " +
                                                                                        "Price,Quantity, DeliveryLot, DeliveryPlace, PerformancePeriod, " +
                                                                                        "(select UNITNAME from UNIT where UNITNO= p.RequireUnit) ReqireUnit " +
                                                                                        "FROM PurchaseBody p inner join MaterialInfo on p.MaterialNo = MaterialInfo.MaterialNo");

            sugarQueryable = sugarQueryable.Where(e => e.PurchaseNo == PurchaseNo);

            return sugarQueryable;
        }

        static public PurchaseBody getPurcheaseBodyByMaterialNoAndLot(string MaterialNo,string Lot)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            PurchaseBody purchaseBody = db.Queryable<PurchaseBody>().Where(e => e.MaterialNo == MaterialNo && e.DeliveryLot == Lot).First();



            return purchaseBody;
        }

        static public ISugarQueryable<PurchaseDetailViewModel> getPurcheaseBodyByReqNo(string RequirementNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<PurchaseDetailViewModel> sugarQueryable = db.SqlQueryable<PurchaseDetailViewModel>("SELECT PurchaseNo,RequirementNo, p.SerialNo, p.MaterialNo,MaterialInfo.MaterialName,MaterialInfo.Spec, " +
                                                                                        "Price,Quantity, DeliveryLot, DeliveryPlace, convert(varchar, PerformancePeriod, 111) as PerformancePeriod, " +
                                                                                        "(select UNITNAME from UNIT where UNITNO= p.ReqireUnit) ReqireUnit " +
                                                                                        "FROM PurchaseBody p inner join MaterialInfo on p.MaterialNo = MaterialInfo.MaterialNo");

            sugarQueryable = sugarQueryable.Where(e => e.RequirementNo == RequirementNo);

            return sugarQueryable;
        }

        static public PurchaseDetailViewModel getPurcheaseBody(string PurchaseNo,string SerialNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            PurchaseDetailViewModel purchaseDetailViewModel = db.SqlQueryable<PurchaseDetailViewModel>("SELECT PurchaseNo, p.SerialNo, p.MaterialNo,MaterialInfo.MaterialName,MaterialInfo.Spec,MaterialInfo.Unit, " +
                                                                                        "Price,Quantity, DeliveryLot, DeliveryPlace, PerformancePeriod, " +
                                                                                        "(select UNITNAME from UNIT where UNITNO= p.ReqireUnit) ReqireUnit " +
                                                                                        "FROM PurchaseBody p inner join MaterialInfo on p.MaterialNo = MaterialInfo.MaterialNo")
                                                                                        .Where(e => e.PurchaseNo == PurchaseNo && e.SerialNo == SerialNo).Single();


            return purchaseDetailViewModel;
        }



        static public ISugarQueryable<PurchaseBodyViewModel> getPurcheaseBodyByLot(string PurchaseNo,string Lot)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<PurchaseBodyViewModel> sugarQueryable = null;

            if (db.Queryable<PurchaseHeader>().Where(e=>e.OpenContract && e.PurchaseNo == PurchaseNo).Count() > 0)
            {
                sugarQueryable = db.SqlQueryable<PurchaseBodyViewModel>("SELECT PurchaseBody.PurchaseNo, PurchaseBody.MaterialNo, Price, PurchaseBody.Quantity, " +
                                                                        "PurchaseBody.DeliveryLot,isnull(sum(ReceiveBody.ReceivedQty),0) receivedQty,0 UnreceivedQty FROM PurchaseBody " +
                                                                        "left join ReceiveHeader on PurchaseBody.PurchaseNo = ReceiveHeader.PurchaseNo and PurchaseBody.DeliveryLot = ReceiveHeader.DeliveryLot " +
                                                                        "left join ReceiveBody on ReceiveHeader.OrderNo = ReceiveBody.OrderNo and PurchaseBody.MaterialNo = ReceiveBody.MaterialNo " +
                                                                        "group by PurchaseBody.PurchaseNo, PurchaseBody.MaterialNo, Price, PurchaseBody.Quantity,PurchaseBody.DeliveryLot");

                if (db.Queryable<ReceiveHeader>().Where(e=>e.PurchaseNo == PurchaseNo && e.Status == "0").Count() == 0)
                {
                    sugarQueryable = sugarQueryable.Where(e => e.PurchaseNo == PurchaseNo && e.DeliveryLot == Lot);
                }
                else
                {
                    sugarQueryable = sugarQueryable.Where(e => e.PurchaseNo == "" && e.DeliveryLot == "");
                }
            }
            else
            {
                sugarQueryable = db.SqlQueryable<PurchaseBodyViewModel>("SELECT PurchaseNo, MaterialNo, Price,sum(Quantity) Quantity, " +
                                                                                        "isnull((select sum(ReceiveBody.ReceivedQty) Quantity from ReceiveHeader inner join ReceiveBody on ReceiveBody .OrderNo = ReceiveHeader.OrderNo where PurchaseNo = b.PurchaseNo and ReceiveHeader.DeliveryLot = b.DeliveryLot and MaterialNo=b.MaterialNo  group by ReceiveBody.MaterialNo,ReceiveHeader.DeliveryLot),0) receivedQty, " +
                                                                                        "sum(Quantity) - isnull((select sum(ReceiveBody.ReceivedQty) Quantity from ReceiveHeader inner join ReceiveBody on ReceiveBody .OrderNo = ReceiveHeader.OrderNo  where PurchaseNo = b.PurchaseNo and ReceiveHeader.DeliveryLot = b.DeliveryLot  and MaterialNo=b.MaterialNo  group by ReceiveBody.MaterialNo,ReceiveHeader.DeliveryLot),0) UnreceivedQty, " +
                                                                                        " DeliveryLot, DeliveryPlace FROM PurchaseBody b where PurchaseNo ='" + PurchaseNo + "' and DeliveryLot='" + Lot + "' " +
                                                                                        "group by PurchaseNo, MaterialNo, Price,DeliveryLot, DeliveryPlace ").Where(e => e.PurchaseNo == PurchaseNo && e.DeliveryLot == Lot);
            }

            

            return sugarQueryable;
        }

        static public List<PurLotClass> getRecvLots(string PurchaseNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<PurLotClass> purLots = null;

            try
            {
                purLots = db.Ado.SqlQuery<PurLotClass>("SELECT PurchaseNo,sum(Quantity) - isnull((select sum(ReceiveBody.ReceivedQty) Quantity  " +
                                                        "from ReceiveHeader inner join ReceiveBody on ReceiveBody .OrderNo = ReceiveHeader.OrderNo   " +
                                                        " where PurchaseNo = b.PurchaseNo and ReceiveHeader.DeliveryLot = b.DeliveryLot and ReceiveBody.MaterialNo = b.MaterialNo " +
                                                        " group by ReceiveHeader.DeliveryLot),0) Qty,DeliveryLot FROM PurchaseBody b  where PurchaseNo = @PurchaseNo group by PurchaseNo, DeliveryLot,b.MaterialNo " +
                                                        "having sum(Quantity) - isnull((select sum(ReceiveBody.ReceivedQty) Quantity  " +
                                                        "from ReceiveHeader inner join ReceiveBody on ReceiveBody .OrderNo = ReceiveHeader.OrderNo   " +
                                                        "where PurchaseNo = b.PurchaseNo and ReceiveHeader.DeliveryLot = b.DeliveryLot and ReceiveBody.MaterialNo = b.MaterialNo " +
                                                        "group by ReceiveHeader.DeliveryLot),0) > 0 ", new { PurchaseNo = PurchaseNo });

            }
            catch (Exception ex)
            {

            }

            return purLots;
        }

        static public List<PurLotClass> getTransInboundLots(string PurchaseNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<PurLotClass> purLots = null;
            
            try
            {
                purLots = db.Ado.SqlQuery<PurLotClass>("select PurchaseNo, DeliveryLot from ReceiveHeader  " +
                                                      "where IsTransToInbound='0' and PurchaseNo=@PurchaseNo and Status='1'", new { PurchaseNo = PurchaseNo });

                //purLots = db.Ado.SqlQuery<PurLotClass>("select distinct h.PurchaseNo, h.DeliveryLot from ReceiveHeader h   " +
                //                                        "inner join ReceiveBody b on h.OrderNo = b.OrderNo where h.PurchaseNo = @PurchaseNo   " +
                //                                        " group by b.MaterialNo,h.DeliveryLot,h.PurchaseNo " +
                //                                        " having sum(ReceivedQty) -isnull((select sum(InboundQty) aa  from  InboundHeader  " +
                //                                        "inner join InboundBody on InboundHeader.OrderNo = InboundBody.OrderNo " +
                //                                        "where InboundHeader.PurNo = @PurchaseNo and InboundHeader.DeliveryLot=h.DeliveryLot and InboundBody.MaterialNo = b.MaterialNo " +
                //                                        "group by InboundBody.MaterialNo),0) > 0", new { PurchaseNo = PurchaseNo });


            }
            catch(Exception ex)
            {

            }

            return purLots;
        }

        static public bool IsOpenContract(string PurchaseNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            return db.Queryable<PurchaseHeader>().Where(e => e.OpenContract && e.PurchaseNo == PurchaseNo).Count() > 0;
        }

        static public List<PurLotClass> getTransRecvLots(string PurchaseNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<PurLotClass> purLots = null;

            try
            {
                if (db.Queryable<PurchaseHeader>().Where(e => e.OpenContract && e.PurchaseNo == PurchaseNo).Count() > 0)
                {
                    
                    if (db.Queryable<ReceiveHeader>().Where(e => e.PurchaseNo == PurchaseNo && e.Status == "0").Count() > 0)
                    {
                        purLots = new List<PurLotClass>();
                    }
                    else
                    {
                        purLots = new List<PurLotClass>
                        {
                            new PurLotClass
                            {
                                PurchaseNo = PurchaseNo,
                                DeliveryLot = "1",
                            }
                        };
                    }
                }
                else
                {
                    purLots = db.Ado.SqlQuery<PurLotClass>("SELECT distinct PurchaseNo,0 Qty,DeliveryLot FROM PurchaseBody b  where PurchaseNo = @PurchaseNo   " +
                                                                            "group by PurchaseNo, DeliveryLot,b.MaterialNo having sum(Quantity) - isnull((select sum(ReceiveBody.ReceivedQty) Quantity     " +
                                                                            " from ReceiveHeader inner join ReceiveBody on ReceiveBody .OrderNo = ReceiveHeader.OrderNo   " +
                                                                            "  where PurchaseNo = b.PurchaseNo and ReceiveHeader.DeliveryLot = b.DeliveryLot and ReceiveBody.MaterialNo = b.MaterialNo group by ReceiveHeader.DeliveryLot),0) > 0  ",
                                                                            new { PurchaseNo = PurchaseNo });
                }

                

            }
            catch (Exception ex)
            {

            }

            return purLots;
        }

        static public bool IsClose(string PurchaseNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            PurchaseHeader header = db.Queryable<PurchaseHeader>().Where(e => e.PurchaseNo == PurchaseNo).Single();

            if (header == null) return false;

            return (header.Status == "1" ? true : false) ;
        }

        static public bool canTransToRecv(string PurchaseNo,string lot)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ReceiveHeader header = db.Queryable<ReceiveHeader>().Where(e => e.PurchaseNo == PurchaseNo && e.Status == "0" && e.DeliveryLot == lot).Single();

            if (header == null) return true;
            else return false;
        }

        static public bool canTransToInbound(string PurchaseNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            InboundHeader header = db.Queryable<InboundHeader>().Where(e => e.PurNo == PurchaseNo && e.Status == "0").Single();

            if (header == null) return true;
            else return false;
        }

        static public bool savePurchaseBody(PurchaseBody purchaseBody)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            if (db.Updateable(purchaseBody).ExecuteCommand() > 0) return true;
            else return false;
            
        }

        static public bool addPurchaseBody(PurchaseBody purchaseBody)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            if (db.Insertable(purchaseBody).ExecuteCommand() > 0) return true;
            else return false;

        }

        static public bool savePurchaseHeader(PurchaseSaveHeader purchaseSaveHeader)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            PurchaseHeader purchaseHeader = new PurchaseHeader
            {
                ContractPriceIncludeVAT = purchaseSaveHeader.ContractPriceIncludeVAT,
                ContractPriceWithoutVAT = purchaseSaveHeader.ContractPriceWithoutVAT,
                Mobile = purchaseSaveHeader.Mobile,
                PurchaseDate = purchaseSaveHeader.PurchaseDate,
                PurchaseMan = purchaseSaveHeader.PurchaseMan,
                PurchaseMethod = purchaseSaveHeader.PurchaseMethod,
                PurchaseName = purchaseSaveHeader.PurchaseName,
                PurchaseNo = purchaseSaveHeader.PurchaseNo,
                PurchaseUnit = purchaseSaveHeader.PurchaseUnit,
                RequirementNo = purchaseSaveHeader.RequirementNo,
                Tel = purchaseSaveHeader.Tel,
                UpdateDateTime = purchaseSaveHeader.UpdateDateTime,
                VendorContact = purchaseSaveHeader.VendorContact,
                VendorName = purchaseSaveHeader.VendorName
            };

            foreach (string b in purchaseSaveHeader.BudgetSource)
            {
                purchaseHeader.BudgetSource += b + "@";
            }

            purchaseHeader.BudgetSource = purchaseHeader.BudgetSource.TrimEnd('@');

            if (db.Updateable(purchaseHeader).ExecuteCommand() > 0) return true;
            else return false;

        }

        static public bool deletePurBody(string PurchaseNo, string SerialNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            if (db.Deleteable<PurchaseBody>(e=>e.PurchaseNo==PurchaseNo && e.SerialNo == SerialNo).ExecuteCommand() > 0) return true;
            else return false;

        }

        static public bool TransToRecv(string PurchaseNo,string Lot,string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            PurchaseHeader purchaseHeader = getPurcheaseHeader(PurchaseNo);
           

            List<PurchaseBodyViewModel> purchaseBodies = db.SqlQueryable<PurchaseBodyViewModel>("SELECT PurchaseNo, MaterialNo, Price,sum(Quantity) Quantity, " +
                                                                                    "isnull((select sum(ReceiveBody.ReceivedQty) Quantity from ReceiveHeader inner join ReceiveBody on ReceiveBody .OrderNo = ReceiveHeader.OrderNo where PurchaseNo = b.PurchaseNo and ReceiveHeader.DeliveryLot = b.DeliveryLot and MaterialNo=b.MaterialNo  group by ReceiveBody.MaterialNo,ReceiveHeader.DeliveryLot),0) receivedQty, " +
                                                                                    "sum(Quantity) - isnull((select sum(ReceiveBody.ReceivedQty) Quantity from ReceiveHeader inner join ReceiveBody on ReceiveBody .OrderNo = ReceiveHeader.OrderNo  where PurchaseNo = b.PurchaseNo and ReceiveHeader.DeliveryLot = b.DeliveryLot  and MaterialNo=b.MaterialNo  group by ReceiveBody.MaterialNo,ReceiveHeader.DeliveryLot),0) UnreceivedQty, " +
                                                                                    " DeliveryLot, DeliveryPlace FROM PurchaseBody b where PurchaseNo ='" + PurchaseNo + "' and DeliveryLot='" + Lot + "' " +
                                                                                    "group by PurchaseNo, MaterialNo, Price,DeliveryLot, DeliveryPlace ").Where(e => e.PurchaseNo == PurchaseNo && e.DeliveryLot == Lot && e.UnreceivedQty > 0).ToList();

            ReceiveHeader receiveHeader = new ReceiveHeader
            {
                OrderNo = RecvFactory.getOrderNo(),
                AddDateTime = DateTime.Now,
                DeliveryLot = Lot,
                PurchaseNo = PurchaseNo,
                ReceiveMan = ID,
                Status = "0",
                IsTransToInbound = false,
                UpdateDateTime = DateTime.Now,
                updateMan = ID
            };

            int count = 1;
            List<ReceiveBody> receiveBodies = new List<ReceiveBody>();
            foreach(PurchaseBodyViewModel purchaseBody in purchaseBodies)
            {
                receiveBodies.Add(new ReceiveBody
                {
                    OrderNo = receiveHeader.OrderNo,
                    Quantity = purchaseBody.UnreceivedQty,
                    MaterialNo = purchaseBody.MaterialNo,
                    Status = "0",
                    SerialNo = count.ToString("0000")
                });
                count++;
            }

            db.Ado.BeginTran();
            try
            {
                //db.Ado.ExecuteCommand("update PurchaseHeader set Status='1',IsCreateRecv='1' where PurchaseNo=@PurchaseNo", new { PurchaseNo = PurchaseNo });
                db.Insertable(receiveHeader).ExecuteCommand();
                db.Insertable(receiveBodies).ExecuteCommand();
            }
            catch(Exception ex)
            {
                db.Ado.RollbackTran();
                return false;
            }
            finally
            {
                db.Ado.CommitTran();
            }

            return true;
        }

        static public bool OpenContractToRecv(OpenContractToPurViewModel openContractToPurViewModel,string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            PurchaseHeader purchaseHeader = getPurcheaseHeader(openContractToPurViewModel.PurchaseNo);
            
            ReceiveHeader receiveHeader = new ReceiveHeader
            {
                OrderNo = RecvFactory.getOrderNo(),
                AddDateTime = DateTime.Now,
                DeliveryLot = openContractToPurViewModel.Lot,
                PurchaseNo = openContractToPurViewModel.PurchaseNo,
                ReceiveMan = ID,
                Status = "0",
                IsTransToInbound = false,
                UpdateDateTime = DateTime.Now,
                updateMan = ID
            };

            int count = 1;
            List<ReceiveBody> receiveBodies = new List<ReceiveBody>();
            foreach (PurchaseBodyViewModel purchaseBody in openContractToPurViewModel.PurBodies)
            {
                receiveBodies.Add(new ReceiveBody
                {
                    OrderNo = receiveHeader.OrderNo,
                    Quantity = purchaseBody.Quantity,
                    MaterialNo = purchaseBody.MaterialNo,
                    Status = "0",
                    SerialNo = count.ToString("0000")
                });
                count++;
            }

            db.Ado.BeginTran();
            try
            {
                db.Insertable(receiveHeader).ExecuteCommand();
                db.Insertable(receiveBodies).ExecuteCommand();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                return false;
            }
            finally
            {
                db.Ado.CommitTran();
            }

            return true;
        }

        static public bool TransToInbound(string PurchaseNo, List<string> Lots, string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            PurchaseHeader purchaseHeader = getPurcheaseHeader(PurchaseNo);


            string temp = "";
            for (int i = 0; i < Lots.Count; i++)
            {
                temp += "'" + Lots[i] + "',";
               
            }
            

            List<TransToInboundViewModel> transToInboundViewModels = db.Ado.SqlQuery<TransToInboundViewModel>("SELECT ReceiveHeader.PurchaseNo,ReceiveBody.MaterialNo , " +
                                                                                        "isnull(sum(ReceiveBody.ReceivedQty),0) ReceivedQty,Price  " +
                                                                                        "FROM ReceiveHeader inner join ReceiveBody on ReceiveBody.OrderNo = ReceiveHeader.OrderNo  " +
                                                                                        "inner join PurchaseBody on ReceiveHeader.PurchaseNo = PurchaseBody.PurchaseNo and PurchaseBody.MaterialNo = ReceiveBody.MaterialNo " +
                                                                                        "where ReceiveHeader.Status='1' and ReceiveHeader.PurchaseNo=@PurchaseNo  and ReceiveHeader.DeliveryLot in (" + temp.TrimEnd(',') + ") " +
                                                                                        "group by ReceiveBody.MaterialNo,ReceiveHeader.PurchaseNo,Price", 
                                                                                        new { PurchaseNo = PurchaseNo });

            List<ReceiveHeader> receiveHeaders = db.SqlQueryable<ReceiveHeader>("select * from ReceiveHeader where DeliveryLot in (" + temp.TrimEnd(',') + ")").ToList();

            foreach(ReceiveHeader receiveHeader in receiveHeaders)
            {
                receiveHeader.IsTransToInbound = true;
            }


            if (transToInboundViewModels.Count == 0) return false;

            InboundHeader inboundHeader = new InboundHeader
            {
                OrderNo = InboundFactory.getOrderNo(),
                PurNo = PurchaseNo,
                InboundMan = ID,
                InboundDate = DateTime.Now,
                Status = "0",
                AddDateTime = DateTime.Now,
                UpdateDateTime = DateTime.Now
            };

            int count = 1;
            List<InboundBody> inboundBodies = new List<InboundBody>();
            foreach (TransToInboundViewModel transToInboundViewModel in transToInboundViewModels)
            {
                inboundBodies.Add(new InboundBody
                {
                    OrderNo = inboundHeader.OrderNo,
                    Quantity = transToInboundViewModel.ReceivedQty,
                    MaterialNo = transToInboundViewModel.MaterialNo,
                    InboundQty = 0,
                    SaveStockAlert = true,
                    Price = transToInboundViewModel.Price,
                    SerialNo = count.ToString("0000")
                });
                count++;
            }

            db.Ado.BeginTran();
            try
            {
                //db.Ado.ExecuteCommand("update PurchaseHeader set Status='2' where PurchaseNo=@PurchaseNo", new { PurchaseNo = PurchaseNo });
                db.Insertable(inboundHeader).ExecuteCommand();
                db.Insertable(inboundBodies).ExecuteCommand();
                db.Updateable(receiveHeaders).ExecuteCommand();
            }
            catch(Exception ex)
            {
                db.Ado.RollbackTran();
                return false;
            }
            finally
            {
                db.Ado.CommitTran();
            }

            return true;
        }



        static public string getDetailSerialNo(string PurchaseNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from PurchaseBody where PurchaseNo=@PurchaseNo";

            string SerialNo = "";

            try
            {
                SerialNo = db.Ado.SqlQuerySingle<string>(sql, new { PurchaseNo = PurchaseNo });
            }
            catch (Exception ex)
            {

            }

            SerialNo = SerialNo.Substring(0,1) + (int.Parse(SerialNo.Substring(1, 3)) + 1).ToString("000");

            return SerialNo;
        }
    }
}