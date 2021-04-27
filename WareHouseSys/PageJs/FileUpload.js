//To save an array of attachments 
var AttachmentArray = [];


//to make sure the error message for number of files will be shown only one time.
var filesCounterAlertStatus = false;

//un ordered list to keep attachments thumbnails
var ul = document.createElement('ul');

function fileUploadInit() {
    init();

    $('div').on('click', '.img-wrap .close', function () {
        var id = $(this).closest('.img-wrap').find('img.thumb').data('id');

        //to remove the deleted item from array
        var elementPos = AttachmentArray.map(function (x) { return x.FileName; }).indexOf(id);
        if (elementPos !== -1) {
            AttachmentArray.splice(elementPos, 1);
        }
        $(this).closest(".img-wrap").closest("li").remove();

        //to remove li tag
        var lis = document.querySelectorAll('#imgList li');
        for (var i = 0; li = lis[i]; i++) {
            if (li.innerHTML == "") {
                li.parentNode.removeChild(li);
            }
        }

    });
}


function init() {
    ul.className = ("thumb-Images");
    ul.id = "imgList";
    //add javascript handlers for the file upload event
    //document.querySelector('#files').addEventListener('change', handleFileSelect, false);

    $("#files").on("change", function (e) {
        //to make sure the user select file/files
        if (!e.target.files) return;
        
        //To obtaine a File reference
        var files = e.target.files;

        // Loop through the FileList and then to render image files as thumbnails.       
        for (var i = 0, f; f = files[i]; i++) {

            //instantiate a FileReader object to read its contents into memory
            var fileReader = new FileReader();

            // Closure to capture the file information and apply validation.
            fileReader.onload = (function (readerEvt) {
                return function (e) {

                    //Apply the validation rules for attachments upload
                    ApplyFileValidationRules(readerEvt);

                    //Render attachments thumbnails.
                    RenderThumbnail(e, readerEvt);

                    //Fill the array of attachment
                    FillAttachmentArray(e, readerEvt);
                };
            })(f);

            fileReader.readAsDataURL(files[i]);
        }

        document.querySelector('#files').value = '';
    });
    
}

//the handler for file upload event
function handleFileSelect(e) {
    //to make sure the user select file/files
    if (!e.target.files) return;

    //To obtaine a File reference
    var files = e.target.files;

    // Loop through the FileList and then to render image files as thumbnails.       
    for (var i = 0, f; f = files[i]; i++) {

        //instantiate a FileReader object to read its contents into memory
        var fileReader = new FileReader();

        // Closure to capture the file information and apply validation.
        fileReader.onload = (function (readerEvt) {
            return function (e) {

                //Apply the validation rules for attachments upload
                ApplyFileValidationRules(readerEvt);

                //Render attachments thumbnails.
                RenderThumbnail(e, readerEvt);

                //Fill the array of attachment
                FillAttachmentArray(e, readerEvt);
            };
        })(f);

        // Read in the image file as a data URL.
        // readAsDataURL: The result property will contain the file/blob's data encoded as a data URL.
        // More info about Data URI scheme https://en.wikipedia.org/wiki/Data_URI_scheme
        fileReader.readAsDataURL(files[i]);
    }
    //document.getElementById('files').addEventListener('change', handleFileSelect, false);
}


//Apply the validation rules for attachments upload
function ApplyFileValidationRules(readerEvt) {
    //To check file type according to upload conditions
    if (CheckFileType(readerEvt.type) == false) {
        alert("The file (" + readerEvt.name + ") does not match the upload conditions, You can only upload jpg/png/gif files");
        e.preventDefault();
        return;
    }

    //To check file Size according to upload conditions
    //if (CheckFileSize(readerEvt.size) == false) {
    //    alert("The file (" + readerEvt.name + ") 上傳附件不得大於5M");
    //    e.preventDefault();
    //    return;
    //}

    //To check files count according to upload conditions
    if (CheckFilesCount(AttachmentArray) == false) {
        if (!filesCounterAlertStatus) {
            filesCounterAlertStatus = true;
            alert("You have added more than 10 files. According to upload conditions you can upload 10 files maximum");
        }
        e.preventDefault();
        return;
    }
}

//To check file type according to upload conditions
function CheckFileType(fileType) {
    return true;
    //if (fileType == "image/jpeg") {
    //    return true;
    //}
    //else if (fileType == "image/png") {
    //    return true;
    //}
    //else if (fileType == "image/gif") {
    //    return true;
    //}
    //else {
    //    return false;
    //}
}

