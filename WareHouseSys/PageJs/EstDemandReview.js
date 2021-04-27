function onGridDataBound(e) {
    var data = this.dataSource.view();
    var actionStr = "<div class='btn-group dropleft'>" +
        "<button class='btn btn-primary dropdown-toggle' type='button' id='about-us' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" +
        "動作" +
        " <span class='caret' ></span>" +
        "</button>" +
        "<ul class='dropdown-menu' aria-labelledby='about-us' > " +
        "<li><a  href='#' onclick ='Review(this)' >審核</a></li>" +
        "</ul></div>";


    for (var i = 0; i < data.length; i++) {
        var dataItem = data[i];
        if (dataItem.Status !== "結案") {
            var tr = $("#grid").find("[data-uid='" + dataItem.uid + "']");
            $(actionStr).appendTo(tr.find("[name='ActionList']"));
            tr.find("[name='ActionList']").append();
        }


    }
}

function error_handler(e) {
    alert("發生錯誤!");
}

$(document).ready(function () {
    $("#EstDemandExcelBtn").click(function (e) {
        var grid = $("#grid").data("kendoGrid");
        // Get selected rows
        var sel = $("input:checked", grid.tbody).closest("tr");
        // Get data item for each
        var items = [];
        $.each(sel, function (idx, row) {
            var item = grid.dataItem(row);
            items.push(item.OrderNo);
        });

        var ds = new kendo.data.DataSource({
            transport: {
                read: {
                    url: "../api/EstDemand/ExcelData",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST"
                },
                parameterMap: function (options, operation) {
                    return JSON.stringify(items);
                }
            },
            schema: {
                model: {
                    fields: {
                        OrderNo: { type: "string" },
                        MaterialNo: { type: "string" },
                        SerialNo: { type: "string" },
                        MaterialName: { type: "string" },
                        Spec: { type: "string" },
                        Annual: { type: "string" },
                        Season: { type: "string" },
                        ApplyUnit: { type: "string" },
                        ApplyMan: { type: "string" },
                        ApplyDate: { type: "string" },
                        Quantity: { type: "number" },
                        EstPriceWithOutTax: { type: "number" },
                        EstTotalPriceWithOutTax: { type: "number" },
                        VendorName: { type: "string" },
                        Vendor1: { type: "string" },
                        Vendor2: { type: "string" },
                        Vendor3: { type: "string" },
                        PurchaseName: { type: "string" },
                        DemanDate: { type: "date" }
                    }
                }
            }
        });
        //ds.read();

        var rows = [{
            cells: [
                { value: "單號" },
                { value: "料號" },
                { value: "項次" },
                { value: "品名" },
                { value: "規格" },
                { value: "年度" },
                { value: "季" },
                { value: "申請單位" },
                { value: "申請人" },
                { value: "申請日期" },
                { value: "數量" },
                { value: "預估單價(未稅)" },
                { value: "預估總額(未稅)" },
                { value: "指定廠牌" },
                { value: "商源1" },
                { value: "商源2" },
                { value: "商源3" },
                { value: "案名" },
                { value: "需求日期" }
            ]
        }];


        ds.fetch(function () {
            var data = this.data();
            for (var i = 0; i < data.length; i++) {
                // Push single row for every record.
                rows.push({
                    cells: [
                        { value: data[i].OrderNo },
                        { value: data[i].MaterialNo },
                        { value: data[i].SerialNo },
                        { value: data[i].MaterialName },
                        { value: data[i].Spec },
                        { value: data[i].Annual },
                        { value: data[i].Season },
                        { value: data[i].ApplyUnit },
                        { value: data[i].ApplyMan },
                        { value: data[i].ApplyDate },
                        { value: data[i].Quantity },
                        { value: data[i].EstPriceWithOutTax },
                        { value: data[i].EstTotalPriceWithOutTax },
                        { value: data[i].VendorName },
                        { value: data[i].Vendor1 },
                        { value: data[i].Vendor2 },
                        { value: data[i].Vendor3 },
                        { value: data[i].PurchaseName },
                        { value: data[i].DemanDate }
                    ]
                });
            }

            var workbook = new kendo.ooxml.Workbook({
                sheets: [
                    {
                        columns: [
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true },
                            { autoWidth: true }
                        ],
                        // The title of the sheet.
                        title: "Orders",
                        // The rows of the sheet.
                        rows: rows
                    }
                ]
            });
            // Save the file as an Excel file with the xlsx extension.
            kendo.saveAs({ dataURI: workbook.toDataURL(), fileName: "EstDemand.xlsx" });
        });
    });
});



function Review(e) {
    var row = e.closest("tr");
    var dataItem = $("#grid").data("kendoGrid").dataItem(row);

    EstDemandReviewDialog = $("<div></div>").kendoWindow({
        title: "審核預估需求單",
        actions: ["Close"],
        content: "../EstDemand/EstDemandDoReview?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1900,
        position: {
            top: "50px",
            left: "0px"
        },
        refresh: function (e) {
            EstDemandReviewInit();
        },
        close: function (e) {
            EstDemandReviewDialog.destroy();
        }
    }).data("kendoWindow").open();
}

function openDetail(e) {
    var row = e.closest("tr");
    var dataItem = $("#grid").data("kendoGrid").dataItem(row);

    detailDialog = $("<div></div>").kendoWindow({
        title: "預估需求單明細",
        actions: ["Close"],
        content: "../EstDemand/EstDemandDetail?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1900,
        position: {
            top: "50px",
            left: "0px"
        },
        refresh: function (e) {
            EstDemandDetailInit();
        },
        close: function (e) {
            detailDialog.destroy();
        }
    }).data("kendoWindow").open();
}