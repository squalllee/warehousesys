using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.ComponentModel;
using System.Linq;
using SqlSugar;

namespace WareHouseSys.Controllers
{
    public class RecvController : BaseController
    {
        public ActionResult RecvSearch()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReceiveRecordSearch()
        {
            return View();
        }

        public ActionResult ReceiveRecordSearchGrid([DataSourceRequest] DataSourceRequest request)
        {
            ISugarQueryable<ReceiveSearchViewModel> sugarQueryable = RecvFactory.getReceiveSearchViewModel(request);

            string sortStr = "";
            if (request.Sorts.Any())
            {
                foreach (SortDescriptor sortDescriptor in request.Sorts)
                {
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        sortStr += String.Format("{0} {1}", sortDescriptor.Member, "asc") + ",";

                    }
                    else
                    {
                        sortStr += String.Format("{0} {1}", sortDescriptor.Member, "desc") + ",";
                    }
                }
            }

            if (sortStr != "") sugarQueryable = sugarQueryable.OrderBy(sortStr.TrimEnd(','));
            List<ReceiveSearchViewModel> receiveSearchViewModels = null;
            if (request.PageSize == 0)
                receiveSearchViewModels = sugarQueryable.ToList();
            else
                receiveSearchViewModels = sugarQueryable.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            string filePath = Server.MapPath("~") + "\\Attatchment\\Recv\\";

            foreach (ReceiveSearchViewModel receiveSearchViewModel in receiveSearchViewModels)
            {
                if (Directory.Exists(filePath + receiveSearchViewModel.OrderNo))
                {
                    foreach (string f in Directory.GetFiles(filePath + receiveSearchViewModel.OrderNo))
                    {
                        receiveSearchViewModel.AttUrl = Path.GetFileName(f);
                    }
                }
            }

            request.Page = 1;
            DataSourceResult dataSourceResult = receiveSearchViewModels.ToDataSourceResult(request);
            dataSourceResult.Total = sugarQueryable.Count();
            return Json(dataSourceResult);
        }

        public ActionResult RecvDetail(string OrderNo)
        {
            RecvHeaderViewModel recvBodyViewModel = RecvFactory.getRecvViewHeader(OrderNo);

            string filePath = Server.MapPath("~") + "\\Attatchment\\Recv\\" + OrderNo ;
            List<Attachment> attachments = new List<Attachment>();

            if (Directory.Exists(filePath))
            {
                foreach (string f in Directory.GetFiles(filePath))
                {
                    attachments.Add(new Attachment
                    {
                        FileName = Path.GetFileName(f),
                    });
                }
            }

            ViewBag.Attachments = attachments;

            ViewBag.OrderNo = OrderNo;
            return View(recvBodyViewModel);
        }

        public ActionResult RecvData(string OrderNo,string Lot)
        {
            RecvHeaderViewModel recvBodyViewModel = RecvFactory.getRecvViewHeader(OrderNo, Lot);
            return View(recvBodyViewModel);
        }

        public ActionResult FillRecvData(RecvDataViewModel obj)
        {
            ViewBag.WarehouseInfos = WarehouseInfoFactory.getWarehouseInfo();
            ViewBag.StorageInfo = StorageInfoFactory.getStorageInfo(obj.WarehouseId);
            
            return View(obj);
        }


        public ActionResult RecvClose(string OrderNo,string Lot)
        {
            List<Attachment> attachments = new List<Attachment>();
            string filePath = Server.MapPath("~") + "\\Attatchment\\Recv\\" + OrderNo;
            if (Directory.Exists(filePath))
            {
                foreach (string f in Directory.GetFiles(filePath))
                {

                    byte[] bytes = System.IO.File.ReadAllBytes(f);
                    string file = Convert.ToBase64String(bytes);
                    attachments.Add(new Attachment
                    {
                        FileName = Path.GetFileName(f),
                        Content = file
                    });
                }
            }

            ViewBag.OrderNo = OrderNo;
            ViewBag.Lot = Lot;
            return View(attachments);
        }

        public ActionResult getAttatchment(string OrderNo, string FileName)
        {
            string pathSource = Server.MapPath("~") + "\\Attatchment\\Recv\\" + OrderNo + "\\" + FileName;
           
            string Extension = Path.GetExtension(FileName);
            string contentType = "";

            switch (Extension.ToUpper())
            {
                case ".PNG":
                    contentType = "image/png";
                    break;
                case ".JPG":
                    contentType = "image/jpeg";
                    break;
                case ".PDF":
                    contentType = "application/pdf";
                    break;
            }
            FileStream fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);

            return new FileStreamResult(fsSource, contentType);
        }

        public ActionResult TransToInventory(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;

            InboundHeaderViewModel inboundHeaderViewModel = InboundFactory.getInboundViewHeader(OrderNo);

            return View(inboundHeaderViewModel);
        }
    }
}