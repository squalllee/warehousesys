function ScrapCloseInit() {
    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    $("#BtnSaveScrap").click(function () {
        if (AttachmentArray.length === 0) {
            alert('必需上傳已核可的報廢單!');
            return;
        }
        var obj = {};
        obj.attachments = AttachmentArray;
        obj.OrderNo = $("[name='OrderNo']").val();

        $.ajax({
            url: "../api/Scrap/CloseScrap",
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            data: JSON.stringify(obj),
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('儲存成功');
                $("#grid").data("kendoGrid").dataSource.read();
                ScrapCloseDialog.close();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('儲存失敗');
            }
        });
    });

    var ScrapDetailDatasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "../Scrap/ScrapBodyViewGrid?OrderNo=" + $("[name='OrderNo']").val(),
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
        schema: {
            data: "data",
            model: {
                id: "MaterialNo"
            }
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });

    ScrapCloseGrid = $("#ScrapCloseGrid").kendoGrid({
        dataSource: ScrapDetailDatasource,
        resizable: true,
        height: 400,
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
                width: 200
            }, {
                field: "MaterialClass",
                title: "材質分類",
                template: "#=JSON.stringify(MaterialClass).replace('[','').replace(/\"/g,'').replace(']','')#",
                width: 200
            }, {
                field: "Quantity",
                title: "報廢量",
                width: 100
            }
        ]
    }).data("kendoGrid");
}