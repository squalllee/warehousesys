var scheduleTable;

$(document).ready(function () {
    $("#System").selectmenu();
    $("#newSystem").selectmenu();
    $("#SubSystem").selectmenu();
    $("#EqName").selectmenu();
    $("#Status").selectmenu();
    $("#newScheduleCycle").selectmenu();
    $("#newEquipment").selectmenu();
    $("#newSubSystem").selectmenu();
    
  
    scheduleTable = $('#schedule-table').DataTable({
        processing: true,
        "searching": true,
        "info": false,
        serverSide: true,
        //deferLoading: 0,
        "lengthChange": false,
        "sort": false,
        "ajax": {
            "url": "../ScheduleApi/getSchedule",
            "type": 'POST'
        },
        "columns": [
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#'>" + row.ScheduleId + "</a>";
                }
            },
            { "data": "ScheduleName" },
            { "data": "SystemName" },
            { "data": "SubSystemName" },
            { "data": "EquipmentName" },
            { "data": "Description" },
            { "data": "ExcuteDay" },
            {
                data: null, render: function (data, type, row) {
                    if (row.IsProcess === "N") {
                        return "否";
                    }
                    else {
                        return "是";
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    if (row.Status === "0") {
                        return "待送陳";
                    }
                    else if (row.Status === "1") {
                        return "待審核";
                    }
                    else {
                        return "已核可";
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-warning'>修改</a><a href='#' class='btn btn-success'>送陳</a><a href='#' class='btn btn-danger'>停用</a>";
                }
            }

        ]
    });

    $("#System").on("selectmenuchange", function () {
        if ($(this).val() === "") {
            $('option', $("#SubSystem")).remove();
            $("#SubSystem").append($("<option></option>").attr("value", "").text("請選擇"));
            $("#SubSystem").selectmenu("refresh");
            return;
        }

        $.ajax({
            type: 'Get',
            url: "../api/Schedule/getSubSytem/" + $(this).val(),
            datatype: 'json',
            success: function (data) {
                $('option', $("#SubSystem")).remove();
                   
                $("#SubSystem").append($("<option></option>").attr("value", "").text("請選擇"));
                $(data).each(function (i) {
                    $("#SubSystem").append($("<option></option>").attr("value", data[i].SubSystemId).text(data[i].SubSystemName));
                       
                });
                $("#SubSystem").selectmenu("refresh");
            },
            error: function (response) {
                alert("系統發生錯誤");
            }
        });
    });
   
});


function Search() {
    var obj = $("#searchForm").serializeArray();

    var o = {};
    $.each(obj, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });

    scheduleTable.search(JSON.stringify(o)).draw();
}