<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReqMaterialReport.aspx.cs" Inherits="WareHouseSys.Report.ReqMaterial.ReqMaterialReport" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692FBEA5521E1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script src="https://kendo.cdn.telerik.com/2019.3.1023/js/jquery.min.js"></script>
     <script>
        $(document).ready(function () {
            $("[id$=_iframe]").bind("load", function () {
               $(this).contents().find('img').each(function () {
                     var curSrc = $(this).attr('src');
                     $(this).attr('src', curSrc.replace("Crys", "Reports/Crys"));
               });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
    </form>
</body>
</html>
