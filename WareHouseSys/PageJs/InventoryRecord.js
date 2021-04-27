function PrintInventoryRecord(OrderNo) {
    window.open("../Report/Inventory/InventoryRecord.aspx?OrderNo=" + OrderNo);
}

function InventoryRecordInit() {
   
    datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#InventoryRecordGrid").data("url"),
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
            model: {
                id: "MaterialNo"
            }
        },
        serverPaging: true,
        pageSize: 10000,
        serverSorting: true,
        serverFiltering: false
    });
    datasource.bind("error", dataSource_error);

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    $("#InventoryRecordGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1300,
        sortable: true,
        filterable: {
            extra: false,
            operators: {
                string: {
                    eq: "等於",
                    neq: "不等於"
                }
            }
        },
        columns: [
            {
                field: "MaterialNo",
                title: "品號",
                width: 200,
                locked: true,
                lockable: true

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
            },
            {
                field: "Quantity",
                title: "庫存",
                width: 100
            },
            {
                field: "SecondCheckQty",
                title: "盤點量",
                width: 100,
                template: "<span class='#if(SecondCheckQty !== Quantity) {#diff#}#'>#=SecondCheckQty#</span>"
            },
            {
                field: "diffQty",
                title: "盤差",
                width: 100,
                template: "<span class='#if(Quantity - SecondCheckQty !== 0) {#diff#}#'>#=diffQty#</span>"
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