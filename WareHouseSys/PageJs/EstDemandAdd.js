function EstDemandAddInit() {
    $("#BtnSave").click(function (e) {
        var validator = $("#EstDemandAddForm").kendoValidator().data("kendoValidator");
        var datas = EstDemandGrid.dataSource.view();
        if (datas.length === 0) {
            alert('必需輸入物料資料!');
            return;
        }
        if (validator.validate()) {
            var obj = {};
            obj.demandHeader = formToJSON($("#EstDemandAddForm"));

            obj.demandBodies = datas;

            $.ajax({
                url: "../api/EstDemand/SaveDemand",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    EstDemandAddDialog.close();
                    $("#grid").data("kendoGrid").dataSource.read();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    $("#BtnUpdate").click(function (e) {
        var validator = $("#EstDemandAddForm").kendoValidator().data("kendoValidator");
        if (validator.validate()) {

            var obj = formToJSON($("#EstDemandAddForm"));
            obj.OrderNo = $("[name='OrderNo']").val();

            $.ajax({
                url: "../api/EstDemand/updateDemandHeader",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('修改成功');
                    EstDemandAddDialog.close();
                    $("#grid").data("kendoGrid").dataSource.read();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('修改失敗');
                }
            });
        }
    });


    var EstDemandAddDatasource = new kendo.data.DataSource({
        offlineStorage: "products-offline",
        transport: {
            read: {
                url: "../EstDemand/EstDemandBodies?OrderNo=" + $("[name='OrderNo']").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            create: {
                url: "../api/EstDemand/addDemandBody",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            update: {
                url: "../api/EstDemand/updateDemandBody",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            destroy: {
                url: "../api/EstDemand/deleteDemandBody",
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
            model: {
                id: "MaterialNo",
                fields: {
                    Quantity: { type: "number", validation: { required: true, min: 1 } },
                    //EstPriceWithOutTax: {type: "number" },
                    //EstTotalPriceWithOutTax: { type: "number", validation: { required: true, min: 1}  },
                    Vendor1: { type: "string", validation: { required: true } },
                    PurchaseName: { type: "string", validation: { required: true } },
                    DemanDate: { type: "date", validation: { required: true } }
                }

            }
        },
        change: function (e) {

            if (e.action === "itemchange") {
                model = e.items[0];
                if (e.field === "MaterialNo") {
                    
                    var selectedData = $("[name='MaterialNo']").data("kendoMultiColumnComboBox").dataItem();
                    var VendorName = $("[name='VendorName']").data("kendoMultiColumnComboBox");

                    $("[name='MaterialName']").text(selectedData.MaterialName);
                    $("[name='Spec']").text(selectedData.Spec);
                    VendorName.value(selectedData.MaterialNo.substr(11));

                    model.VendorId = selectedData.MaterialNo.substr(11);
                    model.MaterialNo = selectedData.MaterialNo;
                    model.MaterialName = selectedData.MaterialName;
                    model.Spec = selectedData.Spec;
                }
                else if (e.field === "Quantity" || e.field === "EstPriceWithOutTax") {

                    if ($("[name='EstPriceWithOutTax']").data("kendoNumericTextBox").value() !== null && $("[name='Quantity']").data("kendoNumericTextBox").value() !== null) {
                        var qty = $("[name='EstPriceWithOutTax']").data("kendoNumericTextBox").value();
                        var unitPrice = $("[name='Quantity']").data("kendoNumericTextBox").value();
                        $("[name='EstTotalPriceWithOutTax']").text(qty * unitPrice);

                        model.EstTotalPriceWithOutTax = qty * unitPrice;
                    }

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
        EstDemandAddDatasource.online(false);
    }
    else {
        EstDemandAddDatasource.online(true);
    }


    EstDemandGrid = $("#EstDemandGrid").kendoGrid({
        dataSource: EstDemandAddDatasource,
        resizable: true,
        height: 600,
        width: 1850,
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
                width: 200,
                editor: EditSpan
            },
            {
                field: "Spec",
                title: "規格",
                width: 300,
                editor: EditSpan
            },
            {
                field: "Quantity",
                title: "數量",
                width: 300
            },
            {
                field: "EstPriceWithOutTax",
                title: "預估單價(未稅)",
                width: 300,
                editor: EditNumber
            },
            {
                field: "EstTotalPriceWithOutTax",
                title: "預估總額(未稅)",
                width: 300,
                editor: EditSpan
            },
            {
                field: "VendorName",
                title: "指定廠牌",
                width: 300,
                editor: VendorCombobox
            },
            {
                field: "Vendor1",
                title: "商源1",
                width: 300
            },
            {
                field: "Vendor2",
                title: "商源2",
                width: 300
            },
            {
                field: "Vendor3",
                title: "商源3",
                width: 300
            },
            {
                field: "PurchaseName",
                title: "案名",
                width: 300
            },
            {
                field: "DemanDate",
                title: "需求日期",
                template: '#= DemanDate !== null ? kendo.toString(DemanDate, "yyyy/MM/dd") : ""#',
                width: 300
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + (options.model[options.field] === undefined ? "" : options.model[options.field])  + "<span>").appendTo(Container);
}

function EditNumber(Container, options) {
    $("<input name='" + options.field + "' type='number' /> ").appendTo(Container).kendoNumericTextBox({
        format: "c",
        decimals: 3
    });
}


function MultiMaterialCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text'  required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇料號",
        dataTextField: "MaterialNo",
        dataValueField: "MaterialName",
        filter: "contains",
        filterFields: ["MaterialNo", "MaterialName"],
        height: 400,
        autoBind: true,
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

function VendorCombobox(Container, options) {
    var Vendor = $("<input  name='" + options.field + "' type='text' required validationMessage='原廠代碼必需選擇' />").appendTo(Container).kendoMultiColumnComboBox({
        filter: "contains",
        enable: false,
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