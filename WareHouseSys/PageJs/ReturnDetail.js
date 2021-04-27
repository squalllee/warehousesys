function ReturnDetailInit() {
    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#ReturnDetailGrid").data("url"),
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
                id: "SerialNo",
                fields: {
                    SerialNo: { type: "string", editable: false },
                    MaterialNo: { type: "string", editable: false },
                    MaterialName: { type: "string", editable: false },
                    Spec: { type: "string", editable: false },
                    WareHouseName: { type: "string", editable: false },
                    StorageId: { type: "string", editable: false },
                    Lot: { type: "string", editable: false },
                    Note: { type: "string" },
                    Quantity: { type: "number", editable: false },
                    ReturnQty: { type: "number", validation: { min: 0 } }
                }
            }
        },
        serverPaging: true,
        pageSize: 10,
        serverSorting: true,
        serverFiltering: true,
        pageable: true
    });

    $("#ReturnDetailGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1450,
        sortable: true,
        columns: [
            {
                field: "SerialNo",
                title: "序號",
                width: 50

            }, {
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
                width: 150
            }, {
                field: "Lot",
                title: "批號",
                width: 100

            },
            {
                field: "Quantity",
                title: "退料量",
                width: 100
            }, {
                field: "ReturnQty",
                title: "收料量",
                width: 100
            }, {
                field: "Note",
                title: "備註",
                width: 150
            }
        ]
    }).data("kendoGrid");
}