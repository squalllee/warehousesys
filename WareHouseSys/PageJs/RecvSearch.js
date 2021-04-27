let RecvTable;
$(document).ready(function () {

    $("#btnSearch").click(function () {
        RecvTable.draw();
    });

    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

   
    RecvTable = DataTableInit($("#RecvTable"), function (d) { 
        let formObj = $("form[name=RecvForm]").serializeObject();

        $("form[name=RecvForm] input:checkbox").each(function (i, chk) {
            formObj[chk.name] = chk.checked;
        });
        $.extend(true, d, formObj);
    },
        [{ "data": "OrderNo" }, { "data": "PurchaseNo" }, { "data": "DeliveryLot" }, { "data": "ReceiveDate" }, { "data": "ReceiveMan" }, { "data": "Status" }, { "data": "Status" }],
        [
            {
                render: function (data, type, row) {
                    var html = "";
                    html = '<div class="btn-group dropleft">' +
                        '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                        '動作' +
                        ' <span class="caret"></span>' +
                        '</button>' +
                        '<ul class="dropdown-menu" aria-labelledby="about-us">' +
                        '<li><a  href="#" onclick="Recv(\'' + row.OrderNo + '\',\'' + row.DeliveryLot + '\')">收貨</a></li><li><a  href="#" onclick="PrintRecv(\'' + row.OrderNo + '\')">列印</a></li><li><a  href="#" onclick="RecvClose(\'' + row.OrderNo + '\',\'' + row.DeliveryLot + '\')">結案</a></li></ul ></div >';

                    if (row.Status === "辦理中") {
                        return html;
                    }
                    else {
                        return "";
                    }
                   

                },
                targets: -1
            },
            {
                render: function (data, type, row) {
                    var html = "<a href='#' onclick='openRecvDetail(\"" + row.OrderNo + "\")'>" + row.OrderNo + "</a>";
                    return html;
                },
                targets: 0
            },
            {
                render: function (data, type, row) {
                    return formattedDate(data);
                },
                targets: [3]
            }
        ]
    );

    DialogInit($("#RecvDataDialog"), "收貨作業", {
        "儲存": function () {
            var validator = $("#RecvHeaderForm").kendoValidator().data("kendoValidator");
            if (validator.validate()) {
                var obj = formToJSON($("#RecvHeaderForm"));
                var datas = RecvDataTable.rows().data();

                var ret = true;
                $.each(datas, function (i) {
                    if (datas[i].WareHouseName === null || datas[i].StorageId === null) {
                        Warning("警告!", "請先收貨再儲存!");
                        ret = false;
                        return false;
                    }
                });

                if (!ret) return;

                doPost($("#RecvDataDialog").data("saveurl"), JSON.stringify(obj), function () {
                    alert('儲存成功');
                    $("#RecvDataDialog").dialog("close");
                }, function () {
                    alert('儲存失敗');
                });
            }
        },
        "關閉": function () {
            $(this).dialog("close");
        }
    }, 1200, 600);
});
//------------------------------------------------------------------------------
function openRecvDetail(OrderNo) {
    $("<div></div>").kendoWindow({
        title: "收貨明細",
        actions: ["Close"],
        content: $("#RecvDetailDialog").data("url") + "?OrderNo=" + OrderNo,
        visible: false,
        modal: true,
        width: 1200,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            RecvDetailInit();
        },
        close: function (e) {
            this.destroy();
        }
    }).data("kendoWindow").open();

}

