function CreateReceive(no, lot, obj) {
    $.ajax({
        type: 'GET',
        url: "../api/Purchase/CreateReceive/" + no + "/" + lot,
        beforeSend: ajaxLoading,
        success: function (result) {
            ajaxLoadEnd();
            $(obj).hide();
            alert('產生收貨單成功!');
        },
        error: function (err) {
            ajaxLoadEnd();
        }
    });
}

function ajaxLoading() {
    $("#tt").mLoading({
        text:"收貨單產生中"
    });
}

function ajaxLoadEnd() {
    $("#tt").mLoading('hide');
}

function formatDate1(val, row) {
    return formattedDate(val);
}

function formattedDate(date) {
    if (date == null) return '';
    var d = new Date(date || Date.now()),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year,month,day].join('/');
}