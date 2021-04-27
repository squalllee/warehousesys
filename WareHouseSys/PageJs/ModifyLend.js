var doLendGrid;
function ModifyLendInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    $("#BtnSaveLend").click(function () {
        var validator = $("#LendModifyForm").kendoValidator().data("kendoValidator");
        if (AttachmentArray.length === 0) {
            alert('必需上傳已核可的借料單!');
            return;
        }

        if (validator.validate()) {
            var obj = {};
            obj.lendHeaderViewModel = formToJSON($("#LendModifyForm"));
            obj.attachment = AttachmentArray;
            url = $("#LendModifyForm").data("saveurl");

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
                    $("#ModifyLendDialog").data("kendoWindow").close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#doLendGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#doLendGrid").data("updateurl"),
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
                    OrderNo: { type: "string", editable: false },
                    MaterialNo: { type: "string", editable: false},
                    MaterialName: { type: "string", editable: false },
                    Spec: { type: "string", editable: false },
                    Quantity: { editable: false },
                    LendQty: {
                        type: "number",
                        validation: {
                            min: 0,
                            required: true
                        }
                    },
                    WareHouseName: { type: "string" }
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" && e.field === "WareHouseName") {
                var model = e.items[0];
                var selectedData = $("[name='WareHouseName']").data("kendoMultiColumnComboBox").dataItem();

               
                model.WareHouseName = selectedData.WareHouseName;
                model.WareHouseId = selectedData.WarehouseId;
                model.StorageId = selectedData.StorageId;
                model.Lot = selectedData.Lot;

                $("[name='StorageId']").text(model.StorageId);
                $("[name='Lot']").text(model.Lot);
              
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


    doLendGrid = $("#doLendGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1300,
        sortable: true,
        columns: [
            { command: ["edit"], title: "&nbsp;", width: 100, locked: true, lockable: true },
            {
                field: "MaterialNo",
                title: "品號",
                width: 200

            }, {
                field: "MaterialName",
                title: "品名",
                width: 200
            },
            {
                field: "Spec",
                title: "規格",
                width: 200
            }, {
                field: "WareHouseName",
                title: "庫別",
                width: 200,
                editor: MultiWareHouseCombobox,
                template: '#= WareHouseName !== null ? WareHouseName : "" #'
            }, {
                field: "StorageId",
                title: "儲位",
                editor: EditSpan,
                width: 100
            }, {
                field: "Lot",
                title: "批號",
                editor: EditSpan,
                width: 100
            },
            {
                field: "Quantity",
                title: "申請數量",
                width: 100
            },
            {
                field: "LendQty",
                title: "借出數量",
                width: 100
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
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

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + options.model[options.field] + "<span>").appendTo(Container);
}