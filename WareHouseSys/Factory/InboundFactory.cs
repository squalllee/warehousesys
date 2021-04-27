using SqlSugar;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;
using Kendo.Mvc.UI;

namespace WareHouseSys.Factory
{
    public class InboundFactory
    {

        static public ISugarQueryable<InboundSearchViewModel> getInboundSearchViewModel(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<InboundSearchViewModel> sugarQueryable = db.SqlQueryable<InboundSearchViewModel>("SELECT InboundBody.OrderNo, InboundBody.SerialNo, InboundBody.MaterialNo, InboundBody.Expiration ,InboundBody.Quantity, InboundBody.InboundQty, InboundBody.WarehouseId, InboundBody.StorageId, InboundBody.OccupiedStorageId, InboundBody.Note, InboundBody.Lot, InboundBody.Price, InboundBody.SaveStockAlert, " +
                "PurNo, TMNAME InboundMan, DeliveryLot, InboundDate, UNIT.UNITNAME, MaterialInfo.MaterialName, Spec, WarehouseInfo.WareHouseName," +
                "case when InboundHeader.Status = '1' then '已陳核' when InboundHeader.Status = '0' then '辦理中' else '已作廢' end Status  " +
                "FROM  InboundBody " +
                "inner join InboundHeader on InboundBody.OrderNo = InboundHeader.OrderNo " +
                "inner join WarehouseInfo on WarehouseInfo.WarehouseId = InboundBody.WareHouseId " +
                "inner join MaterialInfo on MaterialInfo.MaterialNo = InboundBody.MaterialNo " +
                "inner join Employee on Employee.KEYNO = InboundHeader.InboundMan " + "inner join UNIT on Employee.UNITNO = UNIT.UNITNO ");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

        static public ISugarQueryable<InboundHeaderViewModel> getInboundHeader(FilterCriteria filter, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<InboundHeaderViewModel> sugarQueryable = db.SqlQueryable<InboundHeaderViewModel>("SELECT OrderNo,PurNo,(select TMNAME from Employee where KEYNO = InboundMan) InboundMan,Inboundman InboundManId " +
                ",InboundDate,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status " +
                "FROM InboundHeader");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            //sugarQueryable.Where(e => e.InboundManId == ID);

            return sugarQueryable;
        }

        static public ISugarQueryable<InboundBodyViewModel> getInboundBody(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<InboundBodyViewModel> sugarQueryable = db.SqlQueryable<InboundBodyViewModel>("SELECT OrderNo,InboundBody.SerialNo,InboundBody.MaterialNo,MaterialInfo.MaterialName,MaterialInfo.Unit " +
                ",MaterialInfo.Spec,InboundBody.Expiration,Quantity,InboundQty,WarehouseId,StorageId,OccupiedStorageId,Note,Lot,SaveStockAlert " +
                "FROM InboundBody inner join MaterialInfo on InboundBody.MaterialNo = MaterialInfo.MaterialNo");


            sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        static public InboundHeaderViewModel getInboundViewHeader(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            InboundHeaderViewModel inboundHeaderViewModel = db.SqlQueryable<InboundHeaderViewModel>("SELECT OrderNo,PurNo,(select TMNAME from Employee where KEYNO = InboundMan) InboundMan,Inboundman InboundManId " +
                ",InboundDate,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status " +
                "FROM InboundHeader").Where(e => e.OrderNo == OrderNo).Single();

            return inboundHeaderViewModel;
        }

        static public string getOrderNo()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = "B" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM InboundHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

        static public ISugarQueryable<RecvDataViewModel> getInboundData(string OrderNo, string Lot)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<RecvDataViewModel> sugarQueryable = db.SqlQueryable<RecvDataViewModel>("select DeliveryLot,ReceiveHeader.OrderNo,ReceiveBody.SerialNo,ReceiveBody.Note,ReceiveBody.MaterialNo,MaterialInfo.Spec,MaterialInfo.MaterialName,MaterialInfo.Unit,ReceiveBody.WarehouseId,StorageId, " +
                                                                                        "WarehouseInfo.WareHouseName,Quantity,isnull(ReceivedQty,0) ReceivedQty,Quantity - isnull(ReceivedQty,0) UnreceivedQty " +
                                                                                        "from ReceiveBody inner join ReceiveHeader on ReceiveBody.OrderNo = ReceiveHeader.OrderNo " +
                                                                                        "inner join MaterialInfo on MaterialInfo.MaterialNo = ReceiveBody.MaterialNo " +
                                                                                        "left join WarehouseInfo on WarehouseInfo.WarehouseId = ReceiveBody.WarehouseId ").Where(e => e.OrderNo == OrderNo && e.DeliveryLot == Lot);

            return sugarQueryable;
        }

        static public bool updateInboundData(InboundBodyViewModel updateObj,string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            InboundBody inboundBody = new InboundBody
            {
                MaterialNo = updateObj.MaterialNo,
                InboundQty = updateObj.InboundQty,
                Note = updateObj.Note,
                OccupiedStorageId = updateObj.OccupiedStorageId,
                OrderNo = updateObj.OrderNo,
                StorageId = updateObj.Storage.StorageId,
                WarehouseId = updateObj.warehouseInfo.WarehouseId,
                SerialNo = updateObj.SerialNo,
                Lot = updateObj.Lot,
                Quantity = updateObj.Quantity,
                Expiration = updateObj.Expiration,
                SaveStockAlert = updateObj.SaveStockAlert
            };
            if (db.Updateable(inboundBody).ExecuteCommand() > 0) return true;
            else return false;
        }

        static public bool addInboundData(InboundBodyViewModel updateObj, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            InboundBody inboundBody = new InboundBody
            {
                MaterialNo = updateObj.MaterialNo,
                InboundQty = updateObj.InboundQty,
                Note = updateObj.Note,
                OccupiedStorageId = updateObj.OccupiedStorageId,
                OrderNo = updateObj.OrderNo,
                StorageId = updateObj.Storage.StorageId,
                WarehouseId = updateObj.warehouseInfo.WarehouseId,
                SerialNo = getSerialNo(updateObj.OrderNo),
                Lot = updateObj.Lot,
                Quantity = updateObj.Quantity,
                Expiration = updateObj.Expiration,
                SaveStockAlert = updateObj.SaveStockAlert
            };
            if (db.Insertable(inboundBody).ExecuteCommand() > 0) return true;
            else return false;
        }

        static public bool deleteInboundData(InboundBodyViewModel updateObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            InboundBody inboundBody = db.Queryable<InboundBody>().Where(e => e.OrderNo == updateObj.OrderNo && e.SerialNo == updateObj.SerialNo).Single();
            
            if (db.Deleteable(inboundBody).ExecuteCommand() > 0) return true;
            else return false;
        }

        static public bool updateInboundHeader(InboundHeaderViewModel updateObj, string ID)
        {
            bool retValue = true;
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            InboundHeader inboundHeader = db.Queryable<InboundHeader>().Where(e => e.OrderNo == updateObj.OrderNo).Single();

            PurchaseHeader purchaseHeader = db.Queryable<PurchaseHeader>().Where(e => e.PurchaseNo == inboundHeader.PurNo).Single();

            inboundHeader.InboundDate = DateTime.Parse(updateObj.InboundDate);

            inboundHeader.InboundMan = updateObj.InboundMan;

            //List<InboundBody> inboundBodies = db.Queryable<InboundBody>().Where(e => e.OrderNo == updateObj.OrderNo).ToList();

            //foreach(InboundBody inboundBody in inboundBodies)
            //{
            //    if(purchaseHeader.PurClass == "新品")
            //    {
            //        inboundBody.Lot = "N" + DateTime.Parse(updateObj.InboundDate).ToString("yyyyMMdd");
            //    }
            //    else if(purchaseHeader.PurClass == "代購")
            //    {
            //        inboundBody.Lot = "CP" + DateTime.Parse(updateObj.InboundDate).ToString("yyyyMMdd");
            //    }
            //    else if(purchaseHeader.PurClass == "備品")
            //    {
            //        inboundBody.Lot = "CJ" + DateTime.Parse(updateObj.InboundDate).ToString("yyyyMMdd");
            //    }

            //    inboundBody.Expiration = inboundBody.Expiration == null ? null : DateTime.Parse(inboundBody.Expiration).ToString("yyyy/MM/dd");
            //}

            db.Ado.BeginTran();
            try
            {
                db.Updateable(inboundHeader).ExecuteCommand();
                //db.Updateable(inboundBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        static public bool InboundClose(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            bool retValue = true;

            List<InboundBody> inboundBodies = db.Queryable<InboundBody>().Where(e => e.OrderNo == OrderNo).ToList();

            InboundHeader inboundHeader = db.Queryable<InboundHeader>().Where(e => e.OrderNo == OrderNo).Single();
            List<Inventory> inventories = new List<Inventory>();

            List<OnOrderInventory> updateOnOrderInventories = new List<OnOrderInventory>();

            foreach (InboundBody inboundBody in inboundBodies)
            {
                ISugarQueryable<OnOrderInventory> sugarQueryable = db.Queryable<OnOrderInventory>().Where(e => e.MaterialNo == inboundBody.MaterialNo);

                OnOrderInventory onOrderInventory = sugarQueryable.Single();
                onOrderInventory.OnOrderInventoryQty -= int.Parse(inboundBody.Quantity.ToString());
                onOrderInventory.UpdateDateTime = DateTime.Now;
                updateOnOrderInventories.Add(onOrderInventory);

                inventories.Add(new Inventory
                {
                    MaterialNo = inboundBody.MaterialNo,
                    Quantity = inboundBody.InboundQty,
                    OccupiedStorageId  = inboundBody.OccupiedStorageId,
                    StorageId = inboundBody.StorageId,
                    WarehouseId = inboundBody.WarehouseId,
                    SaveStockAlert = inboundBody.SaveStockAlert,
                    Lot = inboundBody.Lot
                    //Lot = inboundBody.Expiration == null? DateTime.Parse(inboundHeader.InboundDate.ToString()).ToString("yyyyMMdd") : DateTime.Parse(inboundBody.Expiration).ToString("yyyyMMdd")
                });
            }

            db.Ado.BeginTran();
            try
            {
                db.Updateable(updateOnOrderInventories).ExecuteCommand();

                db.Ado.ExecuteCommand("update InboundHeader set Status='1' where OrderNo=@OrderNo ", new { OrderNo = OrderNo });

                foreach(Inventory inventory in inventories)
                {
                    if(db.Queryable<Inventory>().Where(e => e.MaterialNo == inventory.MaterialNo && e.Lot == inventory.Lot && e.WarehouseId == inventory.WarehouseId && e.StorageId == inventory.StorageId).Count() > 0)
                    {
                        db.Ado.ExecuteCommand("update inventory set Quantity = Quantity + " + inventory.Quantity.ToString() + " where MaterialNo=@MaterialNo and Lot=@Lot and WarehouseId=@WarehouseId and StorageId=@StorageId", new { MaterialNo = inventory.MaterialNo,Lot= inventory.Lot, WarehouseId= inventory.WarehouseId, StorageId = inventory.StorageId });
                    }
                    else
                    {
                        db.Insertable(inventory).ExecuteCommand();
                    }
                }

                db.Ado.CommitTran();
            }
            catch(Exception ex)
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        static public bool SaveInboundHeader(RecvHeaderViewModel saveObj, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ReceiveHeader receiveHeader = db.Queryable<ReceiveHeader>().Where(e => e.OrderNo == saveObj.OrderNo && e.DeliveryLot == saveObj.DeliveryLot).Single();

            receiveHeader.ReceiveDate = saveObj.ReceiveDate;
            receiveHeader.ReceiveStatus = saveObj.ReceiveStatus;
            receiveHeader.UpdateDateTime = DateTime.Now;
            receiveHeader.updateMan = ID;
            receiveHeader.IsDocument = saveObj.IsDocument;

            if (db.Updateable(receiveHeader).ExecuteCommand() > 0) return true;
            else return false;
        }

        static public bool SaveDirectInboundData(InboundSaveModel saveObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            saveObj.InboundHeader.OrderNo = getOrderNo();
            saveObj.InboundHeader.Status = "0";
            saveObj.InboundHeader.UpdateDateTime = DateTime.Now;
            saveObj.InboundHeader.AddDateTime = DateTime.Now;
            saveObj.InboundHeader.InboundMan = saveObj.InboundHeader.InboundMan;
           
            int serialNo = 1;
            foreach(InboundBody inboundBody in saveObj.inboundBodies)
            {
                inboundBody.InboundQty = inboundBody.Quantity ?? 0;
                inboundBody.OrderNo = saveObj.InboundHeader.OrderNo;
                inboundBody.SerialNo = serialNo.ToString("0000");
                inboundBody.Price = 0.0f;
                serialNo++;
            }


            bool retValue = true;

            db.Ado.BeginTran();
            try
            {
                db.Insertable(saveObj.InboundHeader).ExecuteCommand();
                db.Insertable(saveObj.inboundBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;
        }

        static public string getSerialNo(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from InboundBody where OrderNo=@OrderNo";

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