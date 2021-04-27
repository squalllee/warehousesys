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

function Review(e) {
    var row = e.closest("tr");
    var dataItem = $("#grid").data("kendoGrid").dataItem(row);

    MaterialUpdateReviewDialog = $("<div></div>").kendoWindow({
        title: "物料變更審核",
        actions: ["Close"],
        content: "../MaterialUpdate/MaterialUpdateDoReview?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1900,
        position: {
            top: "50px",
            left: "0px"
        },
        refresh: function (e) {
            MaterialUpdateDoReviewInit(dataItem.OrderNo);
        },
        close: function (e) {
            this.destroy();
        }
    }).data("kendoWindow").open();
}