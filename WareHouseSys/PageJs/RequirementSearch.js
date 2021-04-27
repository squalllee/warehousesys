let RequirementTable;
let RequireDetailTable;
let RequireDetailUpdateTable;
$(document).ready(function () {
    $("#btnSearch").click(function () {
        RequirementTable.draw();
    });

   
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    RequirementTable = $("#RequireTable").DataTable({
        ajax: {
            method: "post",
            url: $("#RequireTable").data("url"),
            data: function (d) {
                let formObj = $("form[name=RequirementForm]").serializeObject();

                $("form[name=RequirementForm] input:checkbox").each(function (i, chk) {
                    formObj[chk.name] = chk.checked;
                });

                $.extend(true, d, formObj);
            }
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        "columns": [{ "data": "OrderNo" }, { "data": "ApplicationDate" }, { "data": "Applicant" }, { "data": "Status" }, { "data": "Status" }],
        columnDefs: [
            {
                render: function (data, type, row) {
                    var html = "";
                    if (row.Status === "辦理中") {
                        html = '<div class="dropdown">' +
                            '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                            '動作' +
                            ' <span class="caret"></span>' +
                            '</button>' +
                            '<ul class="dropdown-menu" aria-labelledby="about-us">';
                        html += '<li><a  href="#" onclick="Update(\'' + row.OrderNo + '\')">修改</a></li><li><a  href="#" onclick="Print(\'' + row.OrderNo + '\')">列印</a></li><li><a  href="#" onclick="Finish(\'' + row.OrderNo + '\')">結案</a></li><li><a  href="#" onclick="Delete(\'' + row.OrderNo + '\')">作廢</a></li></ul></div>';
                    }
                    else if (row.Status === "已結案")
                    {
                        html = '<div class="dropdown">' +
                            '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                            '動作' +
                            ' <span class="caret"></span>' +
                            '</button>' +
                            '<ul class="dropdown-menu" aria-labelledby="about-us">';
                        html += '<li><a  href="#" onclick="TransToPur(\'' + row.OrderNo + '\')">轉採購單</a></li></ul></div>';

                        
                    }
                    //else if (row.Status === "已轉採購單(開口契約)") {
                    //    html = '<div class="dropdown">' +
                    //        '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                    //        '動作' +
                    //        ' <span class="caret"></span>' +
                    //        '</button>' +
                    //        '<ul class="dropdown-menu" aria-labelledby="about-us">';
                    //    html += '<li><a  href="#" onclick="TransToPurWithOpenContract(\'' + row.OrderNo + '\')">轉採購單</a></li></ul></div>';


                    //}
                  
                    return html;
                },
                targets: -1
            },
            {
                render: function (data, type, row) {
                    var html = "<a href='#' onclick='openReqDetail(\"" + row.OrderNo + "\")'>" + row.OrderNo + "</a>";
                    return html;
                },
                targets: 0
            },
            {
                render: function (data, type, row) {
                    return formattedDate(data);
                },
                targets: [1]
            }
        ]
    });

    $("#ReqDetailDialog").dialog({
        autoOpen: false,
        title: "需求明細",
        width: 1200,
        height: 600,
        modal:true,
        buttons: {
            "關閉": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#TransToPurDialog").dialog({
        autoOpen: false,
        title: "新增採購單",
        width: 1900,
        height: 900,
        modal: true,
        buttons: {
            "儲存": function () {
                if ($("#PurchaseAddForm").valid()) {
                    var purchaseHeader = formToJSON($("#PurchaseAddForm"));
                    purchaseHeader.OpenContract = $("#OpenContract").data("kendoSwitch").check();
                    //var purchaseBodyList = [];

                    //$.each(PurchaseAddDetailTable.rows().data(), function (index, val) {
                    //    purchaseBodyList.push(val);
                    //});

                    var saveObj = { purchaseHeader: purchaseHeader, purchaseBodies: grid.dataSource.view() };

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
                            RequirementTable.draw();
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

    $("#PurDetailAddDialog").dialog({
        autoOpen: false,
        title: "新增採購明細",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "儲存": function () {
                var obj = formToJSON($("#PurLocalAddForm"));
                PurchaseAddDetailTable.row.add(obj).draw();
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
                obj.OpenContract = $("#OpenContract").data("kendoSwitch").check();
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
    
    

    $("#ReqUpdateDialog").dialog({
        autoOpen: false,
        title: "修改",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "儲存": function () {

                var validator = $("#updateReqHeaderForm").kendoValidator().data("kendoValidator");

                if (validator.validate()) {
                    var obj = formToJSON($("#updateReqHeaderForm"));
                    obj.SpecifyBrand = $("#SpecifyBrand").prop("checked");
                    if (obj.special === "緊急辦理") {
                        obj.Emergency = true;
                        obj.EmergencyReason = $("#Reason").val();
                    }
                    else if (obj.special === "臨時需求") {
                        obj.Temporary = true;
                        obj.TemporaryReason = $("#Reason").val();
                    }
                   
                    $.ajax({
                        url: "../api/Requirement/updateRequiremenetHeader/",
                        dataType: 'text',
                        type: 'post',
                        contentType: 'application/json',
                        data: JSON.stringify(obj),
                        processData: false,
                        success: function (data, textStatus, jQxhr) {
                            alert('儲成成功');
                            RequirementTable.draw();
                            $("#ReqUpdateDialog").dialog("close");
                        },
                        error: function (jqXhr, textStatus, errorThrown) {
                            alert('儲成失敗');

                        }
                    });
                }
                
            },
            "關閉": function () {
                $('#imgList').empty();
                RequireDetailUpdateTable.draw();
                $(this).dialog("close");
            }
        }
       
    });

    $("#ReqCloseDialog").dialog({
        autoOpen: false,
        title: "結案",
        width: 1300,
        height: 600,
        modal: true,
        buttons: {
            "結案": function () {
                if (AttachmentArray.length === 0) {
                    alert('需上傳需求單!');
                    return;
                }
                $.ajax({
                    url: $("#ReqCloseDialog").data("closeurl") + "?OrderNo=" + $("#OrderNo").val(),
                        dataType: 'text',
                        type: 'post',
                        contentType: 'application/json',
                        data: JSON.stringify(AttachmentArray),
                        processData: false,
                        success: function (data, textStatus, jQxhr) {
                            alert('儲成成功');
                            AttachmentArray = [];
                            $('#imgList').empty();
                            RequirementTable.draw();
                            $("#ReqCloseDialog").dialog("close");
                        },
                        error: function (jqXhr, textStatus, errorThrown) {
                            alert('儲成失敗');
                        }
                    });
                
            },
            "關閉": function () {
                AttachmentArray = [];
                $('#imgList').empty();
                $(this).dialog("close");
            }

        }
    });  
});

function openReqDetail(OrderNo) {
    $('#ReqDetailDialog').load($("#ReqDetailDialog").data("url") + "?OrderNo=" +  OrderNo, function () {
        $(this).dialog('open');
        ReqDetailInit();
    });
}

function ReqDetailInit() {

    RequireDetailTable = $("#RequireDetailTable");

    RequireDetailTable.DataTable({
        ajax: {
            method: "post",
            url: RequireDetailTable.data("url")
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        "scrollX": true,
        "columns": [{ "data": "SerialNo" }, { "data": "MaterialNo" },{ "data": "RequireDate" }, { "data": "PeriodStart1" },
            { "data": "PeriodEnd1" }, { "data": "PeriodQty1" }, { "data": "PeriodStart2" }, { "data": "PeriodEnd2" },
            { "data": "PeriodQty2" }, { "data": "RepairClass" }, { "data": "RequireReason" }],
        columnDefs: [
            {
                render: function (data, type, row) {
                    return formattedDate(data);
                },
                targets: [2,3,4,6,7]
            }
        ]
    });
}

function specialSelect(e) {
    var dataItem = this.dataItem(e.item.index());
    if (dataItem === "緊急辦理" || dataItem === "臨時需求") {
        $("[name='Reason']").prop("disabled", false);
        $("[name='Reason']").prop("required", true);
        $("[name='Reason']").focus();
    }
    else {
        $("[name='Reason']").prop("disabled", true);
        $("[name='Reason']").prop("required", false);
        $("[name='Reason']").val("");
    }
}

function Finish(OrderNo) {
    $('#ReqCloseDialog').load($("#ReqCloseDialog").data("url") + "?OrderNo=" + OrderNo, function () {
        $(this).dialog('open');
        ReqCloseInit();
    });
}

let RequireDetailCloseTable;
function ReqCloseInit() {

    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });
    fileUploadInit();

    RequireDetailCloseTable =  $("#RequireDetailCloseTable").DataTable({
        ajax: {
            method: "post",
            url: $("#RequireDetailCloseTable").data("url")
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        "columns": [{ "data": "MaterialNo" }, { "data": "RequirementQty" }, { "data": "RequireDate" }, { "data": "PeriodStart1" },
        { "data": "PeriodEnd1" }, { "data": "PeriodQty1" }, { "data": "PeriodStart2" }, { "data": "PeriodEnd2" },
            { "data": "PeriodQty2" }, { "data": "RepairClass" }, { "data": "RequireReason" }],
        columnDefs: [
            {
                render: function (data, type, row) {
                    return formattedDate(data);
                },
                targets: [2, 3, 4, 6, 7]
            }
        ]
    });

}

function ReqUpdateInit() {
    $("#btnAdd").click(function (e) {
        $('#ReqDetailAddDialog').load($("#ReqDetailAddDialog").data("url"), function () {
            $(this).dialog('open');
            ReqAddInit();
        });
    });

    $("[name='AcceptanceStd']").click(function (e) {
        if (e.target.value === "其它") {
            $("[name='AcceptanceReason']").prop("disabled", false);
            $("[name='AcceptanceReason']").prop("required", true);
            $("[name='AcceptanceReason']").focus();
        }
        else {
            $("[name='AcceptanceReason']").prop("disabled", true);
            $("[name='AcceptanceReason']").prop("required", false);
            $("[name='AcceptanceReason']").val("");
        }
    });

    RequireDetailUpdateTable = $("#RequireDetailUpdateTable").DataTable({
        ajax: {
            method: "post",
            url: $("#RequireDetailUpdateTable").data("url")
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        "columns": [{ "data": "MaterialNo" }, { "data": "RequirementQty" }, { "data": "RequireDate" }, { "data": "PeriodStart1" },
        { "data": "PeriodEnd1" }, { "data": "PeriodQty1" }, { "data": "PeriodStart2" }, { "data": "PeriodEnd2" },
            { "data": "PeriodQty2" }, { "data": "RepairClass" }, { "data": "RequireReason" }, { "data": "RequireReason" }],
        columnDefs: [
            {
                render: function (data, type, row) {
                    var html = '<div class="dropdown">' +
                        '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                        '動作' +
                        ' <span class="caret"></span>' +
                        '</button>' +
                        '<ul class="dropdown-menu" aria-labelledby="about-us">';
                    html += '<li><a  href="#" onclick="updateDetail(this)">修改</a></li><li><a  href="#" onclick="delDetail(this)">作廢</a></li></ul></div>';

                    return html;
                },
                targets: -1
            },
            {
                render: function (data, type, row) {
                    return formattedDate(data);
                },
                targets: [2, 3, 4, 6, 7]
            }
        ]
    });

    
    $("#ReqDetailAddDialog").dialog({
        autoOpen: false,
        title: "需求明細新增",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "儲存": function () {
                var validator = $("#requireAddForm").kendoValidator().data("kendoValidator");
                if (validator.validate()) {
                    let obj = formToJSON($('#requireAddForm'));
                    obj.OrderNo = $("#OrderNo").val();
                    $.ajax({
                        url: "../api/Requirement/SaveRequirementDetail/",
                        dataType: 'text',
                        type: 'post',
                        contentType: 'application/json',
                        data: JSON.stringify(obj),
                        processData: false,
                        success: function (data, textStatus, jQxhr) {
                            alert('儲存成功');
                            RequireDetailUpdateTable.draw();
                            $("#ReqDetailAddDialog").dialog("close");
                        },
                        error: function (jqXhr, textStatus, errorThrown) {
                            alert('儲存失敗');
                        }
                    });
                  
                }
            },
            "關閉": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#ReqDetailUpdateDialog").dialog({
        autoOpen: false,
        title: "需求明細修改",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "儲存": function () {

                var validator = $("#updateReqDetailForm").kendoValidator().data("kendoValidator");

                if (validator.validate())  {
                    let saveObj = {
                        OrderNo: $("#OrderNo").val(),
                        SerialNo: $("#SerialNo").val(),
                        MaterialNo: $("#UpdateMaterialNo").val(),
                        RequireUnit: $("#UpdateRequireUnit").val(),
                        RequireDate: $("#UpdateRequireDate").val(),
                        RequirementQty: $("#UpdateRequirementQty").val(),
                        EstPrice: $("#UpdateEstPrice").val(),
                        Inventory: $("#UpdateInventory").val(),
                        OnOrderInventory: $("#UpdateOnOrderInventory").val(),
                        PeriodStart1: $("#UpdatePeriodStart1").val(),
                        PeriodEnd1: $("#UpdatePeriodEnd1").val(),
                        PeriodQty1: $("#UpdatePeriodQty1").val(),
                        PeriodStart2: $("#UpdatePeriodStart2").val(),
                        PeriodEnd2: $("#UpdatePeriodEnd2").val(),
                        PeriodQty2: $("#UpdatePeriodQty2").val(),
                        DeliveryPeriod: $("#UpdateDeliveryPeriod").val(),
                        RequireReason: $("#UpdateRequireReason").val(),
                        RepairClass: $("#RepairClass").val()
                    };
                    //saveObj.AttachmentArray = AttachmentArray;

                    $.ajax({
                        url: "../api/Requirement/UpdateRequirementDetail/",
                        dataType: 'text',
                        type: 'post',
                        contentType: 'application/json',
                        data: JSON.stringify(saveObj),
                        processData: false,
                        success: function (data, textStatus, jQxhr) {
                            alert('儲成成功');
                            AttachmentArray = [];
                            $('#imgList').empty();
                            RequireDetailUpdateTable.draw();
                        },
                        error: function (jqXhr, textStatus, errorThrown) {
                            alert('儲成失敗');
                        }
                    });
                    $(this).dialog("close");
                }

            },
            "關閉": function () {
                $(this).dialog("close");
            }
        }
    });
}

function updateDetail(e) {
    var obj = RequireDetailUpdateTable
        .row($(e).closest("tr"))
        .data();

    $('#ReqDetailUpdateDialog').load($("#ReqDetailUpdateDialog").data("url") + "?OrderNo=" + obj.OrderNo + "&SerialNo=" + obj.SerialNo, function () {
        $(this).dialog('open');
        DetailUpdateInit();
    });
}

function DetailUpdateInit() {

    $(".datetime").datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $(".combobox").combobox();

    $("#UpdateMaterialNo").kendoMultiColumnComboBox({
        placeholder: "選擇料號",
        dataTextField: "MaterialNo",
        dataValueField: "MaterialNo",
        text: $("#UpdateMaterialNo").val(),
        value: $("#UpdateMaterialNo").val(),
        filter: "contains",
        filterFields: ["MaterialNo", "MaterialName"],
        height: 400,
        autoBind: false,
        enabled:false,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/Material/getMaterialInfo"
                }
            }
        },
        columns: [
            { field: "MaterialName", title: "品名", width: 250 },
            { field: "MaterialNo", title: "料號", width: 150 }
        ],
        select: function (e) {
            var obj = e.dataItem;
            $("#UpdateMaterialName").val(obj.MaterialName);
            $("#UpdateSpec").val(obj.Spec);
            $("#UpdateUnit").val(obj.Unit);
            $("#UpdateSafetyStock").val(obj.SafetyStock);
            $("#UpdateReplaceNo").val(obj.ReplaceNo);
            $("#UpdateReplaceQuantity").val(obj.ReplaceQuantity);

            if (obj.OnOrderInventory === null) $("#UpdateOnOrderInventory").val('0');
            else $("#UpdateOnOrderInventory").val(obj.OnOrderInventory);
            $("#UpdateInventory").val(obj.Inventory);
            if (obj.IsFix) $("#UpdateIsFix").val('是');
            else $("#UpdateIsFix").val('否');

            $("#UpdateRepairPeriod").val(obj.RepairPeriod);
            $("#UpdateFailureRate").val(obj.FailureRate);
            $("#UpdateEqQuantity").val(obj.EqQuantity);
            if (obj.IsDangerous) $("#UpdateIsDangerous").val('是');
            else $("#UpdateIsDangerous").val('否');

            $("#UpdateExpiration").val(obj.Expiration);
            if (obj.Simple) $("#UpdateSimple").val('是');
            else $("#UpdateSimple").val('否');
        }
    });
    
}

function Print(OrderNo) {
    window.open("../Report/Requirement/RequirementReport.aspx?OrderNo=" + OrderNo);
}

function Update(OrderNo) {
    $('#ReqUpdateDialog').load($("#ReqUpdateDialog").data("url") + "?OrderNo=" + OrderNo, function () {
        $(this).dialog('open');
        ReqUpdateInit();
    });
}

function Delete(OrderNo) {
    if (confirm("是否確定要作廢?")) {
        $.ajax({
            url: "../api/Requirement/DeleteRequirement/?OrderNo=" + OrderNo,
            dataType: 'text',
            type: 'get',
            contentType: 'application/json',
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('作廢成功');

                RequirementTable.draw();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('作廢失敗');
            }
        });
    }
    
}

function ReqAddInit() {
    $('.datetime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $(".combobox").combobox();

    $("#MaterialNo").kendoMultiColumnComboBox({
        placeholder: "選擇料號",
        dataTextField: "MaterialNo",
        dataValueField: "MaterialNo",
        filter: "contains",
        filterFields: ["MaterialNo", "MaterialName"],
        height: 400,
        autoBind: false,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/Material/getMaterialInfo"
                }
            }
        },
        columns: [
            { field: "MaterialName", title: "品名", width: 250 },
            { field: "MaterialNo", title: "料號", width: 150 }
        ],
        select: function (e) {
            var obj = e.dataItem;
            $("#MaterialName").val(obj.MaterialName);
            $("#Spec").val(obj.Spec);
            $("#Unit").val(obj.Unit);
            $("#SafetyStock").val(obj.SafetyStock);
            $("#ReplaceNo").val(obj.ReplaceNo);
            $("#ReplaceQuantity").val(obj.ReplaceQuantity);

            if (obj.OnOrderInventory === null) $("#OnOrderInventory").val('0');
            else $("#OnOrderInventory").val(obj.OnOrderInventory);
            $("#Quantity").val(obj.Quantity);
            if (obj.IsFix) $("#IsFix").val('是');
            else $("#IsFix").val('否');

            $("#RepairPeriod").val(obj.RepairPeriod);
            $("#FailureRate").val(obj.FailureRate);
            $("#EqQuantity").val(obj.EqQuantity);
            if (obj.IsDangerous) $("#IsDangerous").val('是');
            else $("#IsDangerous").val('否');

            $("#Expiration").val(obj.Expiration);
            if (obj.Simple) $("#Simple").val('是');
            else $("#Simple").val('否');

            $("#Spec").val(obj.Spec);
        }
    });
}


function delDetail(e) {
    var obj = RequireDetailUpdateTable
        .row($(e).closest("tr"))
        .data();
    $.ajax({
        url: "../api/Requirement/DeleteRequirementDetail/?OrderNo=" + $("#OrderNo").val() + "&MaterialNo=" + obj.MaterialNo + "&SerialNo=" + obj.SerialNo,
        dataType: 'text',
        type: 'get',
        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            alert('作廢成功');
           
            RequireDetailUpdateTable.draw();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert('作廢失敗');
        }
    });
}

function TransToPur(OrderNo) {
    $('#TransToPurDialog').load($("#TransToPurDialog").data("url") + "?requireNo=" + OrderNo, function () {
        $(this).dialog('open');
        TransToPurInit();
    });
   
}


function TransToPurWithOpenContract(OrderNo) {

    transToPurDialog = $("<div></div>").kendoWindow({
        title: "新增採購單",
        actions: ["Close"],
        content: $("#TransToPurDialog").data("url") + "?requireNo=" + OrderNo,
        visible: false,
        modal: true,
        width: 1000,
        position: {
            top: "50px",
            left: "15%"
        },
        refresh: function (e) {
            transToPurDialogInit();
        },
        close: function (e) {
            transToPurDialog.destroy();
        }
    }).data("kendoWindow").open();
}

let PurchaseAddDetailTable;
let purDataSource = [];
function TransToPurInit() {
    $('#BudgetSource').fastselect({
        placeholder: "請選擇科目"
    });

    $("dateTime").datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $(".combobox").combobox();

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    var datasource = new kendo.data.DataSource({
        offlineStorage: "offlineStorage",
        transport: {
            read: {
                url: $("#grid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                return JSON.stringify(options);
            }
        },
        requestEnd: function (e) {
            datasource.online(false);
        },
        schema: {
            data: "data",
            model: {
                id: "SerialNo",
                fields: {
                    MaterialNo: { type: "string", editable: false },
                    MaterialName: { type: "string", editable: false },
                    Spec: { type: "string", editable: false },
                    Unit: { type: "string", editable: false },
                    PerformancePeriod: { type: "date" },
                    Quantity: {
                        type: "number",
                        validation: {
                            min: 1,
                            required: true
                        }
                    }
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" && e.field === "RequireUnitName") {
                var model = e.items[0];
                var selectedData = $("[name='RequireUnitName']").data("kendoMultiColumnComboBox").dataItem();

                model.RequireUnitName = selectedData.UNITNAME;
                model.RequireUnit = selectedData.UNITNO;


                return;
            }
            if (e.action === "add") {
                e.items[0].PerformancePeriod = formattedDate(e.items[0].PerformancePeriod);
            }
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });

    datasource.bind("error", dataSource_error);

    grid = $("#grid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 370,
        width: 1850,
        //pageable : true,
        dataBound: function (e) {
            var totalPriceWithoutTax = 0;
            var data = grid.dataSource.view();
            data.forEach(function (obj) {
                if (obj.Price !== "") {
                    totalPriceWithoutTax += parseInt(obj.Price * obj.Quantity);
                }
            });

            $("[name='ContractPriceWithoutVAT']").val(totalPriceWithoutTax);
            $("[name='ContractPriceIncludeVAT']").val(Math.round(totalPriceWithoutTax * 1.05));

            $("#ContractPriceWithoutVATDisplay").text("總契約金額(未稅):" + totalPriceWithoutTax);
            $("#ContractPriceIncludeVATDisplay").text("總契約金額(含稅):" + Math.round(totalPriceWithoutTax * 1.05));

        },
        columns: [
            { command: ["edit", "destroy", { text: "複制", click: Copy }], title: "&nbsp;", width: 250, locked: true, lockable: true },
            {
                field: "MaterialNo",
                title: "品號",
                width: 200

            },
            {
                field: "SerialNo",
                hidden: true
            },
            {
                field: "MaterialName",
                title: "品名",
                width: 200
            },
            {
                field: "Spec",
                title: "規格",
                width: 200
            }, {
                field: "Unit",
                title: "單位",
                width: 80
            },
            {
                field: "Quantity",
                title: "契約數量",
                width: 100
            },
            {
                field: "Price",
                title: "單價未稅",
                width: 100
            },
            {
                field: "DeliveryLot",
                title: "交貨批次",
                width: 100
            },
            {
                field: "DeliveryPlace",
                title: "交貨地點",
                width: 200
            },
            {
                field: "PerformancePeriod",
                title: "履約期限",
                template: '#= PerformancePeriod !== null ? kendo.toString(PerformancePeriod, "yyyy/MM/dd") : ""#',

                width: 200
            },
            {
                field: "RequireUnitName",
                title: "需求單位",
                width: 200,
                editor: UnitCombobox
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
    
}

function UnitCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇需求單位",
        dataTextField: "UNITNAME",
        dataValueField: "UNITNO",
        filter: "contains",
        filterFields: ["UNITNAME", "UNITNO"],
        height: 400,
        autoBind: false,
        text: options.model["RequireUnitName"],
        value: options.model["RequireUnit"],
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/Unit/getUnit"
                }
            }
        },
        columns: [
            { field: "UNITNAME", title: "單位名稱", width: 150 },
            { field: "UNITNO", title: "單位代碼", width: 150 }
        ]
    });

}

function Copy(e) {

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    grid.dataSource.add(JSON.parse(kendo.stringify(dataItem)));

}

function PurLocalAddInit() {
    $(".dateTime").datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $(".combobox").combobox();

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
                url: "../api/Material/getMaterialInfoByNo?MaterialNo=" + ui.item.value,
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

function updatePurDetail(e) {
    var obj = PurchaseAddDetailTable
        .row($(e).closest("tr"))
        .data();
    $('#PurLocalUpdateDialog').load($("#PurLocalUpdateDialog").data("url"), obj, function () {
        PurLocalUpdateInit();
        $("#PurLocalUpdateDialog").dialog('open');
    });
}

function PurLocalUpdateInit() {
    $(".dateTime").datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $(".combobox").combobox();
}

function selectUnit(e) {
    $(e).closest("td").find("[name='RequireUnitName']").val($(e).closest("td").find("[name='ReqireUnit'] option:selected").text());
    
}

function BrandChange(e) {
    if (e.checked) {
        $("[name='SpecifyReason']").prop("disabled", false);
        $("[name='SpecifyReason']").prop("required", true);
        $("[name='SpecifyReason']").focus();
    }
    else {
        $("[name='SpecifyReason']").prop("required", false);
        $("[name='SpecifyReason']").prop("disabled", true);
        $("[name='SpecifyReason']").val("");
    }
}

function EmergencyChange(e) {
    if (e.checked) {
        $("[name='EmergencyReason']").prop("disabled", false);
        $("[name='EmergencyReason']").prop("required", true);
        $("[name='EmergencyReason']").focus();
    }
    else {
        $("[name='EmergencyReason']").prop("disabled", true);
        $("[name='EmergencyReason']").prop("required", false);
        $("[name='EmergencyReason']").val("");
    }
}

function TemporaryChange(e) {
    if (e.checked) {
        $("[name='TemporaryReason']").prop("disabled", false);
        $("[name='TemporaryReason']").prop("required", true);
        $("[name='TemporaryReason']").focus();
    }
    else {
        $("[name='TemporaryReason']").prop("disabled", true);
        $("[name='TemporaryReason']").prop("required", false);
        $("[name='TemporaryReason']").val("");
    }
}

function AcceptanceChange(e) {
    if (e.checked) {
        $("[name='AcceptanceReason']").prop("disabled", false);
        $("[name='AcceptanceReason']").prop("required", true);
        $("[name='AcceptanceReason']").focus();
    }
    else {
        $("[name='AcceptanceReason']").prop("disabled", true);
        $("[name='AcceptanceReason']").prop("required", false);
        $("[name='AcceptanceReason']").val("");
    }
}

function delPurDetail(e) {
    var obj = PurchaseAddDetailTable
        .row($(e).closest("tr"))
        .data();
    $.ajax({
        url: "../api/Requirement/DeleteRequirementDetail/?OrderNo=" + obj.OrderNo + "&MaterialNo=" + obj.MaterialNo + "&SerialNo=" + obj.reqSerialNo,
        dataType: 'text',
        type: 'get',
        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            alert('作廢成功');

            PurchaseAddDetailTable.draw();
        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert('作廢失敗');
        }
    });
}


