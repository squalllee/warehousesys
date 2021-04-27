function InboundDialogInit() {
    $("#btnSave").click(function (e) {
        var validator = $("#InboundAddForm").kendoValidator().data("kendoValidator");
        var InboundHeader = formToJSON($("#InboundAddForm"));
        var inboundBodies = InboundAddGrid.dataSource.view();

        if (inboundBodies.length === 0) {
            alert('請新增品項');
            return;
        }
        if (validator.validate()) {

            var obj = {};
            obj.InboundHeader = InboundHeader;
            obj.inboundBodies = inboundBodies;


            $.ajax({
                url: "../api/Inbound/SaveDirectInoundData",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    InboundAddDialog.close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    localStorage.clear();
    var dataSource = new kendo.data.DataSource({
        offlineStorage: "products-offline",
        schema: {
            model: {
                id: "MaterialNo",
                fields: {
                    SerialNo: { type: "string", validation: { required: true } },
                    MaterialNo: { type: "string", validation: { required: true } },
                    Expiration: { type: "date" },
                    Quantity: { type: "number", validation: { required: true, min: 1 } },
                    WarehouseId: { type: "string", validation: { required: true } },
                    WareHouseName: { type: "string", validation: { required: true } },
                    StorageId: { type: "string", validation: { required: true } },
                    OccupiedStorageId: { type: "string" },
                    Note: { type: "string" },
                    Lot: { type: "string", validation: { required: true } },
                    SaveStockAlert: { type:"boolean"}
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange") {
                if (e.field === "WareHouseName") {
                    var model = e.items[0];
                    var selectedData = $("[name='WareHouseName']").data("kendoMultiColumnComboBox").dataItem();

                    model.WarehouseId = selectedData.WarehouseId;
                    model.WareHouseName = selectedData.WareHouseName;
                }
                else if (e.field === "MaterialNo") {
                    model = e.items[0];
                    selectedData = $("[name='MaterialNo']").data("kendoMultiColumnComboBox").dataItem();

                    model.MaterialNo = selectedData.MaterialNo;
   
                }
                
            }
        }
    });
    dataSource.online(false);

    InboundAddGrid = $("#InboundAddGrid").kendoGrid({
        dataSource: dataSource,
        //height: 300,
        width: 1750,
        height: 600,
        resizable: true,
        scrollable: true,
        toolbar: [
            { name: "create", text: "新增項次" }
        ],
        columns: [
            { command: ["edit", "destroy"], title: "&nbsp;", width: 200, locked: true, lockable: true },
            {
            field: "MaterialNo",
            title: "品號",
            width: 250,
            editor: MultiMaterialCombobox
        }, {
            field: "Expiration",
            title: "有效期限",
                width: 150,
                template: "#= kendo.toString(Expiration, 'yyyy/MM/dd') #"
        }, {
                field: "WareHouseName",
                title: "入庫倉",
                width: 150,
                editor: InWareHouse
        }, {
            field: "StorageId",
                title: "擺放儲位",
                width: 150,
                editor: StorageInfo
        }, {
            field: "OccupiedStorageId",
                title: "佔用儲位",
                width: 150,
                editor: StorageInfo
        }, {
            field: "Lot",
                title: "批號",
                width: 150
            }, {
                field: "Quantity",
                title: "應入庫量",
                width: 150
            }, {
                field: "SaveStockAlert",
                title: "存量管制",
                width: 150,
                editor: EditCheck,
                template: '#= SaveStockAlert ? "是" : "否" #'
            },{
            field: "Note",
                title: "備註",
                width: 300
        }
        ],
        editable: "inline"
    }).data("kendoGrid");
}

function EditCheck(Container, options) {
    $("<input type='checkbox' name='" + options.field + "' class='k-checkbox' checked='" + options.model[options.field] + "'>").appendTo(Container);
}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + options.model[options.field] + "<span>").appendTo(Container);
}

function MultiMaterialCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇料號",
        dataTextField: "MaterialName",
        dataValueField: "MaterialNo",
        filter: "contains",
        filterFields: ["MaterialNo", "MaterialName"],
        height: 400,
        autoBind: true,
       // text: options.model.MaterialName,
        value: options.model.MaterialNo,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/Material/getMaterialInfo"
                }
            }
        },
        columns: [
            { field: "MaterialName", title: "品名", width: 250 },
            { field: "MaterialNo", title: "料號", width: 150 }
        ]
    });
}

function InWareHouse(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇庫別",
        dataTextField: "WareHouseName",
        dataValueField: "WarehouseId",
        text: options.model.WareHouseName,
        value: options.model.WarehouseId,
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
        ],
        change: function (e) {
            $("[name='StorageId']").data("kendoMultiColumnComboBox").dataSource.transport.options.read.url = "../api/getStorageInfo/" + $("[name='WareHouseName']").data("kendoMultiColumnComboBox").value();
            $("[name='StorageId']").data("kendoMultiColumnComboBox").dataSource.read();

            $("[name='OccupiedStorageId']").data("kendoMultiColumnComboBox").dataSource.transport.options.read.url = "../api/getStorageInfo/" + $("[name='WareHouseName']").data("kendoMultiColumnComboBox").value();
            $("[name='OccupiedStorageId']").data("kendoMultiColumnComboBox").dataSource.read();
        }
    });
}

function StorageInfo(Container, options) {
    $("<input name='" + options.field + "' type='text' " + (options.field === "StorageId" ?"required":"") + "/>").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇儲位",
        dataTextField: "StorageId",
        dataValueField: "StorageId",
        value: options.model[options.field],
        filter: "contains",
        filterFields: ["StorageId"],
        height: 400,
        autoBind: false,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/getStorageInfo/" + $("[name='WareHouseName']").data("kendoMultiColumnComboBox").value()
                }
            }
        },
        columns: [
            { field: "StorageId", title: "儲位", width: 150 }
        ]
    });
}