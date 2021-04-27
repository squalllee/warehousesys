$(document).ready(function () {
    $("#RecvDateStart").datebox({
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
    $("#RecvDateEnd").datebox({
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
    return '<center><a href="#" onclick="openDetail(\'' + row.OrderNo + '\');">' + row.OrderNo + '</a ></center > ';
}

function RecvSearch() {
    var o = {};
    o.RecvNo = $("#txtRecvNo").textbox("getText");
    o.RecvDateStart = $('#RecvDateStart').datebox('getValue');
    o.RecvDateEnd = $('#RecvDateEnd').datebox('getValue');

    $('#dg').datagrid('load', o);
}