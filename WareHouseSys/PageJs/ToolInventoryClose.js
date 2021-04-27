function InventoryCloseInit() {
    $("#BtnClose").click(function () {
        $.ajax({
            url: $("#InventoryCloseForm").data("closeurl") + "/" + $("[name='OrderNo']").val(),
            dataType: 'text',
            type: 'get',
            contentType: 'application/json',
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('儲存成功');
                ToolInventorySearchGrid.dataSource.read();
                $("#InventoryCloseDialog").data("kendoWindow").close();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('儲存失敗');
            }
        });
    });

    datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#InventoryCloseGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                return JSON.stringify(options);
            }
        },
        schema: {
            data: "data",
            total: "Total",
            model: {
                id: "MaterialNo"
            }
        },
        serverPaging: true,
        pageSize: 10000,
        serverSorting: true,
        serverFiltering: false
    });
    datasource.bind("error", dataSource_error);

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    $("#InventoryCloseGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 450,
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
                field: "KeepMan",
                title: "保管人",
                width: 200
            }, 
            {
                field: "Quantity",
                title: "庫存",
                width: 100
            },
            {
                field: "FirstCheckQty",
                title: "初盤量",
                width: 100
            },
            {
                field: "SecondCheckQty",
                title: "複盤量",
                width: 100
            },
            {
                field: "diffQty",
                title: "盤差",
                width: 100
            },
            {
                field: "Note",
                title: "備註",
                width: 200
            }
        ]
    }).data("kendoGrid");
}