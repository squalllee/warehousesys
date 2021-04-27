using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModel;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class PurchaseController : ApiController
    {
        [Route("api/Purchase/getPurchaseHeader")]
        [HttpPost]
        public IHttpActionResult Post(PurchaseParameters parameters)
        {
            ISugarQueryable<PurchaseHeader> sugarQueryable = PurchaseFactory.getPurcheaseHeader(parameters);

            List<Employee> employees = EmployeeFactory.getAllEmployee();
            List<UNIT> Units = UnitFactory.getAllUint();

            List<PurchaseHeader> purchases = null;
            try
            {
                purchases = sugarQueryable.Skip(parameters.rows * (parameters.page - 1)).Take(10).ToList();
            }
            catch
            {

            }

            foreach( PurchaseHeader purchase in purchases)
            {
                purchase.PurchaseMan = employees.Where(e => e.KEYNO == purchase.PurchaseMan).Select(e => e.TMNAME).SingleOrDefault();
                purchase.PurchaseUnit = Units.Where(e => e.UNITNO == purchase.PurchaseUnit).Select(e => e.UNITNAME).SingleOrDefault();

                if (purchase.Status == "0") purchase.Status = "辦理中";
                else if (purchase.Status == "1") purchase.Status = "已結案";
                else if (purchase.Status == "2") purchase.Status = "已退回";
                else if (purchase.Status == "3") purchase.Status = "已抽回";
            }

            dgReturnObj retObject = new dgReturnObj()
            {
                total = sugarQueryable.Count(),
                rows = purchases
            };
            return Ok(retObject);
        }

        [Route("api/Purchase/getPurBody/{PurchaseNo}")]
        [HttpPost]
        public IHttpActionResult getPurBodys(string PurchaseNo, FormDataCollection form)
        {
            int draw = int.Parse(form.Get("draw"));
            int start = int.Parse(form.Get("start"));
            int length = int.Parse(form.Get("length"));

            string col_index = form.Get("order[0][column]");
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : form.Get($@"columns[{col_index}][data]");
            string asc_desc = string.IsNullOrEmpty(form.Get("order[0][dir]")) ? "desc" : form.Get("order[0][dir]");//防呆

            ISugarQueryable<PurchaseDetailViewModel> sugarQueryable = PurchaseFactory.getPurcheaseBody(PurchaseNo);

            sugarQueryable = sugarQueryable.OrderBy($@"{sortColName} {asc_desc}");
            List<PurchaseDetailViewModel> purchaseDetailViewModels = sugarQueryable.Skip(start).Take(length).ToList();
            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = sugarQueryable.Count(),
                  recordsFiltered = sugarQueryable.Count(),
                  data = purchaseDetailViewModels
              };

            return Ok(returnObj);
        }

        [Route("api/Purchase/getPurBodyByLot/{PurchaseNo}/{Lot}")]
        [HttpPost]
        public IHttpActionResult getPurBodysByLot(string PurchaseNo,string Lot, FormDataCollection form)
        {
            int draw = int.Parse(form.Get("draw"));
            int start = int.Parse(form.Get("start"));
            int length = int.Parse(form.Get("length"));

            string col_index = form.Get("order[0][column]");
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : form.Get($@"columns[{col_index}][data]");
            string asc_desc = string.IsNullOrEmpty(form.Get("order[0][dir]")) ? "desc" : form.Get("order[0][dir]");//防呆

            ISugarQueryable<PurchaseBodyViewModel> sugarQueryable = PurchaseFactory.getPurcheaseBodyByLot(PurchaseNo,Lot);

            sugarQueryable = sugarQueryable.OrderBy($@"{sortColName} {asc_desc}");
            List<PurchaseBodyViewModel> purchaseBodyViewModels = sugarQueryable.Skip(start).Take(length).ToList();

            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = sugarQueryable.Count(),
                  recordsFiltered = sugarQueryable.Count(),
                  data = purchaseBodyViewModels
              };

            return Ok(returnObj);
        }

        [Route("api/Purchase/TransToRecv/{PurchaseNo}/{Lot}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult TransToRecv(string PurchaseNo,string Lot)
        {
            string ID = User.Identity.Name;
            PurchaseFactory.TransToRecv(PurchaseNo, Lot, ID);

            return Ok(); 
        }

        [Route("api/Purchase/OpenContractToRecv")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult OpenContractToRecv(OpenContractToPurViewModel openContractToPurViewModel)
        {
            string ID = User.Identity.Name;
            PurchaseFactory.OpenContractToRecv(openContractToPurViewModel,ID);

            return Ok();
        }

        [Route("api/Purchase/TransToInbound")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult TransToInbound(TransToInboundSaveModel transToInboundSaveModel)
        {
            string ID = User.Identity.Name;

            if(PurchaseFactory.TransToInbound(transToInboundSaveModel.PurchaseNo, transToInboundSaveModel.Lots, ID))
            {
                return Ok();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(
                  (HttpStatusCode)422,
                  new HttpError("失敗")
              ));
            }

            
        }
        
        [Route("api/Purchase/SavePurBody")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult SavePurBody(PurchaseDetailViewModel purchaseDetailViewModel)
        {
            PurchaseBody purchaseBody = new PurchaseBody
            {
                PurchaseNo = purchaseDetailViewModel.PurchaseNo,
                MaterialNo = purchaseDetailViewModel.MaterialNo,
                DeliveryLot = purchaseDetailViewModel.DeliveryLot,
                DeliveryPlace = purchaseDetailViewModel.DeliveryPlace,
                PerformancePeriod = purchaseDetailViewModel.PerformancePeriod,
                Price = purchaseDetailViewModel.Price,
                Quantity = purchaseDetailViewModel.Quantity,
                RequireUnit = purchaseDetailViewModel.ReqireUnit,
                SerialNo = purchaseDetailViewModel.SerialNo
            };

            if(PurchaseFactory.savePurchaseBody(purchaseBody))
                return Ok();
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }
        }

        [Route("api/Purchase/AddPurBody")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddPurBody(PurchaseDetailViewModel purchaseDetailViewModel)
        {
            PurchaseBody purchaseBody = new PurchaseBody
            {
                PurchaseNo = purchaseDetailViewModel.PurchaseNo,
                MaterialNo = purchaseDetailViewModel.MaterialNo,
                DeliveryLot = purchaseDetailViewModel.DeliveryLot,
                DeliveryPlace = purchaseDetailViewModel.DeliveryPlace,
                PerformancePeriod = purchaseDetailViewModel.PerformancePeriod,
                Price = purchaseDetailViewModel.Price,
                Quantity = purchaseDetailViewModel.Quantity,
                RequireUnit = purchaseDetailViewModel.ReqireUnit,
                SerialNo = PurchaseFactory.getDetailSerialNo(purchaseDetailViewModel.PurchaseNo)
            };

            if (PurchaseFactory.addPurchaseBody(purchaseBody))
                return Ok();
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }
        }

        [Route("api/Purchase/savePurHeader")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult savePurHeader(PurchaseSaveHeader purchaseHeader)
        {
            if (PurchaseFactory.savePurchaseHeader(purchaseHeader))
                return Ok();
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }
        }

        [Route("api/Purchase/deletePurBody/{PurchaseNo}/{SerialNo}")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult deletePurBody(string PurchaseNo,string SerialNo)
        {

            if (PurchaseFactory.deletePurBody(PurchaseNo, SerialNo))
                return Ok();
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗")
               ));
            }
        }

        [Route("api/Purchase/PurchaseSearch")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PurchaseSearch(FormDataCollection form)
        {
            string ID = User.Identity.Name;
            string PurchaseNo = form.Get("PurchaseNo");
            string PurchaseDateStart = form.Get("PurchaseDateStart");
            string PurchaseDateEnd = form.Get("PurchaseDateEnd");

            int draw = int.Parse(form.Get("draw"));
            int start = int.Parse(form.Get("start"));
            int length = int.Parse(form.Get("length"));

            string col_index = form.Get("order[0][column]");
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : form.Get($@"columns[{col_index}][data]");
            string asc_desc = string.IsNullOrEmpty(form.Get("order[0][dir]")) ? "desc" : form.Get("order[0][dir]");//防呆

            ISugarQueryable<PurchaseHeader> sugarQueryable = PurchaseFactory.getPurcheaseHeader(PurchaseNo, PurchaseDateStart, PurchaseDateEnd, ID);
            sugarQueryable.OrderBy(String.Format("{0} {1}", sortColName, asc_desc));
            List<PurchaseHeader> reqs = sugarQueryable.Skip(start).Take(length).ToList();

            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = sugarQueryable.Count(),
                  recordsFiltered = sugarQueryable.Count(),
                  data = reqs
              };

            return Ok(returnObj);
        }
    }
}
