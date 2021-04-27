function FirstCheckInit() {
    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    $("#BtnSetFirstCheck").click(function () {
        if (AttachmentArray.length === 0) {
            alert('必需上傳盤點表!');
            return;
        }

        $.ajax({
            url: $("#BtnSetFirstCheck").data("complete"),
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            processData: false,
            data: JSON.stringify(AttachmentArray),
            success: function (data, textStatus, jQxhr) {
                alert('儲存成功');
                ToolInventorySearchGrid.dataSource.read();
                AttachmentArray = [];
                $('#imgList').empty();
                $("#FirstCheckDialog").data("kendoWindow").close();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('儲存失敗');
            }
        });
    });

    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: $("#FirstCheckGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: $("#FirstCheckGrid").data("updateurl"),
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
                    FirstCheckQty: {
                        type: "number",
                        validation: {
                            min: 1,
                            required: true
                        }
                    },
                    WareHouseName: { type: "string", editable: false },
                    Note: { type: "string" }
                }
            }
        },
        batch: true
    });
    datasource.bind("error", dataSource_error);



    function dataSource_error(e) {
        alert('發生錯誤');
    }

    FirstCheckGrid = $("#FirstCheckGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 450,
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
            },
            {
                field: "KeepMan",
                title: "保管人",
                width: 200
            },
            {
                field: "Quantity",
                title: "應盤量",
                width: 100
            },
            {
                field: "FirstCheckQty",
                title: "初盤量",
                width: 100,
                template: "<span class='#if(FirstCheckQty !== Quantity) {#diff#}#'>#=FirstCheckQty#</span>",
                editor: EditFirstCheckQty
            }
            ,
            {
                field: "Note",
                title: "備註",
                width: 100
            }
        ],
        editable: true
    }).data("kendoGrid");
}

function EditFirstCheckQty(Container, options) {
    $('<input name="' + options.field + '" type="number" title="numeric"  min="0"  step="1" style="width: 100 %;" />').appendTo(Container).kendoNumericTextBox();
}