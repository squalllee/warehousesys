function OpenFillInboundData(e) {
    DialogInit($("#FillInboundDataDialog"), "入庫作業", {
        "儲存": function () {
            var obj = formToJSON("#InboundForm");

            var datas = InboundDetailUpdateGrid.dataSource.view();

            var ret = true;
            $.each(datas, function (i) {
                if (datas[i].InboundQty === "0" || datas[i].warehouseInfo === null || datas[i].Storage === null) {
                    Warning("警告!", "請先入庫再儲存!");
                    ret = false;
                    return false;
                }
            });

            if (!ret) return;

            $.ajax({
                url: $("#FillInboundDataDialog").data("saveurl"),
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲成成功');
                    InboundGrid.data("kendoGrid").dataSource.read();
                    $("#FillInboundDataDialog").dialog("close");
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲成失敗');
                }
            }); 
        },
        "關閉": function () {
            $(this).dialog("close");
        }
    }, 1850, 850);

    var row = e.closest("tr");
    var grid = $("#InboundGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);
    var obj = {
        OrderNo: dataItem.OrderNo,
        //InboundMan: dataItem.InboundMan,
        InboundMan: dataItem.InboundManId,
        InboundManId: dataItem.InboundManId,
        InboundDate: kendo.toString(dataItem.InboundDate, "yyyy/MM/dd")
    };

    $('#FillInboundDataDialog').load($("#FillInboundDataDialog").data("url"), obj, function () {
        $(this).dialog('open');
        FillInboundDataInit(obj);
    });
}
function dataSource_error(e) {
    alert('發生錯誤');
}

var InboundDetailUpdateGrid;
function FillInboundDataInit(obj) {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#InboundDetailUpdateGrid").data("url") + "?OrderNo=" + obj.OrderNo,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#InboundDetailUpdateGrid").data("updateurl") ,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            destroy: {
                url: $("#InboundDetailUpdateGrid").data("destroy"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                if ((operation === "update" || operation === "destroy") && options.models) {
                    return JSON.stringify({
                        OrderNo: options.models[0].OrderNo,

                        SerialNo: options.models[0].SerialNo,

                        InboundQty: options.models[0].InboundQty,

                        Quantity: options.models[0].Quantity,

                        WarehouseId: options.models[0].WarehouseId,

                        StorageId: options.models[0].Storage.StorageId,

                        Storage: options.models[0].Storage,

                        OccupiedStorageId: options.models[0].OccupiedStorage === null ? "" : options.models[0].OccupiedStorageId,

                        Note: options.models[0].Note,

                        Lot: options.models[0].Lot,

                        MaterialNo: options.models[0].MaterialNo,

                        Expiration: kendo.toString(options.models[0].Expiration, "yyyy/MM/dd"),

                        SaveStockAlert: options.models[0].SaveStockAlert,

                        warehouseInfo: {
                            WareHouseName: options.models[0].warehouseInfo.WareHouseName,
                            WarehouseId: options.models[0].warehouseInfo.WarehouseId
                        }
                    });
                }
                else {
                    return JSON.stringify(options);
                }
            }
        },
        schema: {
            data: "data",
            total: "Total",
            model: {
                id: "SerialNo",
                fields: {
                    SerialNo: { editable: false, type: "string" },
                    MaterialNo: { editable: false, type: "string" },
                    MaterialName: { editable: false, type: "string" },
                    Expiration: { type: "date" },
                    Quantity: { editable: false, type: "number" }, 
                    Note: { type: "string" }
                }
            }
        },
        serverPaging: true,
        pageSize: 10,
        batch: true,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });
    datasource.bind("error", dataSource_error);

    InboundDetailUpdateGrid = $("#InboundDetailUpdateGrid").kendoGrid({
        dataSource: datasource ,
        resizable: true,
        scrollable: true,
        height: 600,
        width: 1800,
        sortable: true,
        pageable: true,
        columns: [
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
                width: 400
        },
          {
            field: "Expiration",
            title: "有效期限",
                template: '#= Expiration !== null ? kendo.toString(Expiration, "yyyy/MM/dd") : ""#',
                width: 200
        }, {
            field: "Quantity",
                title: "應入庫量",
                width: 100
        }, {
            field: "InboundQty",
                title: "已入庫量",
                width: 100,
                template: '#=InboundQty#',
                editor: EditQty
            }, {
                field: "warehouseInfo",
                title: "入庫倉",
                width: 200,
                editor: WarehouseIdDropDownEditor,
                template: '#= warehouseInfo !== null ? warehouseInfo.WareHouseName : ""#'

            }
            , {
                field: "Lot",
                title: "批號",
                width: 150
            },{
            field: "Storage",
            title: "擺放儲位",
            width: 150,
            editor: StorageAutoComplete,
            template: '#= Storage !== null ? Storage.StorageId : ""#'
        }, {
            field: "OccupiedStorage",
            title: "佔用儲位",
            width: 150,
            editor: StorageAutoComplete,
            template: '#= OccupiedStorage !== null ? OccupiedStorage.StorageId : ""#'
            }, {
            field: "SaveStockAlert",
            title: "存量管制",
            width: 150,
            editor: EditCheck,
            template: '#= SaveStockAlert ? "是" : "否" #'
            },{
            field: "Note",
            title: "備註",
            width: 150
            },
            { command: ["edit", "destroy",{ text: "複制", click: Copy }], title: "&nbsp;",  width: 250, locked: true, lockable: true}
        ],
        editable: "inline"
    }).data("kendoGrid");

    
}

