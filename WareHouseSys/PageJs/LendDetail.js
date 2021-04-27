function LendDetailInit() {
    function dataSource_error(e) {
        alert('發生錯誤');
    }


    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#LendDetailGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                if (operation === "create") {
                    options.OrderNo = $("[name='OrderNo']").val();
                }
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
    datasource.bind("error", dataSource_error);


    $("#LendDetailGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
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
                field: "Quantity",
                title: "申請數量",
                width: 100
            },
            {
                field: "LendQty",
                title: "借出數量",
                width: 100
            }
        ]
    }).data("kendoGrid");
}