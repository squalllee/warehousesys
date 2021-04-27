var ExtendSearchGrid;
$(document).ready(function () {
    ExtendSearchGrid = $("#ExtendSearchGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#ExtendSearchGrid").data("url"),
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
                        ExtendDate: { type: "date" },
                        AddDateTime: { type: "date" }
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
                field: "AddDateTime",
                title: "申請日期",
                template: '#=kendo.toString(AddDateTime, "yyyy/MM/dd")#',
                width: 200
            }, {
                field: "Days",
                title: "展延天數",
                width: 200
            }, {
                field: "ExtendDate",
                title: "展延期限",
                template: '#= ExtendDate !== null ? kendo.toString(ExtendDate, "yyyy/MM/dd") : ""#',
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

                    html += '<li><a  href="#" onclick="Update(this)">修改</a></li><li><a  href="#" onclick="Print(this)">列印</a></li><li><a  href="#" onclick="DeleteExtend(this)">作廢</a></li>';

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

function DeleteExtend(e) {
    var row = e.closest("tr");
    var dataItem = ExtendSearchGrid.dataItem(row);

    if (confirm("是否確定要作廢？")) {
        $.ajax({
            url: $("#ExtendSearchGrid").data("deleteurl") + "/" + dataItem.OrderNo,
            dataType: 'text',
            type: 'get',
            contentType: 'application/json',
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('作廢成功');

                $("#ExtendSearchGrid").data("kendoGrid").dataSource.read();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('作廢失敗');
            }
        });
    }

}

function Print(e) {
    var row = e.closest("tr");
    var dataItem = ExtendSearchGrid.dataItem(row);
    window.open("../Report/Extend/Extend.aspx?OrderNo=" + dataItem.OrderNo);
}

function Update(e) {
    var row = e.closest("tr");
    var dataItem = ExtendSearchGrid.dataItem(row);

    ExtendUpdateDialog = $("<div></div>").kendoWindow({
        title: "展延修改",
        actions: ["Close"],
        content: "../Extend/ExtendUpdate?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1100,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            ExtendUpdateInit();
        },
        close: function (e) {
            this.destroy();
        }
    }).data("kendoWindow").open();
}

function openDetail(e) {
    var row = e.closest("tr");
    var dataItem = ExtendSearchGrid.dataItem(row);

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
