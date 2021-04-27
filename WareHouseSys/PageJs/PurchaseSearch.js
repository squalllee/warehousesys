let PurchaseTable;
$(document).ready(function () {

    $("#btnSearch").click(function () {
        PurchaseTable.draw();
    });


    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    PurchaseTable = $("#PurchaseTable").DataTable({
        ajax: {
            method: "post",
            url: $("#PurchaseTable").data("url"),
            data: function (d) {
                let formObj = $("form[name=PurchaseForm]").serializeObject();

                $("form[name=PurchaseForm] input:checkbox").each(function (i, chk) {
                    formObj[chk.name] = chk.checked;
                });

                $.extend(true, d, formObj);
            }
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        "columns": [{ "data": "PurchaseNo" }, { "data": "PurchaseDate" }, { "data": "PurchaseName" }, { "data": "ContractPriceWithoutVAT" }, { "data": "ContractPriceIncludeVAT" },
            { "data": "PurchaseMethod" }, { "data": "BudgetSource" }, { "data": "PurchaseUnit" },{ "data": "PurchaseMan" }, { "data": "VendorName" },
            { "data": "VendorContact" }, { "data": "Tel" }, { "data": "Mobile" }, { "data": "RequirementNo" },{ "data": "Status" }, { "data": "RequireNo" }],
        columnDefs: [
            {
                render: function (data, type, row) {
                    var html = "";
                    html = '<div class="dropdown">' +
                        '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                        '動作' +
                        ' <span class="caret"></span>' +
                        '</button>' +
                        '<ul class="dropdown-menu" aria-labelledby="about-us">';
                    if (row.Status === "0") {
                        //<li><a href="#" onclick="UpdatePur(\'' + row.PurchaseNo + '\')">修改</a></li>
                        html += '<li><a  href="#" onclick="TransToRecv(\'' + row.PurchaseNo + '\')">轉收貨單</a></li><li><a  href="#" onclick="TransToInbound(\'' + row.PurchaseNo + '\')">轉入庫單</a></li>';
                    }
                   

                    return html + '</ul></div>';
                },
                targets: -1
            },
            {
                render: function (data, type, row) {
                    var html = "<a href='#' onclick='openPurDetail(\"" + row.PurchaseNo + "\")'>" + row.PurchaseNo + "</a>";
                    return html;
                },
                targets: 0
            },
            {
                render: function (data, type, row) {
                    if (row.Status === "0") return "辦理中";
                    else if (row.Status === "1") return "轉收貨單";
                    else if (row.Status === "2") return "轉入庫單";
                    else return "";
                },
                targets: 14
            },
            {
                render: function (data, type, row) {
                    return formattedDate(data);
                },
                targets: [1]
            }
        ]
    });
    
    

    $("#PurDetailDialog").dialog({
        autoOpen: false,
        title: "採購明細",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "關閉": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#TransToPurDialog").dialog({
        autoOpen: false,
        title: "新增採購單",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "儲存": function () {
                if ($("#PurchaseAddForm").valid()) {
                    var purchaseHeader = formToJSON($("#PurchaseAddForm"));
                    var purchaseBodyList = [];

                    $.each(PurchaseAddDetailTable.rows().data(), function (index, val) {
                        purchaseBodyList.push(val);
                    });

                    var saveObj = { purchaseHeader: purchaseHeader, purchaseBodies: purchaseBodyList };

                    $.ajax({
                        url: $("#TransToPurDialog").data("saveurl"),
                        dataType: 'text',
                        type: 'post',
                        contentType: 'application/json',
                        data: JSON.stringify(saveObj),
                        processData: false,
                        success: function (data, textStatus, jQxhr) {
                            alert('儲成成功');

                            $("#TransToPurDialog").dialog("close");
                        },
                        error: function (jqXhr, textStatus, errorThrown) {
                            alert('儲成失敗');
                        }
                    });
                }


            },
            "關閉": function () {
                $(this).dialog("close");
            }
        }
    });

    
    $("#PurUpdateDialog").dialog({
        autoOpen: false,
        title: "修改採購單",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "儲存": function () {
                var obj = formToJSON($("#PurchaseHeaderForm"));
                $.ajax({
                    url: $("#PurUpdateDialog").data("saveurl"),
                    dataType: 'text',
                    type: 'post',
                    contentType: 'application/json',
                    data: JSON.stringify(obj),
                    processData: false,
                    success: function (data, textStatus, jQxhr) {
                        alert('儲成成功');
                        PurchaseTable.draw();
                        $("#PurUpdateDialog").dialog("close");
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        alert('儲成失敗');
                    }
                });
                $(this).dialog("close");
            },
            "關閉": function () {
                $(this).dialog("close");
            }
        }
    });


    $("#PurLocalUpdateDialog").dialog({
        autoOpen: false,
        title: "採購單修改",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "儲存": function () {
                var obj = formToJSON($("#PurLocalUpdateForm"));
                obj.SerialNo = parseInt(obj.SerialNo);

                PurchaseAddDetailTable.rows().every(function () {
                    var d = this.data();
                    if (d.SerialNo === obj.SerialNo) {
                        this.data(obj);
                        this.invalidate();
                    }
                });
                PurchaseAddDetailTable.draw();

                $(this).dialog("close");
            },
            "關閉": function () {
                $(this).dialog("close");
            }
        }
    });
});
//------------------------------------------------------------------------------
function openPurDetail(purchaseNo) {
    $('#PurDetailDialog').load($("#PurDetailDialog").data("url") + "?PurchaseNo=" + purchaseNo, function () {
        $(this).dialog('open');
        PurDetailInit();
    });
}

