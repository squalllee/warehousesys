var WareHouseName = "";
var StorageId = "";
var Lot = "";
function MaterialPickingAddInit() {
    $("#BtnUpdatePicking").click(function () {
        var validator = $("#PickingAddForm").kendoValidator().data("kendoValidator");
        var pickingHeader = formToJSON($("#PickingAddForm"));
        pickingHeader.EmergencyPicking = $("#EmergencyPicking").prop("checked");
        if (validator.validate()) {

            $.ajax({
                url: "../api/Picking/updatePickingHeader",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(pickingHeader),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    MaterialPickingGrid.dataSource.read();
                    MaterialPickingDialog.close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    $("#BtnSavePicking").click(function (e) {
        var validator = $("#PickingAddForm").kendoValidator().data("kendoValidator");
        var pickingHeader = formToJSON($("#PickingAddForm"));
        var pickingBodies = addPickingDataGrid.dataSource.view();
        pickingHeader.EmergencyPicking = $("#EmergencyPicking").prop("checked");

        if (pickingBodies.length === 0) {
            alert('請新增品項');
            return;
        }
        if (validator.validate()) {
            var obj = {};
            obj.pickingHeader = pickingHeader;
            obj.pickingBodies = pickingBodies;


            $.ajax({
                url: "../api/Picking/SavePickingData",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    MaterialPickingGrid.dataSource.read();
                    MaterialPickingAddDialog.close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    var MaterialPickingAddDatasource = new kendo.data.DataSource({
        offlineStorage: "products-offline",
        transport: {
            read: {
                url: "../Picking/MaterialPickingBodyDetail?OrderNo=" + $("[name='OrderNo']").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            create: {
                url: "../api/Picking/AddPickingBody",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            update: {
                url: "../api/Picking/UpdateMaterialPickingBody",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            destroy: {
                url: "../api/Picking/deleteMaterialPickingBody",
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
                MaterialPickingAddDatasource.read();
            }
        },
        schema: {
            data: "data",
            total: "Total",
            model: {
                id: "SerialNo",
                fields: {
                    MaterialNo: { type: "string" },
                    MaterialName: { type: "string" },
                    Spec: { type: "string" },
                    Quantity: { type: "number", validation: { required: true, min: 1 }},
                    Note: { type: "string" }
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" && e.field === "MaterialNo") {
                var model = e.items[0];
                var selectedData = $("[name='MaterialNo']").data("kendoMultiColumnComboBox").dataItem();

                model.Lot = selectedData.Lot;
                model.MaterialName = selectedData.MaterialName;
                model.MaterialNo = selectedData.MaterialNo;
                model.Spec = selectedData.Spec;
                model.StorageId = selectedData.StorageId;
                model.WareHouseName = selectedData.WareHouseName;
                model.WareHouseId = selectedData.WarehouseId;

                $("[name='MaterialName']").text(selectedData.MaterialName);
                $("[name='Spec']").text(selectedData.Spec);
                $("[name='WareHouseName']").text(selectedData.WareHouseName);
                $("[name='StorageId']").text(selectedData.StorageId);
                $("[name='Lot']").text(selectedData.Lot);
                $("[name='Quantity']").data("kendoNumericTextBox").max(selectedData.Qty);

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
        MaterialPickingAddDatasource.online(false);
    }
    else {
        MaterialPickingAddDatasource.online(true);
    }

   

    addPickingDataGrid = $("#addPickingDataGrid").kendoGrid({
        dataSource: MaterialPickingAddDatasource,
        resizable: true,
        height: 500,
        width : 1400,
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
        toolbar: [
            { name: "create", text: "新增項次" }
        ],
        columns: [
            { command: ["edit","destroy"], title: "&nbsp;", width: 100, locked: true, lockable: true },
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
                width: 200
            }, {
                field: "WareHouseName",
                title: "庫別",
                width: 200,
                editor: EditSpan
            }, {
                field: "StorageId",
                title: "儲位",
                width: 150,
                editor: EditSpan
            }, {
                field: "Lot",
                title: "批號",
                width: 100,
                editor: EditSpan

            }, {
                field: "Quantity",
                title: "領料量",
                width: 100
            }
            , {
                field: "Note",
                title: "備註",
                width: 150
            }
        ],
        editable: "inline",
        edit: function (e) {
            
            var model = e.model;
            WareHouseName = model.WareHouseName;
            StorageId = model.StorageId;
            Lot = model.Lot;

        }
    }).data("kendoGrid");
      
}

function MultiMaterialCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇料號",
        dataTextField: "MaterialNo",
        dataValueField: "MaterialName",
        filter: "contains",
        filterFields: ["MaterialNo", "MaterialName"],
        height: 400,
        autoBind: true,
        // text: options.model.MaterialName,
        text: options.model.MaterialNo,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "POST",
                    url: "../api/Material/getMaterialInfoAll/" + $("[name='WGroupId']").val()
                }
            }
        },
        columns: [
            { field: "MaterialName", title: "品名", width: 250 },
            { field: "MaterialNo", title: "料號", width: 150 },
            { field: "WareHouseName", title: "庫別", width: 250 },
            { field: "StorageId", title: "儲位", width: 250 },
            { field: "Spec", title: "規格", width: 250 },
            { field: "Lot", title: "批號", width: 250 },
            { field: "Qty", title: "庫存", width: 250 }
        ],
        
        dataBound: function (e) {

            var combo = e.sender;
            var test2 = "";
            combo.select(function (dataItem) {
                if (dataItem.MaterialNo === combo.value() && dataItem.WareHouseName === WareHouseName && dataItem.StorageId == StorageId && dataItem.Lot == Lot)
                    test2 = dataItem.Qty;
                return (dataItem.MaterialNo === combo.value() && dataItem.WareHouseName === WareHouseName && dataItem.StorageId == StorageId && dataItem.Lot == Lot);
            });
            $("[name='Quantity']").data("kendoNumericTextBox").max(test2);
            
        }
        
    });
}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + (options.model[options.field] === undefined ? '' : options.model[options.field])+ "<span>").appendTo(Container);
}

function EditQty(Container, options) {
    $('<input name="' + options.field + '" type="number" title="numeric"  min="0" step="1" style="width: 100 %; " />').appendTo(Container).kendoNumericTextBox();
}
