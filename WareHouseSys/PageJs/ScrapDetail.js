function ScrapDetailInit() {
    var ScrapDetailDatasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "../Scrap/ScrapBodyViewGrid?OrderNo=" + $("[name='OrderNo']").val(),
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

    ScrapDetailGrid = $("#ScrapDetailGrid").kendoGrid({
        dataSource: ScrapDetailDatasource,
        resizable: true,
        height: 400,
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
                width: 200
            },
            {
                field: "MaterialClass",
                title: "材質分類",
                width: 100
            },
            {
                field: "Quantity",
                title: "報廢量",
                width: 100
            }
        ]
    }).data("kendoGrid");
}