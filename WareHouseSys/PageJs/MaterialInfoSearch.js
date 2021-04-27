function onGridDataBound(e) {
    var data = this.dataSource.view();

    var actionStr = "";

    for (var i = 0; i < data.length; i++) {
        var dataItem = data[i];

        actionStr = "<a href='#' onclick='openImage(\"" + dataItem.MaterialNo + "\")'><img src='../images/image.png' style='width:80%' /></a> ";
        var tr = $("#grid").find("[data-uid='" + dataItem.uid + "']");
        $(actionStr).appendTo(tr.find("[name='attatchment']"));
        tr.find("[name='attatchment']").append();
    }
}

function openImage(MaterialNo) {

    $("<div></div>").kendoWindow({
        title: "物料圖片",
        actions: ["Close"],
        content: "../Material/MaterialImage?MaterialNo=" + MaterialNo,
        visible: false,
        modal: true,
        width: 800,
        position: {
            top: "20px",
            left: "15%"
        },
        refresh: function (e) {
            MaterialImageInit(MaterialNo);
        },
        close: function (e) {
            this.destroy();
        }
    }).data("kendoWindow").open();
}


function MaterialImageInit(MaterialNo){
    var template = kendo.template($("#template").html());

    var initialFiles = [];
    var files = [];
    if ($("#fileNames").val() !== "") {
        var fileNmaes = $("#fileNames").val().split(',');
        $.each(fileNmaes, function (index, val) {
            initialFiles.push({ name: val, MaterialNo: MaterialNo });
            files.push({ name: val, size: 0, extension: val.substr(val.lastIndexOf('.') + 1)});
        });
    }
    
    //var initialFiles = [{ name: "1.jpg", MaterialNo: MaterialNo }, { name: "2.jpg", MaterialNo: MaterialNo }, { name: "3.jpg", MaterialNo: MaterialNo }, { name: "4.jpg", MaterialNo: MaterialNo }, { name: "5.jpg", MaterialNo: MaterialNo }, { name: "6.jpg", MaterialNo: MaterialNo  }];

    $("#products").html(kendo.render(template, initialFiles));

    $("#files").kendoUpload({
        async: {
            saveUrl: "../Material/MaterialImagUpload?MaterialNo=" + MaterialNo,
            autoUpload: true
        },
        validation: {
            allowedExtensions: [".jpg", ".jpeg", ".png", ".bmp", ".gif"]
        },
        files: files,
        success: onSuccess,
        showFileList: false,
        dropZone: ".dropZoneElement"
    });

    var rawFile;
    function onSuccess(e) {
        if (e.operation == "upload") {
            for (var i = 0; i < e.files.length; i++) {
                var file = e.files[i].rawFile;

                if (file) {
                    rawFile = file;
                    var reader = new FileReader();

                    reader.onloadend = function () {
                        $("<div class='product img-wrap'><img class='close' src='../images/uploader_closebox.png' style='width: 20%; height: 20%' onclick='removeImg(this)' data-MaterialNo='" + MaterialNo + "' data-filename='" + rawFile.name+"'><a href='#' onclick='largeImg(this)'><img src=" + this.result + " /></a></div>").appendTo($("#products"));
                    };

                    reader.readAsDataURL(file);
                }
            }
        }
    }
}

function largeImg(imageSrc) {
    event.preventDefault();
    $("#largeImg").attr("src", $(imageSrc).children()[0].src);
}

function removeImg(e) {

    var MaterialNo = $(e).data("materialno");
    var FileName = $(e).data("filename");

    $.ajax({
        url: "../Material/MaterialImagDelete?MaterialNo=" + MaterialNo + "&FileName=" + FileName,
        dataType: 'text',
        type: 'get',
        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            $(e).parent().remove();
            var imageParent = $("#largeImg").parent();

            $("#largeImg").remove();

            $("<img id='largeImg' style='width:90%; height:90%'></img>").appendTo(imageParent);

        },
        error: function (jqXhr, textStatus, errorThrown) {
            alert('刪除失敗');
        }
    });
}
   