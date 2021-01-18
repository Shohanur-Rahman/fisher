var FormWizard = function () {

    return {
        //main function to initiate the module
        init: function () {
            if (!jQuery().bootstrapWizard) {
                return;
            }

            function format(state) {
                if (!state.id) return state.text; // optgroup
                return "<img class='flag' src='../../assets/global/img/flags/" + state.id.toLowerCase() + ".png'/>&nbsp;&nbsp;" + state.text;
            }

            $("#country_list").select2({
                placeholder: "Select",
                allowClear: true,
                formatResult: format,
                width: 'auto',
                formatSelection: format,
                escapeMarkup: function (m) {
                    return m;
                }
            });

            var form = $('#frmCOSHHInfo');
            var error = $('.alert-danger', form);
            var success = $('.alert-success', form);

            form.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block help-block-error', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                messages: { // custom messages for radio buttons and checkboxes
                },

                errorPlacement: function (error, element) { // render error placement for each input type
                    error.insertAfter(element); // for other inputs, just perform default behavior
                },

                invalidHandler: function (event, validator) { //display error alert on form submit   
                    success.hide();
                    error.show();
                    App.scrollTo(error, -200);
                },

                highlight: function (element) { // hightlight error inputs
                    $(element)
                        .closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
                },

                unhighlight: function (element) { // revert the change done by hightlight
                    $(element)
                        .closest('.form-group').removeClass('has-error'); // set error class to the control group
                },

                success: function (label) {
                    label
                        .addClass('valid') // mark the current input as valid and display OK icon
                        .closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
                },

                submitHandler: function (form) {
                    success.show();
                    error.hide();
                    form[0].submit();
                    //add here some ajax code to submit your form or just call form.submit() if you want to submit the form without ajax
                }

            });

            var displayConfirm = function () {
                $('#tab4 .form-control-static', form).each(function () {
                    var input = $('[name="' + $(this).attr("data-display") + '"]', form);
                    if (input.is(":radio")) {
                        input = $('[name="' + $(this).attr("data-display") + '"]:checked', form);
                    }
                    if (input.is(":text") || input.is("textarea")) {
                        $(this).html(input.val());
                    } else if (input.is("select")) {
                        $(this).html(input.find('option:selected').text());
                    } else if (input.is(":radio") && input.is(":checked")) {
                        $(this).html(input.attr("data-title"));
                    } else if ($(this).attr("data-display") == 'payment[]') {
                        var payment = [];
                        $('[name="payment[]"]:checked', form).each(function () {
                            payment.push($(this).attr('data-title'));
                        });
                        $(this).html(payment.join("<br>"));
                    }
                });
            }

            var handleTitle = function (tab, navigation, index) {


                var total = navigation.find('li').length;
                var current = index + 1;

                 // set wizard title
                var coshhID = $("#COSHHId").val();
                if (coshhID > 0) {
                    $('.step-title', $('#form_wizard_1')).text('Step ' + (index + 1) + ' of ' + total);
                }
                if (coshhID <= 0 || coshhID == null || coshhID == "")
                {
                    var substanceName = " - " + $("#txtCOSHHName").val();

                    $('.step-title', $('#form_wizard_1')).html('Step ' + (index + 1) + ' of ' + total + "<span class='caption- subject font-blue bold uppercase dummy_substance_name'>" + substanceName + "</span>");
                }               

                // set done steps          
                jQuery('li', $('#form_wizard_1')).removeClass("done");
                var li_list = navigation.find('li');
                for (var i = 0; i < index; i++) {
                    jQuery(li_list[i]).addClass("done");
                }

                if (current == 1) {
                    $('#form_wizard_1').find('.button-previous').hide();
                } else {
                    $('#form_wizard_1').find('.button-previous').show();
                }

                if (current >= total) {
                    $('#form_wizard_1').find('.button-next').hide();
                    $('#form_wizard_1').find('.button-submit').show();

                    displayConfirm();
                } else {
                    $('#form_wizard_1').find('.button-next').show();
                    $('#form_wizard_1').find('.button-submit').hide();

                    //$(".dummy_substance_name").val(substanceName);
                }
                App.scrollTo($('.page-title'));
            }

            // default form wizard
            $('#form_wizard_1').bootstrapWizard({
                'nextSelector': '.button-next',
                'previousSelector': '.button-previous',
                onTabClick: function (tab, navigation, index, clickedIndex) {
                    return false;

                    success.hide();
                    error.hide();
                    if (form.valid() == false) {
                        return false;
                    }

                    handleTitle(tab, navigation, clickedIndex);
                },
                onNext: function (tab, navigation, index) {
                    success.hide();
                    error.hide();

                    if (form.valid() == false) {
                        return false;
                    }

                    //This is the custom validation code added by us, to handle custom error
                    //set by us. Here globalIsValid is global variable. declared in _UserLayout.chtml
                    //head section
                    if (typeof globalIsValid !== 'undefined' && globalIsValid == false) {
                        error.show();
                        App.scrollTo($('.page-title'));
                        return false;
                    }

                    //This is the custom validation code added by us, to handle custom error
                    //set by us. Here globalImageIsValid is global variable. declared in _UserLayout.chtml
                    if (typeof globalImageIsValid !== 'undefined' && globalImageIsValid == false) {
                        return false;
                    }

                    handleTitle(tab, navigation, index);
                },
                onPrevious: function (tab, navigation, index) {
                    success.hide();
                    error.hide();

                    handleTitle(tab, navigation, index);
                },
                onTabShow: function (tab, navigation, index) {

                    var total = navigation.find('li').length;
                    var current = index + 1;
                    var $percent = (current / total) * 100;
                    $('#form_wizard_1').find('.progress-bar').css({
                        width: $percent + '%'
                    });
                }
            });

            $('#form_wizard_1').find('.button-previous').hide();
            $('#form_wizard_1 .button-submit').click(function () {
                //alert('Finished! Hope you like it :)');
                // globalIsValid = true;

            }).hide();

            //apply validation on select2 dropdown value change, this only needed for chosen dropdown integration.
            $('#country_list', form).change(function () {
                form.validate().element($(this)); //revalidate the chosen dropdown value and show error or success message for the input
            });
        }

    };

}();

jQuery(document).ready(function () {
    FormWizard.init();
});