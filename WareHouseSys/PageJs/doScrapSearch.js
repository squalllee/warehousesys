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
            actionStr += "<li><a  href='#' onclick ='ScrapClose(this)' >結案</a></li>" +
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

function ScrapClose(e) {
    var row = e.closest("tr");
    var grid = $("#grid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    ScrapCloseDialog = $("<div></div>").kendoWindow({
        title: "報廢結案作業",
        actions: ["Close"],
        content: "../Scrap/ScrapClose?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1200,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            ScrapCloseInit();
        },
        close: function (e) {
            this.destroy();
        }
    }).data("kendoWindow").open();
}