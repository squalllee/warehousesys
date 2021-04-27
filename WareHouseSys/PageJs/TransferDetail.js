function TransferDetailInit () {
    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#TransferDetailGrid").data("url"),
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

    $("#TransferDetailGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        //height: $(document.body).height(),
        //width: $(document.body).width(),
        width: 1750,
        sortable: true,
        pageable: true,
        pageable: {
            pageSize: 10,
            pageSizes: true,
            pageSizes: [10, 100, 1000],
            buttonCount: 5,
            refresh: true,
            messages: {
                last: "最末頁",
                first: "第一頁",
                next: "下一頁",
                previous: "上一頁",
                morePages: "更多頁",
                itemsPerPage: "每頁筆數",
                display: "第 {0} - {1} 筆 共 {2} 筆記錄",
                refresh: "重新整理",
                empty: "沒有符合記錄"
            }
        },
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
                editor: EditSpan,
                width: 200
            }, {
                field: "OutWareHouseName",
                title: "轉出庫",
                width: 250
            }, {
                field: "OutStorageId",
                title: "轉出儲位",
                editor: EditSpan,
                width: 100

            }, {
                field: "Lot",
                title: "批號",
                width: 100

            }, {
                field: "InWareHouseName",
                title: "轉入庫",
                width: 200
            }, {
                field: "InStorageId",
                title: "轉入儲位",
                width: 100

            }, {
                field: "Quantity",
                title: "移撥量",
                width: 100
            }, {
                field: "TransferOutQty",
                title: "轉出量",
                width: 100
            },{
                field: "TransferInQty",
                title: "轉入量",
                width: 100
            }
            , {
                field: "Note",
                title: "備註",
                width: 150
            }
        ]
    }).data("kendoGrid");
}