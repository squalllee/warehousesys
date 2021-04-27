function ReqMaterialReviewInit() {

    $("#BtnReviewSave").click(function (e) {
        var obj = {};
        obj.OrderNo = $("[name='OrderNo']").val();
        obj.attachments = AttachmentArray;

        $.ajax({
            url: "../api/ReqMaterialInfo/closeReqMaterial",
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            data: JSON.stringify(obj),
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('儲存成功');
                ReqMaterialReviewDialog.close();
                $("#grid").data("kendoGrid").dataSource.read();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('儲存失敗');
            }
        });
    });

    $("#imgFileOpen").click(function () {
        $("#files").trigger("click");
    });

    fileUploadInit();

    var ReqMaterialReviewDatasource = new kendo.data.DataSource({
        offlineStorage: "products-offline",
        transport: {
            read: {
                url: "../ReqMaterialInfo/ReqMaterialBodies?OrderNo=" + $("[name='OrderNo']").val(),
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
                id: "OrderNo"
            }
        },
        serverPaging: true,
        pageSize: 10,
        batch: false,
        serverSorting: true,
        serverFiltering: false,
        pageable: true
    });

    ReqMaterialReviewGrid = $("#ReqMaterialReviewGrid").kendoGrid({
        dataSource: ReqMaterialReviewDatasource,
        resizable: true,
        height: 600,
        width: 1850,
        sortable: true,
        pageable: true,
        pageable: {
            pageSize: 10,
            pageSizes: true,
            pageSizes: [10, 100, 1000],             
            buttonCount: 5,
            refresh: true,
            messages: {
               last: "最末頁",
               first: "第一頁",
               next: "下一頁",
               previous: "上一頁",
               morePages: "更多頁",
               itemsPerPage: "每頁筆數",
               display: "第 {0} - {1} 筆 共 {2} 筆記錄",
               refresh: "重新整理",
               empty: "沒有符合記錄"
            }
        },
        columns: [
            {
                field: "MaterialName",
                title: "品名",
                width: 200
            },
            {
                field: "ReplaceNo",
                title: "替代件料號",
                width: 200
            },
            {
                field: "Spec",
                title: "規格",
                width: 300
            },
            {
                field: "UnitName",
                title: "計量單位",
                width: 200
            }, {
                field: "LineAbb",
                title: "線別/設備別",
                template: "#= LineAbb ==='0' ? '綠線' : '否' #",
                width: 130
            }, {
                field: "SystemName",
                title: "系統別",
                width: 150
            }
            , {
                field: "SubSystemName",
                title: "子系統別",
                width: 200
            }
            , {
                field: "EqQuantity",
                title: "設備零件數量",
                width: 150
            }, {
                field: "SafetyStock",
                title: "安全量",
                width: 150
            }, {
                field: "EstUnitPrice",
                title: "預估單價",
                width: 150
            }, {
                field: "Length",
                title: "物料尺寸-長(cm)",
                width: 150
            }, {
                field: "Witdh",
                title: "物料尺寸-寬(cm)",
                width: 150
            }, {
                field: "Height",
                title: "物料尺寸-高(cm)",
                width: 150
            }, {
                field: "weight",
                title: "物料重量-公斤(kg)",
                width: 150
            }, {
                field: "VendorId",
                title: "原廠代碼/開發",
                width: 150
            }, {
                field: "FixClassName",
                title: "檢修分類",
                width: 150
            }, {
                field: "AffectClassName",
                title: "影響類別",
                width: 150
            }, {
                field: "ROP",
                title: "請購點",
                width: 150
            }, {
                field: "EstPurPeriod",
                title: "預估購備期(天)",
                width: 150
            }, {
                field: "EstAnnConsumption",
                title: "預估年耗用量",
                width: 150
            }, {
                field: "IsFix",
                title: "可修件",
                template: "#= IsFix ? '是' : '否' #",
                width: 150
            }, {
                field: "IsDangerous",
                title: "危害物",
                template: "#= IsDangerous ? '是' : '否' #",
                width: 150
            }, {
                field: "IsLimitTime",
                title: "時限品",
                template: "#= IsLimitTime ? '是' : '否' #",
                width: 150
            }, {
                field: "SpecifyBrand",
                title: "指定廠牌",
                template: "#= SpecifyBrand ? '是' : '否' #",
                width: 150
            },{
                field: "Expiration",
                title: "保存期限(天)",
                width: 150
            }
        ]
    }).data("kendoGrid");
}