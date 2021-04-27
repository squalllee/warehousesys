var TransferDetailUpdateGrid;
function TransferEstablishAddDialogInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    var datasource = new kendo.data.DataSource({
        offlineStorage:"offlineStorage",
        transport: {
            read: {
                url: $("#TransferDetailUpdateGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#TransferDetailUpdateGrid").data("updateurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            destroy: {
                url: $("#TransferDetailUpdateGrid").data("deleteurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            create: {
                url: $("#TransferDetailUpdateGrid").data("addurl"),
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
            var response = e.response;
            var type = e.type;
            if (type === "create") {
                $("#TransferDetailUpdateGrid").data("kendoGrid").dataSource.read();
            }
        },
        schema: {
            data: "data",
            total: "Total",
            model: {
                id: "MaterialNo",
                fields: {
                    OrderNo: { type: "string" },
                    MaterialNo: { type: "string"},
                    MaterialName: { type: "string"},
                    Spec: { type: "string" },
                    Quantity: {
                        type: "number",
                        validation: {
                            min: 1,
                            required: true
                        }
                    },
                    InWareHouseName: { type: "string" },
                    OutWareHouseName: { type: "string" },
                    OutStorageId: { type: "string" },
                    Lot: { type: "string"},
                    Note: { type: "string"}
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" && e.field === "MaterialNo") {
                var model = e.items[0];
                var selectedData = $("[name='MaterialNo']").data("kendoMultiColumnComboBox").dataItem();

                model.MaterialName = selectedData.MaterialName;
                model.MaterialNo = selectedData.MaterialNo;
                model.Spec = selectedData.Spec;
                model.Lot = selectedData.Lot;
                model.OutWareHouseName = selectedData.WareHouseName;
                model.OutWareHouseId = selectedData.WarehouseId;
                model.OutStorageId = selectedData.StorageId;

                $("[name='MaterialName']").text(model.MaterialName);
                $("[name='Spec']").text(model.Spec);
                $("[name='Lot']").text(model.Lot);
                $("[name='OutWareHouseName']").text(model.OutWareHouseName);
                $("[name='OutWareHouseId']").text(model.OutWareHouseId);
                $("[name='OutStorageId']").text(model.OutStorageId);

                return;
            }
            if (e.action === "itemchange" && e.field === "InWareHouseName") {
                model = e.items[0];
                selectedData = $("[name='InWareHouseName']").data("kendoMultiColumnComboBox").dataItem();
                model.InWareHouseId = selectedData.WarehouseId;
                model.InWareHouseName = selectedData.WareHouseName;
            }
            else if (e.action === "sync") {
                $("[name='WGroupId']").prop("disabled", true);
            }
            else if (e.action === "remove") {
                if ($("#TransferDetailUpdateGrid").data("kendoGrid").dataSource.view().length === 0) {
                    $("[name='WGroupId']").prop("disabled", false);
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
    datasource.bind("error", dataSource_error);

    if ($("[name='OrderNo']").val() === "") {
        localStorage.clear();
        datasource.online(false);
    }
    else {
        datasource.online(true);
    }
   

    TransferDetailUpdateGrid = $("#TransferDetailUpdateGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1700,
        sortable: true,
        pageable: true,
        toolbar: [
            { name: "create", text: "新增項次" }
        ],
        columns: [
            { command: ["edit","destroy"], title: "&nbsp;", width: 100, locked: true, lockable: true },
           {
                field: "MaterialNo",
                title: "品號",
               width: 200,
                editor:MultiMaterialCombobox

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
                field: "Lot",
                title: "批號",
                editor: EditSpan,
                width: 200
            }, {
                field: "OutWareHouseName",
                title: "轉出庫",
                width: 200,
                editor: EditSpan,
                template: '#= OutWareHouseName !== null ? OutWareHouseName : "" #'
            }, {
                field: "OutStorageId",
                title: "轉出儲位",
                width: 200,
                editor: EditSpan,
                template: '#= OutStorageId !== null ? OutStorageId : "" #'
            },
            {
                field: "Quantity",
                title: "轉出量",
                width: 100
            },  {
                field: "InWareHouseName",
                title: "轉入庫",
                width: 200,
                editor: InWareHouseName,
                template: '#= InWareHouseName !== null ? InWareHouseName : "" #'
            }
            , {
                field: "Note",
                title: "備註",
                width: 150
            }
        ],
        editable: "inline"
    }).data("kendoGrid");

    $("#BtnSaveTransfer").click(function (e) {
        var validator = $("#TransferEstablishForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var obj = formToJSON($("#TransferEstablishForm"));
            var url = "";
            if ($("[name='OrderNo']").val() === "") {
                obj.TransferBodies = $("#TransferDetailUpdateGrid").data("kendoGrid").dataSource.view();
                url = $("#TransferEstablishForm").data("saveurl");
            }
            else {
                url = $("#TransferEstablishForm").data("updateurl");
            }

            $.ajax({
                url: url,
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    TransferEstablishGrid.dataSource.read();
                    $("#TransferEstablishAddDialog").data("kendoWindow").close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });
}

function QtyEdit(Container, options) {
    $("<input name='" + options.field + "'>").appendTo(Container).kendoNumericTextBox();
   
}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + options.model[options.field] + "<span>").appendTo(Container);
}

function MultiMaterialCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇料號",
        dataTextField: "MaterialNo",
        dataValueField: "MaterialName",
        filter: "contains",
        filterFields: ["MaterialNo","MaterialName"],
        height: 400,
        autoBind: false,
        text: options.model["MaterialNo"],
        value: options.model["MaterialName"],
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
            { field: "Lot", title: "批號", width: 150 },
            { field: "WarehouseId", title: "庫別", width: 150 },
            { field: "WareHouseName", title: "庫別名稱", width: 150 },
            { field: "StorageId", title: "儲位", width: 150 },
            { field: "Qty", title: "庫存", width: 100 }
        ]
        ,
        change: function (e) {
            var value = this.value();
            $("[name='Quantity']").data("kendoNumericTextBox").max(this.dataItem().Qty);
        }
    });
   
}

function OutWareHouseName(Container, options) {
    $("<span name='" + options.field + "'>" + options.model[options.field] + "<span>").appendTo(Container);
}

function InWareHouseName(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇庫別",
        dataTextField: "WareHouseName",
        dataValueField: "WarehouseId",
        text: options.model["InWareHouseName"],
        value: options.model["InWareHouseId"],
        filter: "contains",
        filterFields: ["WareHouseName", "WarehouseId"],
        height: 400,
        autoBind: false,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/WareHouse/getWarehouseInfo/"
                }
            }
        },
        columns: [
            { field: "WarehouseId", title: "庫別", width: 150 },
            { field: "WareHouseName", title: "庫別名稱", width: 150 }
        ]
    });
}
