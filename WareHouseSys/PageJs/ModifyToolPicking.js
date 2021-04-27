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

                        MaterialNo: options.MaterialNo,

                        KeepManId: options.KeepManId,

                        KeepUnitId: options.KeepUnitId
                    });
                }
                else {
                    return JSON.stringify(options);
                }
            }
        },
        schema: {
            data: "data",
            model: {
                id: "SerialNo",
                fields: {
                    SerialNo: { editable: false },
                    MaterialNo: { editable: false },
                    MaterialName: { editable: false },
                    Spec: { editable: false },
                    Quantity: { editable: false },
                    PickedQty: { type: "number" },
                    copy: {type:"string"}
                }
            }
        },
        change: function (e) {
            var model = e.items[0];
            if (e.action === "itemchange" && e.field === "Storage") {
                
                model.Lot = $("[name='Storage']").data("kendoMultiColumnComboBox").dataItem().Lot;
                model.WareHouseName = $("[name='Storage']").data("kendoMultiColumnComboBox").dataItem().WareHouseName;
                $("[name='Lot']").text(model.Lot);
                $("[name='WareHouseName']").text(model.WareHouseName);
            }
            else if (e.action === "itemchange" && e.field === "KeepMan") {
                model.KeepMan = $("[name='KeepMan']").data("kendoMultiColumnComboBox").dataItem().TMNAME;
                model.KeepManId = $("[name='KeepMan']").data("kendoMultiColumnComboBox").dataItem().KEYNO;
                model.KeepUnit = $("[name='KeepMan']").data("kendoMultiColumnComboBox").dataItem().UNITNAME;
                model.KeepUnitId = $("[name='KeepMan']").data("kendoMultiColumnComboBox").dataItem().UNITNO;
                $("[name='KeepUnit']").text(model.KeepUnit);
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
        height: 300,
        sortable: true,
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
                editor: WareHouseNameSpan,
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
                field: "KeepMan",
                title: "保管人",
                width: 150,
                editor: KeepManCombobox
            }, {
                field: "KeepUnit",
                title: "保管單位",
                width: 200,
                editor: KeepUnitSpan,
                template: '#= KeepUnit !== null ? KeepUnit : "" #'
            }, {
                field: "Note",
                title: "備註",
                width: 150
            }, {
                field:"copy",
                title: "複制",
                width: 150,
                editor: function (container, options) {
                    $("<span></span>").appendTo(container);
                },
                template: function (container) {
                    return '<button name="copy" class="btn btn-default" type="button" style="margin-left:10px" onclick="cloneRow(this)"><i class="fas fa-plus"></i>複制</button>';
                }
            }


        ],
        editable: "inline"
    }).data("kendoGrid");

    $("#BtnSavePicking").click(function (e) {
        if (AttachmentArray.length === 0) {
            alert('必需上傳已核可的手工具領料單!');
        }
        else {
            var obj = formToJSON($("#PickingForm"));
            obj.EmergencyPicking = $("#EmergencyPicking").prop("checked");
            obj.attachments = AttachmentArray;
            $.ajax({
                url: $("#PickingForm").data("saveurl"),
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    ToolPickingGrid.dataSource.read();
                    AttachmentArray = [];
                    $('#imgList').empty();
                    $("#ModifyToolPickingDialog").data("kendoWindow").close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }

    });
}

function KeepManCombobox(Container, option) {
    
   var KeepManCmb = $('<input required data-required-msg="必需輸入" name="' + option.field + '"/>').appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇保管人",
        dataTextField: "TMNAME",
        dataValueField: "KEYNO",
        filter: "contains",
        filterFields: ["TMNAME", "KEYNO"],
        height: 400,
        autoBind: false,
       minLength: 1,
       text: option.model.KeepMan,
       value: option.model.KeepManId,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/Employee/getEmployeeUnit"
                }
            }
        },
        columns: [
            { field: "TMNAME", title: "姓名", width: 150 },
            { field: "KEYNO", title: "員工編號", width: 150 },
            { field: "UNITNO", title: "單位代碼", width: 150 },
            { field: "UNITNAME", title: "單位名稱", width: 150 }
       ],
       change: function (e) {
           alert('ddd');
       }
    }).data("kendoMultiColumnComboBox");
    
 
}

function LotSpan(Container, option) {
    $('<span name="Lot">' + (option.model.Lot === null ? "" : option.model.Lot) + '</span>').appendTo(Container);
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
        $("[name='Storage']").data('kendoComboBox').value(e.model.StorageId);
}

function KeepUnitSpan(container, options) {
    $('<span name="' + options.field + '">' + (options.model.KeepUnit === null ? "" : options.model.KeepUnit) + '</span>').appendTo(container);
}
function NumberInput(container, options) {
    $('<input name="' + options.field + '" type="number" title="numeric"  min="0" max="100" step="1" style="width: 100 %; " />').appendTo(container).kendoNumericTextBox();
}

function StorageAutoComplete(container, options) {

     var StorageCmb = $('<input required data-required-msg="必需輸入" name="' + options.field + '"/>').appendTo(container).kendoMultiColumnComboBox({
        placeholder: "選擇批號",
        dataTextField: "StorageId",
        dataValueField: "StorageId",
        filter: "contains",
         filterFields: ["StorageId"],
         text: options.model.StorageId,
         value: options.model.StorageId,
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
     }).data("kendoMultiColumnComboBox");

}

function cloneRow(e) {
    var row = e.closest("tr");
    var grid = $("#PickingDetailUpdateGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("#AddPickingToolDialog").kendoWindow({
        title: "新增手工具領料",
        actions: ["Close"],
        content: $("#AddPickingToolDialog").data("url") + "?OrderNo=" + dataItem.OrderNo + "&SerialNo=" + dataItem.SerialNo,
        visible: false,
        modal: true,
        width: 1000,
        position: {
            top: "20px",
            left: "15%"
        },
        activate: function (e) {
            AddPickingDialogInit(dataItem);
        }
    }).data("kendoWindow").center().open();
}