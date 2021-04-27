function ReqMaterialAddInit() {
    $("#BtnSave").click(function (e) {
        var validator = $("#ReqMaterialAddForm").kendoValidator().data("kendoValidator");
        if (validator.validate()) {
            var obj = {};
            obj.reqMaterialHeader = formToJSON($("#ReqMaterialAddForm"));

            obj.reqMaterialBodies = ReqMaterialGrid.dataSource.view();
            var items = ReqMaterialGrid.dataSource.view();
            if (items.length != 0) {
                $.ajax({
                    url: "../api/ReqMaterialInfo/SaveReqMaterial",
                    dataType: 'text',
                    type: 'post',
                    contentType: 'application/json',
                    data: JSON.stringify(obj),
                    processData: false,
                    success: function (data, textStatus, jQxhr) {
                        alert('儲存成功');
                        ReqMaterialAddDialog.close();
                        $("#grid").data("kendoGrid").dataSource.read();
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        alert('儲存失敗');
                    }
                });
            }
            else {
                alert('無資料');
            }
        }
    });

    $("#BtnUpdate").click(function (e) {
        var validator = $("#ReqMaterialAddForm").kendoValidator().data("kendoValidator");
        if (validator.validate()) {

            var obj = formToJSON($("#ReqMaterialAddForm"));
            obj.OrderNo = $("[name='OrderNo']").val();

            $.ajax({
                url: "../api/ReqMaterialInfo/updateReqMaterialHeader",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    ReqMaterialupdateDialog.close();
                    $("#grid").data("kendoGrid").dataSource.read();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    var ReqMaterialAddDatasource = new kendo.data.DataSource({
        offlineStorage: "products-offline",
        transport: {
            read: {
                url: "../ReqMaterialInfo/ReqMaterialBodies?OrderNo=" + $("[name='OrderNo']").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            create: {
                url: "../api/ReqMaterialInfo/addReqMaterial",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
                
            },
            update: {
                url: "../api/ReqMaterialInfo/updateReqMaterial",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            destroy: {
                url: "../api/ReqMaterialInfo/deleteReqMaterial",
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
                ReqMaterialAddDatasource.read();
            }
        },
        schema: {
            data: "data",
            model: {
                id: "SerialNo",
                fields: {
                    MaterialName: { type: "string", validation: { required: true } },
                    //EqQuantity: { type: "number" },
                    ROP: { defaultValue:'0'},
                    SafetyStock: { type: "number"},
                    EstUnitPrice: { type: "number", validation: { required: true, min: 1 } },
                    EstPurPeriod: { type: "number", validation: { required: true, min: 1 } },
                    Length: { type: "number", min: 0 },
                    Witdh: { type: "number", min: 0 },
                    Height: { type: "number", min: 0 },
                    weight: { type: "number", min: 0 },
                    EstAnnConsumption: { type: "number", validation: { required: true, min: 0 } },
                    IsFix: {type:"boolean"},
                    IsDangerous: { type: "boolean" },
                    IsLimitTime: { type: "boolean" }, 
                    SpecifyBrand: {type:"boolean"},
                    //Expiration: { type: "number" },
                    VendorId: { type: "string", validation: { required: true } },
                    Spec: { type: "string", validation: { required: true } }
                }

            }
        },
        change: function (e) {
           
            if (e.action === "itemchange") {
                if (e.field === "ReplaceNo") {
                    model = e.items[0];
                    var selectedData = $("[name='ReplaceNo']").data("kendoMultiColumnComboBox").dataItem();

                    model.ReplaceNo = selectedData.MaterialNo;
                }
                else if (e.field === "UnitName") {
                    model = e.items[0];
                    selectedData = $("[name='UnitName']").data("kendoMultiColumnComboBox").dataItem();

                    model.Unit = selectedData.UnitNo;
                    model.UnitName = selectedData.UnitName;
                }
                else if (e.field === "LineName") {
                    model = e.items[0];
                    selectedData = $("[name='LineName']").data("kendoComboBox").dataItem();

                    model.LineAbb = selectedData.value;
                    model.LineName = selectedData.text;
                }
                else if (e.field === "SystemName") {
                    model = e.items[0];
                    selectedData = $("[name='SystemName']").data("kendoMultiColumnComboBox").dataItem();

                    model.SystemId = selectedData.SystemId;
                    model.SystemName = selectedData.SystemName;
                }
                else if (e.field === "SubSystemName") {
                    model = e.items[0];
                    selectedData = $("[name='SubSystemName']").data("kendoMultiColumnComboBox").dataItem();

                    model.SubSystemId = selectedData.SubSystemId;
                    model.SubSystemName = selectedData.SubSystemName;
                }
                else if (e.field === "FixClassName") {
                    model = e.items[0];
                    selectedData = $("[name='FixClassName']").data("kendoMultiColumnComboBox").dataItem();

                    model.FixClass = selectedData.value;
                    model.FixClassName = selectedData.text;
                }
                else if (e.field === "AffectClassName") {
                    model = e.items[0];
                    selectedData = $("[name='AffectClassName']").data("kendoMultiColumnComboBox").dataItem();

                    model.AffectClass = selectedData.value;
                    model.AffectClassName = selectedData.text;
                }
                
            }
        },
        serverPaging: true,
        pageSize: 1000,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });

    if ($("[name='OrderNo']").val() === "") {
        ReqMaterialAddDatasource.online(false);
    }
    else {
        ReqMaterialAddDatasource.online(true);
    }
    

    ReqMaterialGrid = $("#ReqMaterialGrid").kendoGrid({
        dataSource: ReqMaterialAddDatasource,
        resizable: true,
        height: 600,
        width : 1850,
        sortable: true,
        toolbar: [
            { name: "create", text: "新增項次" }
        ],
        columns: [
            { command: ["edit", "destroy"], title: "&nbsp;", width: 100, locked: true, lockable: true },
            {
                field: "MaterialName",
                title: "品名",
                width: 200
            },
            {
                field: "ReplaceNo",
                title: "替代件料號",
                editor: MultiMaterialCombobox,
                width: 200
            },
            {
                field: "Spec",
                title: "規格",
                width: 300
            },
            {
                field: "UnitName",
                title: "計量單位",
                editor: UnitCombobox,
                width: 200
            }, {
                field: "LineName",
                title: "線別/設備別",
                editor: LineCombobox,
                width: 130
            }, {
                field: "SystemName",
                title: "系統別",
                editor: SystemCombobox,
                width: 150
            }
            , {
                field: "SubSystemName",
                title: "子系統別",
                editor: SubSystemCombobox,
                width: 200
            }
            , {
                field: "EqQuantity",
                title: "設備零件數量",
                width: 150
            }, {
                field: "SafetyStock",
                title: "安全量",
                width: 150
            }, {
                field: "EstUnitPrice",
                title: "預估單價",
                width: 150
            },{
                field: "Length",
                title: "物料尺寸-長(cm)",
                width: 150
            }, {
                field: "Witdh",
                title: "物料尺寸-寬(cm)",
                width: 150
            }, {
                field: "Height",
                title: "物料尺寸-高(cm)",
                width: 150
            }, {
                field: "weight",
                title: "物料重量-公斤(kg)",
                width: 150
            }, {
                field: "VendorId",
                title: "原廠代碼/開發",
                editor: VendorCombobox,
                width: 250
            }, {
                field: "FixClassName",
                title: "檢修分類",
                editor: FixClassCombobox,
                width: 150
            },{
                field: "AffectClassName",
                title: "影響類別",
                editor: AffectCombobox,
                width: 150
            }, {
                field: "ROP",
                title: "請購點",
                width: 150
            }, {
                field: "EstPurPeriod",
                title: "預估購備期(天)",
                width: 150
            }, {
                field: "EstAnnConsumption",
                title: "預估年耗用量",
                width: 150
            }, {
                field: "IsFix",
                title: "可修件",
                template: "#= IsFix ? '是' : '否' #",
                editor: LimitCheck,
                width: 150
            }, {
                field: "IsDangerous", 
                title: "危害物",
                template: "#= IsDangerous ? '是' : '否' #",
                editor: LimitCheck,
                width: 150
            }, {
                field: "SpecifyBrand",
                title: "指定廠牌",
                template: "#= SpecifyBrand ? '是' : '否' #",
                editor: SpecifyBrandCheck,
                width: 150
            }, {
                field: "IsLimitTime",
                title: "時限品",
                template: "#= IsLimitTime ? '是' : '否' #",
                editor: LimitCheck,
                width: 150
            }, {
                field: "Expiration",
                title: "保存期限(月)",
                width: 150
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}

function Expiration(Container, options) {
    $("<input name='" + options.field + "' type='number'  step='1' style='width: 100%;' />").appendTo(Container).kendoNumericTextBox();
}

function ROP(Container, options) {
    $("<input name='" + options.field + "' class='k-textbox'  value='0' style='width: 100%;' />").appendTo(Container);

    if (options.model[options.field] === "") {
        $("[name='ROP']").val(options.model[options.field]);
    }
    else {
        $("[name='ROP']").val("0");
    }
}

function LimitCheck(Container, options) {
    $("<input type='checkbox' name='" + options.field + "' class='k-checkbox' checked='" + options.model[options.field] + "'>").appendTo(Container);
    //$("<input type='checkbox' name='" + options.field + "' class='k-checkbox' checked='" + options.model[options.field] + "'>").appendTo(Container).change(function () {

    //    if (this.name === "IsLimitTime") {
    //        if (this.checked) {
    //            $("[name='Expiration']").data("kendoNumericTextBox").min(1);
    //        }
    //        else {
    //            $("[name='Expiration']").data("kendoNumericTextBox").min(0);
    //        }
    //    }
        
    //});
}

function SpecifyBrandCheck(Container, options) {
    
    $("<input type='checkbox' name='" + options.field + "' class='k-checkbox' checked='" + options.model[options.field] + "'>").appendTo(Container).change(function () {
        if (this.checked) {
            if ($("[name='VendorId']").data("kendoMultiColumnComboBox").value() === "LO") {
                $("[name='VendorId']").data("kendoMultiColumnComboBox").value("");
            }
        }
        else {
            $("[name='VendorId']").data("kendoMultiColumnComboBox").value("LO");
        }
    });
}

function LineCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required validationMessage='線別必需選擇' />").appendTo(Container).kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        value:"0",
        dataSource: [
            { text: "綠線", value: "0" },
            { text: "跨線共用", value: "9" }
        ]
    });
}

function UnitCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required validationMessage='計量單位必需選擇' />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇計量單位",
        dataTextField: "UnitName",
        dataValueField: "UnitNo",
        filter: "contains",
        filterFields: ["UnitName", "UnitNo"],
        height: 400,
        autoBind: true,
        // text: options.model.MaterialName,
        text: options.model.UnitName,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/MeasurementUnit/getMeasurementUnit"
                }
            }
        },
        columns: [
            { field: "UnitName", title: "單位名稱", width: 250 },
            { field: "UnitNo", title: "單位", width: 150 }
        ]
    });
}

function MultiMaterialCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text'  />").appendTo(Container).kendoMultiColumnComboBox({
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

function SystemCombobox(Container, options) {
    systemCombobox = $("<input id='SystemName' name='" + options.field + "' type='text' required validationMessage='系統別必需選擇' />").appendTo(Container).kendoMultiColumnComboBox({
        filter: "contains",
        autoBind: true,
        placeholder: "選擇系統",
        dataTextField: "SystemName",
        dataValueField: "SystemId",
        filterFields: ["SystemId", "SystemName"],
        value: options.model.SystemId,
        select: systemSelect,
        //dataBound:systemBound,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/System/getSystemInfo"
                }
            }
        },
        columns: [
            { field: "SystemName", title: "系統名稱", width: 250 },
            { field: "SystemId", title: "系統代碼", width: 150 }
        ]
    }).data("kendoComboBox");

}

