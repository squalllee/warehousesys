$(document).ready(function () {
    $("#ScrapAddBtn").click(function (e) {
        ScrapAddDialog = $("<div></div>").kendoWindow({
            title: "報廢作業",
            actions: ["Close"],
            content: "../Scrap/AddScrap",
            visible: false,
            modal: true,
            width: 1400,
            position: {
                top: "50px",
                left: "15%"
            },
            refresh: function (e) {
                AddScrapInit();
            },
            close: function (e) {
                this.destroy();
            }
        }).data("kendoWindow").open();
    });
});
function onGridDataBound() {
    var data = this.dataSource.view();
    var actionStr = "<div class='btn-group dropleft'>" +
        "<button class='btn btn-primary dropdown-toggle' type='button' id='about-us' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" +
        "動作" +
        " <span class='caret' ></span>" +
        "</button>" +
        "<ul class='dropdown-menu' aria-labelledby='about-us' > ";



    for (var i = 0; i < data.length; i++) {
        var dataItem = data[i];

        if (dataItem.StatusId === "0") {
            actionStr += "<li><a  href='#' onclick ='update(this)' >修改</a></li>" +
                "<li><a  href='#' onclick ='print(this)' >列印</a></li>" +
                "</ul></div>";
        }
       

        var tr = $("#grid").find("[data-uid='" + dataItem.uid + "']");
        $(actionStr).appendTo(tr.find("[name='ActionList']"));
        tr.find("[name='ActionList']").append();
        actionStr = "<div class='btn-group dropleft'>" +
            "<button class='btn btn-primary dropdown-toggle' type='button' id='about-us' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" +
            "動作" +
            " <span class='caret' ></span>" +
            "</button>" +
            "<ul class='dropdown-menu' aria-labelledby='about-us' > ";
    }
}

function update(e) {
    var row = e.closest("tr");
    var grid = $("#grid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    ScrapAddDialog = $("<div></div>").kendoWindow({
        title: "報廢作業",
        actions: ["Close"],
        content: "../Scrap/AddScrap?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1400,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            AddScrapInit();
        },
        close: function (e) {
            this.destroy();
        }
    }).data("kendoWindow").open();
}

function print(e) {
    var row = e.closest("tr");
    var grid = $("#grid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    window.open("../Report/Scrap/ScrapReport.aspx?OrderNo=" + dataItem.OrderNo + "&Type=" + dataItem.ScrapType);
}

function openDetail(e) {
    var row = e.closest("tr");
    var grid = $("#grid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("<div></div>").kendoWindow({
        title: "報廢作業",
        actions: ["Close"],
        content: "../Scrap/ScrapDetail?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1200,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            ScrapDetailInit();
        },
        close: function (e) {
            this.destroy();
        }
    }).data("kendoWindow").open();
}