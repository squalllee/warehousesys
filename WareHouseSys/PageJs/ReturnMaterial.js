var ReturnMaterialGrid;
$(document).ready(function () {
    ReturnMaterialGrid = $("#ReturnMaterialGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#ReturnMaterialGrid").data("url"),
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
        //width: 1450,
        sortable: true,
        pageable: true,
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
            template: function (container) {
                if (container.Status === "結案") {
                    return "";
                }

                html = '<div class="btn-group dropleft">' +
                    '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                    '動作' +
                    ' <span class="caret"></span>' +
                    '</button>' +
                    '<ul class="dropdown-menu" aria-labelledby="about-us">';
                if (container.Status === "辦理中") {
                    html += '<li><a  href="#" onclick="OpenReturn(this)">退料</a></li></li>';
                }

                return html + "</ul ></div >";
            }
        }
        ]
    }).data("kendoGrid");
});

function OpenReturn(e) {
    var row = e.closest("tr");
    var grid = $("#ReturnMaterialGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("#doReturnDialog").kendoWindow({
        title: "退料作業",
        actions: ["Close"],
        content: $("#doReturnDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1700,
        position: {
            top: "100px", left: "5%"
        },
        refresh: function (e) {
            ModifyReturnInit();
        }
    }).data("kendoWindow").open();
}

function openDetail(e) {
    var row = e.closest("tr");
    var grid = $("#ReturnMaterialGrid").data("kendoGrid");
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