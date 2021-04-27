function MaterialReturnDetailInit() {
    $("#TransferReturn").click(function (e) {
        var grid = $("#PickingReturnGrid").data("kendoGrid");

        var sel = $("input:checked", grid.tbody).closest("tr");

        var items = [];
        $.each(sel, function (idx, row) {
            var item = grid.dataItem(row);
            items.push(item);
        });

        if (items.length === 0) {
            alert('必需至少選擇一筆要退料的品項!');
            return;
        }

        var obj = {};
        obj.OrderNo = $("#PickingNo").val();
        obj.WGroupId = $("#WGroupId").val();
        obj.ReturnBodies = items;

        $.ajax({
            url: $("#PickingReturnGrid").data("transferurl"),
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            data: JSON.stringify(obj),
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('儲存成功');

                $("#TransferReturnDialog").data("kendoWindow").close();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('儲存失敗');
            }
        });
 
    });

    var datasource = new kendo.data.DataSource({
        offlineStorage: "offlineStorage",
        transport: {
            read: {
                url: $("#PickingReturnGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            }
        },
        requestEnd: function (e) {
            datasource.online(false);
        },
        schema: {
            data: "data",
            model: {
                id: "SerialNo",
                fields: {
                    SerialNo: { type: "string", editable: false },
                    MaterialNo: { type: "string", editable: false },
                    MaterialName: { type: "string", editable: false  },
                    Spec: { type: "string", editable: false  },
                    WareHouseName: { type: "string", editable: false },
                    StorageId: { type: "string", editable: false },
                    Lot: { type: "string", editable: false },                  
                    Note: { type: "string", editable: false },
                    PickedQty: { type: "number", editable: false},
                    ReturnedQty: { type: "number", editable: false },
                    CanReturnQty: { type: "number", validation: { min: 0 }}
                }
            }
        }
    });

    $("#PickingReturnGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1700,
        sortable: true,
        columns: [
            { selectable: true, width: "50px" },
            {
                command: ["edit"],
                title: "&nbsp;",
                width: 80,
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
                width: 200
            }, {
                field: "StorageId",
                title: "儲位",
                width: 150
            }, {
                field: "Lot",
                title: "批號",
                width: 100

            },
            {
                field: "PickedQty",
                title: "領料數量",
                width: 100
            }, {
                field: "ReturnedQty",
                title: "已退量",
                width: 100
            }, {
                field: "CanReturnQty",
                title: "退料量",
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