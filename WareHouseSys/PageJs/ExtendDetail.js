function ExtendDetailInit() {
    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#ExtendDetailGrid").data("url"),
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
    datasource.bind("error", dataSource_error);

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    doBackGrid = $("#ExtendDetailGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1050,
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
                field: "NotReturnQty",
                title: "未還量",
                width: 100
            }, {
                field: "ExtendQty",
                title: "展延數量",
                width: 100
            }
        ]
    }).data("kendoGrid");
    
}