function systemSelect(e) {
    var dataItem = e.dataItem;
    $("[name='SubSystemName']").data("kendoMultiColumnComboBox").dataSource.transport.options.read.url = "../api/System/getSubSystemInfo?SystemId=" + dataItem.SystemId;
    $("[name='SubSystemName']").data("kendoMultiColumnComboBox").value("");
    $("[name='SubSystemName']").data("kendoMultiColumnComboBox").dataSource.read();
}

function SubSystemCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required validationMessage='子系統別必需選擇' />").appendTo(Container).kendoMultiColumnComboBox({
        filter: "contains",
        autoBind: false,
        placeholder: "選擇子系統",
        dataTextField: "SubSystemName",
        dataValueField: "SubSystemId",
        filterFields: ["SubSystemId", "SubSystemName"],
        value: options.model.SystemId,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/System/getSubSystemInfo?SystemId=" + $("#SystemName").data("kendoMultiColumnComboBox").value()
                }
            }
        },
        columns: [
            { field: "SubSystemName", title: "子系統名稱", width: 250 },
            { field: "SubSystemId", title: "子系統代碼", width: 150 }
        ]
    }).data("kendoComboBox");

    //$("[name='SubSystemName']").data("kendoComboBox").value(options.model.SubSystemName);
}



function FixClassCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required validationMessage='檢修分類必需選擇' />").appendTo(Container).kendoMultiColumnComboBox({
        dataTextField: "text",
        dataValueField: "value",
        filter: "contains",
        filterFields: ["text", "value"],
        dataSource: [
            { text: "預檢", value: "P" },
            { text: "故檢", value: "C" },
            { text: "大修", value: "H" },
            { text: "週轉", value: "R" },
            { text: "共通", value: "G" },
            { text: "專案", value: "T" }
        ],
        columns: [
            { field: "text", title: "分類名稱", width: 250 },
            { field: "value", title: "分類代碼", width: 150 }
        ]
    });
}

function AffectCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required validationMessage='影響分類必需選擇' />").appendTo(Container).kendoMultiColumnComboBox({
        dataTextField: "text",
        dataValueField: "value",
        filter: "contains",
        filterFields: ["text", "value"],
        dataSource: [
            { text: "關鍵", value: "K" },
            { text: "罕用", value: "S" },
            { text: "重要", value: "M" },
            { text: "一般", value: "G" }
        ],
        columns: [
            { field: "text", title: "類別名稱", width: 250 },
            { field: "value", title: "類別代碼", width: 150 }
        ]
    });
}

function VendorCombobox(Container, options) {
    $("<input  name='" + options.field + "' type='text' required validationMessage='原廠代碼必需選擇' />").appendTo(Container).kendoMultiColumnComboBox({
        filter: "contains",
        autoBind: true,
        placeholder: "選擇原廠代碼",
        dataTextField: "VendorName",
        dataValueField: "VendorId",
        filterFields: ["VendorName", "VendorId"],
        value: options.model.VendorId,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/VendorInfo/getVendorInfo"
                }
            }
        },
        columns: [
            { field: "VendorName", title: "廠商名稱", width: 250 },
            { field: "VendorId", title: "廠商代碼", width: 150 }
        ],
        change: function (e) {
            var value = this.value();
            var SpecifyBrand = $("[name='SpecifyBrand']").prop("checked");
            if (SpecifyBrand) {
                if (value === "LO") {
                    alert("指定廠牌無法選擇通用，請重新選擇");
                    $("[name='VendorId']").data("kendoMultiColumnComboBox").value("");
                }
            }


            var widget = e.sender;

            if (widget.value() && widget.select() === -1) {
                //custom has been selected
                widget.value(""); //reset widget
            }
        }
    }).data("kendoComboBox");

}