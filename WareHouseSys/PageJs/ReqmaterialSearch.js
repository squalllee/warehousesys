$(document).ready(function () {
    $("#ReqMaterialAddBtn").click(function () {
        ReqMaterialAddDialog = $("<div></div>").kendoWindow({
            title: "新增料號申請單",
            actions: ["Close"],
            content: "../ReqMaterialInfo/ReqMaterialAdd?OrderNo=",
            visible: false,
            modal: true,
            width: 1900,
            position: {
                top: "50px",
                left: "0px"
            },
            refresh: function (e) {
                ReqMaterialAddInit();
            },
            close: function (e) {
                ReqMaterialAddDialog.destroy();
            }
        }).data("kendoWindow").open();
    });
});

function onGridDataBound(e) {
    var data = this.dataSource.view();
    var actionStr = "<div class='btn-group dropleft'>" +
        "<button class='btn btn-primary dropdown-toggle' type='button' id='about-us' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>" +
        "動作" +
        " <span class='caret' ></span>" +
        "</button>" +
        "<ul class='dropdown-menu' aria-labelledby='about-us' > " +
        "<li><a  href='#' onclick ='Update(this)' >修改</a></li>" +
        "<li><a  href='#' onclick ='Print(this)' >列印</a></li>" +
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


function Update(e) {
    var row = e.closest("tr");
    var dataItem = $("#grid").data("kendoGrid").dataItem(row);

    ReqMaterialupdateDialog = $("<div></div>").kendoWindow({
        title: "新增料號申請單",
        actions: ["Close"],
        content: "../ReqMaterialInfo/ReqMaterialAdd?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1900,
        position: {
            top: "50px",
            left: "0px"
        },
        refresh: function (e) {
            ReqMaterialAddInit();
        },
        close: function (e) {
            ReqMaterialupdateDialog.destroy();
        }
    }).data("kendoWindow").open();
}

function Print(e) {
    var row = e.closest("tr");
    var dataItem = $("#grid").data("kendoGrid").dataItem(row);
    window.open("../Report/ReqMaterial/ReqMaterialReport.aspx?OrderNo=" + dataItem.OrderNo);
}