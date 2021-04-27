function TransferToScrapInit() {
    $("#TransferScrap").click(function (e) {
        event.preventDefault();
        var validator = $("#ScrapForm").kendoValidator().data("kendoValidator");
        if (validator.validate()) 
        {
            
            var grid = $("#PickingScrapGrid").data("kendoGrid");

            var sel = $("input:checked", grid.tbody).closest("tr");

            var items = [];
            $.each(sel, function (idx, row) {
                var item = grid.dataItem(row);
                items.push(item);
            });

            if (items.length === 0) {
                alert('必需至少選擇一筆要報廢的品項!');
                return;
            }

            var obj = {};
            obj.scrapHeaderViewModel = formToJSON($("#ScrapForm"));
            obj.OrderNo = $("#PickingNo").val();
            obj.ScrapBodies = items;

            $.ajax({
                url: $("#PickingScrapGrid").data("transferurl"),
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');

                    TransferToScrapDialog.close();
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
                url: $("#PickingScrapGrid").data("url"),
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
                    MaterialName: { type: "string", editable: false },
                    Spec: { type: "string", editable: false },
                    WareHouseName: { type: "string", editable: false },
                    StorageId: { type: "string", editable: false },
                    Lot: { type: "string", editable: false },
                    Note: { type: "string", editable: false },
                    PickedQty: { type: "number", editable: false },
                    ScrapedQty: { type: "number", editable: false },
                    CanScrapQty: { type: "number", validation: { min: 0 } }
                }
            }
        }
    });

    $("#PickingScrapGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 300,
        sortable: true,
        columns: [
            { selectable: true, width: "50px" },
            {
                command: ["edit"],
                title: "&nbsp;",
                width: 150,
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
                field: "ScrapedQty",
                title: "已報廢量",
                width: 100
            }, {
                field: "CanScrapQty",
                title: "報廢量",
                width: 100,
                editor: CanScrapQty
            }, {
                field: "Note",
                title: "備註",
                width: 150
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}

function CanScrapQty(Container, options) {
    $("<input name='" + options.field + "' type='number'  step='1' max=" + (options.model.PickedQty - options.model.ScrapedQty) + "  style = 'width: 100 %;' /> ").appendTo(Container).kendoNumericTextBox();
}