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
        "<li><a  href='#' onclick ='Delete(this)' >作廢</a></li>" +
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
    $("#EstDemandAddBtn").click(function (e) {
        EstDemandAddDialog = $("<div></div>").kendoWindow({
            title: "新增預估需求單",
            actions: ["Close"],
            content: "../EstDemand/EstDemandAdd?OrderNo=",
            visible: false,
            modal: true,
            width: 1900,
            position: {
                top: "50px",
                left: "0px"
            },
            refresh: function (e) {
                EstDemandAddInit();
            },
            close: function (e) {
                EstDemandAddDialog.destroy();
            }
        }).data("kendoWindow").open();
    });
});

function Update(e) {
    var row = e.closest("tr");
    var dataItem = $("#grid").data("kendoGrid").dataItem(row);

    EstDemandAddDialog = $("<div></div>").kendoWindow({
        title: "修改預估需求單",
        actions: ["Close"],
        content: "../EstDemand/EstDemandAdd?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1900,
        position: {
            top: "50px",
            left: "0px"
        },
        refresh: function (e) {
            EstDemandAddInit();
        },
        close: function (e) {
            EstDemandAddDialog.destroy();
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

function Delete(e) {
    kendo.confirm("是否確定要作廢?").then(function () {
        var row = e.closest("tr");
        var dataItem = $("#grid").data("kendoGrid").dataItem(row);
            $.ajax({
                url: "../api/EstDemand/deleteDemand?OrderNo=" + dataItem.OrderNo,
                dataType: 'text',
                type: 'get',
                contentType: 'application/json',
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('作廢成功');
                    $("#grid").data("kendoGrid").dataSource.read();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('作廢失敗');
                }
            });
        }, function () {
        
    });
}

function Print(e) {
    var row = e.closest("tr");
    var dataItem = $("#grid").data("kendoGrid").dataItem(row);
    window.open("../Report/EstDemand/EstDemandReport.aspx?OrderNo=" + dataItem.OrderNo);
}

