using SqlSugar;
using System.Configuration;
using System.Web.Configuration;
using WareHouseSys.DBModels;

namespace WareHouseSys.Factory
{
    public class JobListFactory
    {
        static public bool CreateJob(JobList Job)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            bool retValue = true;

            try
            {
                db.Insertable<JobList>(Job).ExecuteCommand();
            }
            catch
            {
                retValue = false;
            }
           
            return retValue;
        }
    }
}