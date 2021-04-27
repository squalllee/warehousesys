
function InboundClose(e) {
    DialogInit($("#InboundCloseDialog"), "結案作業", {
        "結案": function () {
            if (AttachmentArray.length === 0) {
                alert('必需上傳已核可的入庫單!');
                $(this).dialog("close");
            }
            else {
                var row = e.closest("tr");
                var grid = $("#InboundGrid").data("kendoGrid");
                var dataItem = grid.dataItem(row);

                $.ajax({
                    url: $("#InboundCloseDialog").data("saveurl") + "/" + dataItem.OrderNo,
                    dataType: 'text',
                    type: 'post',
                    contentType: 'application/json',
                    data: JSON.stringify(AttachmentArray),
                    processData: false,
                    success: function (data, textStatus, jQxhr) {
                        alert('儲成成功');
                        AttachmentArray = [];
                        $('#imgList').empty();
                        InboundGrid.data("kendoGrid").dataSource.read();
                        $("#InboundCloseDialog").dialog("close");
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        alert('儲成失敗');
                    }
                });
            }
        },
        "關閉": function () {
            $(this).dialog("close");
        }
    }, 1500, 800);

    var row = e.closest("tr");
    var grid = $("#InboundGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);
    var obj = {
        OrderNo: dataItem.OrderNo,
        InboundMan: dataItem.InboundMan,
        InboundManId: dataItem.InboundManId,
        InboundDate: kendo.toString(dataItem.InboundDate, "yyyy/MM/dd")
    };

    $('#InboundCloseDialog').load($("#InboundCloseDialog").data("url"), obj, function () {
        $(this).dialog('open');
        InboundCloseInit();
    });
}

function dataSource_error(e) {
    alert('發生錯誤');
}

var InboundCloseGrid;
function InboundCloseInit() {
    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#InboundCloseGrid").data("url") + "?OrderNo=" + $("[name='OrderNo']").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options) {
                return JSON.stringify(options);
            }
        },
        schema: {
            data: "data"
        },
        serverPaging: true,
        pageSize: 10,
        serverSorting: true,
        pageable: true
    });
    datasource.bind("error", dataSource_error);

    InboundCloseGrid = $("#InboundCloseGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        scrollable: true,
        height: 500,
        width: 1450,
        sortable: true,
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
                template: '#=InboundQty#'
            }, {
                field: "warehouseInfo",
                title: "入庫倉",
                width: 200,
                template: '#= warehouseInfo !== null ? warehouseInfo.WareHouseName : ""#'

            }
            , {
                field: "Storage",
                title: "擺放儲位",
                width: 150,
                template: '#= Storage !== null ? Storage.StorageId : ""#'
            }, {
                field: "OccupiedStorage",
                title: "佔用儲位",
                width: 150,
                editor: StorageAutoComplete,
                template: '#= OccupiedStorage !== null ? OccupiedStorage.StorageId : ""#'
            }, {
                field: "Lot",
                title: "批號",
                width: 150
            }, {
                field: "Note",
                title: "備註",
                width: 150
            }
        ]
    }).data("kendoGrid");
}