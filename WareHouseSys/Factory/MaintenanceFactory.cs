using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class MaintenanceFactory
    {
        public static List<string> getWorkNos(string filter)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("MaintainConnection");
     
            List<string> workNoList = db.Ado.SqlQuery<WorkOrder>("select Top 30 RepairNo from InfoWork  inner join WorkOrder on WorkOrder.RepairNo like InfoWork.WorkNo + '%' where Status not in ('-1','-2','4') and InfoWork.WorkNo like @RepairNo + '%'  union " +
                    "select Top 30 RepairNo from PreventiveWork inner join PreventiveWorkContent on PreventiveWorkContent.RepairNo like  PreventiveWork.WorkNo + '%' where Status not in ('-1','-2','4') and PreventiveWork.WorkNo like @RepairNo + '%' " +
                    "union select Top 30 RepairNo from OtherWork inner join OtherWorkContent  on OtherWorkContent.RepairNo like OtherWork.WorkNo + '%' where Status not in ('-1','-2','4') and OtherWork.WorkNo like @RepairNo + '%'", new { RepairNo= filter }).Select(e=> e.RepairNo).ToList();

            return workNoList;
        }

        public static List<string> getUnCloseWorkNos(string filter)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("MaintainConnection");

            List<string> workNoList = db.Ado.SqlQuery<WorkOrder>("select Top 30 RepairNo from InfoWork  inner join WorkOrder on WorkOrder.RepairNo like InfoWork.WorkNo + '%' where  InfoWork.WorkNo like @RepairNo + '%'  union " +
                    "select Top 30 RepairNo from PreventiveWork inner join PreventiveWorkContent on PreventiveWorkContent.RepairNo like  PreventiveWork.WorkNo + '%' where  PreventiveWork.WorkNo like @RepairNo + '%' " +
                    "union select Top 30 RepairNo from OtherWork inner join OtherWorkContent  on OtherWorkContent.RepairNo like OtherWork.WorkNo + '%' where   OtherWork.WorkNo like @RepairNo + '%'", new { RepairNo = filter }).Select(e => e.RepairNo).ToList();

            return workNoList;
        }
    }
}