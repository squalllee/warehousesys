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


    if (data.Status === "辦理中") {
        actionStr += "<li><a  href='#' onclick ='Review(this)' >審核</a></li>";
    }

    actionStr += "</ul></div>";

    for (var i = 0; i < data.length; i++) {
        if (data[i].Status === "辦理中") {
            var dataItem = data[i];
            var tr = $("#grid").find("[data-uid='" + dataItem.uid + "']");
            $(actionStr).appendTo(tr.find("[name='ActionList']"));
            tr.find("[name='ActionList']").append();
        }
    }
}

function error_handler(e) {
    alert('發生錯誤');
}

function openDetail(e) {
    var row = e.closest("tr");
    var dataItem = $("#grid").data("kendoGrid").dataItem(row);

    detailDialog = $("<div></div>").kendoWindow({
        title: "料號申請明細",
        actions: ["Close"],
        content: "../ReqMaterialInfo/ReqMaterialDetail?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1900,
        position: {
            top: "50px",
            left: "0px"
        },
        refresh: function (e) {
            ReqMaterialDetailInit();
        },
        close: function (e) {
            detailDialog.destroy();
        }
    }).data("kendoWindow").open();
}

function Review(e) {
    var row = e.closest("tr");
    var dataItem = $("#grid").data("kendoGrid").dataItem(row);

    ReqMaterialReviewDialog = $("<div></div>").kendoWindow({
        title: "新增料號申請單",
        actions: ["Close"],
        content: "../ReqMaterialInfo/ReqMaterialDoReview?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1900,
        position: {
            top: "50px",
            left: "0px"
        },
        refresh: function (e) {
            ReqMaterialReviewInit();
        },
        close: function (e) {
            ReqMaterialReviewDialog.destroy();
        }
    }).data("kendoWindow").open();
}
