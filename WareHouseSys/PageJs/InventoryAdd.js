var InventorySrcGrid;
var InventoryDesGrid;
var datasource;
function InventoryAddInit() {
   
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $("#BtnSaveInventory").click(function () {
        var validator = $("#InventoryAddForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var obj = {};
            obj.StockInventoryHeaderViewModel = formToJSON($("#InventoryAddForm"));

            var items = InventoryDesGrid.dataSource.view();

            if (items.length === 0) {
                alert('盤點單不得為空!');
                return;
            }

            $(items).each(function (index, item) {
                item.Quantity = item.Qty;
            });

            obj.stockInventoryBodyViewModels = items;

            var url = $("#InventoryAddForm").data("saveurl");

            $.ajax({
                url: url,
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    $("#InventoryAddDialog").data("kendoWindow").close();
                    InventorySearchGrid.dataSource.read();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#InventorySrcGrid").data("url"),
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

    InventorySrcGrid = $("#InventorySrcGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 780,
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
            { selectable: true, width: "50px" },
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
            },
            {
                field: "Qty",
                title: "庫存",
                width: 100
            }
        ]
    }).data("kendoGrid");

    InventoryDesGrid = $("#InventoryDesGrid").kendoGrid({
        resizable: true,
        height: 600,
        width: 850,
        sortable: true,
        columns: [
            { selectable: true, width: "50px" },
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
            },
            {
                field: "Qty",
                title: "庫存",
                width: 100
            }
            
        ]
    }).data("kendoGrid");
}

function onWareHouseChange(e) {
    removeAll();
    WareHouseId = this.dataItem(e.item.index()).WarehouseId;
    $("[name='WareHouseMgr'").val(this.dataItem(e.item.index()).KEYNO);
    $("[name='InventoryWarHouse'").val(WareHouseId);
    $("[name='WGroupId'").val(this.dataItem(e.item.index()).WGroupId);
    
    datasource.transport.options.read.url = $("#InventorySrcGrid").data("url") + "?WareHouseId=" + WareHouseId;
    datasource.read();
}

function addInventory() {
    var sourcegrid = $('#InventorySrcGrid').data('kendoGrid');        
    var destinationgrid = $('#InventoryDesGrid').data('kendoGrid'); 

    sourcegrid.select().each(function () {
        var dataItem = sourcegrid.dataItem($(this));
        destinationgrid.dataSource.add(dataItem);     
    });
    destinationgrid.refresh();                      
}

function removeInventory() {
    var destinationgrid = $("#InventoryDesGrid").data("kendoGrid");

    var rows = destinationgrid.select();
    rows.each(function (index, row) {
        destinationgrid.removeRow($(row).closest('tr'));
    });

    destinationgrid.refresh();   
 
}

function removeAll() {

    destinationgrid = $("#InventoryDesGrid").data('kendoGrid');
    if (destinationgrid.dataSource.data().length > 0) {
        destinationgrid.dataSource.data([]);
        destinationgrid.refresh();    
    }
}