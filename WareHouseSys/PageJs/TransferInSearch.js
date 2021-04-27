var TransferInSearchGrid;
function dataSource_error(e) {
    alert('發生錯誤');
}

$(document).ready(function () {
    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#TransferInSearchGrid").data("url"),
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
            total: "Total",
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });
    datasource.bind("error", dataSource_error);

    TransferInSearchGrid = $("#TransferInSearchGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 500,
        sortable: true,
        pageable: true,
        columns: [
            {
                field: "OrderNo",
                title: "移撥單號",
                width: 100,
                template: '<span style="cursor:pointer" onclick="OpenDetail(this);">#=OrderNo#</span>'

            }, {
                field: "ApplicantMan",
                title: "申請人",
                width: 200
            }, {
                field: "TransferOutMan",
                title: "移出人員",
                width: 200
            }, {
                field: "TransferInMan",
                title: "移入人員",
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


                        html += '<li><a  href="#" onclick="TransferIn(this)">移入</a></li>';

                        return html + "</ul ></div >";
                    }

                    return "";
                }
            }
        ],
        editable: "inline"
    }).data("kendoGrid");


});

function TransferIn(e) {
    var row = e.closest("tr");
    var grid = $("#TransferInSearchGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("#TransferInDialog").kendoWindow({
        title: "移撥單資訊",
        actions: ["Close"],
        content: $("#TransferInDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1900,
        position: {
            top: "50px",
            left: "0px"
        },
        refresh: function (e) {
            TransferInDialogInit();
        }
    }).data("kendoWindow").open();
}

function OpenDetail(e) {
    var row = e.closest("tr");
    var grid = $("#TransferInSearchGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("#TransferDetailDialog").kendoWindow({
        title: "移撥單明細",
        actions: ["Close"],
        content: $("#TransferDetailDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1800,
        //width: $(document.body).width(),
        position: {
        //    top: "20px",
            top: "0px",
            left: "0px",
            //left: "15%"
        },
        refresh: function (e) {
            TransferDetailInit();
        }
    }).data("kendoWindow").open();
}