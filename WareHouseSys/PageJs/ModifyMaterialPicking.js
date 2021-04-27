var PickingDetailUpdateGrid;
function dataSource_error(e) {
    alert('發生錯誤');
}
function modifyGridInit() {
    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#PickingDetailUpdateGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#PickingDetailUpdateGrid").data("updateurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                if (operation === "update" && options) {
                    return JSON.stringify({
                        OrderNo: options.OrderNo,

                        SerialNo: options.SerialNo,

                        PickedQty: options.PickedQty,

                        Quantity: options.Quantity,

                        WarehouseId: options.WareHouseId,

                        Storage: options.Storage, 

                        StorageId: options.Storage.StorageId,

                        Note: options.Note,

                        Lot: options.Lot,

                        MaterialNo: options.MaterialNo
                    });
                }
                else {
                    return JSON.stringify(options);
                }
            }
        },
        schema: {
            data: "data",
            total: "Total",
            model: {
                id: "SerialNo",
                fields: {
                    SerialNo: { editable: false },
                    MaterialNo: { editable: false },
                    MaterialName: { editable: false },
                    Spec: { editable: false },
                    Quantity: { editable: false },
                    PickedQty: { type: "number" }                
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" && e.field === "Storage") {
                var model = e.items[0];
                model.Lot = $("[name='Storage']").data("kendoMultiColumnComboBox").dataItem().Lot;
                model.WareHouseName = $("[name='Storage']").data("kendoMultiColumnComboBox").dataItem().WareHouseName;
                model.WareHouseId = $("[name='Storage']").data("kendoMultiColumnComboBox").dataItem().WarehouseId;
                $("[name='Lot']").text(model.Lot);
                $("[name='WareHouseName']").text(model.WareHouseName);
                
               
            }
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });
    datasource.bind("error", dataSource_error);

    PickingDetailUpdateGrid = $("#PickingDetailUpdateGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 500,
        width: 1700,
        sortable: true,
        pageable: true,
        columns: [
            { command: ["edit"], title: "&nbsp;", width: 100, locked: true, lockable: true },
            {
                field: "SerialNo",
                title: "序號",
                width: 50

            }, {
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
                width: 200,
                editor:WareHouseNameSpan,
                template: '#= WareHouseName !== null ? WareHouseName : "" #'
            }, {
                field: "Storage",
                title: "儲位",
                width: 150,
                editor: StorageAutoComplete,
                template: '#= Storage !== null ? Storage.StorageId : StorageId#'
            }, {
                field: "Lot",
                title: "批號",
                width: 100,
                editor: LotSpan,
                template: '#= Lot !== null ? Lot : "" #'

            }, {
                field: "Quantity",
                title: "未領量",
                width: 100

            }
            , {
                field: "PickedQty",
                title: "已領量",
                width: 100,
                editor: NumberInput
            }, {
                field: "Note",
                title: "備註",
                width: 150
            },{
                title: "複制",
                width: 150,
                template: function (container) {
                    return '<button name="copy" class="btn btn-default" type="button" style="margin-left:10px" onclick="cloneRow(this)"><i class="fas fa-plus"></i>複制</button>';
                }
            }

            
        ],
        editable: "inline"
    }).data("kendoGrid");

    $("#BtnSavePicking").click(function (e) {
        if (AttachmentArray.length === 0) {
            alert('必需上傳已核可的領料單!');
        }
        else {
            var obj = formToJSON($("#PickingForm"));
            obj.EmergencyPicking = $("#EmergencyPicking").prop("checked");
            obj.attachments = AttachmentArray;
            var validator = $("#PickingForm").kendoValidator().data("kendoValidator");

            if (validator.validate()) {
                $.ajax({
                    url: $("#PickingForm").data("saveurl"),
                    dataType: 'text',
                    type: 'post',
                    contentType: 'application/json',
                    data: JSON.stringify(obj),
                    processData: false,
                    success: function (data, textStatus, jQxhr) {
                        alert('儲存成功');
                        MaterialPickingGrid.dataSource.read();
                        AttachmentArray = [];
                        $('#imgList').empty();
                        $("#ModifyMaterialPickingDialog").data("kendoWindow").close();
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        alert('儲存失敗');
                    }
                });
            }
                
        }
        
    });
}

function LotSpan(Container, option) {
    $('<span name="Lot">' + option.model.Lot + '</span>').appendTo(Container);
}

function WareHouseNameSpan(Container, option) {
    $('<span name="WareHouseName">' + option.model.WareHouseName + '</span>').appendTo(Container);
}

function selectStorage(e) {
    var StorageDataSource = new kendo.data.DataSource({
        transport: {
            read: "../api/getStorageInfo/" + e.sender._old,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST"
        }
    });
    StorageDataSource.read();
    $("[name='Storage']").data('kendoComboBox').setDataSource(StorageDataSource);
    if ($("[name='Storage']") !== null)
        $("[name='Storage']").data('kendoComboBox').text('');
}

function NumberInput(container, options) {
    $('<input name="' + options.field + '" type="number" title="numeric"  min="0" max="100" step="1" style="width: 100 %; " />').appendTo(container).kendoNumericTextBox();
}

function StorageAutoComplete(container, options) {

    $('<input required data-required-msg="必需輸入" name="' + options.field + '"/>').appendTo(container).kendoMultiColumnComboBox({
        placeholder: "選擇批號",
        dataTextField: "StorageId",
        dataValueField: "StorageId",
        filter: "contains",
        filterFields: ["StorageId"],
        height: 400,
        autoBind: false,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "POST",
                    url: "../api/Material/getMaterialInfo/" + options.model.MaterialNo + "/" + options.model.WGroupId
                }
            }
        },
        columns: [
            { field: "WarehouseId", title: "庫別", width: 150 },
            { field: "WareHouseName", title: "庫別名稱", width: 150 },
            { field: "StorageId", title: "儲位", width: 150 },
            { field: "Lot", title: "批號", width: 150 },
            { field: "Qty", title: "庫存", width: 100 }
        ],
        change: function (e) {
            var item = this.dataItem();

            $("[name='PickedQty']").data("kendoNumericTextBox").value("0");
            $("[name='PickedQty']").data("kendoNumericTextBox").max(item.Qty);
            

        }
    });
    

    if (options.model.warehouseInfo !== null)
        setStorage(options.model.warehouseInfo.WarehouseId, $('[name="' + options.field + '"]'));
}

function cloneRow(e) {
    var row = e.closest("tr");
    var grid = $("#PickingDetailUpdateGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("#AddPickingMaterialDialog").kendoWindow({
        title: "新增領料",
        actions: ["Close"],
        content: $("#AddPickingMaterialDialog").data("url") + "?OrderNo=" + dataItem.OrderNo + "&SerialNo=" + dataItem.SerialNo,
        visible: false,
        modal: true,
        width: 1000,
        activate: function (e) {           
            AddPickingDialogInit(dataItem);
        }
    }).data("kendoWindow").center().open();
}

