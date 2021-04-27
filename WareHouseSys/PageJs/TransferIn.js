var TransferInGrid;
function TransferInDialogInit() {
    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    $("#BtnSaveTransfer").click(function (e) {
        var validator = $("#TransferInForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var obj = formToJSON($("#TransferInForm"));
            if (AttachmentArray.length === 0) {
                alert('必需上傳紙本移轉單!');
                return;
            }
            obj.attachment = AttachmentArray;

            $.ajax({
                url: $("#TransferInForm").data("saveurl"),
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    AttachmentArray = [];
                    $('#imgList').empty();
                    TransferInSearchGrid.dataSource.read();
                    $("#TransferInDialog").data("kendoWindow").close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    var datasource = new kendo.data.DataSource({
        offlineStorage: "offlineStorage",
        transport: {
            read: {
                url: $("#TransferInGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            create: {
                url: $("#TransferInGrid").data("createurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#TransferInGrid").data("updateurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            destroy: {
                url: $("#TransferInGrid").data("deleteurl"),
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
                id: "SerialNo",
                fields: {
                    OrderNo: { type: "string" },
                    MaterialNo: { type: "string", editable: false },
                    TransferOutQty: {type:"number",editable:false},
                    MaterialName: { type: "string" },
                    Spec: { type: "string" },
                    TransferInQty: {
                        type: "number",
                        validation: {
                            min: 1,
                            required: true
                        }
                    },
                    InWareHouseName: { type: "string" },
                    InStorageId: { type: "string" },
                    Lot: { type: "string" },
                    Note: { type: "string" }
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" && e.field === "InWareHouseName") {
                var model = e.items[0];
                var selectedData = $("[name='" + e.field+"']").data("kendoMultiColumnComboBox").dataItem();

                model.InWareHouseName = selectedData.WareHouseName;
                model.InWareHouseId = selectedData.WarehouseId;
                model.InStorageId = selectedData.StorageId;


                $("[name='InWareHouseName']").text(model.InWareHouseName);
                $("[name='InWareHouseId']").text(model.InWareHouseId);
                $("[name='InStorageId']").text(model.InStorageId);
              

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

    TransferInGrid = $("#TransferInGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1850,
        pageable: true,
        sortable: true,
        columns: [
            {
                command: ["edit", "destroy", { text: "複制", click: rowClone }],
                title: "&nbsp;",
                width: 250,
                locked: true,
                lockable: true
            },
            {
                field: "MaterialNo",
                title: "品號",
                width: 200
            }, {
                field: "MaterialName",
                title: "品名",
                width: 200,
                editor: EditSpan,
                template: '#= MaterialName !== null ? MaterialName : "" #'
            },
            {
                field: "Spec",
                title: "規格",
                editor: EditSpan,
                width: 200
            }, {
                field: "OutWareHouseName",
                title: "轉出庫",
                width: 250,
                editor: EditSpan,
                template: '#= OutWareHouseName !== null ? OutWareHouseName : "" #'
            }, {
                field: "OutStorageId",
                title: "轉出儲位",
                editor: EditSpan,
                width: 100,
                template: '#= OutStorageId !== null ? OutStorageId : "" #'

            }, {
                field: "Lot",
                title: "批號",
                width: 100,
                editor: EditSpan,
                template: '#= Lot !== null ? Lot : "" #'

            }, {
                field: "InWareHouseName",
                title: "轉入庫",
                width: 200,
                editor: MultiWareHouseCombobox,
                template: '#= InWareHouseName !== null ? InWareHouseName : "" #'
            }, {
                field: "InStorageId",
                title: "轉入儲位",
                editor: EditSpan,
                width: 100,
                template: '#= InStorageId !== null ? InStorageId : "" #'

            }, {
                field: "TransferOutQty",
                title: "轉出量",
                width: 100
            }, {
                field: "TransferInQty",
                title: "轉入量",
                width: 100,
                editor:QtyEdit
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

function QtyEdit(Container, options) {
    $("<input name='" + options.field + "'>").appendTo(Container).kendoNumericTextBox({
        max: options.model['TransferOutQty']
    });

}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + options.model[options.field] + "<span>").appendTo(Container);
}

function MultiWareHouseCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇轉入庫別",
        dataTextField: "WareHouseName",
        dataValueField: "WarehouseId",
        filter: "contains",
        filterFields: ["WarehouseId", "WareHouseName","StorageId"],
        height: 400,
        autoBind: false,
        text: options.model["InWareHouseName"],
        value: options.model["InWareHouseId"],
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url:"../api/WareHouse/getWarehouseInfoWithStorage"
                    //url: "../api/Material/getMaterialInfoAll/"  + $("[name='WGroupId']").val()
                }
            }
        },
        columns: [
            { field: "WarehouseId", title: "庫別", width: 150 },
            { field: "WareHouseName", title: "庫別名稱", width: 150 },
            { field: "StorageId", title: "儲位", width: 150 }
        ]
    });

}

function rowClone(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var newItem = jQuery.extend({}, dataItem);
    newItem.Quantity = 0;

    $.ajax({
        url: $("#TransferInGrid").data("createurl"),
        dataType: 'text',
        type: 'post',
        contentType: 'application/json',
        data: JSON.stringify(newItem),
        processData: false,
        success: function (data, textStatus, jQxhr) {
            TransferInGrid.dataSource.read();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert('儲存失敗');
        }
    });

}
