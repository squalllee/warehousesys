using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using WareHouseSys.Factory;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Controllers.api
{
    public class InventoryStockController : ApiController
    {
        [Route("api/InventoryStock/SaveInventory")]
        [HttpPost]
        public IHttpActionResult SaveInventory(InventorySaveModel inventorySaveModel)
        {
            if (StockInventoryFactory.saveInventory(inventorySaveModel))
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

        [Route("api/InventoryStock/SaveToolInventory")]
        [HttpPost]
        public IHttpActionResult SaveToolInventory(ToolInventorySaveModel toolInventorySaveModel)
        {
            if (ToolInventoryFactory.saveToolInventory(toolInventorySaveModel))
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


        [Route("api/InventoryStock/deleteInventory/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult deleteInventory(string OrderNo)
        {
            if (StockInventoryFactory.deleteInventory(OrderNo))
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

        [Route("api/InventoryStock/deleteToolInventory/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult deleteToolInventory(string OrderNo)
        {
            if (ToolInventoryFactory.deleteToolInventory(OrderNo))
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

        

        [Route("api/InventoryStock/updateFirstCheckQty")]
        [HttpPost]
        public IHttpActionResult updateFirstCheckQty(List<StockInventoryBodyViewModel> stockInventoryBodyViewModels)
        {
            if (StockInventoryFactory.updateFirstCheckQty(stockInventoryBodyViewModels))
            {
                return Ok(stockInventoryBodyViewModels);
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

        [Route("api/InventoryStock/updateToolFirstCheckQty")]
        [HttpPost]
        public IHttpActionResult updateToolFirstCheckQty(List<ToolInventoryBodyViewModel> toolInventoryBodyViewModels)
        {
            if (ToolInventoryFactory.updateFirstCheckQty(toolInventoryBodyViewModels))
            {
                return Ok(toolInventoryBodyViewModels);
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

        
        [Route("api/InventoryStock/updateSecondCheckQty")]
        [HttpPost]
        public IHttpActionResult updateSecondCheckQty(List<StockInventoryBodyViewModel> stockInventoryBodyViewModels)
        {
            if (StockInventoryFactory.updateSecondCheckQty(stockInventoryBodyViewModels))
            {
                return Ok(stockInventoryBodyViewModels);
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

        [Route("api/InventoryStock/updateToolSecondCheckQty")]
        [HttpPost]
        public IHttpActionResult updateToolSecondCheckQty(List<ToolInventoryBodyViewModel> toolInventoryBodyViewModels)
        {
            if (ToolInventoryFactory.updateToolSecondCheckQty(toolInventoryBodyViewModels))
            {
                return Ok(toolInventoryBodyViewModels);
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

        
        [Route("api/InventoryStock/setSecondCheckComplete/{OrderNo}")]
        [HttpPost]
        public IHttpActionResult setSecondCheckComplete(string OrderNo, List<Attachment> atts)
        {
            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Inventory\\" + OrderNo + "\\SecondCheck";
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

            if (StockInventoryFactory.setSecondCheckComplete(OrderNo))
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

        [Route("api/InventoryStock/setToolSecondCheckComplete/{OrderNo}")]
        [HttpPost]
        public IHttpActionResult setToolSecondCheckComplete(string OrderNo, List<Attachment> atts)
        {
            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\ToolInventory\\" + OrderNo + "\\SecondCheck";
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

            if (ToolInventoryFactory.setToolSecondCheckComplete(OrderNo))
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
        

        [Route("api/InventoryStock/setToolFirstCheckComplete/{OrderNo}")]
        [HttpPost]
        public IHttpActionResult setToolFirstCheckComplete(string OrderNo, List<Attachment> atts)
        {
            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\ToolInventory\\" + OrderNo + "\\FirstCheck";
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

            if (ToolInventoryFactory.setToolFirstCheckComplete(OrderNo))
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


        [Route("api/InventoryStock/setFirstCheckComplete/{OrderNo}")]
        [HttpPost]
        public IHttpActionResult setFirstCheckComplete(string OrderNo,List<Attachment> atts)
        {
            string filePath = HostingEnvironment.MapPath("~") + "\\Attatchment\\Inventory\\" + OrderNo + "\\FirstCheck";
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

            if (StockInventoryFactory.setFirstCheckComplete(OrderNo))
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


        [Route("api/InventoryStock/InventoryClose/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult InventoryClose(string OrderNo)
        {
            if (StockInventoryFactory.InventoryClose(OrderNo))
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

        [Route("api/InventoryStock/ToolInventoryClose/{OrderNo}")]
        [HttpGet]
        public IHttpActionResult ToolInventoryClose(string OrderNo)
        {
            if (ToolInventoryFactory.ToolInventoryClose(OrderNo))
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
        

    }
}
