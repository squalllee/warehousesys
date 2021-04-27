var ReturnGrid;
$(document).ready(function () {
    ReturnGrid = $("#ReturnGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#ReturnGrid").data("url"),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST"
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: {
                data: "data",
                total: "Total",
                model: {
                    fields: {
                        InBoundDate
                            : { type: "date" }
                    }
                }
            },
            serverPaging: true,
            pageSize: 10,
            serverSorting: true,
            serverFiltering: true,
            pageable: true
        },
        height: 650,
        sortable: true,
        pageable: true,
        dataBinding: function (e) {
            this.dataSource.data().forEach(function (val, index) {
                val.EmergencyPicking = val.EmergencyPicking ? "是" : "否";
            });
        },
        filterable: {
            extra: false,
            operators: {
                string: {
                    eq: "等於",
                    neq: "不等於"
                }
            }
        },
        columns: [{
            field: "OrderNo",
            title: "退料單號",
            width: 160,
            template: '<span style="cursor:pointer" onclick="openDetail(this);">#=OrderNo#</span>'
        }, {
            field: "PickingNo",
            title: "領料單號",
            width: 160
        }, {
            field: "ReturnMan",
            title: "退料人員",
            width: 200
            },
            {
            field: "InBoundMan",
            title: "入庫人員",
            width: 150

        }, {
            field: "InBoundDate",
            title: "入庫日期",
            template: '#= InBoundDate !== null ? kendo.toString(InBoundDate, "yyyy/MM/dd") : ""#',
            width: 150
        },
        {
            field: "Status",
            title: "狀態",
            width: 80
        },
        {
            title: "動作",
            width: 150,
            //template: "<div class='btn-group dropleft action' data-url='../test/Index'></div >"
            template: function (container) {
                $("<div class='btn - group dropleft'></div >").load("");
                html = '<div class="btn-group dropleft">' +
                    '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                    '動作' +
                    ' <span class="caret"></span>' +
                    '</button>' +
                    '<ul class="dropdown-menu" aria-labelledby="about-us">';
                if (container.Status === "辦理中") {

                    html += '<li><a  href="#" onclick="OpenModifyReturnDialog(this)">修改</a></li><li><a  href="#" onclick="DeleteReturn(this)">作廢</a></li><li><a  href="#" onclick="Print(this)">列印</a></li>';

                }

                if (container.Status === "結案") {
                    return "";
                }
                return html + "</ul ></div >";

            }
        }
        ]
    }).data("kendoGrid");
});

function OpenModifyReturnDialog(e) {
    var row = e.closest("tr");
    var grid = $("#ReturnGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    ModifyReturnaDialog = $("#ModifyReturnaDialog").kendoWindow({
        title: "退料修改",
        actions: ["Close"],
        content: $("#ModifyReturnaDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1600,
        position: {
            top: "100px", left: "15%"
        },
        refresh: function (e) {
            ModifyReturnInit();
        }
    }).data("kendoWindow").center().open();
}

function Print(e) {
    var row = e.closest("tr");
    var grid = $("#ReturnGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    window.open("../Report/Return/Return.aspx?OrderNo=" + dataItem.OrderNo);
}

function DeleteReturn(e) {
    var row = e.closest("tr");
    var grid = $("#ReturnGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);
    $.ajax({
        url: $("#ReturnGrid").data("deleteurl") + "/" + dataItem.OrderNo,
        dataType: 'text',
        type: 'get',
        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            alert('作廢成功');
            $("#ReturnGrid").data("kendoGrid").dataSource.read();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert('作廢失敗');
        }
    });
}

function openDetail(e) {
    var row = e.closest("tr");
    var grid = $("#ReturnGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("#ReturnDetailDialog").kendoWindow({
        title: "退料明細",
        actions: ["Close"],
        content: $("#ReturnDetailDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1500,
        position: {
            top: "100px", left: "15%"
        },
        refresh: function (e) {
            ReturnDetailInit();
        }
    }).data("kendoWindow").open();
}