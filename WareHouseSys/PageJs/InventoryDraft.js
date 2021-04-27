var InventorySearchGrid;
$(document).ready(function () {
    $("#InventoryAdd").click(function (e) {
        $("#InventoryAddDialog").kendoWindow({
            title: "新增盤點單",
            actions: ["Close"],
            content: $("#InventoryAddDialog").data("url"),
            visible: false,
            modal: true,
            position: {
                top: "50px",
                left: "0px"
            },
            width: 1900,
            refresh: function (e) {
                InventoryAddInit();
            }
        }).data("kendoWindow").open();
    });

    InventorySearchGrid = $("#InventorySearchGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#InventorySearchGrid").data("url"),
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
                        InventoryDate: { type: "date" }
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
            title: "盤點單號",
            width: 160,
            template: '<span style="cursor:pointer" onclick="openDetail(this);">#=OrderNo#</span>'
        }, {
            field: "InventoryUnit",
            title: "盤點單位",
            width: 160
        }, {
                field: "Period",
            title: "盤點週期",
            width: 200
        },
        {
            field: "InventoryWarHouse",
            title: "盤點庫別",
            width: 150
        }, {
            field: "InventoryDate",
            title: "盤點日期",
            template: '#= InventoryDate !== null ? kendo.toString(InventoryDate, "yyyy/MM/dd") : ""#',
            width: 150
        }, {
            field: "InventoryAttr",
            title: "盤點屬性",
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

                    html += '<li><a  href="#" onclick="Print(this)">列印</a></li><li><li><a  href="#" onclick="FirstCheck(this)">初盤</a></li><li><a  href="#" onclick="DeleteInventory(this)">作廢</a></li>';

                }
                else if (container.Status === "完成初盤") {
                    
                    html += '<li><a  href="#" onclick="SecondCheck(this)">複盤</a></li><li><a  href="#" onclick="Print(this)">列印</a></li><li><a  href="#" onclick="InventoryRecord(this)">盤點記錄表</a></li><li><a  href="#" onclick="InventoryClose(this)">結案</a></li>';

                }              
                else if (container.Status === "結案") {
                    html +=  '<li><a  href="#" onclick="InventoryRecord(this)">盤點記錄表</a></li>';
                }
                return html + "</ul ></div >";

            }
        }
        ]
    }).data("kendoGrid");
});

function Print(e) {
    var row = e.closest("tr");
    var dataItem = InventorySearchGrid.dataItem(row);
    window.open("../Report/Inventory/Inventory.aspx?OrderNo=" + dataItem.OrderNo);
}

function DeleteInventory(e) {
    var row = e.closest("tr");
    var dataItem = InventorySearchGrid.dataItem(row);

    if (confirm("是否確定要作廢？")) {
        $.ajax({
            url: $("#InventorySearchGrid").data("deleteurl") + "/" + dataItem.OrderNo,
            dataType: 'text',
            type: 'get',
            contentType: 'application/json',
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('作廢成功');

                $("#InventorySearchGrid").data("kendoGrid").dataSource.read();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('作廢失敗');
            }
        });
    }
}

function openDetail(e) {
    var row = e.closest("tr");
    var dataItem = InventorySearchGrid.dataItem(row);

    $("#InventoryDetailDialog").kendoWindow({
        title: "盤點明細",
        actions: ["Close"],
        content: $("#InventoryDetailDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1350,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
           InventoryDetailInit();
        }
    }).data("kendoWindow").open();
}

function FirstCheck(e) {
    var row = e.closest("tr");
    var dataItem = InventorySearchGrid.dataItem(row);

    $("#FirstCheckDialog").kendoWindow({
        title: "初盤作業",
        actions: ["Close"],
        content: $("#FirstCheckDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1350,
        position: {
            top: "50px",
            left: "15%"
        },
        close: function (e) {
            AttachmentArray = [];
            $('#imgList').empty();
        },
        refresh: function (e) {
            FirstCheckInit();
        }
    }).data("kendoWindow").open();
}

function SecondCheck(e) {
    var row = e.closest("tr");
    var dataItem = InventorySearchGrid.dataItem(row);

    $("#SecondCheckDialog").kendoWindow({
        title: "複盤作業",
        actions: ["Close"],
        content: $("#SecondCheckDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1350,
        position: {
            top: "50px",
            left: "15%"
        },
        close: function (e) {
            AttachmentArray = [];
            $('#imgList').empty();
        },
        refresh: function (e) {
            SecondCheckInit();
        }
    }).data("kendoWindow").open();
}

function InventoryClose(e) {
    var row = e.closest("tr");
    var dataItem = InventorySearchGrid.dataItem(row);

    $("#InventoryCloseDialog").kendoWindow({
        title: "結案作業",
        actions: ["Close"],
        content: $("#InventoryCloseDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1350,
        position: {
            top: "50px",
            left: "15%"
        },
        close: function (e) {
            AttachmentArray = [];
            $('#imgList').empty();
        },
        refresh: function (e) {
            InventoryCloseInit();
        }
    }).data("kendoWindow").open();
}

function InventoryRecord(e) {
    var row = e.closest("tr");
    var dataItem = InventorySearchGrid.dataItem(row);

    $("#InventoryRecordDialog").kendoWindow({
        title: "盤點記錄表",
        actions: ["Close"],
        content: $("#InventoryRecordDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1350,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            InventoryRecordInit();
        }
    }).data("kendoWindow").open();
}
