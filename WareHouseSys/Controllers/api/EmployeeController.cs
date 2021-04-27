using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WareHouseSys.DBModels;
using WareHouseSys.Factory;

namespace WareHouseSys.Controllers.api
{
    public class EmployeeController : ApiController
    {
        [Route("api/Employee/getEmployee")]
        [HttpGet]
        public IHttpActionResult getEmployee()
        {
            try
            {
                List<Employee> employees = EmployeeFactory.getAllEmployee();

                return Ok(employees);
            }
            catch(Exception ex)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗:"+ex.Message)
               ));
            }
           
            
        }

        [Route("api/Employee/getEmployeeUnit")]
        [HttpGet]
        public IHttpActionResult getEmployeeUnit()
        {
            try
            {
                var empUnit = EmployeeFactory.getAllEmployeeUnit();
                return Ok(empUnit);
            }
            catch (Exception ex)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(
                   (HttpStatusCode)422,
                   new HttpError("失敗:" + ex.Message)
               ));
            }


        }
    }
}
