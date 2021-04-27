var ExtendMaterialGrid;
$(document).ready(function () {
    ExtendMaterialGrid = $("#ExtendMaterialGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#ExtendMaterialGrid").data("url"),
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
                        ExtendDate: { type: "date" }
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
        pageable: {
            pageSize: 10,
            pageSizes: true,
            pageSizes: [10, 100, 1000],
            buttonCount: 5,
            refresh: true,
            messages: {
                last: "最末頁",
                first: "第一頁",
                next: "下一頁",
                previous: "上一頁",
                morePages: "更多頁",
                itemsPerPage: "每頁筆數",
                display: "第 {0} - {1} 筆 共 {2} 筆記錄",
                refresh: "重新整理",
                empty: "沒有符合記錄"
            }
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
            title: "展延單號",
            width: 160,
            template: '<span style="cursor:pointer" onclick="openDetail(this);">#=OrderNo#</span>'
        }, {
            field: "LendNo",
            title: "借料單號",
            width: 160
        }, {
            field: "ExtendMan",
            title: "展延人員",
            width: 200
        }, {
            field: "ExtendDate",
            title: "展延日期",
            template: '#= ExtendDate !== null ? kendo.toString(ExtendDate, "yyyy/MM/dd") : ""#',
            width: 200
        }, {
            field: "Days",
            title: "展延天數",
            width: 200
        },
        {
            field: "ApprovedMan",
            title: "核可人員",
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
                html = '<div class="btn-group dropleft">' +
                    '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                    '動作' +
                    ' <span class="caret"></span>' +
                    '</button>' +
                    '<ul class="dropdown-menu" aria-labelledby="about-us">';
                if (container.Status === "辦理中") {

                    html += '<li><a  href="#" onclick="doExtend(this)">展延</a></li>';

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


function openDetail(e) {
    var row = e.closest("tr");
    var dataItem = ExtendMaterialGrid.dataItem(row);

    $("#ExtendDetailDialog").kendoWindow({
        title: "展延明細",
        actions: ["Close"],
        content: $("#ExtendDetailDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1100,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            ExtendDetailInit();
        }
    }).data("kendoWindow").open();
}

function doExtend(e) {
    var row = e.closest("tr");
    var dataItem = ExtendMaterialGrid.dataItem(row);

    $("#doExtendDialog").kendoWindow({
        title: "展延明細",
        actions: ["Close"],
        content: $("#doExtendDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1100,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            doExtendInit();
        }
    }).data("kendoWindow").open();
}

