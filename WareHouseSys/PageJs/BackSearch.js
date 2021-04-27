var BackSearchGrid;
$(document).ready(function () {
    BackSearchGrid = $("#BackSearchGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#BackSearchGrid").data("url"),
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
                        OutBoundDate: { type: "date" },
                        Overdue: {type:"bool"}
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
            title: "還料單號",
            width: 160,
            template: '<span style="cursor:pointer" onclick="openDetail(this);">#=OrderNo#</span>'
        }, {
                field: "LendNo",
                title: "借料單號",
                width: 160
            }, {
            field: "BackMan",
            title: "還料人員",
            width: 200
        },
        {
            field: "InBoundMan",
            title: "入庫人員",
            width: 150
        }, {
            field: "InBoundDate",
            title: "歸還日期",
            template: '#= InBoundDate !== null ? kendo.toString(InBoundDate, "yyyy/MM/dd") : ""#',
            width: 150
            }, {
                field: "Overdue",
                title: "逾期歸還",
            template: '#= Overdue === true ? "是" : "否"#',
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

                    html += '<li><a  href="#" onclick="Print(this)">列印</a></li><li><a  href="#" onclick="DeleteBack(this)">作廢</a></li>';

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

function DeleteBack(e) {
    var row = e.closest("tr");
    var dataItem = BackSearchGrid.dataItem(row);

    if (confirm("是否確定要作廢？")) {
        $.ajax({
            url: $("#BackSearchGrid").data("deleteurl") + "/" + dataItem.OrderNo,
            dataType: 'text',
            type: 'get',
            contentType: 'application/json',
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('作廢成功');

                $("#BackSearchGrid").data("kendoGrid").dataSource.read();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('作廢失敗');
            }
        });
    }
    
}

function Print(e) {
    var row = e.closest("tr");
    var dataItem = BackSearchGrid.dataItem(row);
    window.open("../Report/Back/Back.aspx?OrderNo=" + dataItem.OrderNo);
}

function openDetail(e) {
    var row = e.closest("tr");
    var dataItem = BackSearchGrid.dataItem(row);

    $("#BackDetailDialog").kendoWindow({
        title: "還料明細",
        actions: ["Close"],
        content: $("#BackDetailDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1500,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            BackDetailInit();
        }
    }).data("kendoWindow").open();
}
