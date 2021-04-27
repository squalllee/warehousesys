function AddScrapInit() {
    
    $("#ScrapUpdateBtn").click(function (e) {
        event.preventDefault();
        var validator = $("#ScrapForm").kendoValidator().data("kendoValidator");
        if (validator.validate()) {
            var obj = formToJSON($("#ScrapForm"));
            obj.OrderNo = $("[name='OrderNo']").val();
         
            $.ajax({
                url: "../api/Scrap/SaveScrapHeader",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');

                    ScrapAddDialog.close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });
    $("#ScrapNewBtn").click(function (e) {
        event.preventDefault();
        var validator = $("#ScrapForm").kendoValidator().data("kendoValidator");
        if (validator.validate()) {

            var grid = $("#ScrapAddGrid").data("kendoGrid");

            var items = grid.dataSource.view();

            if (items.length === 0) {
                alert('至少要有一筆欲報廢的品項!');
                return;
            }

            var obj = {};
            obj.scrapHeaderViewModel = formToJSON($("#ScrapForm"));
            obj.scrapBodyViewModels = items;

            $.ajax({
                url: "../api/Scrap/AddScrap",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');

                    ScrapAddDialog.close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    var ScrapAddDatasource = new kendo.data.DataSource({
        offlineStorage: "products-offline",
        transport: {
            read: {
                url: "../Scrap/ScrapBodyViewGrid?OrderNo=" + $("[name='OrderNo']").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            create: {
                url: "../api/Scrap/addScrapBody",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            update: {
                url: "../api/Scrap/updateScrapBody",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"

            },
            destroy: {
                url: "../api/Scrap/deleteScrapBody",
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
                ScrapAddDatasource.read();
            }
        },
        schema: {
            data: "data",
            parse: function (data) {
                if (data.data !== undefined) {
                    for (var i = 0; i < data.data.length; i++) {
                        data.data[i].MaterialClass = data.data[i].MaterialClass.split(",");
                    }
                }
                //else {
                //    data.MaterialClass = data.MaterialClass.split(",");
                //}
                return data;
                
                
            },
           
            model: {
                id: "MaterialNo",
                fields: {
                    MaterialNo: { type: "string" },
                    MaterialName: { type: "string" },
                    Spec: { type: "string" },
                    Lot: {type:"string"},
                    WareHouseName: { type: "string" },
                    StorageId: { type: "string" },
                    MaterialClass: { defaultValue: [] }
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange") {
                var model = e.items[0];
                if (e.field === "MaterialNo") {
                    var selectedData = $("[name='MaterialNo']").data("kendoMultiColumnComboBox").dataItem();

                    model.MaterialNo = selectedData.MaterialNo;
                    model.MaterialName = selectedData.MaterialName;
                    model.Unit = selectedData.Unit;
                    model.Spec = selectedData.Spec;

                    $("[name='MaterialName']").text(selectedData.MaterialName);
                    $("[name='Spec']").text(selectedData.Spec);
                    var widget = $("[name='Unit']").getKendoComboBox();
                    var dataSource = widget.dataSource;
                    dataSource.add({
                        Unit: model.Unit, value: model.Unit
                    });

                    dataSource.one("sync", function () {
                        widget.select(dataSource.view().length - 1);
                    });

                    dataSource.sync();

                }
                else if (e.field === "Quantity") {
                    var UnitPrice = $("[name='UnitPrice']").text();
                    var Quantity = $("[name='Quantity']").data("kendoNumericTextBox").value();

                    model.UnitPrice = UnitPrice;
                    model.TotalPrice = parseInt(UnitPrice) * parseInt(Quantity);


                    $("[name='TotalPrice']").text(model.TotalPrice);
                }
                else if (e.field === "Unit") {
                    model.Unit = $("[name='Unit']").getKendoComboBox().value();
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
        ScrapAddDatasource.online(false);
    }
    else {
        ScrapAddDatasource.online(true);
    }
   

    ScrapAddGrid = $("#ScrapAddGrid").kendoGrid({
        dataSource: ScrapAddDatasource,
        resizable: true,
        height: 500,
        width: 1350,
        sortable: true,
        scrollable: true,
        toolbar: [
            { name: "create", text: "新增項次" }
        ],
        columns: [
            { command: ["edit", "destroy"], title: "&nbsp;", width: 200, locked: true, lockable: true },
            {
                field: "MaterialNo",
                title: "品號",
                width: 200,
                editor: MultiMaterialCombobox

            }, {
                field: "MaterialName",
                title: "品名",
                editor: EditSpan,
                width: 200
            },
            {
                field: "Spec",
                title: "規格",
                editor: EditSpan,
                width: 250
            }, {
                field: "Unit",
                title: "單位",
                editor: UnitEdit,
                width: 200
            }, {
                field: "MaterialClass",
                title: "材質分類",
                editor: MaterialClassEdit,
                template: "#=JSON.stringify(MaterialClass).replace('[','').replace(/\"/g,'').replace(']','')#",
                width: 200
            },{
                field: "Quantity",
                title: "報廢量",
                editor: QuantityEdit,
                width: 100
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}

function UnitEdit(Container, options) {
    $("<input name='" + options.field + "' placeholder='選擇單位...' style='width: 100 %;' required />").appendTo(Container).kendoComboBox({
        dataTextField: "Unit",
        dataValueField: "value",
        suggest: true,
        dataSource: [
            { Unit: "批", value: "批" }
        ]
    });
}

function QuantityEdit(Container, options) {
    Quantity = $("<input name='" + options.field + "' type='number'  step='1' min='1' style='width: 100 %;' required />").appendTo(Container).kendoNumericTextBox();
}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + (options.model[options.field] === undefined ? '' : options.model[options.field]) + "<span>").appendTo(Container);
}

function MaterialClassEdit(Container, options) {
   
    //var MaterialClassList = [];
    //if (options.model.MaterialClass !== undefined) {
    //    $.each(options.model.MaterialClass, function (index) {
    //        MaterialClassList.push(options.model.MaterialClass[index]);
    //    });

    //}

    $("<select multiple='multiple' data-value-primitive='true' data-bind='value: " + options.field + "'></select>").appendTo(Container).kendoMultiSelect({
        dataSource: ["鐵", "銅", "鋁", "不鏽鋼", "廢潤滑油", "廢鉛酸電池", "混合五金"]
    });

    //$("<select multiple='multiple' ></select>").appendTo(Container).kendoMultiSelect({
     
    //    dataSource: ["鐵", "銅", "鋁", "不鏽鋼", "廢潤滑油", "廢鉛酸電池", "混合五金"],
    //    //value: $(options.model[options.field]).toArray()
    //    value: ["廢鉛酸電池", "銅", "鋁"]
    //});
}

function MultiMaterialCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇料號",
        dataTextField: "MaterialNo",
        dataValueField: "MaterialName",
        filter: "contains",
       // filterFields: ["MaterialNo", "MaterialName"],
        height: 400,
        autoBind: false,
        minLength: 2,
        text: options.model.MaterialNo,
        dataSource: {
            serverFiltering: true,
            transport: {
                read: {
                    url: "../Material/getMaterialInfoKendo"
                },
                parameterMap: function (options, operation) {

                    return { filter: $("[name='MaterialNo']").data("kendoMultiColumnComboBox").text() };
                }
            }
            
        },
        columns: [
            { field: "MaterialName", title: "品名", width: 250 },
            { field: "MaterialNo", title: "料號", width: 150 },
            { field: "Spec", title: "規格", width: 250 },
            { field: "Unit", title: "單位", width: 250 }
        ],
        change: function () {
            $.ajax({
                url: "../api/Material/getMaterialPrice/" + this.dataItem().MaterialNo + "/" + this.dataItem().Lot,
                dataType: 'text',
                type: 'get',
                contentType: 'application/json',
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    $("[name='UnitPrice']").text(data);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('失敗');
                }
            }); 
        }
    });
}
