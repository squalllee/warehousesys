$(document).ready(function () {
    $("#PurchaseDateStart").datebox({
        formatter: function (date) { return date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate(); },
        parser: function (date) {
            var t = Date.parse(date);
            if (!isNaN(t)) {
                return new Date(Date.parse(date.replace(/-/g, "/")));
            } else {
                return new Date();
            }
        }
    });
    $("#PurchaseDateEnd").datebox({
        formatter: function (date) { return date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate(); },
        parser: function (date) {
            var t = Date.parse(date);
            if (!isNaN(t)) {
                return new Date(Date.parse(date.replace(/-/g, "/")));
            } else {
                return new Date();
            }
        }
    });
});

function OrderStyler(value, row, index) {
    return '<center><a href="#" onclick="openDetail(\'' + row.PurchaseNo + '\');">' + row.PurchaseNo + '</a ></center > ';
}

function CreateStyler(value, row, index) {
    if (row.Status === "已結案" && row.IsCreateRecv === false)
        return '<button type="button" class="btn btn-outline-primary" onclick="CreateReceive(\'' + row.PurchaseNo + '\');">產生</button>';
    else 
        return '<button type="button" class="btn btn-outline-primary" onclick="CreateReceive(\'' + row.PurchaseNo + '\');" disabled title="此單未結案或已產生收貨單!">產生</button>';
}

function PurchaseSearch() {
    var o = {};
    o.PurchaseNo = $("#txtPurchaseNo").textbox("getText");
    o.CreateDateTimeStart = $('#PurchaseDateStart').datebox('getValue');
    o.CreateDateTimeEnd = $('#PurchaseDateEnd').datebox('getValue');
  
    $('#dg').datagrid('load',o);
}



function openDetail(PurchaseNo) {

    $('#dlgDetail').dialog({
        closed: false,
        width: 1200,
        height: 500,
        iconCls: 'icon-list-m1-edit',
        title: '採購明細',
        href: '../FormCreate/ReceiveDetail?PurchaseNo=' + PurchaseNo
    });
}