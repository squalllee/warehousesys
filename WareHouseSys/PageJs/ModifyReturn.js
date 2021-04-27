function ModifyReturnInit() {
    $("#btnSave").click(function (e) {
        var validator = $("#ReturnModifyForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var obj = formToJSON($("#ReturnModifyForm"));

            $.ajax({
                url: "../api/Return/updateReturnHeader",
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    ModifyReturnGrid.dataSource.read();
                    ModifyReturnaDialog.close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#ModifyReturnGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#ModifyReturnGrid").data("updateurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            destroy: {
                url: $("#ModifyReturnGrid").data("deleteurl"),
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
            model: {
                id: "SerialNo",
                fields: {
                    SerialNo: { type: "string", editable: false },
                    MaterialNo: { type: "string", editable: false },
                    MaterialName: { type: "string", editable: false },
                    Spec: { type: "string", editable: false },
                    WareHouseName: { type: "string", editable: false },
                    StorageId: { type: "string", editable: false },
                    Lot: { type: "string", editable: false },
                    Note: { type: "string"},
                    Quantity: { type: "number", validation: { min: 0 } }
                }
            }
        },
        serverPaging: true,
        pageSize: 10,
        serverSorting: true,
        serverFiltering: true,
        pageable: true
    });

    ModifyReturnGrid = $("#ModifyReturnGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1550,
        sortable: true,
        columns: [           
            {
                command: ["edit","destroy"],
                title: "&nbsp;",
                width: 200,
                locked: true,
                lockable: true
            },
            {
                field: "SerialNo",
                title: "序號",
                width: 50

            }, {
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
                field: "WareHouseName",
                title: "庫別",
                width: 200
            }, {
                field: "StorageId",
                title: "儲位",
                width: 150
            }, {
                field: "Lot",
                title: "批號",
                width: 100

            },
            {
                field: "Quantity",
                title: "退料量",
                width: 100
            }, {
                field: "Note",
                title: "備註",
                width: 150
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}