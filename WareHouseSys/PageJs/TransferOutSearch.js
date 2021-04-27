var TransferOutSearchGrid;
function dataSource_error(e) {
    alert('發生錯誤');
}

$(document).ready(function () {
    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#TransferOutSearchGrid").data("url"),
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
        serverFiltering: false,
        pageable: true
    });
    datasource.bind("error", dataSource_error);

    TransferOutSearchGrid = $("#TransferOutSearchGrid").kendoGrid({
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
            }, {
                field: "TransferOutMan",
                title: "移出人員",
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


                        html += '<li><a  href="#" onclick="TransferOut(this)">移出</a></li>';

                        return html + "</ul ></div >";
                    }

                    return "";
                }
            }
        ],
        editable: "inline"
    }).data("kendoGrid");


});

function TransferOut(e) {
    var row = e.closest("tr");
    var grid = $("#TransferOutSearchGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("#TransferOutDialog").kendoWindow({
        title: "移出作業",
        actions: ["Close"],
        content: $("#TransferOutDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1000,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            //alert($("#TransferOutForm").data("saveurl"));
            TransferOutDialogInit();
        }
    }).data("kendoWindow").open();
}