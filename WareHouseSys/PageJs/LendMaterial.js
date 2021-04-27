var LendGrid;
$(document).ready(function () {
    LendGrid = $("#LendGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#LendGrid").data("url"),
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
                        InBoundDate: { type: "date" },
                        ExtendDate: { type: "date" },
                        OutBoundDate: { type: "date"}
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
                if (val.Status === "0") val.Status = "辦理中";
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
            title: "借料單號",
            width: 160,
            template: '<span style="cursor:pointer" onclick="openDetail(this);">#=OrderNo#</span>'
        }, {
            field: "LendMan",
            title: "借料人員",
            width: 200
        },
        {
            field: "OutBoundMan",
            title: "出庫人員",
            width: 150

        }, {
            field: "OutBoundDate",
            title: "出庫日期",
            template: '#= OutBoundDate !== null ? kendo.toString(OutBoundDate, "yyyy/MM/dd") : ""#',
            width: 150
        }, {
            field: "ExtendDate",
            title: "歸還日期",
            template: '#= ExtendDate !== null ? kendo.toString(ExtendDate, "yyyy/MM/dd") : ""#',
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
                $("<div class='btn - group dropleft'></div >").load("");
                html = '<div class="btn-group dropleft">' +
                    '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                    '動作' +
                    ' <span class="caret"></span>' +
                    '</button>' +
                    '<ul class="dropdown-menu" aria-labelledby="about-us">';
                if (container.Status === "辦理中") {

                    html += '<li><a  href="#" onclick="OpenLendMaterial(this)">借料</a></li>';

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

function OpenLendMaterial(e) {
    var row = e.closest("tr");
    var dataItem = LendGrid.dataItem(row);

    $("#ModifyLendDialog").kendoWindow({
        title: "借料作業",
        actions: ["Close"],
        content: $("#ModifyLendDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1350,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            ModifyLendInit();
        }
    }).data("kendoWindow").open();
}

function openDetail(e) {
    var row = e.closest("tr");
    var dataItem = LendGrid.dataItem(row);

    $("#LendDetailDialog").kendoWindow({
        title: "借料明細",
        actions: ["Close"],
        content: $("#LendDetailDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1500,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            LendDetailInit();
        }
    }).data("kendoWindow").open();
}