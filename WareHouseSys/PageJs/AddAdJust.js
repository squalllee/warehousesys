function AddAdjustInit() {
    $("#AdjustUpdateBtn").click(function (e) {
        event.preventDefault();
        var validator = $("#AdjustForm").kendoValidator().data("kendoValidator");
        if (validator.validate()) {
            var obj = formToJSON($("#AdjustForm"));
            obj.OrderNo = $("[name='OrderNo']").val();

            $.ajax({
                url: "../api/Adjust/SaveAdjustHeader",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');

                    AdjustUpdateDialog.close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    $("#AdjustNewBtn").click(function (e) {
        event.preventDefault();
        var validator = $("#AdjustForm").kendoValidator().data("kendoValidator");
        if (validator.validate()) {

            var grid = $("#AdjustAddGrid").data("kendoGrid");

            var items = grid.dataSource.view();

            if (items.length === 0) {
                alert('至少要有一筆欲調整的品項!');
                return;
            }

            var obj = {};
            obj.adjustHeader = formToJSON($("#AdjustForm"));
            obj.adjustBodies = items;

            $.ajax({
                url: "../api/Adjust/AddAdjust",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');

                    AdjustAddDialog.close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    var AdJustAddDatasource = new kendo.data.DataSource({
        offlineStorage: "products-offline",
        transport: {
            read: {
                url: "../Adjust/AdjustBodyViewGrid?OrderNo=" + $("[name='OrderNo']").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            create: {
                url: "../api/Adjust/addAdjustBody",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            update: {
                url: "../api/Adjust/updateAdjustBody",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            destroy: {
                url: "../api/Adjust/deleteAdjustBody",
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
        requestEnd: function (e) {
            if (e.type !== "read") {
                AdJustAddDatasource.read();
            }
        },
        schema: {
            data: "data",
            model: {
                id: "MaterialNo",
                fields: {
                    MaterialNo: { type: "string" },
                    MaterialName: { type: "string" },
                    Spec: { type: "string" },
                    Lot: { type: "string" },
                    WareHouseName: { type: "string" },
                    StorageId: { type: "string" },
                    Reason: { type: "string" }
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange") {
                var model = e.items[0];
                if (e.field === "MaterialNo") {
                    var selectedData = $("[name='MaterialNo']").data("kendoMultiColumnComboBox").dataItem();

                    model.MaterialNo = selectedData.MaterialNo;
                    model.MaterialName = selectedData.MaterialName;
                    model.Unit = selectedData.Unit;
                    model.Spec = selectedData.Spec;
                    model.StorageId = selectedData.StorageId;
                    model.WareHouseName = selectedData.WareHouseName;
                    model.WarehouseId = selectedData.WarehouseId;
                    model.StockQty = selectedData.Qty;
                    model.Lot = selectedData.Lot;

                    $("[name='MaterialName']").text(selectedData.MaterialName);
                    $("[name='Spec']").text(selectedData.Spec);
                    $("[name='Unit']").text(selectedData.Unit);
                    $("[name='WareHouseName']").text(selectedData.WareHouseName);
                    $("[name='StorageId']").text(selectedData.StorageId);
                    $("[name='Lot']").text(selectedData.Lot);
                    $("[name='StockQty']").text(selectedData.Qty);
                    

                }
                else if (e.field === "Quantity") {
                    var UnitPrice = $("[name='UnitPrice']").text();
                    var Quantity = $("[name='Quantity']").data("kendoNumericTextBox").value();

                    model.UnitPrice = UnitPrice;
                    model.TotalPrice = parseInt(UnitPrice) * parseInt(Quantity);


                    $("[name='TotalPrice']").text(model.TotalPrice);
                }
                else if (e.field === "Unit") {
                    model.Unit = $("[name='Unit']").getKendoComboBox().value();
                }

            }
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });

    if ($("[name='OrderNo']").val() === "") {
        AdJustAddDatasource.online(false);
    }
    else {
        AdJustAddDatasource.online(true);
    }


    AdjustAddGrid = $("#AdjustAddGrid").kendoGrid({
        dataSource: AdJustAddDatasource,
        resizable: true,
        height: 600,
        width: 1850,
        sortable: true,
        scrollable: true,
        toolbar: [
            { name: "create", text: "新增項次" }
        ],
        columns: [
            { command: ["edit", "destroy"], title: "&nbsp;", width: 200, locked: true, lockable: true },
            {
                field: "MaterialNo",
                title: "品號",
                width: 200,
                editor: MultiMaterialCombobox

            }, {
                field: "MaterialName",
                title: "品名",
                editor: EditSpan,
                width: 200
            },
            {
                field: "Spec",
                title: "規格",
                editor: EditSpan,
                width: 250
            }, {
                field: "Unit",
                title: "單位",
                editor: EditSpan,
                width: 200
            }, {
                field: "WareHouseName",
                title: "倉庫",
                editor: EditSpan,
                width: 200
            }, {
                field: "StorageId",
                title: "儲位",
                editor: EditSpan,
                width: 200
            }, {
                field: "Lot",
                title: "批號",
                editor: EditSpan,
                width: 200
            },{
                field: "StockQty",
                title: "庫存量",
                editor: EditSpan,
                width: 100
            }, {
                field: "Quantity",
                title: "實際數量",
                editor: QuantityEdit,
                width: 100
            },{
                field: "Reason",
                title: "調整原因",
                width: 300
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}


function QuantityEdit(Container, options) {
    Quantity = $("<input name='" + options.field + "' type='number'  step='1' min='0' style='width: 100 %;' required />").appendTo(Container).kendoNumericTextBox();
}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + (options.model[options.field] === undefined ? '' : options.model[options.field]) + "<span>").appendTo(Container);
}


function MultiMaterialCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇料號",
        dataTextField: "MaterialNo",
        dataValueField: "MaterialName",
        filter: "contains",
        filterFields: ["MaterialNo", "MaterialName"],
        height: 400,
        autoBind: false,
        minLength: 2,
        text: options.model.MaterialNo,
        dataSource: {
            serverFiltering: true,
            transport: {
                read: {
                    url: "../Material/getMaterialInfoKendo"
                },
                parameterMap: function (data, type) {
                    return { filter: $("[name='MaterialNo']").data("kendoMultiColumnComboBox").text() };
                }
            }
        },
        columns: [
            { field: "MaterialName", title: "品名", width: 250 },
            { field: "MaterialNo", title: "料號", width: 150 },
            { field: "Spec", title: "規格", width: 250 },
            { field: "Unit", title: "單位", width: 80 },
            { field: "WareHouseName", title: "倉庫", width: 150 },
            { field: "StorageId", title: "儲位", width: 150 },
            { field: "Lot", title: "批號", width: 150 },
            { field: "Qty", title: "庫存", width: 50 }
        ],
        change: function () {
            $.ajax({
                url: "../api/Material/getMaterialPrice/" + this.dataItem().MaterialNo + "/" + this.dataItem().Lot,
                dataType: 'text',
                type: 'get',
                contentType: 'application/json',
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    $("[name='UnitPrice']").text(data);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('失敗');
                }
            });
        }
    });

}