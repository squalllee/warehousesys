function AddPickingDialogInit(obj) {
    
    $("#PickingToolAddForm").find('[name="StorageId"]').kendoMultiColumnComboBox({
        placeholder: "選擇批號",
        dataTextField: "StorageId",
        dataValueField: "StorageId",
        filter: "contains",
        filterFields: ["StorageId"],
        height: 400,
        autoBind: false,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "POST",
                    url: "../api/Material/getMaterialInfo/" + obj.MaterialNo + "/" + obj.WGroupId
                }
            }
        },
        columns: [
            { field: "WarehouseId", title: "庫別", width: 150 },
            { field: "WareHouseName", title: "庫別名稱", width: 150 },
            { field: "StorageId", title: "儲位", width: 150 },
            { field: "Lot", title: "批號", width: 150 },
            { field: "Qty", title: "庫存", width: 100 }
        ],
        change: onSelectStorage
    });


    $("#PickingToolAddForm").find('[name="KeepMan"]').kendoMultiColumnComboBox({
        placeholder: "選擇保管人",
        dataTextField: "TMNAME",
        dataValueField: "KEYNO",
        filter: "contains",
        filterFields: ["TMNAME", "KEYNO"],
        height: 400,
        autoBind: false,
        minLength: 1,
        dataSource: {
            transport: {
                read: {
                    type: "GET",
                    url: "../api/Employee/getEmployeeUnit"
                }
            }
        },
        columns: [
            { field: "TMNAME", title: "姓名", width: 150 },
            { field: "KEYNO", title: "員工編號", width: 150 },
            { field: "UNITNO", title: "單位代碼", width: 150 },
            { field: "UNITNAME", title: "單位名稱", width: 150 }
        ],
        change:onSelectKeeper
    }).data("kendoMultiColumnComboBox");

    $("#BtnAddPickTool").click(function (e) {
        var obj = formToJSON($("#PickingToolAddForm"));
        obj.KeepUnitId = $("[name='KeepMan']").data("kendoMultiColumnComboBox").dataItem().UNITNO;
        
        $.ajax({
            url: $("#PickingToolAddForm").data("saveurl"),
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            data: JSON.stringify(obj),
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('儲存成功');
                PickingDetailUpdateGrid.dataSource.read();
                $("#AddPickingToolDialog").data("kendoWindow").close();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('儲存失敗');
            }
        });
    });
}

function onSelectStorage(e) {
    var dataItem = this.dataItem();
    
    $("#PickingToolAddForm").find("[name='WarehouseId']").val(dataItem.WarehouseId);
    $("#PickingToolAddForm").find("[name='WareHouseName']").val(dataItem.WareHouseName);
    $("#PickingToolAddForm").find("[name='Lot']").val(dataItem.Lot);
    $("#PickingToolAddForm").find("[name='Quantity']").val(dataItem.Qty);
    

}

function onSelectKeeper(e) {
    var dataItem = this.dataItem();

    $("#PickingToolAddForm").find("[name='KeepUnit']").val(dataItem.UNITNAME);
}