var doBackGrid;
function doBackInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd',
        onSelect: function (dateText) {
            $.ajax({
                url: "../api/Lend/getLimiteDate/" + $("[name='LendNo']").val(),
                dataType: 'text',
                type: 'get',
                contentType: 'application/json',
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    var switchOverdue = $("#Overdue").data("kendoSwitch");
                    if (new Date(dateText) > new Date(data)) {
                        switchOverdue.check(true);
                    }
                    else {
                        switchOverdue.check(false);
                    }
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('取得歸還期限失敗!');
                }
            });
            console.log("Selected date: " + dateText + "; input's current value: " + this.value);
        }
    });

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    $("#BtnSaveBack").click(function (e) {
        if (AttachmentArray.length === 0) {
            alert('必需上傳已核可的還料單!');
            return;
        }

        var validator = $("#doBackForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var switchOverdue = $("#Overdue").data("kendoSwitch");
            var obj = {};
            obj.backHeaderViewModel = formToJSON($("#doBackForm"));
            obj.attachment = AttachmentArray;
            obj.backHeaderViewModel.Overdue = switchOverdue.check();

            var url = $("#doBackForm").data("saveurl");

            $.ajax({
                url: url,
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    AttachmentArray = [];
                    $('#imgList').empty();
                    BackMaterialGrid.dataSource.read();
                    $("#doBackDialog").data("kendoWindow").close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    var datasource = new kendo.data.DataSource({
        offlineStorage: "offlineStorage",
        transport: {
            read: {
                url: $("#doBackGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#doBackGrid").data("updateurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                return JSON.stringify(options);
            }
        },
        schema: {
            data: "data",
            model: {
                id: "MaterialNo",
                fields: {
                    OrderNo: { type: "string", editable: false },
                    MaterialNo: { type: "string", editable: false },
                    MaterialName: { type: "string", editable: false },
                    Spec: { type: "string", editable: false },
                    Lot: { editable: false },
                    LendQty: { editable: false },
                    Quantity: { editable: false },
                    BackQty: {
                        type: "number",
                        validation: {
                            min: 1,
                            required: true
                        }
                    }
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
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });
    datasource.bind("error", dataSource_error);

    doBackGrid = $("#doBackGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 500,
        width: 1450,
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
                width: 100
            },
            {
                field: "LendQty",
                title: "借出數量",
                width: 100
            },
            {
                field: "Quantity",
                title: "歸還數量",
                width: 100
            },
            {
                field: "BackQty",
                title: "實還量",
                editor: EditBackQty,
                width: 100
            }
            ,
            {
                field: "Note",
                title: "備註",
                width: 100
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}

function MultiWareHouseCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇歸還庫別",
        dataTextField: "WareHouseName",
        dataValueField: "WarehouseId",
        filter: "contains",
        filterFields: ["WarehouseId", "WareHouseName", "StorageId"],
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

function EditBackQty(Container, options) {
    $('<input name="' + options.field + '" type="number" title="numeric"  min="0" max="' + options.model.Quantity + '" step="1" style="width: 100 %; " />').appendTo(Container).kendoNumericTextBox();
}