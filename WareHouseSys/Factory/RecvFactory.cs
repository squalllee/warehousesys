using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Configuration;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;
using Kendo.Mvc.UI;

namespace WareHouseSys.Factory
{
    public class RecvFactory
    {

        static public ISugarQueryable<ReceiveSearchViewModel> getReceiveSearchViewModel(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ReceiveSearchViewModel> sugarQueryable = db.SqlQueryable<ReceiveSearchViewModel>("SELECT ReceiveBody.OrderNo,ReceiveBody.MaterialNo, ReceiveBody.SerialNo, ReceiveBody.WareHouseId, ReceiveBody.StorageId, ReceiveBody.Quantity,  ReceiveBody.ReceivedQty,  ReceiveBody.Note, " +
                "PurchaseNo, TMNAME ReceiveMan, ReceiveDate, CONVERT(date, ReceiveHeader.UpdateDateTime, 23) UpdateDateTime, ReceiveStatus, MaterialInfo.MaterialName, Spec, UNITNAME, WarehouseInfo.WareHouseName, DeliveryLot," +
                "(select TMNAME from Employee where ReceiveHeader.updateMan = KEYNO) UpdateMan, " +
                "case when ReceiveHeader.Status = '1' then '已陳核' when ReceiveHeader.Status = '0' then '辦理中' else '已作廢' end Status1, " +
                "case when ReceiveBody.IsRecved = 1 then '是' else '否' end IsRecved1, " +
                "case when ReceiveHeader.IsTransToInbound = 1 then '是' else '否' end IsTransToInbound," +
                "case when ReceiveHeader.IsDocument = 1 then '是' else '否' end IsDocument " +
                "FROM  ReceiveBody inner join ReceiveHeader on ReceiveBody.OrderNo = ReceiveHeader.OrderNo " +
                "inner join MaterialInfo on ReceiveBody.MaterialNo = MaterialInfo.MaterialNo " +
                "inner join WarehouseInfo on WarehouseInfo.WarehouseId = ReceiveBody.WareHouseId " +
                "inner join Employee on ReceiveHeader.ReceiveMan = Employee.KEYNO " +
                "inner join UNIT on Employee.UNITNO = UNIT.UNITNO ");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

        static public ISugarQueryable<ReceiveHeader> getRecvHeader(string RecvNo, string StartDateTime, string EndDateTime,string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<string> wGroups = WareHouseGroupFactory.getWGroupIdByUser(ID);

            ISugarQueryable<ReceiveHeader> sugarQueryable = db.Queryable<ReceiveHeader>();
            try
            {
                if (!string.IsNullOrEmpty(ID) && !wGroups.Contains("W001"))
                {
                    sugarQueryable = sugarQueryable.Where(e => e.ReceiveMan == ID);
                }

                if (!string.IsNullOrEmpty(RecvNo))
                {
                    sugarQueryable = sugarQueryable.Where(e => e.OrderNo == RecvNo);
                }

                if (!string.IsNullOrEmpty(StartDateTime))
                {
                    sugarQueryable = sugarQueryable.Where(e => SqlFunc.ToDate(e.ReceiveDate.ToString()) >= SqlFunc.ToDate(StartDateTime));
                }

                if (!string.IsNullOrEmpty(EndDateTime))
                {
                    sugarQueryable = sugarQueryable.Where(e => SqlFunc.ToDate(e.ReceiveDate.ToString()) < SqlFunc.ToDate(EndDateTime));
                }
            }
            catch(Exception ex)
            {

            }
            
            return sugarQueryable;
        }

        static public ISugarQueryable<RecvBodyViewModel> getRecvViewBody(string RecvNo,string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<RecvBodyViewModel> sugarQueryable = db.SqlQueryable<RecvBodyViewModel>("SELECT H.OrderNo,R.SerialNo,R.MaterialNo,MaterialInfo.MaterialName,H.ReceiveMan,Spec,Unit,Quantity,Note,StorageId,R.Status " +
                                                                                                    "FROM ReceiveBody R inner join MaterialInfo on MaterialInfo.MaterialNo = R.MaterialNo " +
                                                                                                    "inner join ReceiveHeader H on R.OrderNo = H.OrderNo");
            try
            {
                if (!string.IsNullOrEmpty(RecvNo))
                {
                    sugarQueryable = sugarQueryable.Where(e => e.OrderNo == RecvNo);
                }
            }
            catch (Exception ex)
            {

            }

            return sugarQueryable;
        }

        static public RecvHeaderViewModel getRecvViewHeader(string OrderNo, string Lot)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            RecvHeaderViewModel recvHeaderViewModel = db.SqlQueryable<RecvHeaderViewModel>("SELECT OrderNo,PurchaseNo,DeliveryLot,ReceiveDate," +
                                                                                                    "(select TMNAME from Employee where KEYNO=ReceiveMan) ReceiveMan,ReceiveStatus,IsDocument FROM ReceiveHeader")
                                                                                                    .Where(e =>e.OrderNo == OrderNo && e.DeliveryLot == Lot).Single();
           

            return recvHeaderViewModel;
        }

        static public RecvHeaderViewModel getRecvViewHeader(string OrderNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            RecvHeaderViewModel recvHeaderViewModel = db.SqlQueryable<RecvHeaderViewModel>("SELECT OrderNo,PurchaseNo,DeliveryLot,ReceiveDate," +
                                                                                                    "(select TMNAME from Employee where KEYNO=ReceiveMan) ReceiveMan,ReceiveStatus,IsDocument FROM ReceiveHeader")
                                                                                                    .Where(e => e.OrderNo == OrderNo).Single();


            return recvHeaderViewModel;
        }

