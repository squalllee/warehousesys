function openMaterialInfo(e) {
    var row = e.closest("tr");
    var grid = $("#grid").data("kendoGrid");
    var dataItem = grid.dataItem(row);

    MaterialInfoDialog = $("<div></div>").kendoWindow({
        title: "物料基本資料",
        actions: ["Close"],
        content: "../Material/MaterialInfo?MaterialNo=" + dataItem.MaterialNo,
        visible: false,
        modal: true,
        width: 1200,
        height: 700,
        position: {
            top: "100px",
            left: "25%"
        },
        refresh: function (e) {
            MaterialInfoInit();
        },
        close: function (e) {
          
            this.destroy();
        }
    }).data("kendoWindow").open();
}

function MaterialInfoInit() {

}