function PurDetailInit() {
    $("#PurDetailTable").DataTable({
        ajax: {
            method: "post",
            url: $("#PurDetailTable").data("url")
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        "scrollX": true,
        "columns": [{ "data": "SerialNo" }, { "data": "MaterialNo" }, { "data": "MaterialName" }, { "data": "Spec" }, { "data": "Price" },{ "data": "Quantity" }, { "data": "DeliveryLot" },
            { "data": "DeliveryPlace" }, { "data": "PerformancePeriod" }, { "data": "ReqireUnit" }],
        columnDefs: [
            {
                render: function (data, type, row) {
                    return formattedDate(data);
                },
                targets: [8]
            }
        ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api();
            var totalPriceWithoutTax = 0.0;

            data.forEach(function (val, index) {
                if (val.Price !== "") {
                    totalPriceWithoutTax += val.Price * val.Quantity;
                }
            });
     
            $(api.column(8).footer()).html(
                '<div style="display:inline-block;padding-right:100px;font-weight:800" class="pull-left">總計(未稅):' + $("[name='ContractPriceWithoutVAT']").val() + '</div><div style="display:inline-block;padding-right:100px;font-weight:800" class="pull-right">總計(含稅):' + $("[name='ContractPriceIncludeVAT']").val() + '</div>'
            );
        }
    });
}

function UpdatePur(purchaseNo) {
    $('#PurUpdateDialog').load($("#PurUpdateDialog").data("url") + "?PurchaseNo=" + purchaseNo, function () {
        $(this).dialog('open');
        UpdatePurInit();
    });
}
let PurDetailUpdateTable;
let BadgetSource = [];
function UpdatePurInit() {
    $('#BudgetSource').fastselect({
        placeholder: "請選擇科目"
    });
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });
    PurDetailUpdateTable= $("#PurDetailUpdateTable").DataTable({
        ajax: {
            method: "post",
            url: $("#PurDetailUpdateTable").data("url")
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        //"scrollX": true,
        "columns": [{ "data": "MaterialNo" }, { "data": "MaterialName" }, { "data": "Spec" }, { "data": "Unit" }, { "data": "Quantity" }, { "data": "Price" }, { "data": "DeliveryLot" },
            { "data": "DeliveryPlace" }, { "data": "PerformancePeriod" }, { "data": "ReqireUnit" }, { "data": "ReqireUnit" }],
        columnDefs: [
            {
                render: function (data, type, row) {
                    var html = "";
                    html = '<div class="dropdown">' +
                        '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                        '動作' +
                        ' <span class="caret"></span>' +
                        '</button>' +
                        '<ul class="dropdown-menu" aria-labelledby="about-us">';
                    html += '<li><a  href="#" onclick="UpdatePurDetail(this)">修改</a></li><li><a  href="#" onclick="DeletePurDetail(this)">作廢</a></li></ul></div>';
                    return html;
                },
                targets: -1
            },
            {
                render: function (data, type, row) {
                    return formattedDate(data);
                },
                targets: [8]
            }
        ],
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();
            var totalPriceWithoutTax = 0;

            data.forEach(function (obj) {
                if (obj.Price !== "") {
                    totalPriceWithoutTax += parseInt(obj.Price * obj.Quantity);
                }
            });

            $("[name='ContractPriceWithoutVAT']").val(totalPriceWithoutTax);
            $("[name='ContractPriceIncludeVAT']").val(Math.round(totalPriceWithoutTax * 1.05));

            $(api.column(3).footer()).html(
                totalPriceWithoutTax
            );

            $(api.column(8).footer()).html(
                Math.round(totalPriceWithoutTax * 1.05)
            );
        }
    });

    $("#PurAddDetailDialog").dialog({
        autoOpen: false,
        title: "新增採購明細",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "儲存": function () {
                var obj = formToJSON($("#PurDetailAddForm"));
                $.ajax({
                    url: $("#PurAddDetailDialog").data("saveurl") ,
                    dataType: 'text',
                    type: 'post',
                    contentType: 'application/json',
                    data: JSON.stringify(obj),
                    processData: false,
                    success: function (data, textStatus, jQxhr) {
                        alert('儲成成功');
                        PurDetailUpdateTable.draw();
                        $("#PurAddDetailDialog").dialog("close");
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        alert('儲成失敗');
                    }
                });
            },
            "關閉": function () {
                $(this).dialog("close");
            }
        }

    });


    $("#btnPurDetailAdd").click(function () {
        $('#PurAddDetailDialog').load($("#PurAddDetailDialog").data("url") + "?PurchaseNo=" + $("#PurchaseHeaderForm").find("#PurchaseNo").val(), function () {
            $(this).dialog('open');
            PurDetailAddInit();
        });
    });
    
}

function PurDetailAddInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $("#AddMaterialNo").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "../api/Material/getMaterialbyTerm?term=" + $("#AddMaterialNo").val(),
                dataType: "json",
                method: "get",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.MaterialName + '[' + item.MaterialNo + ']',
                            value: item.MaterialNo
                        };
                    }));

                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            $.ajax({
                url: "../api/Material/GetMaterialInfo?MaterialNo=" + ui.item.value,
                dataType: "json",
                method: "get",
                success: function (data) {
                    if (data.length > 0) {
                        $("#AddMaterialName").val(data[0].MaterialName);
                        $("#AddSpec").val(data[0].Spec);
                        $("#AddUnit").val(data[0].Unit);
                    }
                }
            });
        }
    });
}

function UpdatePurDetail(e) {
    
    var obj = PurDetailUpdateTable
        .row($(e).closest("tr"))
        .data();
    $("#PurUpdateDetailDialog").dialog({
        autoOpen: false,
        title: "修改採購明細",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "儲存": function () {
                var obj = formToJSON($("#PurLocalUpdateForm"));
                $.ajax({
                    url: $("#PurDetailUpdateTable").data("saveurl"),
                    dataType: 'text',
                    type: 'post',
                    contentType: 'application/json',
                    data: JSON.stringify(obj),
                    processData: false,
                    success: function (data, textStatus, jQxhr) {
                        alert('儲成成功');
                        PurDetailUpdateTable.draw();
                        $("#PurUpdateDetailDialog").dialog("close");
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        alert('儲成失敗');
                    }
                });
            },
            "關閉": function () {
                $(this).dialog("close");
            }
        }

    });
    $('#PurUpdateDetailDialog').load($("#PurUpdateDetailDialog").data("url") + "?PurchaseNo=" + obj.PurchaseNo + "&SerialNo=" + obj.SerialNo, function () {
        $(this).dialog('open');
        UpdatePurDetailInit();
    });
}

function UpdatePurDetailInit() {
    
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });
}


