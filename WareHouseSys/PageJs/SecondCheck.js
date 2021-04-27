function SecondCheckInit() {
    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    $("#BtnSetSecondCheck").click(function () {
        if (AttachmentArray.length === 0) {
            alert('必需上傳盤點表!');
            return;
        }

        $.ajax({
            url: $("#BtnSetSecondCheck").data("complete"),
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            processData: false,
            data: JSON.stringify(AttachmentArray),
            success: function (data, textStatus, jQxhr) {
                alert('儲存成功');
                AttachmentArray = [];
                InventorySearchGrid.dataSource.read();
                $("#SecondCheckDialog").data("kendoWindow").close();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('儲存失敗');
            }
        });
    });

    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#SecondCheckGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#SecondCheckGrid").data("updateurl"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                if (operation === "update") {
                    return JSON.stringify(options.models);
                }
                else {
                    return JSON.stringify(options);
                }
            }
        },
        schema: {
            data: "data",
            model: {
                id: "MaterialNo",
                fields: {
                    MaterialNo: { type: "string", editable: false },
                    MaterialName: { type: "string", editable: false },
                    Spec: { type: "string", editable: false },
                    Quantity: { editable: false },
                    StorageId: { type: "string", editable: false },
                    Lot: { type: "string", editable: false },
                    SecondCheckQty: {
                        type: "number",
                        validation: {
                            min: 1,
                            required: true
                        }
                    },
                    WareHouseName: { type: "string", editable: false },
                    FirstCheckQty: { editable: false },
                    Note: {type:"string"}
                }
            }
        },
        batch: true
    });
    datasource.bind("error", dataSource_error);

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    $("#SecondCheckGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 600,
        width: 1300,
        sortable: true,
        filterable: {
            extra: false,
            operators: {
                string: {
                    contains: "包含"
                }
            }
        },
        toolbar: [{ name: "save", text: "儲存" }],
        columns: [
            //{ command: ["edit"], title: "&nbsp;", width: 100, locked: true, lockable: true },
            {
                field: "MaterialNo",
                title: "品號",
                width: 200,
                locked: true,
                lockable: true

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
                width: 100
            },
            {
                field: "Quantity",
                title: "應盤量",
                width: 100
            },
            {
                field: "FirstCheckQty",
                title: "初盤量",
                template: "<span class='#if(FirstCheckQty !== Quantity) {#diff#}#'>#=FirstCheckQty#</span>",
                width: 100
            },
            {
                field: "SecondCheckQty",
                title: "複盤量",
                width: 100,
                template: "<span class='#if(SecondCheckQty !== Quantity) {#diff#}#'>#=SecondCheckQty#</span>",
                editor: EditSecondCheckQty
            },
            {
                field: "Note",
                title: "備註",
                width: 100
            }
        ],
        editable: true
    }).data("kendoGrid");
}

function EditSecondCheckQty(Container, options) {
    $('<input name="' + options.field + '" type="number" title="numeric"  min="0"  step="1" style="width: 100 %; " />').appendTo(Container).kendoNumericTextBox();
}