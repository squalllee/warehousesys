using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Hosting;
using System.Web.Http;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class RecvController : ApiController
    {
        [Route("api/Recv/RecvSearch")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult RecvSearch(FormDataCollection form)
        {
            string ID = User.Identity.Name;
            string RecvNo = form.Get("OrderNo");
            string StartDateTime = form.Get("ReceiveDateStart");
            string EndDateTime = form.Get("ReceiveDateEnd");

            int draw = int.Parse(form.Get("draw"));
            int start = int.Parse(form.Get("start"));
            int length = int.Parse(form.Get("length"));

            string col_index = form.Get("order[0][column]");
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : form.Get($@"columns[{col_index}][data]");
            string asc_desc = string.IsNullOrEmpty(form.Get("order[0][dir]")) ? "desc" : form.Get("order[0][dir]");//防呆

            
            ISugarQueryable<ReceiveHeader> sugarQueryable = RecvFactory.getRecvHeader(RecvNo, StartDateTime, EndDateTime,ID);
            
            sugarQueryable.OrderBy(String.Format("{0} {1}", sortColName, asc_desc));
            List<ReceiveHeader> receiveHeaders = sugarQueryable.Skip(start).Take(length).ToList();

            List<Employee> employees = EmployeeFactory.getAllEmployee();

            foreach(ReceiveHeader receiveHeader in receiveHeaders)
            {
                receiveHeader.ReceiveMan = employees.Where(e => e.KEYNO.Trim() == receiveHeader.ReceiveMan.Trim()).Single().TMNAME.Trim();
                if (receiveHeader.Status == "0") receiveHeader.Status = "辦理中";
                if (receiveHeader.Status == "1") receiveHeader.Status = "已結案";
            }

            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = sugarQueryable.Count(),
                  recordsFiltered = sugarQueryable.Count(),
                  data = receiveHeaders
              };

            return Ok(returnObj);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Recv/getRecvBodys/{RecvNo}")]
        public IHttpActionResult getRecvBodys(string RecvNo, FormDataCollection form)
        {
            string ID = User.Identity.Name;
            int draw = int.Parse(form.Get("draw"));
            int start = int.Parse(form.Get("start"));
            int length = int.Parse(form.Get("length"));

            string col_index = form.Get("order[0][column]");
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : form.Get($@"columns[{col_index}][data]");
            string asc_desc = string.IsNullOrEmpty(form.Get("order[0][dir]")) ? "desc" : form.Get("order[0][dir]");//防呆

            ISugarQueryable<RecvBodyViewModel> sugarQueryable = RecvFactory.getRecvViewBody(RecvNo, ID);

            sugarQueryable = sugarQueryable.OrderBy($@"{sortColName} {asc_desc}");
            List<RecvBodyViewModel> recvDetailViewModels = sugarQueryable.Skip(start).Take(length).ToList();
            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = sugarQueryable.Count(),
                  recordsFiltered = sugarQueryable.Count(),
                  data = recvDetailViewModels
              };

            return Ok(returnObj);
        }

        [Route("api/Recv/getRecvHeader")]
        [HttpPost]
        public IHttpActionResult Post(FormDataCollection form)
        {
            string ID = User.Identity.Name;
            string RecvNo = form.Get("RecvNo");
            string StartDateTime = form.Get("RecvDateStart");
            string EndDateTime = form.Get("RecvDateEnd");
            int rows = int.Parse(form.Get("rows"));
            int page = int.Parse(form.Get("page"));
            string sort = form.Get("sort");
            string order = form.Get("order");

            ISugarQueryable<ReceiveHeader> sugarQueryable = RecvFactory.getRecvHeader(RecvNo, StartDateTime, EndDateTime, ID);

            List<Employee> employees = EmployeeFactory.getAllEmployee();
            List<UNIT> Units = UnitFactory.getAllUint();

            List<ReceiveHeader> receives = null;
            try
            {
                receives = sugarQueryable.Skip(rows * (page - 1)).Take(10).ToList();
            }
            catch
            {

            }


            foreach (ReceiveHeader recv in receives)
            {
                recv.ReceiveMan = employees.Where(e => e.KEYNO == recv.ReceiveMan).Select(e => e.TMNAME).SingleOrDefault();

                if (recv.Status == "0") recv.Status = "辦理中";
                else if (recv.Status == "1") recv.Status = "已結案";
                else if (recv.Status == "2") recv.Status = "已退回";
                else if (recv.Status == "3") recv.Status = "已抽回";
            }

            dgReturnObj retObject = new dgReturnObj()
            {
                total = sugarQueryable.Count(),
                rows = receives
            };
            return Ok(retObject);
        }

        [Route("api/Recv/getRecvData")]
        [HttpPost]
        public IHttpActionResult getRecvData(FormDataCollection form,string OrderNo,string Lot)
        {
            string ID = User.Identity.Name;

            int draw = int.Parse(form.Get("draw"));
            int start = int.Parse(form.Get("start"));
            int length = int.Parse(form.Get("length"));

            string col_index = form.Get("order[0][column]");
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : form.Get($@"columns[{col_index}][data]");
            string asc_desc = string.IsNullOrEmpty(form.Get("order[0][dir]")) ? "desc" : form.Get("order[0][dir]");//防呆
            ISugarQueryable<RecvDataViewModel> sugarQueryable = RecvFactory.getRecvData(OrderNo, Lot);

            sugarQueryable.OrderBy(String.Format("{0} {1}", sortColName, asc_desc));
            List<RecvDataViewModel> receives = sugarQueryable.Skip(start).Take(length).ToList();

            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = sugarQueryable.Count(),
                  recordsFiltered = sugarQueryable.Count(),
                  data = receives
              };

            return Ok(returnObj);
        }

        [Route("api/Recv/saveRecvData")]
        [HttpPost]
        public IHttpActionResult saveRecvData(RecvDataViewModel obj)
        {
            string ID = User.Identity.Name;

            if (RecvFactory.SaveRecvData(obj))
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

        [Route("api/Recv/saveRecvHeader")]
        [HttpPost]
        public IHttpActionResult saveRecvHeader(RecvHeaderViewModel obj)
        {
            string ID = User.Identity.Name;

            if (RecvFactory.SaveRecvHeader(obj,ID))
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

        
        [Route("api/Recv/getRecvBodyByLot/{OrderNo}/{Lot}")]
        [HttpPost]
        public IHttpActionResult getRecvBodyByLot(FormDataCollection form, string OrderNo, string Lot)
        {
            string ID = User.Identity.Name;

            int draw = int.Parse(form.Get("draw"));
            int start = int.Parse(form.Get("start"));
            int length = int.Parse(form.Get("length"));

            string col_index = form.Get("order[0][column]");
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : form.Get($@"columns[{col_index}][data]");
            string asc_desc = string.IsNullOrEmpty(form.Get("order[0][dir]")) ? "desc" : form.Get("order[0][dir]");//防呆

            //List<string> Lots = (from p in PurchaseFactory.getTransInboundLots(OrderNo)
            //                    select p.DeliveryLot).ToList();

            //for (int i = Lots.Count - 1; i >= 0; i--)
            //{
            //    if (!PurchaseFactory.canTransToInbound(Lots[i].PurchaseNo))
            //    {
            //        Lots.Remove(Lots[i]);
            //    }
            //}

            

            ISugarQueryable<TransToInboundViewModel> sugarQueryable = RecvFactory.getRecvBodyByLot(OrderNo, Lot);

            sugarQueryable.OrderBy(String.Format("{0} {1}", sortColName, asc_desc));
            List<TransToInboundViewModel> transToInboundViewModels = sugarQueryable.Skip(start).Take(length).ToList();

            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = sugarQueryable.Count(),
                  recordsFiltered = sugarQueryable.Count(),
                  data = transToInboundViewModels
              };

            return Ok(returnObj);
        }

        [Route("api/Recv/RecvClose")]
        [HttpPost]
        public IHttpActionResult RecvClose(List<Attachment> atts, string OrderNo,string Lot)
        {
            string ID = User.Identity.Name;

            if(RecvFactory.CloseRecv(OrderNo,Lot))
            {
                try
                {
                    string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Recv\\" + OrderNo ;
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    foreach (Attachment att in atts)
                    {
                        try
                        {
                            File.WriteAllBytes(filePath + "\\" + att.FileName, Convert.FromBase64String(att.Content));
                        }
                        catch
                        {
                            return NotFound();
                        }
                    }

                    return Ok();
                }
                catch
                {
                    return new System.Web.Http.Results.ResponseMessageResult(
                     Request.CreateErrorResponse(
                         (HttpStatusCode)422,
                         new HttpError("新增失敗!")));
                }
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                     Request.CreateErrorResponse(
                         (HttpStatusCode)422,
                         new HttpError("新增失敗!")));
            }
        }

        
    }
}
