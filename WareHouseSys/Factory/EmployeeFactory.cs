using SqlSugar;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using WareHouseSys.DBModels;

namespace WareHouseSys.Factory
{
    public class EmployeeFactory
    {
        static public Employee getEmployee(string ID)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            Employee emp =  db.Queryable<Employee>().Where(e => e.KEYNO == ID).Single();

            return emp;
        }

        static public List<Employee> getAllEmployee()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<Employee> employees = db.Queryable<Employee>().Where(e=>e.OFFJOBDATE == null || e.OFFJOBDATE == "")
                .Select(e=>new Employee { CreatedTime = e.CreatedTime,
                                            EMAIL = e.EMAIL,
                                            JOBName = e.JOBName,
                                            KEYNO = e.KEYNO.Trim(),
                                            OFFJOBDATE = e.OFFJOBDATE,
                                            TelExtension = e.TelExtension,
                                            TMNAME = e.TMNAME.Trim(),
                                            UNITNO = e.UNITNO.Trim(),
                                            UpdatedTime = e.UpdatedTime,
                                            USERPWD = e.USERPWD
                                            })
                .ToList();

            return employees;
        }

        static public dynamic getAllEmployeeUnit()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            var empUnit = db.SqlQueryable<dynamic>("select trim(KEYNO) KEYNO,trim(TMNAME) TMNAME,trim(Employee.UNITNO) UNITNO,trim(UNITNAME) UNITNAME from Employee " +
                "inner join UNIT on Employee.UNITNO = UNIT.UNITNO where OFFJOBDATE is null or OFFJOBDATE=''").ToList();

            return empUnit;
        }
    }
}