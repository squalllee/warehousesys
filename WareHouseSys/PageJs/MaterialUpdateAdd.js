function MaterialUpdateAddInit() {
    $("#BtnSave").click(function (e) {
        var validator = $("#MaterialUpdateAddForm").kendoValidator().data("kendoValidator");
        if (validator.validate()) {
            var obj = {};
            obj.materialUpdateHeader = formToJSON($("#MaterialUpdateAddForm"));

            obj.materialUpdateBodies = MaterialUpdateGrid.dataSource.view();

            $.ajax({
                url: "../api/MaterialUpdate/SaveReqMaterial",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    MaterialUpdateAddDialog.close();
                    $("#grid").data("kendoGrid").dataSource.read();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    $("#BtnUpdate").click(function (e) {
        var validator = $("#MaterialUpdateAddForm").kendoValidator().data("kendoValidator");
        if (validator.validate()) {

            var obj = formToJSON($("#MaterialUpdateAddForm"));
            obj.OrderNo = $("[name='OrderNo']").val();

            $.ajax({
                url: "../api/MaterialUpdate/updateMaterialUpdateHeader",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    MaterialupdateUpdateDialog.close();
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
                url: "../MaterialUpdate/MaterialUpdateBodies?OrderNo=" + $("[name='OrderNo']").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            create: {
                url: "../api/MaterialUpdate/addMaterialUpdate",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            update: {
                url: "../api/MaterialUpdate/updateMaterialUpdate",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            destroy: {
                url: "../api/MaterialUpdate/deleteMaterialUpdate",
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
        schema: {
            data: "data",
            total: "Total",
            model: {
                id: "OrderNo",
                fields: {
                    MaterialName: { type: "string", validation: { required: true } },
                    EqQuantity: { type: "number" },
                    SafetyStock: { type: "number" },
                    EstUnitPrice: { type: "number", validation: { required: true, min: 1 } },
                    EstPurPeriod: { type: "number", validation: { required: true, min: 1 } },
                    Length: { type: "number", min: 0 },
                    Witdh: { type: "number", min: 0 },
                    Height: { type: "number", min: 0 },
                    weight: { type: "number", min: 0 },
                    EstAnnConsumption: { type: "number", validation: { required: true, min: 1 } },
                    IsFix: { type: "boolean" },
                    IsDangerous: { type: "boolean" },
                    IsLimitTime: { type: "boolean" },
                    Expiration: { type: "number" },
                    VendorId: { type: "string", validation: { required: true } },
                    Spec: { type: "string", validation: { required: true } }
                }
            }
        },
        change: function (e) {

            if (e.action === "itemchange") {
                if (e.field === "MaterialNo") {
                    model = e.items[0];
                    var MaterialInfo = $("[name='MaterialNo']").data("kendoMultiColumnComboBox").dataItem();
                    if (MaterialInfo === undefined) {
                        model.MaterialName = "";
                        model.Spec = "";
                        model.UnitName = "";
                        model.Unit = "";
                        model.EqQuantity = "";
                        model.SafetyStock = "";
                        model.Length = "";
                        model.Witdh = "";
                        model.Height = "";
                        model.weight = "";
                        model.VendorId = "";
                        model.FixClassName = "";
                        model.AffectClassName = "";
                        model.FixClass = "";
                        model.AffectClass = "";
                        model.ROP = "";
                        model.EstPurPeriod = "";
                        model.EstAnnConsumption = "";
                        model.IsFix = false;
                        model.IsDangerous = false;
                        model.IsLimitTime = false;
                        model.Expiration = "";
                    }
                    else {
                        model.MaterialNo = MaterialInfo.MaterialNo;
                        model.MaterialName = MaterialInfo.MaterialName;
                        model.Spec = MaterialInfo.Spec;
                        model.UnitName = MaterialInfo.UnitName;
                        model.Unit = MaterialInfo.Unit;
                        model.EqQuantity = MaterialInfo.EqQuantity;
                        model.SafetyStock = MaterialInfo.SafetyStock;
                        model.Length = MaterialInfo.Length;
                        model.Witdh = MaterialInfo.Witdh;
                        model.Height = MaterialInfo.Height;
                        model.weight = MaterialInfo.weight;
                       //model.VendorId = MaterialInfo.VendorId;
                        model.FixClassName = MaterialInfo.FixClassName;
                        model.AffectClassName = MaterialInfo.AffectClassName;
                        model.FixClass = MaterialInfo.FixClass;
                        model.AffectClass = MaterialInfo.AffectClass;
                        model.ROP = MaterialInfo.ROP;
                        model.EstPurPeriod = MaterialInfo.EstPurPeriod;
                        model.EstAnnConsumption = MaterialInfo.EstAnnConsumption;
                        model.IsFix = MaterialInfo.IsFix;
                        model.IsDangerous = MaterialInfo.IsDangerous;
                        model.IsLimitTime = MaterialInfo.IsLimitTime;
                        model.Expiration = MaterialInfo.Expiration;
                        
                    }

                    $("[name='ReplaceNo']").data("kendoMultiColumnComboBox").value(model.ReplaceNo);
                    $("[name='MaterialName']").val(model.MaterialName);
                    $("[name='Spec']").val(model.Spec);
                    $("[name='UnitName']").data("kendoMultiColumnComboBox").value(model.Unit);
                    $("[name='EqQuantity']").data("kendoNumericTextBox").value(model.EqQuantity);
                    $("[name='SafetyStock']").data("kendoNumericTextBox").value(model.SafetyStock);
                    $("[name='Length']").data("kendoNumericTextBox").value(model.Length);
                    $("[name='Witdh']").data("kendoNumericTextBox").value(model.Witdh);
                    $("[name='Height']").data("kendoNumericTextBox").value(model.Height);
                    $("[name='weight']").data("kendoNumericTextBox").value(model.weight);
                    $("[name='VendorId']").text(model.VendorId);
                    $("[name='FixClassName']").data("kendoMultiColumnComboBox").value(model.FixClass);
                    $("[name='AffectClassName']").data("kendoMultiColumnComboBox").value(model.AffectClass);
                    selectedData = $("[name='FixClassName']").data("kendoMultiColumnComboBox").dataItem();
                    model.FixClassName = selectedData.text;
                    selectedData1 = $("[name='AffectClassName']").data("kendoMultiColumnComboBox").dataItem();
                    model.AffectClassName = selectedData1.text;
                    $("[name='ROP']").val(model.ROP);
                    $("[name='EstPurPeriod']").data("kendoNumericTextBox").value(model.EstPurPeriod);
                    $("[name='EstAnnConsumption']").data("kendoNumericTextBox").value(model.EstAnnConsumption);
                    if (model.IsFix) {
                        $("[name='IsFix']").attr("checked");
                    }
                    else {
                        $("[name='IsFix']").attr("");
                    }

                    if (model.IsDangerous) {
                        $("[name='IsDangerous']").attr("checked");
                    }
                    else {
                        $("[name='IsDangerous']").attr("");
                    }

                    if (model.IsLimitTime) {
                        $("[name='IsLimitTime']").attr("checked");
                    }
                    else {
                        $("[name='IsLimitTime']").attr("");
                    }
                    $("[name='Expiration']").data("kendoNumericTextBox").value(model.Expiration);
                    

                }
                else if (e.field === "ReplaceNo") {
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
        pageSize: 10,
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


    MaterialUpdateGrid = $("#MaterialUpdateGrid").kendoGrid({
        dataSource: ReqMaterialAddDatasource,
        resizable: true,
        height: 600,
        width: 1850,
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
            { command: ["edit", "destroy"], title: "&nbsp;", width: 100, locked: true, lockable: true },
            {
                field: "MaterialNo",
                title: "料號",
                editor: MultiMaterialCombobox,
                width: 200
            },
            {
                field: "MaterialName",
                title: "品名",
                editor: InputEdit,
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
                editor: InputEdit,
                width: 300
            },
            {
                field: "UnitName",
                title: "計量單位",
                editor: UnitCombobox,
                width: 200
            }, {
                field: "EqQuantity",
                title: "設備零件數量",
                editor: numberEdit,
                width: 150
            }, {
                field: "SafetyStock",
                title: "安全量",
                editor: numberEdit,
                width: 150
            }, {
                field: "Length",
                title: "物料尺寸-長(cm)",
                editor: numberEdit,
                width: 150
            }, {
                field: "Witdh",
                title: "物料尺寸-寬(cm)",
                editor: numberEdit,
                width: 150
            }, {
                field: "Height",
                title: "物料尺寸-高(cm)",
                editor: numberEdit,
                width: 150
            }, {
                field: "weight",
                title: "物料重量-公斤(kg)",
                editor: numberEdit,
                width: 150
            }, {
                field: "VendorId",
                title: "原廠代碼/開發",
                editor: EditSpan,
                width: 250
            }, {
                field: "FixClassName",
                title: "檢修分類",
                editor: FixClassCombobox,
                width: 150
            }, {
                field: "AffectClassName",
                title: "影響類別",
                editor: AffectCombobox,
                width: 150
            }, {
                field: "ROP",
                title: "請購點",
                editor: InputEdit,
                width: 150
            }, {
                field: "EstPurPeriod",
                title: "預估購備期(天)",
                editor: numberEdit,
                width: 150
            }, {
                field: "EstAnnConsumption",
                title: "預估年耗用量",
                editor: numberEdit,
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
                field: "IsLimitTime",
                title: "時限品",
                template: "#= IsLimitTime ? '是' : '否' #",
                editor: LimitCheck,
                width: 150
            }, {
                field: "Expiration",
                title: "保存期限(月)",
                editor: numberEdit,
                width: 150
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + options.model[options.field] + "<span>").appendTo(Container);
}

function InputEdit(Container, options) {
    $("<input name='" + options.field + "' type='text' class='k-textbox' style='width: 100%;' />").appendTo(Container);
    
}

function numberEdit(Container, options) {
    $("<input name='" + options.field + "' min='1' style='width: 100%;' />").appendTo(Container).kendoNumericTextBox();
}

function dateEdit(Container, options) {
    
    $("<input name='" + options.field + "'  value='10/10/2011'  style='width: 100%' />").appendTo(Container).kendoDatePicker({
        // defines the start view
        start: "day",

        // defines when the calendar should return date
        depth: "day",

        // display month and year in the input
        format: "yyyy/MM/dd",

        // specifies that DateInput is used for masking the input element
        dateInput: true
    });

}

function Expiration(Container, options) {
    $("<input name='" + options.field + "' type='number'  step='1' style='width: 100 %;' />").appendTo(Container).kendoNumericTextBox();
}

function LimitCheck(Container, options) {
    $("<input type='checkbox' name='" + options.field + "' class='k-checkbox' checked='" + options.model[options.field] + "'>").appendTo(Container).change(function () {

        if (this.name === "IsLimitTime") {
            if (this.checked) {
                $("[name='Expiration']").data("kendoNumericTextBox").min(1);
            }
            else {
                $("[name='Expiration']").data("kendoNumericTextBox").min(0);
            }
        }

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
        ],
        change: function (e) {
            $.ajax({
                url: "../api/Material/IsPurchased?MaterialNo=" + this.text(),
                dataType: 'text',
                type: 'get',
                contentType: 'application/json',
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    if (data === 'true')
                        $("[name='UnitName']").data("kendoMultiColumnComboBox").enable(false);
                    else
                        $("[name='UnitName']").data("kendoMultiColumnComboBox").enable(true);
                },
                error: function (jqXhr, textStatus, errorThrown) {
 
                }
            });
        }
    });
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
        ]
    }).data("kendoComboBox");

}