function transToPurDialogInit() {
    $("#BtnSavePur").click(function (e) {

        var purchaseHeader = formToJSON($("#PurchaseAddForm"));
        var purchaseBodyList = $("#grid").data("kendoGrid").dataSource.view();

        var ret = true;
        $.each(purchaseBodyList, function (i) {
            if (purchaseBodyList[i].DeliveryPlace === "" || purchaseBodyList[i].DeliveryLot === "" || purchaseBodyList[i].Price === "") {
                Warning("警告!", "單身資料不完整，請再確認!");
                ret = false;
                return false;
            }
        });

        if (!ret) return;

        purchaseHeader.PurchaseUnit = purchaseHeader.PurchaseUnitId;
        purchaseHeader.PurchaseMan = purchaseHeader.PurchaseManId;
        purchaseHeader.BudgetSource = purchaseHeader.BudgetSourceString.split("/");

        purchaseHeader.OpenContract = purchaseHeader.OpenContract === "是" ? true : false;


        var obj = {};
        obj.purchaseHeader = purchaseHeader;

        obj.purchaseBodies = purchaseBodyList;

        var url = "../api/Requirement/TransToPur";

        $.ajax({
            url: url,
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            data: JSON.stringify(obj),
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('儲存成功');
                transToPurDialog.close();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert(jQuery.parseJSON(jqXhr.responseText).Message);
            }
        });
    });

    function dataSource_error(e) {
        alert('發生錯誤');
    }

    var datasource = new kendo.data.DataSource({
        offlineStorage: "offlineStorage",
        transport: {
            read: {
                url: $("#grid").data("url"),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST"
            },
            parameterMap: function (options, operation) {
                return JSON.stringify(options);
            }
        },
        requestEnd: function (e) {
            datasource.online(false);
        },
        schema: {
            data: "data",
            model: {
                id: "SerialNo",
                fields: {
                    MaterialNo: { type: "string",editable:false },
                    MaterialName: { type: "string", editable: false},
                    Spec: { type: "string", editable: false },
                    Unit: { type: "string", editable: false },
                    PerformancePeriod: {type:"date"},
                    Quantity: {
                        type: "number",
                        validation: {
                            min: 1,
                            required: true
                        }
                    }
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" && e.field === "RequireUnitName") {
                var model = e.items[0];
                var selectedData = $("[name='RequireUnitName']").data("kendoMultiColumnComboBox").dataItem();

                model.RequireUnitName = selectedData.UNITNAME;
                model.RequireUnit = selectedData.UNITNO;


                return;
            }
            if (e.action === "add") {
                e.items[0].PerformancePeriod = formattedDate(e.items[0].PerformancePeriod);
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

    grid = $("#grid").kendoGrid({
        dataSource: datasource,
        resizable: true,
        height: 300,
        dataBound: function (e) {
            var totalPriceWithoutTax = 0;
            var data = grid.dataSource.view();
            data.forEach(function (obj) {
                if (obj.Price !== "") {
                    totalPriceWithoutTax += parseInt(obj.Price * obj.Quantity);
                }
            });

            $("[name='ContractPriceWithoutVAT']").val(totalPriceWithoutTax);
            $("[name='ContractPriceIncludeVAT']").val(Math.round(totalPriceWithoutTax * 1.05));

            $("#ContractPriceWithoutVATDisplay").text("總契約金額(未稅):" + totalPriceWithoutTax);
            $("#ContractPriceIncludeVATDisplay").text("總契約金額(含稅):" + Math.round(totalPriceWithoutTax * 1.05));

        },
        columns: [
            { command: ["edit", "destroy", { text: "複制", click: Copy }], title: "&nbsp;", width: 250, locked: true, lockable: true },
            {
                field: "MaterialNo",
                title: "品號",
                width: 200

            },
            {
                field: "SerialNo",
                hidden: true
            },
            {
                field: "MaterialName",
                title: "品名",
                width: 200
            },
            {
                field: "Spec",
                title: "規格",
                width: 200
            }, {
                field: "Unit",
                title: "單位",
                width: 80
            },
            {
                field: "Quantity",
                title: "契約數量",
                width: 100
            },
            {
                field: "Price",
                title: "單價未稅",
                width: 100
            },
            {
                field: "DeliveryLot",
                title: "交貨批次",
                width: 100
            },
            {
                field: "DeliveryPlace",
                title: "交貨地點",
                width: 200
            },
            {
                field: "PerformancePeriod",
                title: "履約期限",
                template: '#= PerformancePeriod !== null ? kendo.toString(PerformancePeriod, "yyyy/MM/dd") : ""#',

                width: 200
            },
            {
                field: "RequireUnitName",
                title: "需求單位",
                width: 200,
                editor: UnitCombobox
            }
        ],
        editable: "inline"
    }).data("kendoGrid");
}

function UnitCombobox(Container, options) {
    $("<input name='" + options.field + "' type='text' required />").appendTo(Container).kendoMultiColumnComboBox({
        placeholder: "選擇需求單位",
        dataTextField: "UNITNAME",
        dataValueField: "UNITNO",
        filter: "contains",
        filterFields: ["UNITNAME", "UNITNO"],
        height: 400,
        autoBind: false,
        text: options.model["RequireUnitName"],
        value: options.model["RequireUnit"],
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/Unit/getUnit"
                }
            }
        },
        columns: [
            { field: "UNITNAME", title: "單位名稱", width: 150 },
            { field: "UNITNO", title: "單位代碼", width: 150 }
        ]
    });

}

function Copy(e) {

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    grid.dataSource.add(JSON.parse(kendo.stringify(dataItem)));
    
}