var notification = $("<span id='notification' style='display: none;'></span>").kendoNotification({
    position: {
        pinned: true,
        top: 30,
        right: 30
    },
    autoHideAfter: 0,
    stacking: "down",
    templates: [{
        type: "error",
        template: "<div class='wrong-pass'><img src = '../images/error-icon.png' /><h3>#= title #</h3><p>#= message #</p></div >"
    }]

}).data("kendoNotification");

function Warning(Title, Msg) {
    notification.show({
        title: Title,
        message: Msg
    }, "error");
}