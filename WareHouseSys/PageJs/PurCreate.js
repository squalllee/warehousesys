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
    return '<center><a href="#" onclick="openDetail(\'' + row.OrderNo + '\');">' + row.OrderNo + '</a ></center > ';
}

function CreateStyler(value, row, index) {
    if (row.Status === "已結案" && row.IsCreateRecv === false)
        return '<button type="button" class="btn btn-outline-primary" onclick="CreateReceive(\'' + row.PurchaseNo + '\');">產生</button>';
    else
        return '<button type="button" class="btn btn-outline-primary" onclick="CreateReceive(\'' + row.PurchaseNo + '\');" disabled title="此單未結案或已產生採購單!">產生</button>';
}

function RequireSearch() {
    var o = {};
    o.RequireNo = $("#txtRequireNo").textbox("getText");
    o.CreateDateTimeStart = $('#ApplicationDateStart').datebox('getValue');
    o.CreateDateTimeEnd = $('#ApplicationDateEnd').datebox('getValue');

    $('#dg').datagrid('load', o);
}



function openDetail(RequireNo) {

    $('#dlgDetail').dialog({
        closed: false,
        width: 1300,
        height: 500,
        iconCls: 'icon-list-m1-edit',
        title: '需求明細',
        href: '../FormCreate/PurDetail?RequireNo=' + RequireNo
    });
}


function CreatePur(RequireNo,obj) {
    $.ajax({
        type: 'GET',
        url: "../api/Requirement/CreatePur/" + RequireNo,
        beforeSend: ajaxLoading,
        success: function (result) {
            ajaxLoadEnd();
            $(obj).hide();
            alert('產生採購單成功!');
        },
        error: function (err) {
            ajaxLoadEnd();
        }
    });
}

function ajaxLoading() {
    $("#tt").mLoading({
        text: "採購單產生中"
    });
}

function ajaxLoadEnd() {
    $("#tt").mLoading('hide');
}

