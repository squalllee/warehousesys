function AdjustDetailInit() {
    var AdJustDetailDatasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "../Adjust/AdjustBodyViewGrid?OrderNo=" + $("[name='OrderNo']").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
              
                return JSON.stringify(options);
            }
        },
        schema: {
            data: "data"
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });


    AdjustDetailGrid = $("#AdjustDetailGrid").kendoGrid({
        dataSource: AdJustDetailDatasource,
        resizable: true,
        height: 600,
        width: 1850,
        sortable: true,
        scrollable: true,
        columns: [
            {
                field: "MaterialNo",
                title: "品號",
                width: 200

            }, {
                field: "MaterialName",
                title: "品名",
                width: 200
            },
            {
                field: "Spec",
                title: "規格",
                width: 250
            }, {
                field: "Unit",
                title: "單位",
                width: 200
            }, {
                field: "WareHouseName",
                title: "倉庫",
                width: 200
            }, {
                field: "StorageId",
                title: "儲位",
                width: 200
            }, {
                field: "Lot",
                title: "批號",
                width: 200
            }, {
                field: "StockQty",
                title: "庫存量",
                width: 100
            }, {
                field: "Quantity",
                title: "實際數量",
                width: 100
            }, {
                field: "Reason",
                title: "調整原因",
                width: 300
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}

function openDetail(e) {
    var row = e.closest("tr");
    var grid = $("#grid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("<div></div>").kendoWindow({
        title: "調整明細",
        actions: ["Close"],
        content: "../Adjust/AdjustDetail?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1900,
        position: {
            top: "50px",
            left: "0px"
        },
        refresh: function (e) {
            AdjustDetailInit();
        },
        close: function (e) {
            this.destroy();
        }
    }).data("kendoWindow").open();
}