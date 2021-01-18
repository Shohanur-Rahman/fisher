function CheckEmailForamtValidation() {
    var isEmailValid = true;

    $(".dummy_SpanErrorMsgForSendTo").hide();
    $(".dummy_SpanErrorMsgForSubject").hide();
    $(".dummy_SpanErrorMsgForMessage").hide();


    var regExpEmail = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    var emailField = $.trim($("#EmailSendTo").val());
    var subject = $.trim($("#EmailSubject").val());
    var mailText = $.trim($("#EmailBody").val());

    if (emailField == "") {
        $(".dummy_SpanErrorMsgForSendTo").show();
         $('html, body').animate({ scrollTop: 0 }, 500);
        isEmailValid = false;

    }
    if (subject == "") {
        $(".dummy_SpanErrorMsgForSubject").show();
        $('html, body').animate({ scrollTop: 0 }, 500);
        isEmailValid = false;
    }
    if (mailText == "") {
        $(".dummy_SpanErrorMsgForMessage").show();
        $('html, body').animate({ scrollTop: 0 }, 500);
        isEmailValid = false;
    }

    if (emailField != null && emailField != "") {
        var emails = emailField.split(",");
        var valid = true;
        for (var i in emails) {
            var value = emails[i];
            if (!regExpEmail.test(value)) {
                $(".dummy_SpanErrorMsgForSendTo").show();
                $('html, body').animate({ scrollTop: 0 }, 500);
                isEmailValid = false;
            }
        }
    }


    // isEmailSendButtonEnable global variable from _userLayout
    if (isEmailValid == true && isEmailSendButtonEnable == true) {
        isEmailSendButtonEnable = false;
        isEmailValid = false;
    }
    if (isEmailValid == false && isEmailSendButtonEnable == false) {

        $(".dummy_emailValidationSubmitBtn").removeClass('btn-outline');      
        $(".dummy_emailValidationSubmitBtn").css('pointer-events', 'none');
        $(".dummy_frm_email_send").submit();
        isEmailValid = true;
    }

    return isEmailValid;

}