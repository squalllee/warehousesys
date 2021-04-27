function ModifyReturnInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    $("#BtnSaveReturn").click(function (e) {
        var validator = $("#ReturnForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            if (AttachmentArray.length === 0) {
                alert('必需上傳紙本退料單!');
                return;
            }

            var obj = formToJSON($("#ReturnForm"));
            obj.attachments = AttachmentArray;

            $.ajax({
                url: $("#ReturnForm").data("saveurl"),
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    AttachmentArray = [];
                    $('#imgList').empty();
                    ReturnMaterialGrid.dataSource.read();
                    $("#doReturnDialog").data("kendoWindow").close();
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
                url: $("#doReturnGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#doReturnGrid").data("updateurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options) {
                return JSON.stringify(options);
            }
        },
        schema: {
            data: "data",
            model: {
                id: "SerialNo",
                fields: {
                    SerialNo: { type: "string", editable: false },
                    MaterialNo: { type: "string", editable: false },
                    MaterialName: { type: "string", editable: false },
                    Spec: { type: "string", editable: false },
                    Lot: { type: "string", editable: false },
                    Note: { type: "string" },
                    Quantity: { type: "number", editable: false },
                    ReturnQty: { type: "number", validation: { min: 0 } }
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" && e.field === "WareHouseName") {
                var model = e.items[0];
                var selectedData = $("[name='WareHouseName']").data("kendoMultiColumnComboBox").dataItem();

                model.WareHouseId = selectedData.WarehouseId;
                model.WareHouseName = selectedData.WareHouseName;
                model.StorageId = selectedData.StorageId;

                $("[name='StorageId']").text(model.StorageId);


                return;
            }
            if (e.action === "add") {
                e.items[0].PerformancePeriod = formattedDate(e.items[0].PerformancePeriod);
            }
        },
        serverPaging: true,
        pageSize: 10,
        serverSorting: true,
        serverFiltering: true,
        pageable: true
    });

    $("#doReturnGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1650,
        sortable: true,
        columns: [
            {
                command: ["edit"],
                title: "&nbsp;",
                width: 200,
                locked: true,
                lockable: true
            },
            {
                field: "SerialNo",
                title: "序號",
                width: 50

            }, {
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
                editor: MultiWareHouseCombobox,
                width: 200
            }, {
                field: "StorageId",
                title: "儲位",
                editor: EditSpan,
                width: 150
            }, {
                field: "Lot",
                title: "批號",
                width: 100

            },
            {
                field: "Quantity",
                title: "退料量",
                width: 100
            }, {
                field: "ReturnQty",
                title: "收料量",
                width: 100
            }, {
                field: "Note",
                title: "備註",
                width: 150
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
        filterFields: ["WarehouseId", "WareHouseName","StorageId"],
        height: 400,
        autoBind: false,
        text: options.model["WareHouseName"],
        value: options.model["WareHouseId"],
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/WareHouse/getWarehouseInfoWithStorage"
                }
            }
        },
        columns: [
            { field: "WarehouseId", title: "庫別", width: 150 },
            { field: "WareHouseName", title: "庫別名稱", width: 150 },
            { field: "StorageId", title: "儲位", width: 150 }
        ]
    });

}

function EditSpan(Container, options) {
    $("<span name='" + options.field + "'>" + (options.model[options.field] === undefined ? '' : options.model[options.field]) + "<span>").appendTo(Container);
}