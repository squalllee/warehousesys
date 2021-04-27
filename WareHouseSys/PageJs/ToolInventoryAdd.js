var ToolAddInventoryGrid;
var datasource;
function ToolInventoryAddInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });
    $("#BtnSaveInventory").click(function () {
        var validator = $("#ToolInventoryAddForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var obj = {};
            obj.toolInventoryHeader = formToJSON($("#ToolInventoryAddForm"));

            obj.toolInventoryViewModels = ToolAddInventoryGrid.dataSource.view();

            var url = $("#ToolInventoryAddForm").data("saveurl");

            $.ajax({
                url: url,
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    $("#ToolInventoryAddDialog").data("kendoWindow").close();
                    ToolInventorySearchGrid.dataSource.read();
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
                url: $("#ToolAddInventoryGrid").data("url") + "?UnitId=",
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

    ToolAddInventoryGrid = $("#ToolAddInventoryGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 350,
        sortable: true,
        autoBind: false,
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
                width: 200
            }, {
                field: "KeepMan",
                title: "保管人",
                width: 100
            },
            {
                field: "KeepUnit",
                title: "保管單位",
                width: 100
            },
            {
                field: "Quantity",
                title: "庫存",
                width: 100
            }
        ]
    }).data("kendoGrid");
}

function onKeepUnitChange(e) {

    $("[name='ToolMgrDisplay']").val(this.dataItem(e.item.index()).ToolMgr);
    $("[name='ToolMgr']").val(this.dataItem(e.item.index()).ToolMgrId);

    datasource.transport.options.read.url = $("#ToolAddInventoryGrid").data("url") + "?UnitId=" + this.dataItem(e.item.index()).UNITNO;
    datasource.read();
}