function RecvDetailInit() {
    $("#RecvDetailTable").DataTable({
        ajax: {
            method: "post",
            url: $("#RecvDetailTable").data("url")
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        "columns": [{ "data": "SerialNo" }, { "data": "MaterialNo" }, { "data": "MaterialName" }, { "data": "Spec" }, { "data": "Unit" }, { "data": "Quantity" }, { "data": "Note" },
            { "data": "StorageId" }]
    });
}

function Recv(OrderNo,Lot) {
    $('#RecvDataDialog').load($("#RecvDataDialog").data("url") + "?OrderNo=" + OrderNo + "&Lot=" + Lot, function () {
        $(this).dialog('open');
        RecvInit();
    });
}
let RecvDataTable;
function RecvInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $("input[type=radio],[type=checkbox]").checkboxradio();
    RecvDataTable = $("#RecvDataTable").DataTable({
        ajax: {
            method: "post",
            url: $("#RecvDataTable").data("url")
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        //"scrollX": true,
        "columns": [{ "data": "SerialNo" }, { "data": "MaterialNo" }, { "data": "MaterialName" }, { "data": "Spec" }, { "data": "Unit" }, { "data": "WareHouseName" },
            { "data": "StorageId" }, { "data": "Quantity" }, { "data": "receivedQty" }, { "data": "UnreceivedQty" }, { "data": "UnreceivedQty" }],
        columnDefs: [
            {
                render: function (data, type, row) {
                    var html = "";
                    html = '<div class="btn-group dropleft">' +
                        '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                        '動作' +
                        ' <span class="caret"></span>' +
                        '</button>' +
                        '<ul class="dropdown-menu" aria-labelledby="about-us">';
                    html += '<li><a  href="#" onclick="FillRecvData(this)">收貨</a></li>';
                    return html + "</ul ></div >";
                },
                targets: -1
            }
        ]
    });

}

function FillRecvData(e) {
    var data = RecvDataTable.row($(e).closest("tr")).data();
    $("#FillRecvDataDialog").dialog({
        autoOpen: false,
        title: "填寫收貨資料",
        width: 1200,
        height: 400,
        modal: true,
        buttons: {
            "儲存": function () {
                var validator = $("#AddRecvDataForm").kendoValidator().data("kendoValidator");
                if (validator.validate()) {
                    var obj = formToJSON($("#AddRecvDataForm"));

                    $.ajax({
                        url: $("#RecvDataTable").data("saveurl"),
                        dataType: 'text',
                        type: 'post',
                        contentType: 'application/json',
                        data: JSON.stringify(obj),
                        processData: false,
                        success: function (data, textStatus, jQxhr) {
                            alert('儲成成功');
                            RecvDataTable.draw();
                            $("#FillRecvDataDialog").dialog("close");
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

    $('#FillRecvDataDialog').load($("#FillRecvDataDialog").data("url"), data, function () {
        $(this).dialog('open');
        FillRecvDataInit();
    });
}

function FillRecvDataInit() {
    $(".combobox").combobox();
}

function storageSelect(e) {
    $.ajax({
        url: '../api/getStorageInfo/' + $(e).val(),
        dataType: 'json',
        type: 'get',
        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            $("#AddRecvDataForm").find("[name='StorageId'] option").remove();
            $("#AddRecvDataForm").find("[name='StorageId']").closest("td").find(".custom-combobox-input").val("");
            
            $("#AddRecvDataForm").find("[name='StorageId']").append($("<option></option>").attr("value", "").text(""));
            $("#AddRecvDataForm").find("[name='StorageId'] option[value='']").attr('selected', true);

            $.each(data, function (index) {
                console.log(index + ": " + data[index]);
                $("#AddRecvDataForm").find("[name='StorageId']").append("<option value='" + data[index].StorageId + "'>" + data[index].StorageId + "</option>");
            });
        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert('發生錯誤');
        }
    });
}

function calUnreceivedQty(e) {

    var ReceivedQty = parseInt($("#AddRecvDataForm").find("[name='ReceivedQty']").val());
    var Quantity = parseInt($("#AddRecvDataForm").find("[name='Quantity']").val());

    if (ReceivedQty > Quantity) {
        alert('已收數量不可大於應收數量');
        $("#AddRecvDataForm").find("[name='ReceivedQty']").val(Quantity);
        ReceivedQty = Quantity;
    }
    var UnreceivedQty = Quantity - ReceivedQty;
    $("#AddRecvDataForm").find("[name='UnreceivedQty']").val(UnreceivedQty);
}

function RecvClose(OrderNo, Lot) {
    
    $("#RecvCloseDialog").dialog({
        autoOpen: false,
        title: "結案",
        width: 1200,
        height: 400,
        modal: true,
        buttons: {
            "結案": function () {
                if (AttachmentArray.length === 0) {
                    alert('必需上傳已核可的收貨單!');
                    $(this).dialog("close");
                }
                else {
                    $.ajax({
                        url: $("#RecvCloseDialog").data("closeurl") + "?OrderNo=" + OrderNo + "&Lot=" + Lot,
                        dataType: 'text',
                        type: 'post',
                        contentType: 'application/json',
                        data: JSON.stringify(AttachmentArray),
                        processData: false,
                        success: function (data, textStatus, jQxhr) {
                            alert('儲存成功');
                            AttachmentArray = [];
                            $('#imgList').empty();
                            RecvTable.draw();
                            $("#RecvCloseDialog").dialog("close");
                        },
                        error: function (jqXhr, textStatus, errorThrown) {
                            alert('儲存失敗');
                        }
                    });
                }
                
            },
            "關閉": function () {
                AttachmentArray = [];
                $('#imgList').empty();
                $(this).dialog("close");
            }
        }
    });

    $('#RecvCloseDialog').load($("#RecvCloseDialog").data("url") + "?OrderNo=" + OrderNo + "&Lot=" + Lot, function () {
        $(this).dialog('open');
        RecvCloseInit(OrderNo, Lot);
    });
}

let RecvCloseTable;
function RecvCloseInit(OrderNo, Lot) {
    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    RecvCloseTable = $("#RecvCloseTable").DataTable({
        ajax: {
            method: "post",
            url: $("#RecvCloseTable").data("url")+"?OrderNo=" + OrderNo + "&Lot=" +Lot
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        "columns": [{ "data": "SerialNo" }, { "data": "MaterialNo" }, { "data": "MaterialName" }, { "data": "Spec" }, { "data": "Unit" }, { "data": "WareHouseName" },
        { "data": "StorageId" }, { "data": "Quantity" }, { "data": "receivedQty" }, { "data": "UnreceivedQty" }]
    });
}

function PrintRecv(OrderNo) {
    window.open("../Report/recv/Recv.aspx?OrderNo=" + OrderNo);
}

function TransToInventory(OrderNo) {
    DialogInit($("#TransToInventoryDialog"), "轉入庫單", {
        "轉入庫單": function () {
            var obj = formToJSON($("#AddRecvDataForm"));
            $.ajax({
                url: $("#RecvDataTable").data("saveurl"),
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('轉入庫單成功');
                    RecvDataTable.draw();
                    $("#TransToInventoryDialog").dialog("close");
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('轉入庫單失敗');
                }
            });
        },
        "關閉": function () {
            $(this).dialog("close");
        }
    });
}

