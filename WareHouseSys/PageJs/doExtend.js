var doBackGrid;
function doExtendInit() {
    $('.dateTime').datepicker({
        dateFormat: 'yy/mm/dd'
    });

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    $("#BtnSaveBack").click(function (e) {
        if (AttachmentArray.length === 0) {
            alert('必需上傳已核可的展延單!');
            return;
        }

        var validator = $("#doExtendForm").kendoValidator().data("kendoValidator");

        if (validator.validate()) {
            var obj = {};
            obj.extendHeaderViewModel = formToJSON($("#doExtendForm"));
            obj.attachment = AttachmentArray;

            var url = $("#doExtendForm").data("saveurl");

            $.ajax({
                url: url,
                dataType: 'text',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    alert('儲存成功');
                    AttachmentArray = [];
                    $('#imgList').empty();
                    ExtendMaterialGrid.dataSource.read();
                    $("#doExtendDialog").data("kendoWindow").close();
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
                url: $("#doBackGrid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                return JSON.stringify(options);
            }
        },
        schema: {
            data: "data"
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });
    datasource.bind("error", dataSource_error);


    doBackGrid = $("#doBackGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 500,
        width: 1050,
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
                width: 100
            }, {
                field: "ExtendQty",
                title: "展延數量",
                width: 100
            }
        ]
    }).data("kendoGrid");
}