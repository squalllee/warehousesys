var dataSource = [];
var counter;
let RequireDetailTable;

$(document).ready(function () {
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

    $("#btnAdd").click(function (e) {
        $('#ReqAddDialog').load($("#ReqAddDialog").data("url"), function () {
            $(this).dialog('open');
            ReqAddInit();
        });
    });

    $("#btnSave").click(function (e) {
        var validator = $("#reqHeader").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var reqHeader = formToJSON($("#reqHeader"));
            reqHeader.SpecifyBrand =  $("#SpecifyBrand").data("kendoSwitch").check();
            if (reqHeader.special === "緊急辦理") {
                reqHeader.Emergency = true;
                reqHeader.EmergencyReason = $("#Reason").val();
            }
            else if (reqHeader.special === "臨時需求") {
                reqHeader.Temporary = true;
                reqHeader.TemporaryReason = $("#Reason").val();
            }
            reqHeader.OrderNo = "";
            reqHeader.ApplicationDate = $("#FillDate").val();
            reqHeader.Applicant = $("#RequireMan option:selected").val();

            var reqBodies = [];

            $.each(RequireDetailTable.rows().data(), function (index, val) {
                val.OrderNo = "";
                reqBodies.push(val);
            });

            var saveObj = {
                requirementHeader: reqHeader,
                requirementBodies: reqBodies
            };

            $.ajax({
                url: "../api/Requirement/SaveRequirement/",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(saveObj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲成成功');

                    window.location = "../Requirement/RequirementAdd";
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲成失敗');

                }
            });
        }
        
    });
    

    $("dateTime").datepicker({
        dateFormat: 'yy/mm/dd'
    });

    $(".combobox").combobox();

    RequireDetailTable = $("#RequireDetailTable").DataTable({
        data: dataSource,
        searching: false,
        "processing": true,
        "serverSide": false,
        bLengthChange: false,
        "columns": [{ "data": "MaterialNo" }, { "data": "RequirementQty" }, { "data": "PeriodStart1" },
        { "data": "PeriodEnd1" }, { "data": "PeriodQty1" }, { "data": "PeriodStart2" }, { "data": "PeriodEnd2" },
            { "data": "PeriodQty2" }, { "data": "RepairClass" }, { "data": "RequireReason" } , { "data": "RequireReason" }],
        columnDefs: [
            {
                render: function (data, type, row) {
                    var html = '<div class="dropdown">' +
                        '<button class="btn btn-primary dropdown-toggle" type="button" id="about-us" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                        '動作' +
                        ' <span class="caret"></span>' +
                        '</button>' +
                        '<ul class="dropdown-menu" aria-labelledby="about-us">';
                    html += '<li><a  href="#" onclick="del(this)">刪除</a></li></ul></div>';

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

    $("#ReqAddDialog").dialog({
        autoOpen: false,
        title: "新增需求",
        width: 1200,
        height: 600,
        modal: true,
        buttons: {
            "Ok": function () {
                var validator = $("#requireAddForm").kendoValidator().data("kendoValidator");

                if (validator.validate()) {
                    let obj = formToJSON($('#requireAddForm'));
                    dataSource.push(obj);
                    RequireDetailTable.row.add(obj).draw();

                    $(this).dialog("close");
                }
            },
            "關閉": function () {
                $(this).dialog("close");
            }
        }
    });
});

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
        autoBind: true,
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

function UnitChange(e) {
    $("#RequireUnit").val($("#RequireMan option:selected").data("unit"));
}

function update(e) {
    var obj = RequireDetailTable
        .row($(e).closest("tr"))
        .data();
}

function del(e)
{
    RequireDetailTable
        .row($(e).closest("tr"))
        .remove().draw();
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

function specialSelect(e) {
    var dataItem = this.dataItem(e.item.index());
    if (dataItem === "緊急辦理" || dataItem === "臨時需求") {
        $("[name='Reason']").prop("disabled", false);
        $("[name='Reason']").prop("required", true);
        $("[name='Reason']").focus();
    }
    else{
        $("[name='Reason']").prop("disabled", true);
        $("[name='Reason']").prop("required", false);
        $("[name='Reason']").val("");
    }
}

//function EmergencyChange(e) {
//    var switchInstance = $("#Temporary").data("kendoSwitch");
//    if (e.checked) {
//        $("[name='EmergencyReason']").prop("disabled", false);
//        $("[name='EmergencyReason']").prop("required", true);
//        $("[name='EmergencyReason']").focus();

//        switchInstance.trigger("change");
//    }
//    else {
//        $("[name='EmergencyReason']").prop("disabled", true);
//        $("[name='EmergencyReason']").prop("required", false);
//        $("[name='EmergencyReason']").val("");
//    }
//}

//function TemporaryChange(e) {
//    if (e.checked) {
//        $("[name='TemporaryReason']").prop("disabled", false);
//        $("[name='TemporaryReason']").prop("required", true);
//        $("[name='TemporaryReason']").focus();
//    }
//    else {
//        $("[name='TemporaryReason']").prop("disabled", true);
//        $("[name='TemporaryReason']").prop("required", false);
//        $("[name='TemporaryReason']").val("");
//    }
//}

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