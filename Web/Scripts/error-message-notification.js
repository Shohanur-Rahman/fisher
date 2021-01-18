function ShowErrorMessageFromGridOrAjaxEvent(e) {
    //Show Error Message From jQuery Events

    if (typeof e.errorThrown !== "undefined" && e.errorThrown != "") {
        ShowRamsNotificationMessage(e.errorThrown);
    }

    if (typeof e.statusText !== "undefined" && e.statusText != "") {
        ShowRamsNotificationMessage(e.statusText);
    }

    if (typeof e.message !== "undefined" && e.message != "") {
        ShowRamsNotificationMessage(e.message);
    }

    if (typeof e !== "undefined" && typeof e.message == "undefined" && typeof e.errorThrown == "undefined" && typeof e.statusText == "undefined" && e != "") {
        ShowRamsNotificationMessage(e);
    }

}



function ShowRamsNotificationMessage(divErrorMessage) {
    $("#spnErrorMessageShow").html(divErrorMessage);
    $("#MainContent_divMessage").show().fadeOut(5000);
}