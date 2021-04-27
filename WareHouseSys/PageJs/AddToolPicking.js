var ToolPickingDatasource;
function ToolPickingAddInit() {
    localStorage.clear();
    $("#BtnSaveToolPicking").click(function (e) {
        var validator = $("#AddToolPickingForm").kendoValidator().data("kendoValidator");
        var pickingToolHeader = formToJSON($("#AddToolPickingForm"));
        var pickingToolBodies = PickingToolAddGrid.dataSource.view();
        pickingToolHeader.EmergencyPicking = $("#EmergencyPicking").prop("checked");

        if (pickingToolBodies.length === 0) {
            alert('請新增品項');
            return;
        }
        if (validator.validate()) {
            var obj = {};
            obj.pickingToolHeader = pickingToolHeader;
            obj.pickingToolBodies = pickingToolBodies;

            $.ajax({
                url: "../api/Picking/SaveToolPickingData",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    ToolPickingAddDialog.close();
                    ToolPickingGrid.dataSource.read();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    ToolPickingDatasource = new kendo.data.DataSource({
        offlineStorage: "products-offline",
        schema: {
            model: {
                id: "SerialNo",
                fields: {
                    MaterialNo: { type: "string" },
                    MaterialName: { type: "string" },
                    Spec: { type: "string" },
                    Quantity: { type: "number" },
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
            else if (e.action === "itemchange" && e.field === "KeepManName") {
                model = e.items[0];
                selectedData = $("[name='KeepManName']").data("kendoMultiColumnComboBox").dataItem();

                model.KeepMan = selectedData.KEYNO;
                model.KeepUnit = selectedData.UNITNO;
                model.KeepManName = selectedData.TMNAME;
            }

        },
        serverPaging: false,
        pageSize: 10,
        batch: false,
        serverSorting: false,
        serverFiltering: false,
        pageable: false
    });

    ToolPickingDatasource.online(false);

    PickingToolAddGrid = $("#PickingToolAddGrid").kendoGrid({
        dataSource: ToolPickingDatasource,
        resizable: true,
        height: 300,
        sortable: true,
        toolbar: [
            { name: "create", text: "新增項次" }
        ],
        columns: [
            { command: ["edit", "destroy"], title: "&nbsp;", width: 100, locked: true, lockable: true },
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
                editor: EditSpan,
                width: 200
            }, {
                field: "StorageId",
                title: "儲位",
                editor: EditSpan,
                width: 150
            }, {
                field: "Lot",
                title: "批號",
                editor: EditSpan,
                width: 100

            }, {
                field: "Quantity",
                title: "領料量",
                width: 100

            }, {
                field: "KeepManName",
                title: "保管人",
                editor: KeepManEdit,
                width: 150
            }
            , {
                field: "Note",
                title: "備註",
                width: 150
            }
        ],
        editable: "inline"
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
        ]
    });
}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + (options.model[options.field] === undefined ? '' : options.model[options.field]) + "<span>").appendTo(Container);
}

function EditQty(Container, options) {
    $('<input name="' + options.field + '" type="number" title="numeric"  min="0" step="1" style="width: 100 %; " />').appendTo(Container).kendoNumericTextBox();
}

function KeepManEdit(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇保管人",
        dataTextField: "TMNAME",
        dataValueField: "KEYNO",
        filter: "contains",
        filterFields: ["TMNAME", "KEYNO"],
        height: 400,
        autoBind: true,
        // text: options.model.MaterialName,
        text: options.model.KeepMan,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/Employee/getEmployeeUnit/"
                }
            }
        },
        columns: [
            { field: "TMNAME", title: "姓名", width: 250 },
            { field: "KEYNO", title: "員工編號", width: 150 },
            { field: "UNITNO", title: "單位代碼", width: 250 },
            { field: "UNITNAME", title: "單位名稱", width: 250 }
        ]
    });
}