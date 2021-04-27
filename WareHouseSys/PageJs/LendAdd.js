var LendDetailUpdateGrid;
function LendAddDialogInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    var datasource = new kendo.data.DataSource({
        offlineStorage: "offlineStorage",
        transport: {
            read: {
                url: $("#LendDetailUpdateGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#LendDetailUpdateGrid").data("updateurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            destroy: {
                url: $("#LendDetailUpdateGrid").data("deleteurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            create: {
                url: $("#LendDetailUpdateGrid").data("addurl"),
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
                $("#LendDetailUpdateGrid").data("kendoGrid").dataSource.read();
            }
        },
        schema: {
            data: "data",
            model: {
                id: "MaterialNo",
                fields: {
                    OrderNo: { type: "string" },
                    MaterialNo: { type: "string" },
                    MaterialName: { type: "string" },
                    Spec: { type: "string" },
                    Quantity: {
                        type: "number",
                        validation: {
                            min: 1,
                            required: true
                        }
                    },
                    WareHouseName: { type: "string" }
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
                model.WareHouseName = selectedData.WareHouseName;
                model.WareHouseId = selectedData.WarehouseId;

                $("[name='MaterialName']").text(model.MaterialName);
                $("[name='Spec']").text(model.Spec);
                $("[name='WareHouseName']").text(model.WareHouseName);
                //$("[name='WareHouseId']").text(model.WareHouseId);

                return;
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


    LendDetailUpdateGrid = $("#LendDetailUpdateGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1000,
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
                editor: EditSpan,
                template: '#= MaterialName !== null ? MaterialName : "" #'
            },
            {
                field: "Spec",
                title: "規格",
                editor: EditSpan,
                width: 200
            }, {
                field: "WareHouseName",
                title: "庫別",
                width: 200,
                editor: EditSpan,
                template: '#= WareHouseName !== null ? WareHouseName : "" #'
            },
            {
                field: "Quantity",
                title: "數量",
                width: 100
            }
        ],
        editable: "inline"
    }).data("kendoGrid");

    $("#BtnSaveLend").click(function (e) {
        var validator = $("#LendAddForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var obj = {};
            obj.lendHeaderViewModel = formToJSON($("#LendAddForm"));
            var url = "";
            if ($("[name='OrderNo']").val() === "") {
                obj.LendBodies = $("#LendDetailUpdateGrid").data("kendoGrid").dataSource.view();
                url = $("#LendAddForm").data("saveurl");
            }
            else {
                url = $("#LendAddForm").data("updateurl");
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
                    LendGrid.dataSource.read();
                    $("#LendAddDialog").data("kendoWindow").close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });
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
        filterFields: ["MaterialNo", "MaterialName"],
        height: 400,
        autoBind: false,
        text: options.model["MaterialNo"],
        value: options.model["MaterialName"],
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "POST",
                    url: "../api/Material/getMaterialInfoByWareHouse/" + $("[name='WGroupId']").val()
                }
            }
        },
        columns: [
            { field: "MaterialName", title: "品名", width: 250 },
            { field: "MaterialNo", title: "料號", width: 150 },
            { field: "WarehouseId", title: "庫別", width: 150 },
            { field: "WareHouseName", title: "庫別名稱", width: 150 },
            { field: "Qty", title: "庫存", width: 100 }
        ]
    });
}

function ReasonSelect(e) {
    var dataItem = this.dataItem(e.item.index());
    if (dataItem === "其它") {
        $("[name='OtherReason']").prop("disabled", false);
        $("[name='OtherReason']").prop("required", true);
        $("[name='OtherReason']").focus();
    }
    else {
        $("[name='OtherReason']").prop("disabled", true);
        $("[name='OtherReason']").prop("required", false);
        $("[name='OtherReason']").val("");
    }
}