//To check file Size according to upload conditions
function CheckFileSize(fileSize) {
    if (fileSize < 6000000) {
        return true;
    }
    else {
        return false;
    }
}

//To check files count according to upload conditions
function CheckFilesCount(AttachmentArray) {
    //Since AttachmentArray.length return the next available index in the array, 
    //I have used the loop to get the real length
    var len = 0;
    for (var i = 0; i < AttachmentArray.length; i++) {
        if (AttachmentArray[i] !== undefined) {
            len++;
        }
    }
    //To check the length does not exceed 10 files maximum
    if (len > 9) {
        return false;
    }
    else {
        return true;
    }
}

//Render attachments thumbnails.
function RenderThumbnail(e, readerEvt) {
    var li = document.createElement('li');
    ul.appendChild(li);
    var imgStr = '<div class="img-wrap" > <img class="close" src="../images/uploader_closebox.png">';
    if (readerEvt.name.toUpperCase().indexOf('JPG') > -1 || readerEvt.name.toUpperCase().indexOf('PNG') > -1) {
        imgStr += '<img class="thumb" src="' + e.target.result + '" title="' + escape(readerEvt.name) + '" data-id="' + readerEvt.name + '"/></div>';
    }
    else if (readerEvt.name.toUpperCase().indexOf('DOC') > -1) {
        imgStr += '<img class="thumb" src="../images/doc_icon.png" title="' + escape(readerEvt.name) + '" data-id="' + readerEvt.name + '"/></div>';
    }
    else if (readerEvt.name.toUpperCase().indexOf('XLS') > -1) {
        imgStr += '<img class="thumb" src="../images/xls_icon.png" title="' + escape(readerEvt.name) + '" data-id="' + readerEvt.name + '"/></div>';
    }
    else if (readerEvt.name.toUpperCase().indexOf('PDF') > -1) {
        imgStr += '<img class="thumb" src="../images/pdf_icon.png" title="' + escape(readerEvt.name) + '" data-id="' + readerEvt.name + '"/></div>';
    }
    li.innerHTML = [imgStr].join('');

    document.getElementById('Filelist').insertBefore(ul, null);
}

//Fill the array of attachment
function FillAttachmentArray(e, readerEvt) {
    AttachmentArray.push({
        AttachmentType: 1,
        ObjectType: 1,
        FileName: readerEvt.name,
        FileDescription: "Attachment",
        NoteText: "",
        MimeType: readerEvt.type,
        Content: e.target.result.split("base64,")[1],
        FileSizeInBytes: readerEvt.size
    });
        
}


//Render attachments thumbnails.
function ServerRenderThumbnail(obj) {
    var li = document.createElement('li');
    ul.appendChild(li);
    var imgStr = '<div class="img-wrap" > <img class="close" src="../images/uploader_closebox.png">';
    if (obj.FileName.toUpperCase().indexOf('JPG') > -1 || obj.FileName.toUpperCase().indexOf('PNG') > -1) {
        imgStr += '<img class="thumb" src="data:image/jpeg;base64,'+ obj.Content + '" title="' + obj.FileName + '" data-id="' + obj.FileName + '"/></div>';
    }
    else if (obj.FileName.toUpperCase().indexOf('DOC') > -1) {
        imgStr += '<img class="thumb" src="../images/doc_icon.png" title="' + obj.FileName + '" data-id="' + obj.FileName + '"/></div>';
    }
    else if (obj.FileName.toUpperCase().indexOf('XLS') > -1) {
        imgStr += '<img class="thumb" src="../images/xls_icon.png" title="' + obj.FileName + '" data-id="' + obj.FileName + '"/></div>';
    }
    else if (obj.FileName.toUpperCase().indexOf('PDF') > -1) {
        imgStr += '<img class="thumb" src="../images/pdf_icon.png" title="' + obj.FileName + '" data-id="' + obj.FileName + '"/></div>';
    }
    li.innerHTML = [imgStr].join('');

    document.getElementById('Filelist').insertBefore(ul, null);
}

//Fill the array of attachment
function ServerFillAttachmentArray(obj) {
    AttachmentArray.push({
        AttachmentType: 1,
        ObjectType: 1,
        FileName: obj.FileName,
        FileDescription: "Attachment",
        NoteText: "",
        MimeType: "",
        Content: obj.Content,
        FileSizeInBytes: 0
    });

}