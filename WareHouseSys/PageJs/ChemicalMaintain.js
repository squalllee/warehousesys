$(document).ready(function () {
  

    ChemicalGrid = $("#ChemicalGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: "../Chemical/ChemicalGrid",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST"
                },
                create: {
                    url: "../api/Chemical/ChemicalCreate",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST"
                },
                update: {
                    url: "../api/Chemical/ChemicalUpdate",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST"

                },
                destroy: {
                    url: "../api/Chemical/ChemicalDelete",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST"

                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            requestEnd: function (e) {
                if (e.type !== "read") {
                    ChemicalGrid.dataSource.read();
                }
            },
            schema: {
                data: "data",
                total: "Total",
                model: {
                    id: "MaterialNo",
                    fields: {
                        MaterialName: { type: "string" },
                        SDSMaterialName: { type: "string", validation: { required: true } },
                        HarmDesc: { type: "string", validation: { required: true } },
                        Maker: { type: "string", validation: { required: true } },
                        MakerAddress: { type: "string", validation: { required: true } },
                        MakerTel: { type: "string", validation: { required: true } },
                        Spec: { type: "string", editable: true },
                        Weight: { type: "number", validation: { required: true } },
                        StoreIndex: { type: "number", validation: { required: true } },
                        BottleName: { type: "string"}
                    }
                }
            },
            change: function (e) {
                if (e.action === "itemchange") {
                    model = e.items[0];
                    if (e.field === "MaterialNo") {

                        selectedData = $("[name='MaterialNo']").data("kendoMultiColumnComboBox").dataItem();
                        model.MaterialNo = selectedData.MaterialNo;
                        model.MaterialName = selectedData.MaterialName;
                        model.Spec = selectedData.Spec;

                        $("[name='MaterialName']").text(selectedData.MaterialName);
                        $("[name='Spec']").text(selectedData.Spec);
                    }
                    else if (e.field === "PhysicalStatus") {
                        selectedData = $("[name='PhysicalStatus']").data("kendoComboBox").dataItem();
                        model.PhysicalStatus = selectedData.value;
                    }
                    else if (e.field === "MaterialMix") {
                        selectedData = $("[name='MaterialMix']").data("kendoComboBox").dataItem();
                        model.MaterialMix = selectedData.value;
                    }
                    else if (e.field === "HarmLevel") {
                        selectedData = $("[name='HarmLevel']").data("kendoMultiColumnComboBox").dataItem();
                        model.HarmLevel = selectedData.harmNo;
                    }
                    else if (e.field === "HarmGroup1") {
                        selectedData = $("[name='HarmGroup1']").data("kendoComboBox").dataItem();
                        model.HarmGroup1 = selectedData.value;
                    }
                    else if (e.field === "HarmGroup2") {
                        selectedData = $("[name='HarmGroup2']").data("kendoComboBox").dataItem();
                        model.HarmGroup2 = selectedData.value;
                    }
                    else if (e.field === "CasNo") {
                        selectedData = $("[name='CasNo']").data("kendoMultiSelect").value();
                        model.CasNo = selectedData.toString();
                    }
                }
            },
            serverPaging: true,
            pageSize: 10,
            serverSorting: true,
            serverFiltering: true,
            pageable: true
        },
        height: 650,
        sortable: true,
        pageable: true,
      
        filterable: {
            extra: false,
            operators: {
                string: {
                    eq: "等於",
                    neq: "不等於"
                }
            }
        },
        dataBound: function (e) {
            var data = this.dataSource.view();

            var actionStr = "";

            for (var i = 0; i < data.length; i++) {
                var dataItem = data[i];

                if (dataItem.SDSFile !== "") {
                    actionStr = "<a href='" + $("#baseUrl").val() + "?FileName=" + dataItem.SDSFile + "' target = '_blank' > <img src='../images/pdf.png' /></a > ";
                    var tr = $("#ChemicalGrid").find("[data-uid='" + dataItem.uid + "']");
                    $(actionStr).appendTo(tr.find("[name='attatchment']"));
                    tr.find("[name='attatchment']").append();
                }

                // use the table row (tr) and data item (dataItem)
            }
        },
        toolbar: [
            { name: "create", text: "新增化學品" }
        ],
        columns: [{ command: ["edit", "destroy"], title: "&nbsp;", width: 200, locked: true, lockable: true },
            {
            field: "MaterialNo",
            title: "料號",
                width: 160,
                
            editor: MultiMaterialCombobox
            }, {
                field: "MaterialName",
                title: "品名",
                editor: EditSpan,
                template: "#= MaterialName === undefined ? '': MaterialName#",
                width: 160
            },
            {
                field: "BottleName",
                title: "物品名稱(貼紙 / 瓶身)",
                width: 250
            },
            {
                field: "Spec",
                title: "規格",
                editor: EditSpan,
                width: 250
            }, {
                field: "weight",
                title: "單位重量(ml)",
                width: 160
            }, {
            field: "SDSMaterialName",
            title: "化學品名稱",
            width: 200
        }, {
                field: "PhysicalStatus",
                title: "物理狀態",
                editor: PhysicalStatusCombobox,
            width: 150

            }, {
                field: "MaterialMix",
                title: "純物質/混合物",
                editor: MaterialMixCombobox,
                width: 150

            }, {
                field: "HarmDesc",
                title: "危害描述",
                width: 250

            }, {
                field: "HarmLevel",
                title: "臨界等級",
                editor: HarmLevelCombobox,
                width: 150

            }, {
                field: "CasNo",
                title: "CAS No",
                editor: CaseNoCombobox,
                width: 250

            }, {
                field: "HarmGroup1",
                title: "危害群組1",
                editor: HarmGroup1Combobox,
                width: 150

            }, {
                field: "HarmGroup2",
                title: "危害群組2",
                editor: HarmGroup2Combobox,
                width: 150

            }, {
                field: "Maker",
                title: "製告商",
                width: 150

            }, {
                field: "MakerAddress",
            title: "地址",
            width: 250
        },
        {
            field: "MakerTel",
            title: "電話",
            width: 150
            },
            {
                field: "StoreIndex",
                title: "安全資料索引碼",
                width: 150
            },
            {
                field: "SDSFile",
                title: "SDS文件",
                template: "<div name='attatchment' class='btn-group dropleft'></div>",
                editor: SDSFileEdit,
                width: 200
            }
        
        ],
        editable: "inline"
    }).data("kendoGrid");

});


