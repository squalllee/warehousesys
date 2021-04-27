var ExtendGrid;
function ExtendLendInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    function dataSource_error(e) {
        if (e.xhr.status === 911) {
            alert('此借出單都已歸還無法展延!');
        }
        else {
            alert('發生錯誤');
        }
    }

    $("#BtnSaveBack").click(function (e) {
        var validator = $("#ExtenForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var obj = {};
            obj.extendHeaderViewModel = formToJSON($("#ExtenForm"));

            obj.extendBodies = $("#ExtendGrid").data("kendoGrid").dataSource.view();

            var url = $("#ExtenForm").data("saveurl");

            $.ajax({
                url: url,
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    $("#ExtendLendDialog").data("kendoWindow").close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert(jQuery.parseJSON(jqXhr.responseText).Message);
                }
            });
        }
    });

    var datasource = new kendo.data.DataSource({
        offlineStorage: "offlineStorage",
        transport: {
            read: {
                url: $("#ExtendGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#ExtendGrid").data("updateurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                if (operation === "create") {
                    options.OrderNo = $("[name='OrderNo']").val();
                }
                return JSON.stringify(options);
            }
        },
        requestEnd: function (e) {
            var response = e.response;
            var type = e.type;
            if (type === "read") {
                datasource.online(false);
            }
        },
        schema: {
            data: "data",
            model: {
                id: "MaterialNo"
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


    ExtendGrid = $("#ExtendGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1000,
        sortable: true,
        columns: [
            {
                field: "MaterialNo",
                title: "品號",
                width: 200

            }, {
                field: "MaterialName",
                title: "品名",
                width: 200
            },
            {
                field: "Spec",
                title: "規格",
                width: 200
            }, {
                field: "Lot",
                title: "批號",
                width: 100
            },
            {
                field: "LendQty",
                title: "借出數量",
                width: 100
            },
            {
                field: "NotReturnQty",
                title: "未還量",
                editor: EditReturnQty,
                width: 100
            }, {
                field: "ExtendQty",
                title: "展延數量",
                width: 100
            }
        ]
    }).data("kendoGrid");

}
