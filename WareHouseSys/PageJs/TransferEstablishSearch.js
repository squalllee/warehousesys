var TransferEstablishGrid;
function dataSource_error(e) {
    alert('發生錯誤');
}

$(document).ready(function () {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $("#TransferEstablishAdd").click(function (e) {
        $("#TransferEstablishAddDialog").kendoWindow({
            title: "新增移撥單",
            actions: ["Close"],
            content: $("#TransferEstablishAddDialog").data("url")+ "?OrderNo=999",
            visible: false,
            modal: true,
            position: {
                top: "50px",
                left: "5%"
            },
            width: 1750,
            refresh: function (e) {
                TransferEstablishAddDialogInit();
            }
        }).data("kendoWindow").open();
    });


    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#TransferEstablishGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                return JSON.stringify(options);
            }
        },
        schema: {
            data: "data",
            total: "Total"
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false
    });
    datasource.bind("error", dataSource_error);

    TransferEstablishGrid = $("#TransferEstablishGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 500,
        sortable: true,
        pageable: true,
        columns: [          
            {
                field: "OrderNo",
                title: "移撥單號",
                width: 100

            }, {
                field: "ApplicantMan",
                title: "申請人",
                width: 200
            },
            {
                field: "Status",
                title: "狀態",
                width: 100
            },
            {
                title: "動作",
                width: 100,
                template: function (container) {
                    if (container.Status === "辦理中") {
                        html = '<div class="btn-group dropleft">' +
                            '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                            '動作' +
                            ' <span class="caret"></span>' +
                            '</button>' +
                            '<ul class="dropdown-menu" aria-labelledby="about-us">';


                        html += '<li><a  href="#" onclick="TransferModifiy(this)">俢改</a></li><li><a  href="#" onclick="PrintTransfer(this)">列印</a><li><a  href="#" onclick="scrap(this)">作廢</a></li>';

                        return html + "</ul ></div >";
                    }

                    return "";
                }
            }
        ],
        editable: "inline"
    }).data("kendoGrid");

   
});

function PrintTransfer(e) {
    var row = e.closest("tr");
    var grid = $("#TransferEstablishGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    window.open("../Report/Transfer/Transfer.aspx?OrderNo=" + dataItem.OrderNo);
}

function TransferModifiy(e) {
    var row = e.closest("tr");
    var grid = $("#TransferEstablishGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("#TransferEstablishAddDialog").kendoWindow({
        title: "移撥單修改",
        actions: ["Close"],
        content: $("#TransferEstablishAddDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1750,
        position: {
            top: "50px",
            left: "5%"
        },
        refresh: function (e) {
            TransferEstablishAddDialogInit();
        }
    }).data("kendoWindow").open();
}

function scrap(e) {
    var row = e.closest("tr");
    var grid = $("#TransferEstablishGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    if (confirm("是否確定要作廢?")) {
        $.ajax({
            url: "../api/Transfer/deleteTransfer/" + dataItem.OrderNo,
            dataType: 'text',
            type: 'get',
            contentType: 'application/json',
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('作廢成功');
                TransferEstablishGrid.dataSource.read();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('作廢失敗');
            }
        });
    }
}