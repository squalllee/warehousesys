var TransferOutGrid;
function TransferOutDialogInit() {
    $("#BtnSaveTransfer").click(function (e) {
        var validator = $("#TransferOutForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var obj = formToJSON($("#TransferOutForm"));

            $.ajax({
                url: $("#TransferOutForm").data("saveurl"),
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    TransferOutSearchGrid.dataSource.read();
                    $("#TransferOutDialog").data("kendoWindow").close();
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
                url: $("#TransferOutGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            create: {
                url: $("#TransferOutGrid").data("createurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#TransferOutGrid").data("updateurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            destroy: {
                url: $("#TransferOutGrid").data("deleteurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                return JSON.stringify(options);
            }
        },
        requestEnd: function (e) {
            var response = e.response;
            var type = e.type;
            if (type === "create") {
                $("#TransferOutGrid").data("kendoGrid").dataSource.read();
            }
        },
        schema: {
            data: "data",
            total: "Total",
            model: {
                id: "SerialNo",
                fields: {
                    OrderNo: { type: "string" },
                    MaterialNo: { type: "string" ,editable:false},
                    MaterialName: { type: "string" },
                    Spec: { type: "string" },
                    TransferOutQty: {
                        type: "number",
                        validation: {
                            min: 1,
                            required: true
                        }
                    },
                    OutWareHouseName: { type: "string" },
                    OutStorageId: { type: "string" },
                    Lot: { type: "string" },
                    Note: { type: "string" }
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" && e.field === "OutWareHouseName") {
                var model = e.items[0];
                var selectedData = $("[name='OutWareHouseName']").data("kendoMultiColumnComboBox").dataItem();

                model.OutWareHouseName = selectedData.WareHouseName;
                model.OutWareHouseId = selectedData.WarehouseId;
                model.OutStorageId = selectedData.StorageId;
                model.Lot = selectedData.Lot;

                $("[name='OutWareHouseName']").text(model.OutWareHouseName);
                $("[name='OutWareHouseId']").text(model.OutWareHouseId);
                $("[name='OutStorageId']").text(model.OutStorageId);
                $("[name='Lot']").text(model.Lot);
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

    TransferOutGrid = $("#TransferOutGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 300,
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
            },
            {
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
                editor: MultiWareHouseCombobox,
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
                editor: EditSpan,
                template: '#= InWareHouseName !== null ? InWareHouseName : "" #'
            }, {
                field: "TransferOutQty",
                title: "轉出量",
                width: 100
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

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + options.model[options.field] + "<span>").appendTo(Container);
}

function MultiWareHouseCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇轉出庫別",
        dataTextField: "WareHouseName",
        dataValueField: "WarehouseId",
        filter: "contains",
        filterFields: ["WarehouseId", "WareHouseName"],
        height: 400,
        autoBind: false,
        text: options.model["OutWareHouseName"],
        value: options.model["OutWareHouseId"],
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "POST",
                    url: "../api/Material/getMaterialInfo/" + options.model.MaterialNo + "/" + $("[name='WGroupId']").val()
                }
            }
        },
        columns: [
            { field: "WarehouseId", title: "庫別", width: 150 },
            { field: "WareHouseName", title: "庫別名稱", width: 150 },
            { field: "StorageId", title: "儲位", width: 150 },
            { field: "Lot", title: "批號", width: 150 },
            { field: "Qty", title: "庫存", width: 100 }
        ]
    });

}

function rowClone(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var newItem = jQuery.extend({}, dataItem);
    newItem.Quantity = 0;

    $.ajax({
        url: $("#TransferOutGrid").data("createurl"),
        dataType: 'text',
        type: 'post',
        contentType: 'application/json',
        data: JSON.stringify(newItem),
        processData: false,
        success: function (data, textStatus, jQxhr) {
            TransferOutGrid.dataSource.read();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert('儲存失敗');
        }
    });
  
}
