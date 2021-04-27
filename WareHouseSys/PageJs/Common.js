function formToJSON(element) {
    var obj = {};

    $(element).find('input, select,textarea').each(function () {
        if (this.name === "") {
            return;
        }
        if (this.attributes["type"] != undefined && this.tagName === "INPUT" && this.attributes["type"].value === "radio") {
            if (this.checked) {
                obj[this.name] = this.value;
            }
        }
        else {
            obj[this.name] = $(this).val();
        }
        
    });
    return obj;
}


function formattedDateTime(date) {

    var longDateFormat = 'yyyy/MM/dd HH:mm';
    return jQuery.format.date(date, longDateFormat);
}


function formattedDate(date) {

    var longDateFormat = 'yyyy/MM/dd';
    return jQuery.format.date(date, longDateFormat);

}

$.widget("custom.combobox", {
    _create: function () {
        this.wrapper = $("<span>")
            .addClass("custom-combobox")
            .insertBefore(this.element);
        //.insertAfter( this.element );

        this.element.hide();
        isShow = this.element.data("isshow");
        if (!isShow) {
            this.wrapper.hide();
        }
        //this.element.css('visibility', 'hidden');
        comboboxId = this.element[0].id;
        this._createAutocomplete();
        this._createShowAllButton();
    },

    _createAutocomplete: function () {
        var selected = this.element.children(":selected"),
            value = selected.val() ? selected.text() : "";

        this.input = $("<input>")
            .appendTo(this.wrapper)
            .val(value)
            .attr("title", "")
            .attr("name", "input" + comboboxId)
            .attr("id", "input" + comboboxId)
            .attr("required", this.element.attr("required"))
            .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
            .autocomplete({
                delay: 0,
                minLength: 0,
                source: $.proxy(this, "_source"),
                select: function (event, ui) {
                    $(ui.item.option.parentElement).trigger("change");
                }
            })
            .tooltip({
                classes: {
                    "ui-tooltip": "ui-state-highlight"
                }
            });

        this._on(this.input, {
            autocompleteselect: function (event, ui) {
                ui.item.option.selected = true;
                this._trigger("select", event, {
                    item: ui.item.option
                });
            },

            autocompletechange: "_removeIfInvalid"
        });
    },

    _createShowAllButton: function () {
        var input = this.input,
            wasOpen = false;

        $("<a>")
            .attr("tabIndex", -1)
            .attr("title", "顯示所有選項")
            .tooltip()
            .appendTo(this.wrapper)
            .button({
                icons: {
                    primary: "ui-icon-triangle-1-s"
                },
                text: false
            })
            .removeClass("ui-corner-all")
            .addClass("custom-combobox-toggle ui-corner-right")
            .on("mousedown", function () {
                wasOpen = input.autocomplete("widget").is(":visible");
            })
            .on("click", function () {
                input.trigger("focus");

                // Close if already visible
                if (wasOpen) {
                    return;
                }

                // Pass empty string as value to search for, displaying all results
                input.autocomplete("search", "");
            });

        this.wrapper.css({
            'position': 'relative',
            'display': 'inline-block'
        });


        $('.custom-combobox-toggle').css({
            'position': 'absolute',
            'top': '0',
            'bottom': '0',
            'margin-left': '-1px',
            'padding': '0'
        });

        $('.custom-combobox-input').css({
            'margin': '0',
            'padding': '5px 10px'
        });
    },

    _source: function (request, response) {
        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
        response(this.element.children("option").map(function () {
            var text = $(this).text();
            if (this.value && (!request.term || matcher.test(text)))
                return {
                    label: text,
                    value: text,
                    option: this
                };
        }));
    },

    _removeIfInvalid: function (event, ui) {

        // Selected an item, nothing to do
        if (ui.item) {
            return;
        }

        // Search for a match (case-insensitive)
        var value = this.input.val(),
            valueLowerCase = value.toLowerCase(),
            valid = false;
        this.element.children("option").each(function () {
            if ($(this).text().toLowerCase() === valueLowerCase) {
                this.selected = valid = true;
                return false;
            }
        });

        // Found a match, nothing to do
        if (valid) {
            return;
        }

        // Remove invalid value
        this.input
            .val("")
            .attr("title", value + " didn't match any item")
            .tooltip("open");
        this.element.val("");
        this._delay(function () {
            this.input.tooltip("close").attr("title", "");
        }, 2500);
        this.input.autocomplete("instance").term = "";
    },

    _destroy: function () {
        this.wrapper.remove();
        this.element.show();
    }
});

function ajaxLoading(msg, obj) {
    $(obj).mLoading({
        text: msg
    });
}

function ajaxLoadEnd(obj) {
    $(obj).mLoading('hide');
}

function DialogInit(e,title,buttons,width,height) {
    $(e).dialog({
        autoOpen: false,
        title: title,
        width: width,
        height: height,
        modal: true,
        buttons: buttons
    });
}

function DataTableInit(e, dataFunc, columns, columnDefs) {
    return $(e).DataTable({
        ajax: {
            method: "post",
            url: $(e).data("url"),
            data: dataFunc
        },
        searching: false,
        "processing": true,
        "serverSide": true,
        bLengthChange: false,
        "columns": columns,
        columnDefs: columnDefs
    });
}

function doPost(url, data, successFunc, failFunc) {
    $.ajax({
        url: url,
        dataType: 'text',
        type: 'post',
        contentType: 'application/json',
        data: data,
        processData: false,
        success: successFunc,
        error: failFunc
    });
}
