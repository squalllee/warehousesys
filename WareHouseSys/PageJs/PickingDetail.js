function PickingDetailInit(e) {
    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#PickingDetailGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            }
        },
        schema: {
            data: "data"
        }
    });

    $("#PickingDetailGrid").kendoGrid({
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

            }, {
                field: "Quantity",
                title: "未領量",
                width: 100

            }
            , {
                field: "PickedQty",
                title: "已領量",
                width: 100
            }, {
                field: "Note",
                title: "備註",
                width: 150
            }
        ]
    }).data("kendoGrid");
}