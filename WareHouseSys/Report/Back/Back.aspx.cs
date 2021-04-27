using CrystalDecisions.CrystalReports.Engine;
using System;
using System.IO;
using System.Web.Configuration;

namespace WareHouseSys.Report.Back
{
    public partial class Back1 : System.Web.UI.Page
    {
        ReportDocument repDoc = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string reportPath = Server.MapPath("") + "\\Back.rpt";
            repDoc = new ReportDocument();

            string DBSource = WebConfigurationManager.ConnectionStrings["DBSource"].ToString();
            string DBName = WebConfigurationManager.ConnectionStrings["DBName"].ToString();
            string UserId = WebConfigurationManager.ConnectionStrings["UserId"].ToString();
            string Pwd = WebConfigurationManager.ConnectionStrings["Pwd"].ToString();

            try
            {
                if (!File.Exists(reportPath))
                {
                    Response.Write("指定的報表不存在。 \n");
                }

                repDoc.Load(reportPath);

                repDoc.DataSourceConnections[0].SetConnection(DBSource, DBName, UserId, Pwd);

                repDoc.SetParameterValue("OrderNo", Request["OrderNo"]);


                this.CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                this.CrystalReportViewer1.HasToggleGroupTreeButton = false;
                this.CrystalReportViewer1.ReportSource = repDoc;

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            repDoc.Dispose();
        }
    }
}