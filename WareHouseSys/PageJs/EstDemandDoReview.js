function EstDemandReviewInit() {
    $("#EstDemandReviewBtn").click(function () {
        var obj = {};
        obj.OrderNo = $("[name='OrderNo']").val();
        obj.attachments = AttachmentArray;

        $.ajax({
            url: "../api/EstDemand/ReviewDemand",
            data: JSON.stringify(obj),
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('審核成功');
                $("#grid").data("kendoGrid").dataSource.read();
                EstDemandReviewDialog.close();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('審核失敗');
            }
        });
    });

    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    var EstDemandReviewDatasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "../EstDemand/EstDemandBodies?OrderNo=" + $("[name='OrderNo']").val(),
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
            total: "Total",
            model: {
                id: "MaterialNo",
                fields: {
                    DemanDate: { type: "date", validation: { required: true } }
                }

            }
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });

    $("#EstDemandReviewGrid").kendoGrid({
        dataSource: EstDemandReviewDatasource,
        resizable: true,
        height: 600,
        width: 1850,
        sortable: true,
        pageable: true,
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
                width: 300
            },
            {
                field: "Quantity",
                title: "數量",
                width: 300
            },
            {
                field: "EstPriceWithOutTax",
                title: "預估單價(未稅)",
                width: 300
            },
            {
                field: "EstTotalPriceWithOutTax",
                title: "預估總額(未稅)",
                width: 300
            },
            {
                field: "VendorName",
                title: "指定廠牌",
                width: 300
            },
            {
                field: "Vendor1",
                title: "商源1",
                width: 300
            },
            {
                field: "Vendor2",
                title: "商源2",
                width: 300
            },
            {
                field: "Vendor3",
                title: "商源3",
                width: 300
            },
            {
                field: "PurchaseName",
                title: "案名",
                width: 300
            },
            {
                field: "DemanDate",
                title: "需求日期",
                template: '#= kendo.toString(DemanDate, "yyyy/MM/dd")#',
                width: 300
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}