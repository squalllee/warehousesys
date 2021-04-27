
$(function () {
    //$('#tt').datagrid({
    //    toolbar: [{
    //        iconCls: 'icon-add',
    //        text:'新增',
    //        handler: function () {
    //            var rows = $('#dg').datagrid('getSelections');
    //            alert(rows);
    //        }
    //    }, '-', {
    //            iconCls: 'icon-edit',
    //            text: '修改',
    //            handler: function () { alert('edit'); }
    //        }, '-', {
    //            iconCls: 'icon-remove',
    //            text: '刪除',
    //            handler: function () { alert('remove'); }
    //    }]
    //});

});

var url;
function LocationSearch() {
    var o = {};
    o.txtLocationId = $("#txtLocationId").textbox("getText");
    o.txtLocationDescription = $("#txtLocationDescription").textbox("getText");

    $('#locationdg').datagrid('load', o);
    //$.post('../api/Location/searchLocation', o, function (result) {
    //    $('#locationdg').datagrid('loadData', result);
    //}, 'json');
}

function doAdd() {
    var selectedRow = $("#locationdg").datagrid("getSelected");
    if (selectedRow) {
        $('#dlg').dialog('open').dialog('center').dialog('setTitle', '修改位置資訊');
        $('#fm').form('load', selectedRow);

        //$.ajax({
        //    type: 'Post',
        //    url: "../api/Location/updateLocation",
        //    datatype: 'json',
        //    success: function (data) {
        //        $('option', $("#SubSystem")).remove();

        //        $("#SubSystem").append($("<option></option>").attr("value", "").text("請選擇"));
        //        $(data).each(function (i) {
        //            $("#SubSystem").append($("<option></option>").attr("value", data[i].SubSystemId).text(data[i].SubSystemName));

        //        });
        //        $("#SubSystem").selectmenu("refresh");
        //    },
        //    error: function (response) {
        //        alert("系統發生錯誤");
        //    }
        //});
    }
}
function doUpdate() {
    var selectedRow = $("#locationdg").datagrid("getSelected");
    if (selectedRow) {
        $('#dlg').dialog('open').dialog('center').dialog('setTitle', '修改位置資訊');
        $('#fm').form('load', selectedRow);

        //$.ajax({
        //    type: 'Post',
        //    url: "../api/Location/updateLocation",
        //    datatype: 'json',
        //    success: function (data) {
        //        $('option', $("#SubSystem")).remove();

        //        $("#SubSystem").append($("<option></option>").attr("value", "").text("請選擇"));
        //        $(data).each(function (i) {
        //            $("#SubSystem").append($("<option></option>").attr("value", data[i].SubSystemId).text(data[i].SubSystemName));

        //        });
        //        $("#SubSystem").selectmenu("refresh");
        //    },
        //    error: function (response) {
        //        alert("系統發生錯誤");
        //    }
        //});
    } 
}

function doDelete() {
    var selectedRow = $("#locationdg").datagrid("getSelected");
    if (selectedRow) {
        $.messager.confirm('確認', '是否確定要刪除此位置?', function (r) {
            if (r) {
                //$.post('destroy_user.php', { id: row.id }, function (result) {
                //    if (result.success) {
                //        $('#dg').datagrid('reload');    // reload the user data
                //    } else {
                //        $.messager.show({    // show error message
                //            title: 'Error',
                //            msg: result.errorMsg
                //        });
                //    }
                //}, 'json');
            }
        });
    }
}