function EditQty(Container, options) {
    var maxValue = parseInt(getInboundQty(options.model["MaterialNo"])) + parseInt(options.model[options.field]);
    $('<input value="' + options.model[options.field]+'" name="' + options.field + '" type="number" title="numeric" max="' + maxValue +'"  min="1" step="1" style="width: 100 %; " />').appendTo(Container).kendoNumericTextBox();
}

function Copy(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var inboundQty = getInboundQty(dataItem.MaterialNo);
    
    if (inboundQty === 0) {
        alert('已到達入庫量，無法再複制!');
        return;
    }

    dataItem.InboundQty = inboundQty;
    $.ajax({
        url: $("#InboundDetailUpdateGrid").data("addurl"),
        dataType: 'text',
        type: 'post',
        contentType: 'application/json',
        data: JSON.stringify(dataItem),
        processData: false,
        success: function (data, textStatus, jQxhr) {
            InboundDetailUpdateGrid.dataSource.read();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert('複制失敗');
        }
    }); 
}

function getInboundQty(MaterialNo) {
    var datas = InboundDetailUpdateGrid.dataSource.view();
    var InboundQty = 0;
    var Quantity = 0;

    $.each(datas, function (index) {
        if (datas[index].MaterialNo === MaterialNo) {
            InboundQty += datas[index].InboundQty;
            if (Quantity === 0) {
                Quantity = datas[index].Quantity;
            }
        }
    });
    if (Quantity - InboundQty > 0) {
        return Quantity -  InboundQty;
    }
    else {
        return 0;
    }
}

function WarehouseIdDropDownEditor(container, options) {
    $('<input required data-required-msg="必需輸入" name="' + options.field + '"/>')
        .appendTo(container)
        .kendoDropDownList({
            autoBind: false,
            dataTextField: "WareHouseName",
            dataValueField: "WarehouseId",
            dataSource: {
                transport: {
                    read: "../api/WareHouse/getWarehouseInfo",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST"
                }
            }
            , change: selectStorage
        });
}

function setStorage(warehouseId, comObj) {
    var StorageDataSource = new kendo.data.DataSource({
        transport: {
            read: "../api/getStorageInfo/" + warehouseId,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST"
        }
    });
    StorageDataSource.read();
    $(comObj).data('kendoComboBox').setDataSource(StorageDataSource);

}

function selectStorage(e) {
    var StorageDataSource = new kendo.data.DataSource({
        transport: {
            read: "../api/getStorageInfo/" + e.sender._old,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST"
        }
    });
    StorageDataSource.read();
    $("[name='Storage']").data('kendoComboBox').setDataSource(StorageDataSource);
    if ($("[name='Storage']") !== null)
        $("[name='Storage']").data('kendoComboBox').text('');

    $("[name='OccupiedStorage']").data('kendoComboBox').setDataSource(StorageDataSource);
    if ($("[name='OccupiedStorage']") !== null)
        $("[name='OccupiedStorage']").data('kendoComboBox').text('');
}

function EditCheck(Container, options) {
    $("<input type='checkbox' name='" + options.field + "' class='k-checkbox' checked='" + options.model[options.field] + "'>").appendTo(Container);
}

function StorageAutoComplete(container, options) {
    if (options.field === "OccupiedStorage") {
        $('<input name="' + options.field + '"/>')
            .appendTo(container)
            .kendoComboBox({
                autoBind: false,
                dataTextField: 'StorageId',
                dataValueField: "StorageId",
                filter: "contains",
                minLength: 2
            });
    }
    else {
        $('<input required data-required-msg="必需輸入" name="' + options.field + '"/>')
            .appendTo(container)
            .kendoComboBox({
                autoBind: false,
                dataTextField: 'StorageId',
                dataValueField: "StorageId",
                filter: "contains",
                minLength: 2
            });      
    }

    if (options.model.warehouseInfo !== null)
        setStorage(options.model.warehouseInfo.WarehouseId, $('[name="' + options.field+'"]'));
    
}