using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;
using System.Configuration;
using System.Web.Configuration;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class DocFactory
    {
        static public ISugarQueryable<TransactionRecordDetailViewModel> getTransactionInventoryInfo(DateTime StartDate,DateTime EndDate,string MaterialNo)
        {

            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            ISugarQueryable<TransactionRecordDetailViewModel> sugarQueryable =
                db.SqlQueryable<TransactionRecordDetailViewModel>("SELECT transactionDate, className, OrderNo, MaterialNo, MaterialName, WareHouseName, InQty, InPrice, InTotalPrice ,OutQty, OutPrice, OutTotalPrice ,AdjustQty, AdjustPrice, AdjustTotalPrice,InventoryQty, InventoryPrice, InventoryTotalPrice, Note " +
                "FROM TransactionInventory " +
                 "where FORMAT(transactionDate, 'yyyy-MM-dd') between '" + StartDate.ToString("yyyy-MM-dd") + "' and '" + EndDate.ToString("yyyy-MM-dd") + "' and MaterialNo = '" + MaterialNo + "'");

            return sugarQueryable;
        }
    }
}