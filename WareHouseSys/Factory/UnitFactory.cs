using SqlSugar;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using WareHouseSys.DBModels;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class UnitFactory
    {
        static public UNIT getUint(string UnitNo)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            UNIT Unit = db.Queryable<UNIT>().Where(e => e.UNITNO == UnitNo).Single();

            return Unit;
        }


        static public List<UNIT> getAllUint()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            //List<UNIT> Unit1 = db.Queryable<UNIT>().ToList();
            List<UNIT> Unit1 = db.Queryable<UNIT>()
                     .Select(e => new UNIT
                     {
                         //CreatedTime = e.CreatedTime,
                            UPUNIT = e.UPUNIT.Trim(),
                            UNITMANAGER = e.UNITMANAGER.Trim(),
                            UNITTMNAME = e.UNITTMNAME.Trim(),
                            UPDATEDATE = e.UPDATEDATE,
                            //TelExtension = e.TelExtension,
                            UNITNAME = e.UNITNAME.Trim(),
                            UNITNO = e.UNITNO.Trim(),
                            UPDATETIME = e.UPDATETIME,
                            UPDATEMAN = e.UPDATEMAN
                     })
                     .ToList();

            return Unit1;
        }

        static public List<UNIT> getWareHouseUint()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<UNIT> Unit1 = db.Queryable<UNIT>().Where(e=>e.UNITNO == e.UNITNO.Substring(0,3) + "00" && (e.UNITNO.Substring(0,2) == "K1" || e.UNITNO.Substring(0,2) == "L1")).ToList();

            return Unit1;
        }

        static public List<ToolManagerViewModel> getToolKeepUnit()
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            List<ToolManagerViewModel> Unit = db.SqlQueryable<ToolManagerViewModel>("select ToolManager.UNITNO,UNITNAME,(select TMNAME from Employee where KEYNo=ToolMgr) ToolMgr,ToolMgr ToolMgrId " +
                "from ToolManager inner join UNIT on ToolManager.UNITNO = UNIT.UNITNO").ToList();

            return Unit;
        }
    }
}