function BackDetailInit() {
    var datasource = new kendo.data.DataSource({
        offlineStorage: "offlineStorage",
        transport: {
            read: {
                url: $("#BackDetailGrid").data("url"),
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
            model: {
                id: "MaterialNo"
            }
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });


    doBackGrid = $("#BackDetailGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 500,
        width: 1450,
        sortable: true,
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
                width: 200
            }, {
                field: "WareHouseName",
                title: "庫別",
                width: 200
            }, {
                field: "StorageId",
                title: "儲位",
                width: 100
            }, {
                field: "Lot",
                title: "批號",
                width: 100
            },
            {
                field: "LendQty",
                title: "借出數量",
                width: 100
            },
            {
                field: "Quantity",
                title: "歸還數量",
                width: 100
            },
            {
                field: "BackQty",
                title: "已還量",
                width: 100
            }
            ,
            {
                field: "Note",
                title: "備註",
                width: 100
            }
        ]
    }).data("kendoGrid");
}