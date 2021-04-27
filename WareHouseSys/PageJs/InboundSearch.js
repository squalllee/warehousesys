var InboundGrid;
$(document).ready(function () {

    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $("#InboundAddBtn").click(function (e) {
        InboundAddDialog = $("<div></div>").kendoWindow({
            title: "新增入庫單",
            actions: ["Close"],
            content: "../Inbound/InboundAdd",
            visible: false,
            modal: true,
            width: 1800,
            height: 850,
            position: {
                top: "50px",
                left: "5%"
            },
            refresh: function (e) {
                InboundDialogInit();
            },
            close: function (e) {
                localStorage.clear();
                InboundAddDialog.destroy();
            }
        }).data("kendoWindow").open();
    });

    InboundGrid = $("#InboundGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#InboundGrid").data("url"),
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
                        OrderNo: { type: "string" },
                        PurNo: { type: "string" },
                        InboundMan: { type: "string" },
                        InboundDate: { type: "date" },
                        Status: { type: "string" }
                    }
                }
            },
            //type: "json",
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
                    //startswith: "比對條件",
                    eq: "等於",
                    neq: "不等於"
                }
            }
        },
       
        columns: [{
            field: "OrderNo",
            title: "入庫單號",
            width: 160,
            template: '<span style="cursor:pointer" onclick="openDetail(this);">#=OrderNo#</span>'
        }, {
            field: "PurNo",
            title: "採購單號",
            width: 160
        }, {
            field: "InboundMan",
            title: "入庫人員",
            width: 200
        }, {
            field: "InboundDate",
            title: "入庫日期",
            template: '#= kendo.toString(InboundDate,"yyyy/MM/dd") #'
        }, {
            field: "Status",
            title: "狀態"
        },
        {
            title: "動作",
            template: function (container) {
                html = '<div class="btn-group dropleft">' +
                    '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                    '動作' +
                    ' <span class="caret"></span>' +
                    '</button>' +
                    '<ul class="dropdown-menu" aria-labelledby="about-us">';
                    
                if (container.Status === "辦理中")
                {
                    html += '<li><a  href="#" onclick="OpenFillInboundData(this)">入庫</a></li>' +
                        '<li><a  href="#" onclick="PrintInbound(\'' + container.OrderNo + '\')">列印</a></li>' +
                        '<li><a  href="#" onclick="InboundClose(this)">結案</a></li>';
                }
                else if (container.Status === "結案") {
                    html += '<li><a  href="#" onclick="PrintInbound(\'' + container.OrderNo + '\')">列印</a></li>';
                      
                }
               
                return html + "</ul ></div >";
            }
        }
        ]
    });
    
});

function PrintInbound(OrderNo) {
    window.open("../Report/Inbound/Inbound.aspx?OrderNo=" + OrderNo);
}