function DeletePurDetail(e) {
    var obj = PurDetailUpdateTable
        .row($(e).closest("tr"))
        .data();
     $('<div></div>').appendTo('body')
         .html('<div><h6>是否確定要作廢?</h6></div>')
        .dialog({
            modal: true, title: '提示', zIndex: 10000, autoOpen: true,
            width: 'auto', resizable: false,
            buttons: {
                是: function () {
                    $.ajax({
                        url: $("#PurDetailUpdateTable").data("deleteurl") + obj.PurchaseNo + "/" + obj.SerialNo,
                        dataType: 'text',
                        type: 'post',
                        contentType: 'application/json',
                        data: JSON.stringify(obj),
                        processData: false,
                        success: function (data, textStatus, jQxhr) {
                            PurDetailUpdateTable.draw();
                            alert('作廢成功');
                        },
                        error: function (jqXhr, textStatus, errorThrown) {
                            alert('作廢失敗');
                        }
                    });
                    $(this).dialog("close");
                },
                否: function () {
                    $(this).dialog("close");
                }
            },
            close: function (event, ui) {
                $(this).remove();
            }
        });
}

function TransToRecv(PurchaseNo) {
    TransToRecvDialog = $("#TransToRecvDialog").dialog({
        //autoOpen: false,
        title: "轉收貨單",
        width: 1200,
        height: 900,
        //modal: true,
        buttons: {
            "關閉": function () {
                $(this).dialog("close");
            }
        }
    });
    $('#TransToRecvDialog').load($("#TransToRecvDialog").data("url") + "?PurchaseNo=" + PurchaseNo, function () {
        $(this).dialog('open');
        TransToRecvInit();
    });
}

function TransToRecvInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $("#tt").tabs();

    var TransToRecvSource = new kendo.data.DataSource({
        offlineStorage: "products-offline",
        transport: {
            read: {
                url: $("[name='TransToRecvTable']").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                return JSON.stringify(options);
            }
        }, requestEnd: function (e) {
            if ($("#OpenContract").val() === "N") {
                TransToRecvGrid.hideColumn(0);
                TransToRecvGrid.hideColumn(1);
            }
            else {
                TransToRecvGrid.hideColumn(6);
                $("[data-field='Quantity'] .k-link").html("開口契約數量");
            }
            TransToRecvSource.online(false);
        },
        schema: {
            data: "data",
            total: "Total",
            model: {
                id: "MaterialNo",
                fields: {
                    MaterialNo: { type: "string", editable: false },
                    Price: { type: "string", editable: false },
                    Quantity: { type: "string" },
                    //shouldbeRecved: { type: "string"},
                    receivedQty: { type: "string", editable: false},
                    UnreceivedQty: { type: "string", editable: false},
                    DeliveryLot: { type: "string", editable: false},
                    DeliveryPlace: { type: "string", editable: false}
                }
            }
        },
        serverPaging: true,
        pageSize: 1000,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });

    

    TransToRecvGrid = $("[name='TransToRecvTable']").kendoGrid({
        dataSource: TransToRecvSource,
        resizable: true,
        height: 550,
        width: 1100,
        sortable: true,
        scrollable: true,        
        pageable: true, 
        /*
        pageable: {
            pageSize: 1000,
            pageSizes: true,
            pageSizes: [1000],
            buttonCount: 5,
            refresh: true,
            messages: {
                last: "最末頁",
                first: "第一頁",
                next: "下一頁",
                previous: "上一頁",
                morePages: "更多頁",
                itemsPerPage: "每頁筆數",
                display: "第 {0} - {1} 筆 共 {2} 筆記錄",
                refresh: "重新整理",
                empty: "沒有符合記錄"
            }
        },
        */
        columns: [
            { command: ["edit"], title: "&nbsp;", width: 200, locked: true, lockable: true },
            { selectable: true, width: "50px" },
            {
                field: "MaterialNo",
                title: "料號",
                width: 200

            }, {
                field: "Price",
                title: "單價",
                width: 200
            }, {
                field: "Quantity",
                title: "應交數量",
                width: 200
            }, {
                field: "receivedQty",
                title: "已交數量",
                width: 100
            }, {
                field: "UnreceivedQty",
                title: "未交數量",
                width: 100
            }, {
                field: "DeliveryLot",
                title: "交貨批次",
                width: 100
            }, {
                field: "DeliveryPlace",
                title: "交貨地點",
                width: 100
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
    //$("[name='TransToRecvTable']").each(function (index) {
    //    $(this).DataTable({
    //        ajax: {
    //            method: "post",
    //            url: $(this).data("url")
    //        },
    //        searching: false,
    //        "processing": true,
    //        "serverSide": true,
    //        bLengthChange: false,
    //        //"scrollX": true,
    //        "columns": [{ "data": "MaterialNo" }, { "data": "Price" }, { "data": "Quantity" }, { "data": "receivedQty" }, { "data": "UnreceivedQty" },{ "data": "DeliveryLot" }, { "data": "DeliveryPlace" }]

    //    });
    //});
}

function TransToInbound(PurchaseNo) {
    TransToInboundDialog = $("#TransToInboundDialog").dialog({
        //autoOpen: false,
        title: "轉入庫單",
        width: 1200,
        height: 600,
        //modal: true,
        buttons: {
            "轉入庫單": function () {
                CreateInbound($("[name='inpPurchaseNo']").val());
            },
            "關閉": function () {
                $(this).dialog("close");
            }
        }
    });
    $('#TransToInboundDialog').load($("#TransToInboundDialog").data("url") + "?PurchaseNo=" + PurchaseNo, function () {
        $(this).dialog('open');
        TransToInboundDialogInit();
    });
}

function TransToInboundDialogInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $("#InboundTabs").tabs();

    $("[name='TransToInboundTable']").each(function (index) {
        $(this).DataTable({
            ajax: {
                method: "post",
                url: $(this).data("url")
            },
            searching: false,
            "processing": true,
            "serverSide": true,
            bLengthChange: false,
            //"scrollX": true,
            "columns": [{ "data": "MaterialNo" }, { "data": "MaterialName" }, { "data": "Quantity" }, { "data": "ReceivedQty" }, { "data": "UnReceivedQty" } , { "data": "ReceivedQty" }, { "data": "DeliveryLot" }, { "data": "DeliveryPlace" }]

        });
    });
}

function CreateReceive(no, lot, OpenContract) {
    if (OpenContract === "N") {
        $.ajax({
            type: 'GET',
            url: "../api/Purchase/TransToRecv/" + no + "/" + lot,
            success: function (result) {
                alert('產生收貨單成功!');
                TransToRecvDialog.dialog("close");
            },
            error: function (err) {
                alert('產生收貨單失敗!');
            }
        });
    }
    else {

        // Get selected rows
        var sel = $("input:checked", TransToRecvGrid.tbody).closest("tr");
        // Get data item for each
        var items = [];
        $.each(sel, function (idx, row) {
            var item = TransToRecvGrid.dataItem(row);
            items.push(item);
        });

        if (items.length == 0) {
            alert("請先選擇要轉收貨單的資料!");
            return;
        }

        var obj = {};
        obj.PurchaseNo = no;
        obj.Lot = lot;
        obj.PurBodies = items;

        $.ajax({
            url: "../api/Purchase/OpenContractToRecv",
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            data: JSON.stringify(obj),
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('產生收貨單成功!');
                TransToRecvDialog.dialog("close");
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('產生收貨單失敗!');
            }
        });
        
    }
   
}

function CreateInbound(PurchaseNo) {
    var obj = [];
    var checkList = $(".k-checkbox");
    $.each(checkList, function (i) {
        if (checkList[i].checked) {
            obj.push($(checkList[i]).data("lot").toString());
        }
    });

    if (obj.length === 0) {
        alert("尚未勾取任何批次!");
        return;
    }

    var paramter = {};
    paramter.PurchaseNo = PurchaseNo;
    paramter.Lots = obj;

    $.ajax({
        type: 'POST',
        url: "../api/Purchase/TransToInbound",
        dataType: 'text',
        contentType: 'application/json',
        data: JSON.stringify(paramter),
        success: function (result) {
            alert('產生入庫單成功!');
            TransToInboundDialog.dialog("close");
        },
        error: function (err) {
            alert('產生入庫單失敗!');
        }
    });
}
