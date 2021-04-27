function ModifyBackInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    $("#BtnSaveBack").click(function (e) {
        var validator = $("#BackForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var obj = {};
            obj.backHeaderViewModel = formToJSON($("#BackForm"));
            obj.backBodies = $("#BackGrid").data("kendoGrid").dataSource.view();

            var url = $("#BackForm").data("saveurl");


            $.ajax({
                url: url,
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    $("#BackLendDialog").data("kendoWindow").close();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert('儲存失敗');
                }
            });
        }
    });

    var datasource = new kendo.data.DataSource({
        offlineStorage: "offlineStorage",
        transport: {
            read: {
                url: $("#BackGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#BackGrid").data("updateurl"),
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
        schema: {
            data: "data",
            model: {
                id: "MaterialNo",
                fields: {
                    OrderNo: { type: "string", editable: false },
                    MaterialNo: { type: "string", editable: false },
                    MaterialName: { type: "string", editable: false },
                    Spec: { type: "string", editable: false },
                    StorageId: { editable: false },
                    Lot: { editable: false },
                    BackQty: { editable: false },
                    LendQty: { editable: false },
                    NotReturnQty: {
                        type: "number",
                        validation: {
                            min: 1,
                            required: true
                        }
                    },
                    WareHouseName: { type: "string", editable: false}
                }
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


    BackGrid = $("#BackGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 250,
        sortable: true,
        columns: [
            { command: ["edit"], title: "&nbsp;", width: 100, locked: true, lockable: true },
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
                field: "WareHouseName",
                title: "庫別",
                width: 200,
                template: '#= WareHouseName !== null ? WareHouseName : "" #'
            }, {
                field: "StorageId",
                title: "儲位",
                width: 100
            }, {
                field: "Lot",
                title: "批號",
                width: 100
            },
            {
                field: "Quantity",
                title: "數量",
                width: 100
            }
            ,
            {
                field: "Note",
                title: "備註",
                width: 100
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}