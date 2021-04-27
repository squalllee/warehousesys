function AddPickingDialogInit(obj) {
    $("#PickingMaterialAddForm").find('[name="StorageId"]').kendoMultiColumnComboBox({
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
        change: function (e) {
            var item = this.dataItem();

            $("#PickingMaterialAddForm").find('[name="Inventory"]').val(item.Qty);

            $("#PickingMaterialAddForm").find('[name="Lot"]').val(item.Lot);

            $("#PickingMaterialAddForm").find('[name="WarehouseId"]').val(item.WarehouseId);

            $("#PickingMaterialAddForm").find('[name="WareHouseName"]').val(item.WareHouseName);

        }
    });

    $("#BtnAddPickMaterial").click(function (e) {
        var obj = formToJSON($("#PickingMaterialAddForm"));
        $.ajax({
            url: $("#PickingMaterialAddForm").data("saveurl"),
            dataType: 'text',
            type: 'post',
            contentType: 'application/json',
            data: JSON.stringify(obj),
            processData: false,
            success: function (data, textStatus, jQxhr) {
                alert('儲成成功');
                PickingDetailUpdateGrid.dataSource.read();
                $("#AddPickingMaterialDialog").data("kendoWindow").close();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert('儲成失敗');
            }
        });
    });
}