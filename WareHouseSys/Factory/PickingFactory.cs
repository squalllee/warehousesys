using Kendo.Mvc.UI;
using Models;
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
    public class PickingFactory
    {
        static public ISugarQueryable<ToolPickHeaderViewModel> getToolPickingHeader(FilterCriteria filter, List<string> wgroupIdList)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ToolPickHeaderViewModel> sugarQueryable = db.SqlQueryable<ToolPickHeaderViewModel>("SELECT OrderNo,WGroupId,WorkNo,(select TMNAME from Employee where KEYNO = PickingMan) PickingMan,PickingMan PickingManId, " +
                "isnull((select TMNAME from Employee where KEYNO = OutBoundMan),'') OutBoundMan, OutBoundMan OutBoundManId " +
                ",OutBoundDate,EmergencyPicking,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status FROM PickingToolHeader");

            sugarQueryable = sugarQueryable.Where(e => wgroupIdList.Contains(e.WGroupId));

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            return sugarQueryable;
        }

        static public ToolPickHeaderSaveModel getToolPickingHeader(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ToolPickHeaderSaveModel toolPickHeaderViewModel = db.SqlQueryable<ToolPickHeaderSaveModel>("SELECT OrderNo,WorkNo,(select TMNAME from Employee where KEYNO = PickingMan) PickingMan,PickingMan PickingManId, " +
                "isnull((select TMNAME from Employee where KEYNO = OutBoundMan),'') OutBoundMan, OutBoundMan OutBoundManId " +
                ",OutBoundDate,EmergencyPicking,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status FROM PickingToolHeader").Where(e => e.OrderNo == OrderNo).Single();


            return toolPickHeaderViewModel;
        }

        static public ISugarQueryable<ToolPickBodyViewModel> getToolPickingBodyDetail(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ToolPickBodyViewModel> sugarQueryable = db.SqlQueryable<ToolPickBodyViewModel>("SELECT PickingToolBody.OrderNo, PickingToolBody.SerialNo, PickingToolBody.MaterialNo, MaterialInfo.MaterialName,WGroupId, " +
                "MaterialInfo.Spec, MaterialInfo.Unit, PickingToolBody.Quantity, PickingToolBody.PickedQty, PickingToolBody.WareHouseId,  " +
                " WarehouseInfo.WareHouseName, isnull(PickingToolBody.StorageId,'') StorageId, PickingToolBody.Note, PickingToolBody.Lot, " +
                "PickingToolBody.KeepMan KeepManId,(select TMNAME from Employee where KEYNO=KeepMan) KeepMan," +
                "KeepUnit KeepUnitId,(select UNITNAME from UNIT where UNITNO = KeepUnit ) KeepUnit " +
                "FROM PickingToolBody LEFT OUTER JOIN " +
                "WarehouseInfo ON PickingToolBody.WareHouseId = WarehouseInfo.WarehouseId LEFT  JOIN " +
                "StorageInfo ON PickingToolBody.StorageId = StorageInfo.StorageId INNER JOIN " +
                "MaterialInfo ON PickingToolBody.MaterialNo = MaterialInfo.MaterialNo " +
                "INNER JOIN PickingToolHeader on PickingToolHeader.OrderNo = PickingToolBody.OrderNo");

            if(filter != null)
                sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        static public ISugarQueryable<ToolPickBodyViewModel> getToolPickingBodyDetail(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ToolPickBodyViewModel> sugarQueryable = db.SqlQueryable<ToolPickBodyViewModel>("SELECT PickingToolBody.OrderNo, PickingToolBody.SerialNo, PickingToolBody.MaterialNo, MaterialInfo.MaterialName,WGroupId, " +
                "MaterialInfo.Spec, MaterialInfo.Unit, PickingToolBody.Quantity, PickingToolBody.PickedQty, PickingToolBody.WareHouseId,  " +
                " WarehouseInfo.WareHouseName, isnull(PickingToolBody.StorageId,'') StorageId, PickingToolBody.Note, PickingToolBody.Lot, " +
                "PickingToolBody.KeepMan KeepManId,(select TMNAME from Employee where KEYNO=KeepMan) KeepMan," +
                "KeepUnit KeepUnitId,(select UNITNAME from UNIT where UNITNO = KeepUnit ) KeepUnit " +
                "FROM PickingToolBody LEFT OUTER JOIN " +
                "WarehouseInfo ON PickingToolBody.WareHouseId = WarehouseInfo.WarehouseId LEFT  JOIN " +
                "StorageInfo ON PickingToolBody.StorageId = StorageInfo.StorageId INNER JOIN " +
                "MaterialInfo ON PickingToolBody.MaterialNo = MaterialInfo.MaterialNo " +
                "INNER JOIN PickingToolHeader on PickingToolHeader.OrderNo = PickingToolBody.OrderNo");

            sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        static public ISugarQueryable<ToolPickBodyViewModel> getToolPickingBodyDetail(string OrderNo,string SerialNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ToolPickBodyViewModel> sugarQueryable = db.SqlQueryable<ToolPickBodyViewModel>("SELECT PickingToolBody.OrderNo, PickingToolBody.SerialNo, PickingToolBody.MaterialNo, MaterialInfo.MaterialName,WGroupId, " +
                "MaterialInfo.Spec, MaterialInfo.Unit, PickingToolBody.Quantity, PickingToolBody.PickedQty, PickingToolBody.WareHouseId,  " +
                " WarehouseInfo.WareHouseName, isnull(PickingToolBody.StorageId,'') StorageId, PickingToolBody.Note, PickingToolBody.Lot, " +
                "PickingToolBody.KeepMan KeepManId,(select TMNAME from Employee where KEYNO=KeepMan) KeepMan," +
                "KeepUnit KeepUnitId,(select UNITNAME from UNIT where UNITNO = KeepUnit ) KeepUnit " +
                "FROM PickingToolBody LEFT OUTER JOIN " +
                "WarehouseInfo ON PickingToolBody.WareHouseId = WarehouseInfo.WarehouseId LEFT  JOIN " +
                "StorageInfo ON PickingToolBody.StorageId = StorageInfo.StorageId INNER JOIN " +
                "MaterialInfo ON PickingToolBody.MaterialNo = MaterialInfo.MaterialNo " +
                "INNER JOIN PickingToolHeader on PickingToolHeader.OrderNo = PickingToolBody.OrderNo");


            sugarQueryable.Where(e => e.OrderNo == OrderNo && e.SerialNo == SerialNo);

            return sugarQueryable;
        }

        static public bool updateToolPickingBody(ToolPickBodyViewModel updateObj, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            PickingToolBody pickingBody = new PickingToolBody
            {
                MaterialNo = updateObj.MaterialNo,
                PickedQty = updateObj.PickedQty,
                Note = updateObj.Note,
                OrderNo = updateObj.OrderNo,
                StorageId = updateObj.StorageId,
                WareHouseId = updateObj.WareHouseId,
                SerialNo = updateObj.SerialNo,
                Lot = updateObj.Lot,
                Quantity = updateObj.Quantity,
                KeepMan = updateObj.KeepManId,
                KeepUnit = updateObj.KeepUnitId
            };
            if (db.Updateable(pickingBody).ExecuteCommand() > 0) return true;
            else return false;
        }

        static public ISugarQueryable<MaterialPickHeaderViewModel> getMaterialPickingHeader(FilterCriteria filter, List<string> wgroupIdList,string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<MaterialPickHeaderViewModel> sugarQueryable = db.SqlQueryable<MaterialPickHeaderViewModel>("SELECT OrderNo,WGroupId,WorkNo,(select TMNAME from Employee where KEYNO = PickingMan) PickingMan,PickingMan PickingManId, " +
                "isnull((select TMNAME from Employee where KEYNO = OutBoundMan),'') OutBoundMan, OutBoundMan OutBoundManId " +
                ",OutBoundDate,EmergencyPicking,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status FROM PickingHeader where Status <> '-1'");

            if(wgroupIdList.Count == 0)
                sugarQueryable = sugarQueryable.Where(e => e.PickingManId == ID);

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);
           
            return sugarQueryable;
        }

        static public ISugarQueryable<MaterialPickBodyViewModel> getMaterialPickingBodyDetail(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<MaterialPickBodyViewModel> sugarQueryable = db.SqlQueryable<MaterialPickBodyViewModel>("SELECT PickingBody.OrderNo, PickingBody.SerialNo, PickingBody.MaterialNo, MaterialInfo.MaterialName,WGroupId, " +
                "MaterialInfo.Spec, MaterialInfo.Unit, PickingBody.Quantity, PickingBody.PickedQty, PickingBody.WareHouseId,  " +
                " WarehouseInfo.WareHouseName, isnull(PickingBody.StorageId,'') StorageId, PickingBody.Note, PickingBody.Lot " +
                "FROM PickingBody LEFT OUTER JOIN " +
                "WarehouseInfo ON PickingBody.WareHouseId = WarehouseInfo.WarehouseId LEFT  JOIN " +
                "StorageInfo ON PickingBody.StorageId = StorageInfo.StorageId INNER JOIN " +
                "MaterialInfo ON PickingBody.MaterialNo = MaterialInfo.MaterialNo " +
                "INNER JOIN PickingHeader on PickingHeader.OrderNo = PickingBody.OrderNo");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        static public ISugarQueryable<MaterialPickBodyViewModel> getMaterialPickingBodyDetail(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<MaterialPickBodyViewModel> sugarQueryable = db.SqlQueryable<MaterialPickBodyViewModel>("SELECT PickingBody.OrderNo, PickingBody.SerialNo, PickingBody.MaterialNo, MaterialInfo.MaterialName,WGroupId, " +
                "MaterialInfo.Spec, MaterialInfo.Unit, PickingBody.Quantity, PickingBody.PickedQty, PickingBody.WareHouseId,  " +
                " WarehouseInfo.WareHouseName, isnull(PickingBody.StorageId,'') StorageId, PickingBody.Note, PickingBody.Lot " +
                "FROM PickingBody LEFT OUTER JOIN " +
                "WarehouseInfo ON PickingBody.WareHouseId = WarehouseInfo.WarehouseId LEFT  JOIN " +
                "StorageInfo ON PickingBody.StorageId = StorageInfo.StorageId INNER JOIN " +
                "MaterialInfo ON PickingBody.MaterialNo = MaterialInfo.MaterialNo " +
                "INNER JOIN PickingHeader on PickingHeader.OrderNo = PickingBody.OrderNo");
            
            sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        static public ISugarQueryable<PickingSearchViewModel> getPickingSearchViewModel(DataSourceRequest request)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<PickingSearchViewModel> sugarQueryable = db.SqlQueryable<PickingSearchViewModel>("SELECT  PickingBody.OrderNo,WorkNo,PickingHeader.PickingMan,OutBoundDate,TMNAME,PickingBody.MaterialNo, MaterialInfo.MaterialName,Spec, PickingBody.WareHouseId,WarehouseInfo.WareHouseName, StorageId, PickedQty, Note,Lot, " +
                "(select TMNAME from Employee where PickingHeader.OutBoundMan = KEYNO) OutBoundMan, " +
                "(select UNITNAME from UNIT where PickingHeader.PickingUnit = UNITNO) UNITNAME, " +
                "case when PickingHeader.Status = '1' then '已陳核' when PickingHeader.Status = '0' then '辦理中' else '已作廢' end Status,case when EmergencyPicking = '1' then 'Yes' else 'No' end EmergencyPicking  " +
                "FROM  PickingBody inner join PickingHeader on PickingBody.OrderNo = PickingHeader.OrderNo " +
                "inner join WarehouseInfo on WarehouseInfo.WarehouseId = PickingBody.WareHouseId " +
                "inner join MaterialInfo on PickingBody.MaterialNo = MaterialInfo.MaterialNo " +
                "inner join Employee on PickingHeader.PickingMan = Employee.KEYNO " +
                "inner join UNIT on Employee.UNITNO = UNIT.UNITNO ");

            sugarQueryable = DBUtility.Query(sugarQueryable, request);

            return sugarQueryable;
        }

        static public ISugarQueryable<PickingToReturnViewModel> PickingToReturnBodies(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<PickingToReturnViewModel> sugarQueryable = 
                db.SqlQueryable<PickingToReturnViewModel>("select distinct PickingBody.OrderNo,PickingBody.SerialNo, PickingBody.MaterialNo, MaterialName,Spec, PickedQty,PickingBody.WareHouseId, WareHouseName, StorageId, Note, Lot," +
                "(select isnull(sum(Quantity),0) Quantity from ReturnBody inner join ReturnHeader on ReturnHeader.OrderNo = ReturnBody.OrderNo " +
                " where ReturnHeader.PickingNo = rh.PickingNo and ReturnBody.PickingSerialNo = PickingBody.SerialNo) ReturnedQty," +
                " (PickedQty - (select isnull(sum(Quantity),0) Quantity from ReturnBody inner join ReturnHeader on ReturnHeader.OrderNo = ReturnBody.OrderNo " +
                " where ReturnHeader.PickingNo = rh.PickingNo and ReturnBody.PickingSerialNo = PickingBody.SerialNo)) CanReturnQty from PickingBody " +
                "inner join MaterialInfo on PickingBody.MaterialNo = MaterialInfo.MaterialNo " +
                "inner join WarehouseInfo on PickingBody.WareHouseId = WarehouseInfo.WarehouseId " +
                " left join ReturnHeader rh on  PickingBody.OrderNo =rh.PickingNo");

            sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }

        static public ISugarQueryable<PickingToScrapViewModel> PickingToScrapBodies(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<PickingToScrapViewModel> sugarQueryable =
                db.SqlQueryable<PickingToScrapViewModel>("select distinct PickingBody.OrderNo,PickingBody.SerialNo, PickingBody.MaterialNo, MaterialName,Spec, PickedQty,PickingBody.WareHouseId, WareHouseName, " +
                "StorageId, Note, Lot,(select isnull(sum(Quantity),0) Quantity from ScrapBody " +
                "inner join ScrapHeader on ScrapHeader.OrderNo = ScrapBody.OrderNo  " +
                "where ScrapHeader.PickingNo = rh.PickingNo and ScrapBody.PickingSerialNo = PickingBody.SerialNo) ScrapedQty,  " +
                "(PickedQty - (select isnull(sum(Quantity),0) Quantity from ScrapBody inner join ScrapHeader on ScrapHeader.OrderNo = ScrapBody.OrderNo  " +
                "where ScrapHeader.PickingNo = rh.PickingNo and ScrapBody.PickingSerialNo = PickingBody.SerialNo)) CanScrapQty from PickingBody " +
                "inner join MaterialInfo on PickingBody.MaterialNo = MaterialInfo.MaterialNo  " +
                "inner join WarehouseInfo on PickingBody.WareHouseId = WarehouseInfo.WarehouseId  left join ScrapHeader rh on  PickingBody.OrderNo =rh.PickingNo");

            sugarQueryable.Where(e => e.OrderNo == OrderNo && e.CanScrapQty > 0);

            return sugarQueryable;
        }

        static public MaterialPickBodyViewModel getMaterialPickingBodyDetail(string OrderNo, string SerialNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<MaterialPickBodyViewModel> sugarQueryable = db.SqlQueryable<MaterialPickBodyViewModel>("SELECT PickingBody.OrderNo, PickingBody.SerialNo, PickingBody.MaterialNo, MaterialInfo.MaterialName," +
                "MaterialInfo.Spec, MaterialInfo.Unit, PickingBody.Quantity, PickingBody.PickedQty, PickingBody.WareHouseId,  " +
                "WorkNo,WarehouseInfo.WareHouseName, isnull(PickingBody.StorageId,'') StorageId, PickingBody.Note, PickingBody.Lot " +
                "FROM PickingBody LEFT OUTER JOIN " +
                "WarehouseInfo ON PickingBody.WareHouseId = WarehouseInfo.WarehouseId LEFT OUTER JOIN " +
                "StorageInfo ON PickingBody.StorageId = StorageInfo.StorageId INNER JOIN " +
                "MaterialInfo ON PickingBody.MaterialNo = MaterialInfo.MaterialNo");

            

            sugarQueryable.Where(e => e.OrderNo == OrderNo && e.SerialNo == SerialNo);

            return sugarQueryable.Single(); ;
        }

        static public MaterialPickHeaderSaveModel getMaterialPickingHeader(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            MaterialPickHeaderSaveModel materialPickHeaderViewModel = db.SqlQueryable<MaterialPickHeaderSaveModel>("SELECT OrderNo,WorkNo,(select TMNAME from Employee where KEYNO = PickingMan) PickingMan,PickingMan PickingManId,(select UNITNAME from UNIT where UNITNO = PickingUnit) PickingUnit,PickingUnit PickingUnitId,PickingReason,ApplyDateTime, " +
                "isnull((select TMNAME from Employee where KEYNO = OutBoundMan),'') OutBoundMan, OutBoundMan OutBoundManId " +
                ",OutBoundDate,EmergencyPicking,CASE WHEN Status = '1' THEN '結案' WHEN Status = '0' THEN '辦理中' END Status FROM PickingHeader").Where(e=>e.OrderNo == OrderNo).Single();

            return materialPickHeaderViewModel;
        }

        static public bool updatePickingBody(MaterialPickBodyViewModel updateObj, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            PickingBody pickingBody = new PickingBody
            {
                MaterialNo = updateObj.MaterialNo,
                PickedQty = updateObj.PickedQty,
                Note = updateObj.Note,
                OrderNo = updateObj.OrderNo,
                StorageId = updateObj.StorageId,
                WareHouseId = updateObj.WareHouseId,
                SerialNo = updateObj.SerialNo,
                Lot = updateObj.Lot,
                Quantity = updateObj.Quantity,
            };
            if (db.Updateable(pickingBody).ExecuteCommand() > 0) return true;
            else return false;
        }

        static public bool deletePickingBody(MaterialPickBodyViewModel updateObj, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            PickingBody pickingBody = db.Queryable<PickingBody>().Where(e => e.OrderNo == updateObj.OrderNo && e.SerialNo == updateObj.SerialNo).Single();

            if (db.Deleteable(pickingBody).ExecuteCommand() > 0) return true;
            else return false;
        }

        static public bool TransferToReturn(CreateReturnViewModel returnObj, string ID)
        {
            bool retValue = true;
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            Employee employee = EmployeeFactory.getEmployee(ID);

            ReturnHeader returnHeader = new ReturnHeader
            {
                OrderNo = ReturnFactory.getOrderNo(),
                AddDateTime = DateTime.Now,
                PickingNo = returnObj.OrderNo,
                Status = "0",
                UpdateDateTime = DateTime.Now,
                WGroupId = returnObj.WGroupId,
                ReturnMan = ID,
                ReturnUnit = employee.UNITNO
            };

            List<ReturnBody> returnBodies = new List<ReturnBody>();
            int serialNo = 1;
            foreach(PickingToReturnViewModel returnViewModel in returnObj.ReturnBodies)
            {
                returnBodies.Add(new ReturnBody
                {
                    SerialNo = serialNo.ToString("0000"),
                    Lot = returnViewModel.Lot,
                    PickingSerialNo = returnViewModel.SerialNo,
                    Quantity = returnViewModel.CanReturnQty,
                    StorageId = returnViewModel.StorageId,
                    WareHouseId = returnViewModel.WareHouseId,
                    MaterialNo = returnViewModel.MaterialNo,
                    OrderNo = returnHeader.OrderNo                   
                });

                serialNo++;
            }

            db.Ado.BeginTran();
            try
            {
                db.Insertable(returnHeader).ExecuteCommand();
                db.Insertable(returnBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch(Exception ex)
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        static public bool TransferToScrap(ScrapSaveViewModel scrapObj, string ID)
        {
            bool retValue = true;
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            Employee employee = EmployeeFactory.getEmployee(scrapObj.scrapHeaderViewModel.ApplyMan);

            ScrapHeader scrapHeader = new ScrapHeader
            {
                OrderNo = ScrapFactory.getOrderNo(),
                AddDateTime = DateTime.Now,
                WorkNo = String.Join(",", scrapObj.scrapHeaderViewModel.WorkNo),
                Status = "0",
                UpdateDateTime = DateTime.Now,
                ScrapType = "0", //領料單轉報廢單
                ApplyMan = scrapObj.scrapHeaderViewModel.ApplyMan,
                ApplyDate = scrapObj.scrapHeaderViewModel.ApplyDate,
                Reason = scrapObj.scrapHeaderViewModel.Reason,
                ApplyUnit = employee.UNITNO.Trim()
            };

            PurchaseBody purchaseBody = null;

            List<ScrapBody> scrapBodies = new List<ScrapBody>();
            int serialNo = 1;
            foreach (PickingToScrapViewModel pickingToScrapView in scrapObj.ScrapBodies)
            {
                purchaseBody = PurchaseFactory.getPurcheaseBodyByMaterialNoAndLot(pickingToScrapView.MaterialNo, pickingToScrapView.Lot);
                scrapBodies.Add(new ScrapBody
                {
                    SerialNo = serialNo.ToString("0000"),
                    Lot = pickingToScrapView.Lot,
                    Unit = pickingToScrapView.Unit,
                    Quantity = pickingToScrapView.CanScrapQty,
                    StorageId = pickingToScrapView.StorageId,
                    MaterialNo = pickingToScrapView.MaterialNo,
                    OrderNo = scrapHeader.OrderNo,
                    UnitPrice = purchaseBody == null?0:purchaseBody.Price,
                    TotalPrice = purchaseBody == null ? 0 : pickingToScrapView.CanScrapQty * purchaseBody.Price
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

        static public bool addPickingBody(MaterialPickBodyViewModel addObj, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            PickingBody pickingBody = new PickingBody
            {
                MaterialNo = addObj.MaterialNo,
                PickedQty = addObj.PickedQty,
                Note = addObj.Note,
                OrderNo = addObj.OrderNo,
                StorageId = addObj.StorageId,
                WareHouseId = addObj.WareHouseId,
                SerialNo = getSerialNo(addObj.OrderNo),
                Lot = addObj.Lot,
                Quantity = addObj.Quantity
            };
            if (db.Insertable(pickingBody).ExecuteCommand() > 0) return true;
            else return false;
        }

        static public bool SavePickingData(PickingSaveModel saveObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            saveObj.pickingHeader.OrderNo = getOrderNo();
            saveObj.pickingHeader.Status = "0";
            saveObj.pickingHeader.UpdateDateTime = DateTime.Now;
            saveObj.pickingHeader.ApplyDateTime = saveObj.pickingHeader.ApplyDateTime;
            saveObj.pickingHeader.AddDateTime = DateTime.Now;
            saveObj.pickingHeader.WorkNo = saveObj.pickingHeader.WorkNo;
            saveObj.pickingHeader.PickingReason = saveObj.pickingHeader.PickingReason;

            int serialNo = 1;
            foreach (PickingBody pickingBody in saveObj.pickingBodies)
            {
                pickingBody.PickedQty = pickingBody.Quantity ?? 0;
                pickingBody.OrderNo = saveObj.pickingHeader.OrderNo;
                pickingBody.SerialNo = serialNo.ToString("0000");
                serialNo++;
            }

            bool retValue = true;

            db.Ado.BeginTran();
            try
            {
                db.Insertable(saveObj.pickingHeader).ExecuteCommand();
                db.Insertable(saveObj.pickingBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;

        }

        static public bool SaveToolPickingData(ToolPickingSaveModel saveObj)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            saveObj.pickingToolHeader.OrderNo = getToolOrderNo();
            saveObj.pickingToolHeader.Status = "0";
            saveObj.pickingToolHeader.UpdateDateTime = DateTime.Now;
            saveObj.pickingToolHeader.AddDateTime = DateTime.Now;

            int serialNo = 1;
            foreach (PickingToolBody pickingToolBody in saveObj.pickingToolBodies)
            {
                pickingToolBody.PickedQty = pickingToolBody.Quantity ?? 0;
                pickingToolBody.OrderNo = saveObj.pickingToolHeader.OrderNo;
                pickingToolBody.SerialNo = serialNo.ToString("0000");
                serialNo++;
            }

            bool retValue = true;

            db.Ado.BeginTran();
            try
            {
                db.Insertable(saveObj.pickingToolHeader).ExecuteCommand();
                db.Insertable(saveObj.pickingToolBodies).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }
            return retValue;

        }

        static public bool addPickingToolBody(ToolPickBodyViewModel addObj, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            PickingToolBody pickingBody = new PickingToolBody
            {
                MaterialNo = addObj.MaterialNo,
                PickedQty = addObj.PickedQty,
                Note = addObj.Note,
                OrderNo = addObj.OrderNo,
                StorageId = addObj.StorageId,
                WareHouseId = addObj.WareHouseId,
                SerialNo = getToolSerialNo(addObj.OrderNo),
                Lot = addObj.Lot,
                Quantity = addObj.Quantity,
                KeepMan = addObj.KeepMan,
                KeepUnit = addObj.KeepUnitId
            };
            if (db.Insertable(pickingBody).ExecuteCommand() > 0) return true;
            else return false;
        }

        static public bool savePickingHeader(MaterialPickHeaderSaveModel saveObj, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            PickingHeader pickingHeader = db.Queryable<PickingHeader>().Where(e => e.OrderNo == saveObj.OrderNo).Single();

            List<PickingBody> pickingBodies = db.Queryable<PickingBody>().Where(e => e.OrderNo == saveObj.OrderNo).ToList();

            pickingHeader.OutBoundDate = saveObj.OutBoundDate;
            pickingHeader.OutBoundMan = saveObj.OutBoundMan;
            pickingHeader.EmergencyPicking = saveObj.EmergencyPicking;
            pickingHeader.PickingReason = saveObj.PickingReason;
            pickingHeader.Status = "1";

            bool retValue = true;

            db.Ado.BeginTran();
            try
            {
                foreach(PickingBody pickingBody in pickingBodies)
                {
                   int excuCount = db.Ado.ExecuteCommand("update Inventory set Quantity=Quantity-" + pickingBody.PickedQty + " where " +
                        "MaterialNo=@MaterialNo and WareHouseId=@WareHouseId and StorageId=@StorageId and Lot=@Lot",
                        new { MaterialNo = pickingBody.MaterialNo, WareHouseId= pickingBody.WareHouseId,StorageId=pickingBody.StorageId,Lot=pickingBody.Lot});
                }
                db.Updateable(pickingHeader).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        static public bool updatePickingHeader(MaterialPickHeaderSaveModel saveObj, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            PickingHeader pickingHeader = db.Queryable<PickingHeader>().Where(e => e.OrderNo == saveObj.OrderNo).Single();

            pickingHeader.WorkNo = saveObj.WorkNo;
            pickingHeader.PickingMan = saveObj.PickingMan;
            pickingHeader.PickingUnit = saveObj.PickingUnit;
            pickingHeader.WGroupId = saveObj.WGroupId;
            pickingHeader.EmergencyPicking = saveObj.EmergencyPicking;
            pickingHeader.PickingReason = saveObj.PickingReason;
            pickingHeader.ApplyDateTime = saveObj.ApplyDateTime;


            return db.Updateable(pickingHeader).ExecuteCommand() > 0;
        }

        static public bool deletePicking(string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");


            bool retValue = true;
            db.Ado.BeginTran();
            try
            {
                db.Ado.ExecuteCommand("update  PickingHeader set Status='-1' where OrderNo=@OrderNo",new {OrderNo = OrderNo });
                
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                retValue = false;
                db.Ado.RollbackTran();
            }

            return retValue;
        }

        static public bool saveToolPickingHeader(ToolPickHeaderSaveModel saveObj, string ID)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            PickingToolHeader pickingHeader = db.Queryable<PickingToolHeader>().Where(e => e.OrderNo == saveObj.OrderNo).Single();

            pickingHeader.OutBoundDate = saveObj.OutBoundDate;
            pickingHeader.OutBoundMan = saveObj.OutBoundMan;
            pickingHeader.EmergencyPicking = saveObj.EmergencyPicking;
            pickingHeader.Status = "1";

            if (db.Updateable(pickingHeader).ExecuteCommand() > 0) return true;
            else return false;
        }
        
            static public string getToolSerialNo(string PickingNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from PickingToolBody where OrderNo=@OrderNo";

            string SerialNo = "";

            try
            {
                SerialNo = db.Ado.SqlQuerySingle<string>(sql, new { OrderNo = PickingNo });
            }
            catch (Exception ex)
            {

            }

            SerialNo = (int.Parse(SerialNo) + 1).ToString("0000");

            return SerialNo;
        }
        static public string getSerialNo(string PickingNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string sql = "select isnull(Max(SerialNo),'0000') SerialNo from PickingBody where OrderNo=@OrderNo";

            string SerialNo = "";

            try
            {
                SerialNo = db.Ado.SqlQuerySingle<string>(sql, new { OrderNo = PickingNo });
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

            string OrderPrefix = "C" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM PickingHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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

        static public string getToolOrderNo()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

            DateTime datetime = DateTime.Now;

            string OrderPrefix = "C" + taiwanCalendar.GetYear(datetime).ToString("000") + datetime.Month.ToString("00") + datetime.Day.ToString("00");

            string sql = "SELECT isnull(max(OrderNo),'" + OrderPrefix + "-0000') OrderNo FROM PickingToolHeader where SUBSTRING(OrderNo,1,8) = @OrderPrefix";
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


    }
}