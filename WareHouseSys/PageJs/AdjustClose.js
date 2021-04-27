function AdjustCloseInit() {
    event.preventDefault();
    $("#AdjustCloseBtn").click(function () {
        if (AttachmentArray.length === 0) {
            alert('必需上傳已核可的調整單!');
            return;
        }
        var obj = {};
        obj.attachments = AttachmentArray;
        obj.OrderNo = $("[name='OrderNo']").val();

        $.ajax({
            url: "../api/Adjust/CloseAdjust",
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            data: JSON.stringify(obj),
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('儲存成功');
                $("#grid").data("kendoGrid").dataSource.read();
                AdjustCloseDialog.close();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('儲存失敗');
            }
        });
    });
    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    var AdJustCloseDatasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "../Adjust/AdjustBodyViewGrid?OrderNo=" + $("[name='OrderNo']").val(),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {

                return JSON.stringify(options);
            }
        },
        schema: {
            data: "data"
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });


    AdjustCloseGrid = $("#AdjustCloseGrid").kendoGrid({
        dataSource: AdJustCloseDatasource,
        resizable: true,
        height: 600,
        width: 1850,
        sortable: true,
        scrollable: true,
        columns: [
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
                width: 250
            }, {
                field: "Unit",
                title: "單位",
                width: 200
            }, {
                field: "WareHouseName",
                title: "倉庫",
                width: 200
            }, {
                field: "StorageId",
                title: "儲位",
                width: 200
            }, {
                field: "Lot",
                title: "批號",
                width: 200
            }, {
                field: "StockQty",
                title: "庫存量",
                width: 100
            }, {
                field: "Quantity",
                title: "實際數量",
                width: 100
            }, {
                field: "Reason",
                title: "調整原因",
                width: 300
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}