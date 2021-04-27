function openDetail(e) {
    DialogInit($("#InboundDetailDialog"), "入庫明細", {
        "關閉": function () {
            $(this).dialog("close");
        }
    }, $(document.body).width(), 900);    
    var row = e.closest("tr");
    var grid = $("#InboundGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);
    var obj = {
        OrderNo: dataItem.OrderNo,
        InboundMan: dataItem.InboundMan,
        InboundManId: dataItem.InboundManId,
        InboundDate: kendo.toString(dataItem.InboundDate, "yyyy/MM/dd")
    };

    $('#InboundDetailDialog').load($("#InboundDetailDialog").data("url"), obj, function () {
        $(this).dialog('open');
        InboundDetailInit();
    });
}

function InboundDetailInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

   // $("#InboundDetailDialog").kendoWindow({
   //     width: $(document.body).width(),
   //     position: {
   //         top: "20px",
   //         left: "15%"
   //     }
   // })

    $("#InboundDetailGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#InboundDetailGrid").data("url") + "?OrderNo=" + $("#InboundForm").find("[name='OrderNo']").val(),
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
                total: "Total",
                model: {
                    fields: {
                        SerialNo: { type: "string" },
                        MaterialNo: { type: "string" },
                        MaterialName: { type: "string" },
                        Expiration: { type: "string" },
                        Quantity: {type:"string"},
                        InboundQty: { type: "string" },
                        WarehouseId: {type:"string"},
                        StorageId: { type: "string" },
                        OccupiedStorageId: { type: "string" },
                        Note: { type: "string" },
                        Lot: { type: "string" }
                    }
                }
            },
            serverPaging: true,
            pageSize: 10,
            serverSorting: true,
            serverFiltering: false,
            pageable: true
        },
        //height: 300,
        height: 600,
        //height: $(document.body).height(),
        //width: $(document.body).width(),
        width: $(document.body).width(),
        sortable: true,
        pageable: true,
        columns: [{
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
            width: 100
        }, {
                field: "Quantity",
            title: "應入庫量"
            }, {
                field: "InboundQty",
                title: "已入庫量"
            }, {
                field: "WarehouseId",
                title: "入庫倉"
            }, {
                field: "StorageId",
                title: "擺放儲位"
            }, {
                field: "OccupiedStorageId",
                title: "佔用儲位"
            }, {
                field: "Lot",
                title: "批號"
            }, {
                field: "Note",
                title: "備註"
            }
        ]
    });
}