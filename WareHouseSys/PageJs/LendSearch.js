var LendGrid;
$(document).ready(function () {
    $("#LendAdd").click(function (e) {
        $("#LendAddDialog").kendoWindow({
            title: "新增借料單",
            actions: ["Close"],
            content: $("#LendAddDialog").data("url") + "?OrderNo=",
            visible: false,
            modal: true,
            position: {
                top: "50px",
                left: "15%"
            },
            width: 1050,
            refresh: function (e) {
                LendAddDialogInit();
            }
        }).data("kendoWindow").open();
    });

    LendGrid = $("#LendGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#LendGrid").data("url"),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST"
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: {
                data: "data",
                total: "Total",
                model: {
                    fields: {
                        OutBoundDate: { type: "date" },
                        ExtendDate: { type: "date" },
                        Deadline: { type: "date" }
                    }
                }
            },
            serverPaging: true,
            pageSize: 10,
            serverSorting: true,
            serverFiltering: true,
            pageable: true
        },
        height: 650,
        sortable: true,
        pageable: true,
        dataBinding: function (e) {
            this.dataSource.data().forEach(function (val, index) {
                val.EmergencyPicking = val.EmergencyPicking ? "是" : "否";
            });
        },
        filterable: {
            extra: false,
            operators: {
                string: {
                    eq: "等於",
                    contains: "包含"
                }
            }
        },
        columns: [{
            field: "OrderNo",
            title: "借料單號",
            width: 160,
            template: '<span style="cursor:pointer" onclick="openDetail(this);">#=OrderNo#</span>'
        },{
            field: "LendMan",
            title: "借料人員",
            width: 200
        },
        {
            field: "OutBoundMan",
            title: "出庫人員",
            width: 150

        }, {
            field: "OutBoundDate",
            title: "出庫日期",
            template: '#= OutBoundDate !== null ? kendo.toString(OutBoundDate, "yyyy/MM/dd") : ""#',
            width: 150
            }, {
            field: "ExtendDate",
                title: "歸還期限",
            template: '#= ExtendDate !== null ? kendo.toString(ExtendDate, "yyyy/MM/dd") : ""#',
                width: 150
            },
        {
            field: "Status",
            title: "狀態",
            width: 80
        },
        {
            title: "動作",
            width: 150,
            template: function (container) {
                $("<div class='btn - group dropleft'></div >").load("");
                html = '<div class="btn-group dropleft">' +
                    '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                    '動作' +
                    ' <span class="caret"></span>' +
                    '</button>' +
                    '<ul class="dropdown-menu" aria-labelledby="about-us">';
                if (container.Status === "辦理中") {

                    html += '<li><a  href="#" onclick="OpenModifyLendDialog(this)">修改</a></li><li><a  href="#" onclick="DeleteLend(this)">作廢</a></li><li><a  href="#" onclick="Print(this)">列印</a></li>';

                }

                if (container.Status === "結案") {
                    html += '<li><a  href="#" onclick="OpenBackDialog(this)">歸還</a></li><li><a  href="#" onclick="OpenExtendDialog(this)">展延</a></li>';
                }
                return html + "</ul ></div >";

            }
        }
        ]
    }).data("kendoGrid");
});

function OpenModifyLendDialog(e) {
    var row = e.closest("tr");
    var dataItem = LendGrid.dataItem(row);

    $("#LendAddDialog").kendoWindow({
        title: "借料單修改",
        actions: ["Close"],
        content: $("#LendAddDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1050,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            LendAddDialogInit();
        }
    }).data("kendoWindow").open();
}

function DeleteLend(e) {
    var row = e.closest("tr");
    var dataItem = LendGrid.dataItem(row);

    if (confirm("是否確定要作廢？")) {
        $.ajax({
            url: $("#LendGrid").data("deleteurl") + "/" + dataItem.OrderNo,
            dataType: 'text',
            type: 'get',
            contentType: 'application/json',
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('作廢成功');
                LendGrid.dataSource.read();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('作廢失敗');
            }
        });
    }
    
}

function Print(e) {
    var row = e.closest("tr");
    var dataItem = LendGrid.dataItem(row);
    window.open("../Report/Lend/Lend.aspx?OrderNo=" + dataItem.OrderNo);
}

function openDetail(e) {
    var row = e.closest("tr");
    var dataItem = LendGrid.dataItem(row);

    $("#LendDetailDialog").kendoWindow({
        title: "借料明細",
        actions: ["Close"],
        content: $("#LendDetailDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1500,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            LendDetailInit();
        }
    }).data("kendoWindow").open();
}

function OpenBackDialog(e) {
    var row = e.closest("tr");
    var dataItem = LendGrid.dataItem(row);

    $("#BackLendDialog").kendoWindow({
        title: "轉歸還單",
        actions: ["Close"],
        content: $("#BackLendDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1600,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            BackLendInit();
        }
    }).data("kendoWindow").open();
}

function OpenExtendDialog(e) {
    var row = e.closest("tr");
    var dataItem = LendGrid.dataItem(row);

    $("#ExtendLendDialog").kendoWindow({
        title: "轉展延單",
        actions: ["Close"],
        content: $("#ExtendLendDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1050,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            ExtendLendInit();
        }
    }).data("kendoWindow").open();
}