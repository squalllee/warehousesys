function onGridDataBound(e) {
    var data = this.dataSource.view();

    var actionStr = "";

    for (var i = 0; i < data.length; i++) {
        var dataItem = data[i];

        if (dataItem.AttUrl !== null) {
            //actionStr = "<a href='/WareHouseSys/Picking/getAttatchment?OrderNo=" + dataItem.OrderNo + "&FileName=" + dataItem.AttUrl+"' target='_blank'><img src='../images/pdf.png' /></a>";
            //actionStr = "<a href='../Picking/getAttatchment?OrderNo=" + dataItem.OrderNo + "&FileName=" + dataItem.AttUrl + "' target='_blank'><img src='../images/pdf.png' /></a>";
            //actionStr = "<a href='/WareHouseSys/Picking/getAttatchment?OrderNo=" + dataItem.OrderNo + "&FileName=" + dataItem.AttUrl+"' target='_blank'><img src='../images/pdf.png' /></a>";
            actionStr = "<a href='" + $("#baseUrl").val() + "?OrderNo=" + dataItem.OrderNo + "&FileName=" + dataItem.AttUrl + "' target = '_blank' > <img src='../images/pdf.png' /></a > ";
            var tr = $("#grid").find("[data-uid='" + dataItem.uid + "']");
            $(actionStr).appendTo(tr.find("[name='attatchment']"));
            tr.find("[name='attatchment']").append();
        }

        // use the table row (tr) and data item (dataItem)
    }
}