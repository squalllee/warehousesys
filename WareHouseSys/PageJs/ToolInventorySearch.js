var ToolInventorySearchGrid;
$(document).ready(function () {
    $("#ToolInventoryAdd").click(function (e) {
        $("#ToolInventoryAddDialog").kendoWindow({
            title: "新增手工具盤點單",
            actions: ["Close"],
            content: $("#ToolInventoryAddDialog").data("url"),
            visible: false,
            modal: true,
            position: {
                top: "50px",
                left: "5%"
            },
            width: 1200,
            refresh: function (e) {
                ToolInventoryAddInit();
            }
        }).data("kendoWindow").open();
    });

    ToolInventorySearchGrid = $("#ToolInventorySearchGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#ToolInventorySearchGrid").data("url"),
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
                field: "KeepUnit",
            title: "保管單位",
            width: 160
        }, {
                field: "ToolMgr",
            title: "手工具管理人",
            width: 200
        },
        {
            field: "InventoryAttr",
            title: "盤點屬性",
            width: 150
        }, {
            field: "InventoryDate",
            title: "盤點日期",
            template: '#= InventoryDate !== null ? kendo.toString(InventoryDate, "yyyy/MM/dd") : ""#',
            width: 150
        }, {
            field: "Period",
            title: "盤點週期",
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

                    html += '<li><a  href="#" onclick="Print(this)">列印</a></li><li><li><a  href="#" onclick="FirstCheck(this)">初盤</a></li><li><a  href="#" onclick="DeleteToolInventory(this)">作廢</a></li>';

                }
                else if (container.Status === "完成初盤") {
                    
                    html += '<li><a  href="#" onclick="SecondCheck(this)">複盤</a></li><li><a  href="#" onclick="Print(this)">列印</a></li><li><a  href="#" onclick="ToolInventoryRecord(this)">盤點記錄表</a></li><li><a  href="#" onclick="InventoryClose(this)">結案</a></li>';

                }
                else if (container.Status === "結案") {
                    html +=  '<li><a href="#" onclick="ToolInventoryRecord(this)">盤點記錄表</a></li>';
                }
                return html + "</ul ></div >";

            }
        }
        ]
    }).data("kendoGrid");
});

function Print(e) {
    var row = e.closest("tr");
    var dataItem = ToolInventorySearchGrid.dataItem(row);
    window.open("../Report/Inventory/ToolInventory.aspx?OrderNo=" + dataItem.OrderNo);
}

function FirstCheck(e) {
    var row = e.closest("tr");
    var dataItem = ToolInventorySearchGrid.dataItem(row);

    $("#FirstCheckDialog").kendoWindow({
        title: "初盤作業",
        actions: ["Close"],
        content: $("#FirstCheckDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1000,
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

function DeleteToolInventory(e) {
    var row = e.closest("tr");
    var dataItem = ToolInventorySearchGrid.dataItem(row);

    if (confirm("是否確定要作廢？")) {
        $.ajax({
            url: $("#ToolInventorySearchGrid").data("deleteurl") + "/" + dataItem.OrderNo,
            dataType: 'text',
            type: 'get',
            contentType: 'application/json',
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('作廢成功');

                $("#ToolInventorySearchGrid").data("kendoGrid").dataSource.read();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('作廢失敗');
            }
        });
    }
}

function SecondCheck(e) {
    var row = e.closest("tr");
    var dataItem = ToolInventorySearchGrid.dataItem(row);

    $("#SecondCheckDialog").kendoWindow({
        title: "初盤作業",
        actions: ["Close"],
        content: $("#SecondCheckDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1000,
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
    var dataItem = ToolInventorySearchGrid.dataItem(row);

    $("#InventoryCloseDialog").kendoWindow({
        title: "結案作業",
        actions: ["Close"],
        content: $("#InventoryCloseDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1000,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            InventoryCloseInit();
        }
    }).data("kendoWindow").open();
}

function openDetail(e) {
    var row = e.closest("tr");
    var dataItem = ToolInventorySearchGrid.dataItem(row);

    $("#InventoryDetailDialog").kendoWindow({
        title: "盤點明細",
        actions: ["Close"],
        content: $("#InventoryDetailDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1000,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            InventoryDetailInit();
        }
    }).data("kendoWindow").open();
}

function ToolInventoryRecord(e) {
    var row = e.closest("tr");
    var dataItem = ToolInventorySearchGrid.dataItem(row);

    $("#InventoryRecordDialog").kendoWindow({
        title: "手工具盤點記錄表",
        actions: ["Close"],
        content: $("#InventoryRecordDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1000,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            InventoryRecordInit();
        }
    }).data("kendoWindow").open();
}