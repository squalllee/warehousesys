var MaterialPickingGrid;

$(document).ready(function () {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    MaterialPickingGrid = $("#MaterialPickingGrid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: $("#MaterialPickingGrid").data("url"),
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
                        OutBoundDate
                            : { type: "date" }
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
                    //startswith: "比對條件",
                    eq: "等於",
                    neq: "不等於"
                }
            }
        },
        columns: [{
            field: "OrderNo",
            title: "領料單號",
            width: 160,
            template: '<span style="cursor:pointer" onclick="openDetail(this);">#=OrderNo#</span>'
        }, {
            field: "WorkNo",
            title: "工單單號",
            width: 160
        }, {
            field: "PickingMan",
            title: "領料人員",
            width: 200
        }, {
                field: "OutBoundMan",
                title: "發料人員",
                width: 150
           
        }, {
                field: "OutBoundDate",
                title: "發料日期",
                template: '#= OutBoundDate !== null ? kendo.toString(OutBoundDate, "yyyy/MM/dd") : ""#',
                width: 150
        },
            {
                field: "EmergencyPicking",
                title: "緊急領料",
                width: 80
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
                html = '<div class="btn-group dropleft">' +
                    '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                    '動作' +
                    ' <span class="caret"></span>' +
                    '</button>' +
                    '<ul class="dropdown-menu" aria-labelledby="about-us">';

                if (container.Status === "辦理中") {
                   
                    html += '<li><a  href="#" onclick="OpenPickingModify(this)">發料</a></li>';
                    
                    
                }
              
                return html + "</ul ></div >";
                
            }
        }
        ]
    }).data("kendoGrid");

});

function OpenPickingModify(e) {
    var row = e.closest("tr");
    var grid = $("#MaterialPickingGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("#ModifyMaterialPickingDialog").kendoWindow({     
        title: "發料作業",
        actions: ["Close"],
        content: $("#ModifyMaterialPickingDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1750,
        position: {
            top: "20px", left:"5%"
        },
        refresh: function (e) {
            $("#PickingForm").find(".dateTime").datepicker({
                dateFormat: 'yy/mm/dd'
            });
            modifyGridInit();
        }
    }).data("kendoWindow").open();
}

function openDetail(e) {
    var row = e.closest("tr");
    var grid = $("#MaterialPickingGrid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    $("#PickingDetailDialog").kendoWindow({
        title: "發料明細",
        actions: ["Close"],
        content: $("#PickingDetailDialog").data("url") + "?OrderNo=" + dataItem.OrderNo,
        visible: false,
        modal: true,
        width: 1500,
        position: {
            top: "20px",
            left: "15%"
        },
        refresh: function (e) {
            PickingDetailInit();
        }
    }).data("kendoWindow").open();
}




