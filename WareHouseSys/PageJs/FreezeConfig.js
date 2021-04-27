function onGridDataBound() {
   
}

function dataSource_error() {

}

$(document).ready(function (e) {
    var datasource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "../BasicInfoMaintain/FreezeGrid",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            update: {
                url: "../api/Material/updateMaterialInfo",
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
                id: "MaterialNo",
                fields: {
                    MaterialNo: { type: "string", editable: false },
                    MaterialName: { type: "string", editable: false },
                    Spec: { type: "string", editable: false },
                    Unit: { editable: false }
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" && e.field === "WareHouseName") {
                return;
            }
            
        },
        serverPaging: true,
        pageSize: 50,
        batch: false,
        serverSorting: true,
        serverFiltering: true,
        pageable: true
    });
    datasource.bind("error", dataSource_error);

    doBackGrid = $("#FreezeGrid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        filterable: {
            extra: false,
            operators: {
                string: {
                    eq: "等於",
                    contains: "包含"
                }
            }
        },
        height: 650,
        sortable: true,
        //pageable: {
        //    refresh: true,
        //    pageSizes: true,
        //    buttonCount: 5
        //},
        pageable: true,
        pageable: {
            pageSize: 50,
            pageSizes: true,
            pageSizes: [50, 500, 1000],
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
            { command: ["edit"], title: "&nbsp;", width: 200, locked: true, lockable: true },
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
                width: 500
            },{
                field: "Unit",
                title: "單位",
                width: 100
            }, {
                field: "Freeze",
                title: "是否凍結",
                width: 200,
                editor: EditCheck,
                template: '#= Freeze ? "是" : "否" #'
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
});

function EditCheck(Container, options) {
    $("<input type='checkbox' name='" + options.field + "' class='k-checkbox' checked='" + options.model[options.field] + "'>").appendTo(Container);
}
