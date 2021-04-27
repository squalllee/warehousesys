function onGridDataBound(e) {

}

function error_handler(e) {
    alert(e.xhr.responseText);
}

function EditSpan(container, options) {
    $("<span name='" + options.field + "'>" + (options.model[options.field] === undefined ? '' : options.model[options.field]) + "<span>").appendTo(container);
}