function onSelect(e) {
    if ($("[name='MaterialNo']").data("kendoMultiColumnComboBox").dataItem() != null)
        this.options.async.saveUrl = "../Chemical/SDSUpload?MaterialNo=" + $("[name='MaterialNo']").data("kendoMultiColumnComboBox").dataItem().MaterialNo;
};

function onError(e) {
    // An array with information about the uploaded files
    var files = e.files;

    if (e.operation == "upload") {
        alert("Failed to upload " + files.length + " files");
    }
}

function SDSFileEdit(Container, options) {
    var MaterialNo = "";
    
    MaterialNo = options.model.MaterialNo;
        //if (MaterialNo === "") {
        //    alert('請先選擇品項!');
        //    return;
        //}
        $("<input id='SDSfile' name='" + options.field + "'  type='file' />").appendTo(Container).kendoUpload({
            async: {
                //chunkSize: 11000,// bytes
                saveUrl: "../Chemical/SDSUpload?MaterialNo=" + MaterialNo,
                //saveUrl: "../Chemical/SDSUpload",
                removeUrl: "../Chemical/SDSRemove?MaterialNo=" + MaterialNo,
                autoUpload: true
            },
            validation: {
                allowedExtensions: [".pdf"]
            },
            select: onSelect,
            error:onError,
            multiple: false,
            showFileList: true
        });
    
    

}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + options.model[options.field] + "<span>").appendTo(Container);
}

function PhysicalStatusCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required validationMessage='物理狀態必需選擇' />").appendTo(Container).kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        value: "固體",
        dataSource: [
            { text: "固體", value: "固體" },
            { text: "液體", value: "液體" }
        ]
    });
}

function HarmGroup1Combobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required validationMessage='危害群組必需選擇' />").appendTo(Container).kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        value: "A",
        dataSource: [
            { text: "A", value: "A" },
            { text: "B", value: "B" },
            { text: "C", value: "C" },
            { text: "D", value: "D" },
            { text: "E", value: "E" },
        ]
    });
}

function HarmGroup2Combobox(Container, options) {
    $("<input name='" + options.field + "' type='text'  />").appendTo(Container).kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        value: "",
        dataSource: [
            { text: "", value: "" },
            { text: "S", value: "S" }
        ]
    });
}


function MaterialMixCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required validationMessage='純物質/混合物必需選擇' />").appendTo(Container).kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        value: "混合物",
        dataSource: [
            { text: "混合物", value: "混合物" },
            { text: "純物質", value: "純物質" }
        ]
    });
}

function MultiMaterialCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇料號",
        dataTextField: "MaterialNo",
        dataValueField: "MaterialNo ",
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
                    url: "../api/Material/getHarmMaterialInfo"
                }
            }
        },
        columns: [
            { field: "MaterialName", title: "品名", width: 250 },
            { field: "MaterialNo", title: "料號", width: 150 },
            { field: "Spec", title: "規格", width: 250 }
        ]
    });
}

function addNew( value) {
    var widget = $("[name='CasNo']").getKendoMultiSelect();
    var dataSource = widget.dataSource;

    dataSource.add({
        value: value.toString()
    });


    dataSource.sync();
}


function CaseNoCombobox(Container, options) {
    var casNoArray = [];

    if (options.model.CasNo !== undefined && options.model.CasNo != null) {
        $(options.model.CasNo.split(',')).each(function () {
            var casNo = { value: this.toString() };

            casNoArray.push(casNo);
        });
    }

    $("<select name='" + options.field + "' ></select>").appendTo(Container).kendoMultiSelect({
        placeholder: "輸入CAS NO",
        dataTextField: "value",
        dataValueField: "value",
        dataSource: casNoArray,
        filter: "contains",
        //value: options.model.CasNo === undefined? [""]:options.model.CasNo.split(','),
        noDataTemplate: $("#noDataTemplate").html()
    });

   

}

function HarmLevelCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' />").appendTo(Container).kendoMultiColumnComboBox({
        dataTextField: "harmUpperLimit",
        dataValueField: "harmNo ",
        filter: "contains",
        filterFields: ["harmNo", "harmUpperLimit"],
        height: 400,
        autoBind: true,
        // text: options.model.MaterialName,
        value: options.model.MaterialNo,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/Chemical/getChemicalHarm"
                }
            }
        },
        columns: [
            { field: "harmNo", title: "儲存代碼", width: 250 },
            { field: "harmUpperLimit", title: "儲存上限", width: 150 }
        ]
    });
}