        static public string getOrderNo()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = "A" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM ReceiveHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
            var OrderNo = "";
            try
            {
                OrderNo = db.Ado.SqlQuerySingle<string>(sql, new { OrderPrefix = OrderPrefix });
            }
            catch (Exception ex)
            {

            }

            return OrderPrefix + "-" +(int.Parse(OrderNo.Split('-')[1]) +1).ToString("0000");
        }

        static public ISugarQueryable<RecvDataViewModel> getRecvData(string OrderNo,string Lot)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<RecvDataViewModel> sugarQueryable = db.SqlQueryable<RecvDataViewModel>("select DeliveryLot,ReceiveHeader.OrderNo,ReceiveBody.SerialNo,ReceiveBody.Note,ReceiveBody.MaterialNo,MaterialInfo.Spec,MaterialInfo.MaterialName,MaterialInfo.Unit,ReceiveBody.WarehouseId,StorageId, " +
                                                                                        "WarehouseInfo.WareHouseName,Quantity,isnull(ReceivedQty,0) ReceivedQty,Quantity - isnull(ReceivedQty,0) UnreceivedQty " +
                                                                                        "from ReceiveBody inner join ReceiveHeader on ReceiveBody.OrderNo = ReceiveHeader.OrderNo " +
                                                                                        "inner join MaterialInfo on MaterialInfo.MaterialNo = ReceiveBody.MaterialNo " +
                                                                                        "left join WarehouseInfo on WarehouseInfo.WarehouseId = ReceiveBody.WarehouseId ").Where(e => e.OrderNo == OrderNo && e.DeliveryLot == Lot);

            return sugarQueryable;
        }

        static public ISugarQueryable<TransToInboundViewModel> getRecvBodyByLot(string PurchaseNo, string Lot)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ISugarQueryable<TransToInboundViewModel> sugarQueryable = db.SqlQueryable<TransToInboundViewModel>("select p.PurchaseNo,PurchaseBody.MaterialNo,MaterialName,DeliveryLot,DeliveryPlace,Quantity, " +
                                                                                        "isnull((select sum(ReceivedQty) ReceivedQty from ReceiveHeader  " +
                                                                                        "inner join ReceiveBody on ReceiveHeader.OrderNo = ReceiveBody.OrderNo " +
                                                                                        "where ReceiveHeader.PurchaseNo=p.PurchaseNo and ReceiveHeader.DeliveryLot = PurchaseBody.DeliveryLot and ReceiveBody.MaterialNo = PurchaseBody.MaterialNo " +
                                                                                        "group by MaterialNo,ReceiveHeader.DeliveryLot),0) ReceivedQty, " +
                                                                                        "Quantity - isnull((select sum(ReceivedQty) ReceivedQty from ReceiveHeader  " +
                                                                                        "inner join ReceiveBody on ReceiveHeader.OrderNo = ReceiveBody.OrderNo " +
                                                                                        "where ReceiveHeader.PurchaseNo=p.PurchaseNo and ReceiveHeader.DeliveryLot = PurchaseBody.DeliveryLot and ReceiveBody.MaterialNo = PurchaseBody.MaterialNo " +
                                                                                        "group by MaterialNo,ReceiveHeader.DeliveryLot),0) UnReceivedQty from PurchaseHeader p " +
                                                                                        "inner join PurchaseBody on p.PurchaseNo = PurchaseBody.PurchaseNo " +
                                                                                        "inner join MaterialInfo on MaterialInfo.MaterialNo = PurchaseBody.MaterialNo ").Where(e => e.PurchaseNo == PurchaseNo && e.DeliveryLot == Lot);

            return sugarQueryable;
        }
        
        static public bool SaveRecvData(RecvDataViewModel saveObj)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ReceiveBody receiveBody = db.Queryable<ReceiveBody>().Where(e => e.OrderNo == saveObj.OrderNo && e.SerialNo == saveObj.SerialNo).Single();

            receiveBody.ReceivedQty = int.Parse(saveObj.receivedQty);
            receiveBody.WareHouseId = saveObj.WarehouseId;
            receiveBody.StorageId = saveObj.StorageId;
            receiveBody.Note = saveObj.Note;
            receiveBody.IsRecved = true;

            if (db.Updateable(receiveBody).ExecuteCommand() > 0) return true;
            else return false;
        }

        static public bool CloseRecv(string OrderNo,string Lot)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);
            
            List<ReceiveBody> receiveBodies = db.Ado.SqlQuery<ReceiveBody>("select ReceiveBody.* from ReceiveBody inner join ReceiveHeader on ReceiveHeader.OrderNo=ReceiveBody.OrderNo where ReceiveBody.OrderNo=@OrderNo and DeliveryLot=@DeliveryLot and IsRecved='0'",
                new { OrderNo = OrderNo, DeliveryLot = Lot });

            if (receiveBodies.Count > 0) return false;

            if (db.Ado.ExecuteCommand("update ReceiveHeader set Status='1' where OrderNo=@OrderNo and DeliveryLot=@DeliveryLot", new {OrderNo=OrderNo, DeliveryLot=Lot }) > 0) return true;
            else return false;
        }

        static public bool SaveRecvHeader(RecvHeaderViewModel saveObj,string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            ReceiveHeader receiveHeader = db.Queryable<ReceiveHeader>().Where(e => e.OrderNo == saveObj.OrderNo && e.DeliveryLot == saveObj.DeliveryLot).Single();

            receiveHeader.ReceiveDate = saveObj.ReceiveDate;
            receiveHeader.ReceiveStatus = saveObj.ReceiveStatus;
            receiveHeader.UpdateDateTime = DateTime.Now;
            receiveHeader.updateMan = ID;
            receiveHeader.IsDocument = saveObj.IsDocument;

            if (db.Updateable(receiveHeader).ExecuteCommand() > 0) return true;
            else return false;
        }
    }
}