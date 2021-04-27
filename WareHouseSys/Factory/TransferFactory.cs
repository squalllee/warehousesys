using Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Globalization;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;
using Kendo.Mvc.UI;

namespace WareHouseSys.Factory
{
    public class TransferFactory
    {

        static public ISugarQueryable<TransferSearchViewModel> getTransferSearchViewModel(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<TransferSearchViewModel> sugarQueryable = db.SqlQueryable<TransferSearchViewModel>("SELECT TransferBody.OrderNo,TransferBody.MaterialNo, TransferBody.SerialNo, TransferBody.Quantity, TransferBody.TransferOutQty, TransferBody.TransferInQty, TransferBody.OutStorageId, TransferBody.OutWareHouseId,  TransferBody.InStorageId, TransferBody.InWareHouseId, TransferBody.OccupiedStorageId,  TransferBody.Lot,  TransferBody.Note, " +
                "TMNAME ApplicantMan, ApplicantDate, TransferInDate, TransferOutDate, WGroupId, MaterialInfo.MaterialName, Spec, UNITNAME, " +
                "(select TMNAME from Employee where TransferHeader.TransferOutMan = KEYNO) TransferOutMan, " +
                "(select TMNAME from Employee where TransferHeader.TransferInMan = KEYNO) TransferInMan, " +
                "(select WareHouseName from WarehouseInfo where TransferBody.InWareHouseId = WarehouseId) InWareHouseName, " +
                "(select WareHouseName from WarehouseInfo where TransferBody.OutWareHouseId = WarehouseId) OutWareHouseName, " +
                "case when TransferHeader.Status = '1' then '已陳核' when TransferHeader.Status = '0' then '辦理中' else '已作廢' end Status " +
                "FROM  TransferBody inner join TransferHeader on TransferBody.OrderNo = TransferHeader.OrderNo " +
                "inner join MaterialInfo on TransferBody.MaterialNo = MaterialInfo.MaterialNo " +
                "inner join Employee on TransferHeader.ApplicantMan = Employee.KEYNO " +
                "inner join UNIT on Employee.UNITNO = UNIT.UNITNO ");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

        public static ISugarQueryable<TransferHeaderViewModel> getTransferHeaderViewModel(FilterCriteria filter,string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<TransferHeaderViewModel> sugarQueryable = db.SqlQueryable<TransferHeaderViewModel>("select OrderNo, ApplicantDate, CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status," +
               " (select TMNAME from Employee where KEYNO=ApplicantMan) ApplicantMan,ApplicantMan ApplicantId," +
                "(select TMNAME from Employee where KEYNO=TransferOutMan) TransferOutMan,TransferOutMan TransferOutManId, " +
                "(select TMNAME from Employee where KEYNO=TransferInMan) TransferInMan,TransferInMan TransferInManId from TransferHeader where Status != '2'");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            return sugarQueryable;
        }

        public static ISugarQueryable<TransferHeaderViewModel> getTransferInHeaderViewModel(FilterCriteria filter, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<TransferHeaderViewModel> sugarQueryable = db.SqlQueryable<TransferHeaderViewModel>("select OrderNo, ApplicantDate, CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status," +
               " (select TMNAME from Employee where KEYNO=ApplicantMan) ApplicantMan,ApplicantMan ApplicantId," +
                "(select TMNAME from Employee where KEYNO=TransferOutMan) TransferOutMan,TransferOutMan TransferOutManId, " +
                "(select TMNAME from Employee where KEYNO=TransferInMan) TransferInMan,TransferInMan TransferInManId from TransferHeader where Status != '2'");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            return sugarQueryable.Where(e=>e.TransferOutManId != null);
        }

        public static TransferHeaderViewModel getTransferHeaderViewModel(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<TransferHeaderViewModel> sugarQueryable = db.SqlQueryable<TransferHeaderViewModel>("select OrderNo, ApplicantDate, CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status," +
                " (select TMNAME from Employee where KEYNO=ApplicantMan) ApplicantMan,ApplicantMan ApplicantId," +
                "(select TMNAME from Employee where KEYNO=TransferOutMan) TransferOutMan,TransferOutMan TransferOutManId, " +
                "(select TMNAME from Employee where KEYNO=TransferInMan) TransferInMan,TransferInMan TransferInManId from TransferHeader").Where(e=>e.OrderNo == OrderNo);


            return sugarQueryable.Single();
        }

        public static ISugarQueryable<TransferBodyViewModel> getTransferBodyViewModel(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<TransferBodyViewModel> sugarQueryable = db.SqlQueryable<TransferBodyViewModel>("SELECT OrderNo, TransferBody.SerialNo,TransferBody.MaterialNo,MaterialInfo.MaterialName,MaterialInfo.Spec,MaterialInfo.Unit, Quantity,TransferInQty,TransferOutQty,(select WareHouseName from WarehouseInfo where WarehouseId = OutWareHouseId) OutWareHouseName, OutWareHouseId, OutStorageId," +
                " (select WareHouseName from WarehouseInfo where WarehouseId = InWareHouseId) InWareHouseName, InWareHouseId, InStorageId,OccupiedStorageId, Note, Lot " +
                "FROM TransferBody inner join MaterialInfo on MaterialInfo.MaterialNo = TransferBody.MaterialNo");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        public static bool saveTransfer(TransferSaveModel transferSaveModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferHeader transferHeader = new TransferHeader
            {
                OrderNo = getOrderNo(),
                AddDateTime = DateTime.Now,
                ApplicantMan = transferSaveModel.ApplicantMan,
                UpdateDateTime = DateTime.Now,
                ApplicantDate = DateTime.Now,
                TransferOutDate = DateTime.Now,
                TransferOutMan = transferSaveModel.ApplicantMan,
                WGroupId = transferSaveModel.WGroupId,
                Status = "0"
            };

            List<TransferBody> transferBodies = new List<TransferBody>();

            int count = 1;
            foreach(TransferBodyViewModel transferBodyViewModel in transferSaveModel.TransferBodies)
            {
                transferBodies.Add(new TransferBody
                {
                    OrderNo = transferHeader.OrderNo,
                    MaterialNo = transferBodyViewModel.MaterialNo,
                    OutWareHouseId = transferBodyViewModel.OutWareHouseId,
                    InWareHouseId = transferBodyViewModel.InWareHouseId,
                    Quantity = transferBodyViewModel.Quantity,
                    Lot = transferBodyViewModel.Lot,
                    OutStorageId = transferBodyViewModel.OutStorageId,
                    TransferOutQty = transferBodyViewModel.Quantity,
                    SerialNo = count.ToString("0000")
                });
                count++;
            }

            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(transferHeader).ExecuteCommand();
                db.Insertable(transferBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;

        }

        public static bool updateTransfer(TransferBodyViewModel transferBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferBody transferBody = db.Queryable<TransferBody>().Where(e => e.OrderNo == transferBodyViewModel.OrderNo && e.SerialNo == transferBodyViewModel.SerialNo).Single();
            transferBody.MaterialNo = transferBodyViewModel.MaterialNo;
            transferBody.OutWareHouseId = transferBodyViewModel.OutWareHouseId;
            transferBody.InWareHouseId = transferBodyViewModel.InWareHouseId;
            transferBody.Note = transferBodyViewModel.Note;
            transferBody.Quantity = transferBodyViewModel.Quantity;
            transferBody.TransferOutQty = transferBodyViewModel.Quantity;
            

            return db.Updateable(transferBody).ExecuteCommand() > 0;

        }

        public static bool updateTransferHeader(TransferSaveModel transferSaveModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferHeader transferHeader = db.Queryable<TransferHeader>().Where(e => e.OrderNo == transferSaveModel.OrderNo).Single();

            transferHeader.ApplicantMan = transferSaveModel.ApplicantMan;
            transferHeader.UpdateDateTime = DateTime.Now;
            transferHeader.ApplicantDate = DateTime.Now;


            return db.Updateable(transferHeader).ExecuteCommand() > 0;

        }

        public static bool updateTransferOutHeader(TransferSaveModel transferSaveModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferHeader transferHeader = db.Queryable<TransferHeader>().Where(e => e.OrderNo == transferSaveModel.OrderNo).Single();

            transferHeader.TransferOutMan = transferSaveModel.TransferOutMan;
            transferHeader.UpdateDateTime = DateTime.Now;
            transferHeader.ApplicantDate = DateTime.Now;


            return db.Updateable(transferHeader).ExecuteCommand() > 0;

        }

        public static bool updateTransferInHeader(TransferSaveModel transferSaveModel)
        {
            bool retValue = true;
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferHeader transferHeader = db.Queryable<TransferHeader>().Where(e => e.OrderNo == transferSaveModel.OrderNo).Single();
            List<TransferBody> transferBodies = db.Queryable<TransferBody>().Where(e => e.OrderNo == transferSaveModel.OrderNo).ToList();

            transferHeader.TransferInMan = transferSaveModel.TransferInMan;
            transferHeader.UpdateDateTime = DateTime.Now;
            //transferHeader.ApplicantDate = DateTime.Now;
            transferHeader.TransferInDate = DateTime.Now;
            transferHeader.Status = "1";

            Inventory inventory = null;

            db.Ado.BeginTran();
            try
            {
                foreach(TransferBody transferBody in transferBodies)
                {
                    inventory = db.Queryable<Inventory>().Where(e => e.MaterialNo == transferBody.MaterialNo && e.WarehouseId == transferBody.OutWareHouseId && e.StorageId == transferBody.OutStorageId && e.Lot == transferBody.Lot).Single();
                    inventory.Quantity = inventory.Quantity - transferBody.TransferOutQty;
                    db.Updateable(inventory).ExecuteCommand();
                   
                    inventory = db.Queryable<Inventory>().Where(e => e.MaterialNo == transferBody.MaterialNo && e.WarehouseId == transferBody.InWareHouseId && e.StorageId == transferBody.InStorageId && e.Lot == transferBody.Lot).Single();

                    if (inventory != null)
                    {
                        inventory.Quantity = inventory.Quantity + transferBody.TransferInQty;
                        db.Updateable(inventory).ExecuteCommand();
                    }
                    else
                    {
                        inventory = new Inventory
                        {
                            MaterialNo = transferBody.MaterialNo,
                            Quantity = transferBody.TransferInQty,
                            Lot = transferBody.Lot,
                            StorageId = transferBody.InStorageId,
                            WarehouseId = transferBody.InWareHouseId
                        };
                        db.Insertable(inventory).ExecuteCommand();
                    }
                    
                }
                
                db.Updateable(transferHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch
            {
                retValue = false;
                db.Ado.RollbackTran();
            }
            return retValue;

        }

        public static bool deleteTransfer(TransferBodyViewModel transferBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferBody transferBody = db.Queryable<TransferBody>().Where(e => e.OrderNo == transferBodyViewModel.OrderNo && e.SerialNo == transferBodyViewModel.SerialNo).Single();
           

            return db.Deleteable(transferBody).ExecuteCommand() > 0;

        }

        public static bool deleteTransferBody(TransferBodyViewModel transferBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferBody transferBody = db.Queryable<TransferBody>().Where(e => e.OrderNo == transferBodyViewModel.OrderNo && e.SerialNo == transferBodyViewModel.SerialNo).Single();

            return db.Deleteable(transferBody).ExecuteCommand() > 0;

        }

        public static bool scrapTransfer(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferHeader transferHeader = db.Queryable<TransferHeader>().Where(e => e.OrderNo == OrderNo).Single();

            transferHeader.Status = "2";

            return db.Updateable(transferHeader).ExecuteCommand() > 0;

        }

        public static bool addTransfer(TransferBodyViewModel transferBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferBody transferBody = new TransferBody
            {
                OrderNo = transferBodyViewModel.OrderNo,
                MaterialNo = transferBodyViewModel.MaterialNo,
                SerialNo = getSerialNo(transferBodyViewModel.OrderNo),
                OutWareHouseId = transferBodyViewModel.OutWareHouseId,
                Note = transferBodyViewModel.Note,
                Lot = transferBodyViewModel.Lot,
                OutStorageId = transferBodyViewModel.OutStorageId,
                InWareHouseId = transferBodyViewModel.InWareHouseId,
                TransferOutQty = transferBodyViewModel.Quantity,
                Quantity = transferBodyViewModel.Quantity
            };

            return db.Insertable(transferBody).ExecuteCommand() > 0;

        }

        public static bool addTransferOut(TransferBodyViewModel transferBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferBody transferBody = new TransferBody
            {
                OrderNo = transferBodyViewModel.OrderNo,
                MaterialNo = transferBodyViewModel.MaterialNo,
                SerialNo = getSerialNo(transferBodyViewModel.OrderNo),
                Lot = transferBodyViewModel.Lot,
                OutWareHouseId = transferBodyViewModel.OutWareHouseId,
                OutStorageId = transferBodyViewModel.OutStorageId,
                Note = transferBodyViewModel.Note,
                Quantity = transferBodyViewModel.Quantity,
                TransferOutQty = transferBodyViewModel.TransferOutQty,
                InWareHouseId = transferBodyViewModel.InWareHouseId
            };
         
            return db.Insertable(transferBody).ExecuteCommand() > 0;

        }

        public static bool addTransferIn(TransferBodyViewModel transferBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferBody transferBody = new TransferBody
            {
                OrderNo = transferBodyViewModel.OrderNo,
                MaterialNo = transferBodyViewModel.MaterialNo,
                SerialNo = getSerialNo(transferBodyViewModel.OrderNo),
                Lot = transferBodyViewModel.Lot,
                OutWareHouseId = transferBodyViewModel.OutWareHouseId,
                OutStorageId = transferBodyViewModel.OutStorageId,
                Note = transferBodyViewModel.Note,
                Quantity = transferBodyViewModel.Quantity,
                TransferOutQty = transferBodyViewModel.TransferOutQty,
                InWareHouseId = transferBodyViewModel.InWareHouseId,
                InStorageId = transferBodyViewModel.InStorageId,
                TransferInQty = 0
            };

            return db.Insertable(transferBody).ExecuteCommand() > 0;

        }

        public static bool TransferOutUpdate(TransferBodyViewModel transferBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferBody transferBody = db.Queryable<TransferBody>().Where(e => e.OrderNo == transferBodyViewModel.OrderNo && e.SerialNo == transferBodyViewModel.SerialNo).Single();

            transferBody.Lot = transferBodyViewModel.Lot;
            transferBody.OutWareHouseId = transferBodyViewModel.OutWareHouseId;
            transferBody.OutStorageId = transferBodyViewModel.OutStorageId;
            transferBody.Note = transferBodyViewModel.Note;
            transferBody.TransferOutQty = transferBodyViewModel.TransferOutQty;

            return db.Updateable(transferBody).ExecuteCommand() > 0;

        }

        public static bool TransferInUpdate(TransferBodyViewModel transferBodyViewModel)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TransferBody transferBody = db.Queryable<TransferBody>().Where(e => e.OrderNo == transferBodyViewModel.OrderNo && e.SerialNo == transferBodyViewModel.SerialNo).Single();

            transferBody.InWareHouseId = transferBodyViewModel.InWareHouseId;
            transferBody.InStorageId = transferBodyViewModel.InStorageId;
            transferBody.Note = transferBodyViewModel.Note;
            transferBody.TransferInQty = transferBodyViewModel.TransferInQty;

            return db.Updateable(transferBody).ExecuteCommand() > 0;

        }

        static public string getOrderNo()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = "G" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM TransferHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from TransferBody where OrderNo=@OrderNo";

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