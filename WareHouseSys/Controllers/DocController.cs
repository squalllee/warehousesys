using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using Models;
using System.Drawing;
using System.IO;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Data;
using WareHouseSys.ViewModels;
using SqlSugar;
using WareHouseSys.Models;
using CrystalDecisions.Shared;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace WareHouseSys.Controllers
{
    public class DocController : BaseController
    {
        // GET: Doc
        public ActionResult Index()
        {
            if (Request.QueryString["start"] == null)
            {
                //ViewBag.Startdate = "2020-03-18";
                //ViewBag.Startdate_old = "3/18/2020";
                ViewBag.Startdate = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.Startdate_old = DateTime.Now.ToString("M/d/yyyy");
            }
            else
            {
                ViewBag.Startdate = ParseDatepicker(Request.QueryString["start"]);
                ViewBag.Startdate_old = Request.QueryString["start"];
            }
            if (Request.QueryString["end"] == null)
            {
                //ViewBag.Enddate = "2020-03-18";
                //ViewBag.Enddate_old = "3/18/2020";
                ViewBag.Enddate = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.Enddate_old = DateTime.Now.ToString("M/d/yyyy");

            }
            else
            {
                ViewBag.Enddate = ParseDatepicker(Request.QueryString["end"]);
                ViewBag.Enddate_old = Request.QueryString["end"];
            }
            if (Request.QueryString["goods"] == null)
            {
                ViewBag.Goods = "00000";
            }
            else
            {
                ViewBag.Goods = Request.QueryString["goods"];

            }
            List<MaterialInfoOnly> MaterialInfoOnlys = new List<MaterialInfoOnly>(); 
            List<MaterialInfo> materialInfos = MaterialFactory.getMaterialInfo();
            if (materialInfos.Count() != 0)
            {
                foreach (var r in materialInfos)
                {

                    MaterialInfoOnlys.Add(new MaterialInfoOnly
                    {
                        MaterialNo = r.MaterialNo,
                        MaterialName = r.MaterialName,
                    });

                }

            }
            ViewBag.MaterialInfo = MaterialInfoOnlys;
                /*
                IQueryable<HRUser> HRUserviews;
                List<HRUser> listHRUserviews;
                //IQueryable<OSUser> OSUserviews;
                //List<OSUser> listOSUserviews;
                List<OSUser> OSUsersList = db.OSUser.ToList();
                List<Employeelist> Employeelists = new List<Employeelist>();
                if (OSUsersList.Count() != 0)
                {
                    String KEYID;
                    String TMName;
                    String UnitNo;
                    //String JobType;
                    //String OFFJobDate;
                    IQueryable<OSUnit> OSUnitviews;
                    List<OSUnit> OSUnitList;
                    if (MyMemberShipProvider.AdminFlag == 1)
                    {
                        //admin
                        OSUnitviews = db.OSUnit;
                        OSUnitList = OSUnitviews.ToList();
                    }
                    else
                    {
                        //not admin
                        OSUnitviews = db.OSUnit.Where(e => e.UnitManager.Trim() == ID);
                        OSUnitList = OSUnitviews.ToList();
                    }
                    if (OSUnitList.Count() != 0)
                    {
                        foreach (var r in OSUnitList)
                        {
                            var unitno = r.UnitNo;
                            foreach (var p in OSUsersList)
                            {
                                //if (p.JobType == "1")
                                //{               
                                if (unitno == p.UnitNo)
                                {
                                    KEYID = p.KEYNO.Trim();
                                    TMName = p.TMName.Trim();
                                    UnitNo = p.UnitNo.Trim();
                                    //JobType = p.JobType.Trim();
                                    //OFFJobDate = p.OFFJobDate.Trim();
                                    Employeelists.Add(new Employeelist
                                    {
                                        KEYNO = KEYID,
                                        TMName = TMName,
                                        UnitNo = UnitNo,
                                        //JobType = JobType,
                                        //OFFJobDate = OFFJobDate
                                    });
                                }
                            }
                        }
                    }
                }
                */
                return View();
        }

        public String ParseDatepicker(String fdtimes)
        {
            String myDate = fdtimes;
            //String myDate = 'Tue Nov 18 00:00:00 GMT 2014';
            String strMnth = "";
            String day = "";
            String year = "";
            //int i = 0;
            //String myDatetemp = "";
            //int item = 0;


            string[] sArray = fdtimes.Split('/');//以字元空白作為分隔符號
            int i = 0;
            foreach (var item in sArray)
            {
                //Console.WriteLine(item);
                i++;
                if (i == 1)
                    strMnth = item;
                else if (i == 2)
                    day = item;
                else if (i == 3)
                    year = item;
            }
            /*
            while (!myDate[i].Equals("\0"))
            {
                int j = 0;
                while (!myDate[i].Equals(" "))
                 {
                    myDatetemp[j] = myDate[i];
                    j++;
                 }
                item++;
                if (item == 2)
                    strMnth = myDatetemp;


            }
            */
            //2005-11-5
            string strMonth = "";
            if (strMnth == "1")
                strMonth = "01";
            else if (strMnth == "2")
                strMonth = "02";
            else if (strMnth == "3")
                strMonth = "03";
            else if (strMnth == "4")
                strMonth = "04";
            else if (strMnth == "5")
                strMonth = "05";
            else if (strMnth == "6")
                strMonth = "06";
            else if (strMnth == "7")
                strMonth = "07";
            else if (strMnth == "8")
                strMonth = "08";
            else if (strMnth == "9")
                strMonth = "09";
            else if (strMnth == "10")
                strMonth = "10";
            else if (strMnth == "11")
                strMonth = "11";
            else if (strMnth == "12")
                strMonth = "12";

            if (day == "1")
                day = "01";
            else if (day == "2")
                day = "02";
            else if (day == "3")
                day = "03";
            else if (day == "4")
                day = "04";
            else if (day == "5")
                day = "05";
            else if (day == "6")
                day = "06";
            else if (day == "7")
                day = "07";
            else if (day == "8")
                day = "08";
            else if (day == "9")
                day = "09";

            String strDate = year + '-' + strMonth + '-' + day;
            //System.debug('------>' + strDate);
            //string s = "2011-03-21 13:26";

            //DateTime mydate1 = DateTime.ParseExact(strDate, "yyyy-MM-dd HH:mm:ss", null);
            //DateTime mydate1 = DateTime.Parse(strDate);
            // System.debug('------>' + mydate1);
            return strDate;
        }
        /*
        public static ISugarQueryable<AdjustBodyViewModel> getAdjustBodyViewModel(FilterCriteria filter, string OrderNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<AdjustBodyViewModel> sugarQueryable = db.SqlQueryable<AdjustBodyViewModel>("SELECT OrderNo, AdjustBody.SerialNo, AdjustBody.MaterialNo, Quantity,  " +
                "MaterialName,Spec,AdjustBody.WareHouseId,WareHouseName,StorageId,Unit,Lot, Reason, StockQty FROM AdjustBody " +
                 "inner join WarehouseInfo on WarehouseInfo.WarehouseId = AdjustBody.WareHouseId " +
                "inner join MaterialInfo on MaterialInfo.MaterialNo = AdjustBody.MaterialNo");

            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            sugarQueryable = sugarQueryable.Where(e => e.OrderNo == OrderNo);

            return sugarQueryable;
        }
        */
        public ActionResult TransactionRecord_Read([DataSourceRequest] DataSourceRequest request, string startdate, string enddate, string goods)
        {
            ISugarQueryable<TransactionRecordDetailViewModel> sugarQueryable1 = getTransactionRecordDetailViewModel(startdate, enddate, goods);
            //int Total = sugarQueryable1.Count();
            List<TransactionRecordDetailViewModel> TransactionRecordDetailViewModels = sugarQueryable1.ToList();
            List<TransactionRecordDetailViewModel> TransactionRecordDetailShowViewModels = new List<TransactionRecordDetailViewModel>();
            foreach (var j in TransactionRecordDetailViewModels)
            {
                TransactionRecordDetailShowViewModels.Add(new TransactionRecordDetailViewModel
                {
                    transactionDate = j.transactionDate.Date,
                    OrderNo = j.OrderNo,
                    className = j.className,
                    WareHouseName = j.WareHouseName,
                    MaterialNo = j.MaterialNo,
                    MaterialName = j.MaterialName,
                    InPrice = decimal.Round(j.InPrice,2),
                    InQty = decimal.Round(j.InQty),
                    InTotalPrice = decimal.Round(j.InTotalPrice,2),
                    OutPrice = decimal.Round(j.OutPrice,2),
                    OutTotalPrice = decimal.Round(j.OutTotalPrice,2),
                    OutQty = decimal.Round(j.OutQty),
                    AdjustPrice = decimal.Round(j.AdjustPrice,2),
                    AdjustTotalPrice = decimal.Round(j.AdjustTotalPrice,2),
                    AdjustQty = decimal.Round(j.AdjustQty),
                    InventoryPrice = decimal.Round(j.InventoryPrice,2),
                    InventoryTotalPrice = decimal.Round(j.InventoryTotalPrice,2),
                    InventoryQty = decimal.Round(j.InventoryQty),
                    Note = j.Note
                });
            }

            //return Json(listtbtoworkviews.ToDataSourceResult(request));
            return Json(TransactionRecordDetailShowViewModels.ToDataSourceResult(request));


        }
        public static ISugarQueryable<TransactionRecordDetailViewModel> getTransactionRecordDetailViewModel(string StartTime, string EndTime, string MNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            ISugarQueryable<TransactionRecordDetailViewModel> sugarQueryable =
                db.SqlQueryable<TransactionRecordDetailViewModel>("SELECT transactionDate, className, OrderNo, MaterialNo, MaterialName, WareHouseName, InQty, InPrice, InTotalPrice ,OutQty, OutPrice, OutTotalPrice ,AdjustQty, AdjustPrice, AdjustTotalPrice,InventoryQty, InventoryPrice, InventoryTotalPrice, Note " +
                "FROM TransactionInventory " +
                 "where FORMAT(transactionDate, 'yyyy-MM-dd') between '" + StartTime + "' and '" + EndTime + "' and MaterialNo = '" + MNo + "'");

            return sugarQueryable;
        }
        
        public static ISugarQueryable<TRDMaterialNoViewModel> getTRDMaterialNoViewModel(string MaterialNo)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");
            ISugarQueryable<TRDMaterialNoViewModel> sugarQueryable = db.SqlQueryable<TRDMaterialNoViewModel>("SELECT MaterialNo, MaterialName  " +
                "FROM MaterialInfo where MaterialNo = '" + MaterialNo + "'");
            return sugarQueryable;
        }

        public ActionResult ReportDoc(string start, string end, string goods)
        {
            string ID = HttpContext.User.Identity.Name;
            //string StartTime = "2019-07-01";
            //string EndTime = "2020-07-01";
            //string MaterialNo = "R0.ME.0034.AM";
            string StartTime = start;
            string EndTime = end;
            string MaterialNo = goods;
            ISugarQueryable<TRDMaterialNoViewModel> sugarQueryable = getTRDMaterialNoViewModel(MaterialNo);

            //int Total = sugarQueryable.Count();
            List<TRDMaterialNoViewModel> TRDMaterialNoViewModels = sugarQueryable.ToList();
            List<IOutInventoryViewModel> InOutInventoryViewModels = new List<IOutInventoryViewModel>();
            List<GoodsInfoViewModel> GoodsInfoViewModels = new List<GoodsInfoViewModel>();
            //foreach (var i in TRDMaterialNoViewModels)
            //{
                ISugarQueryable<TransactionRecordDetailViewModel> sugarQueryable1 = getTransactionRecordDetailViewModel(StartTime, EndTime, MaterialNo);
                int Total = sugarQueryable1.Count();
                List<TransactionRecordDetailViewModel> TransactionRecordDetailViewModels = sugarQueryable1.ToList();
                if (TRDMaterialNoViewModels.Count() != 0)
                {
                    GoodsInfoViewModels.Add(new GoodsInfoViewModel
                    {
                        GoodsNumber = TRDMaterialNoViewModels[0].MaterialNo,
                        GoodsName = TRDMaterialNoViewModels[0].MaterialName
                    });
                }

                foreach (var j in TransactionRecordDetailViewModels)
                {
                    InOutInventoryViewModels.Add(new IOutInventoryViewModel
                    {
                        transactionDate = j.transactionDate.Date,
                        OrderNo = j.OrderNo,
                        className = j.className,
                        WareHouseName = j.WareHouseName,
                        InPrice = j.InPrice,
                        InQty = decimal.Round(j.InQty),
                        InTotalPrice = j.InTotalPrice,
                        OutPrice = j.OutPrice,
                        OutTotalPrice = j.OutTotalPrice,
                        OutQty = decimal.Round(j.OutQty),
                        AdjustPrice = j.AdjustPrice,
                        AdjustTotalPrice = j.AdjustTotalPrice,
                        AdjustQty = decimal.Round(j.AdjustQty),
                        InventoryPrice = j.InventoryPrice,
                        InventoryTotalPrice = j.InventoryTotalPrice,
                        InventoryQty = decimal.Round(j.InventoryQty),
                        Note = j.Note
                    });
                }


                string WorkPath = HostingEnvironment.MapPath("~") + "\\Word";
                string fileDownloadName = MaterialNo + ".pdf";
                CrystalDecisions.Web.CrystalReportViewer CrystalReportViewer1 = new CrystalDecisions.Web.CrystalReportViewer();
                string reportPath = HostingEnvironment.MapPath("~") + "\\Report\\InOutInventory\\InOutInventory.rpt";
                //string reportPath = "C:\\Users\\user\\source\\repos\\Warehousesys\\WareHouseSys\\Report\\InOutInventory\\InOutInventory.rpt";
                ReportDocument repDoc = new ReportDocument();

            //try
            //{
            string DBSource = "192.168.3.238";
            string DBName = "WMS_V1";
            string UserId = "sa";
            string Pwd = "mrt+6182461824";
                    repDoc.Load(reportPath);
                    int count = repDoc.DataSourceConnections.Count;
                    repDoc.DataSourceConnections[0].SetConnection(DBSource, DBName, UserId, Pwd);
                    //repDoc.DataSourceConnections[1].SetConnection(DBSource, DBName, UserId, Pwd);
                    //repDoc.DataSourceConnections[0].IntegratedSecurity = false;
                    //repDoc.DataSourceConnections[1].IntegratedSecurity = false;
                    repDoc.Database.Tables["WareHouseSys_ViewModels_GoodsInfoViewModel"].SetDataSource(ReportUtility.ToDataTable<GoodsInfoViewModel>(GoodsInfoViewModels));
                    //repDoc.DataSourceConnections[1].SetConnection(DBSource, DBName, UserId, Pwd);
                    repDoc.Database.Tables["WareHouseSys_ViewModels_IOutInventoryViewModel"].SetDataSource(ReportUtility.ToDataTable<IOutInventoryViewModel>(InOutInventoryViewModels));

                    Stream stream = repDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    byte[] srcBuf = new Byte[stream.Length];
                    stream.Read(srcBuf, 0, srcBuf.Length);
                    stream.Seek(0, SeekOrigin.Begin);
 
                    Response.Clear();

                    // 產生 Pdf 資料流
                    MemoryStream ms = new MemoryStream();
                    ms.Write(srcBuf, 0, srcBuf.Length);
                    // 設定強制下載標頭
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileDownloadName));
                    Response.ContentType = "application/pdf";
                    // 輸出檔案
                    Response.BinaryWrite(ms.ToArray());
                    ms.Close();
                    ms.Dispose();
                    Response.End();
                
            //return View();
            //return Content($"{reportPath}帳號無權存取此頁面!");
            /*
             if (!Directory.Exists(WorkPath))
             {
                 Directory.CreateDirectory(WorkPath);
             }

             if (System.IO.File.Exists(WorkPath + "\\" + fileDownloadName))
             {
                 System.IO.File.Delete(WorkPath + "\\" + fileDownloadName);
             }

             using (FileStream fsWrite = new FileStream(WorkPath + "\\" + fileDownloadName, FileMode.Append))
             {
                 fsWrite.Write(srcBuf, 0, srcBuf.Length);
                 fsWrite.Close();
             };
            */
            //}
            //catch (CrystalReportsException crex)
            //{
            //    throw;
            // }
            // catch (Exception ex)
            // {

             //}
             //finally
            // {
             //    if (repDoc != null)
             //    {
             //        repDoc.Close();
             //        repDoc.Dispose();
             //    }
            // }

            return View();
            // FileStream fsSource = new FileStream(WorkPath + "\\" + fileDownloadName, FileMode.Open, FileAccess.Read);

            // return new FileStreamResult(fsSource, "application/pdf");

            //}

        }

    }
}