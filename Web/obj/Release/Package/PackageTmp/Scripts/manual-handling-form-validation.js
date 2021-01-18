var FormWizard = function () {

    return {
        //main function to initiate the module
        init: function () {
            if (!jQuery().bootstrapWizard) {
                return;
            }

            var form = $('#frmManualHandlingInfo');
            var error = $('.alert-danger', form);
            var success = $('.alert-success', form);

            form.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block help-block-error', // default input error message class
                focusInvalid: false, // do not focus the last invalid input

                messages: { 
                },

                errorPlacement: function (error, element) { 
                    error.insertAfter(element); 
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
                    //}
                }

            });


            var handleTitle = function (tab, navigation, index) {

                var total = navigation.find('li').length;
                var current = index + 1;
                var substanceName = " - " + $("#AssessmentName").val();
                $('.step-title', $('#form_manual_handling')).html('Step ' + (index + 1) + ' of ' + total + "<span class='caption- subject font-blue bold uppercase dummy_substance_name'>" + substanceName + "</span>");

                //set done steps          
                jQuery('li', $('#form_manual_handling')).removeClass("done");
                var li_list = navigation.find('li');
                for (var i = 0; i < index; i++) {
                    jQuery(li_list[i]).addClass("done");
                }

                if (current == 1) {
                    $('#form_manual_handling').find('.button-previous').hide();
                } else {
                    $('#form_manual_handling').find('.button-previous').show();     
                }

                if (current >= total) {
                    $('#form_manual_handling').find('.button-next').hide();
                    $('#form_manual_handling').find('.button-submit').show();

                } else {
                    $('#form_manual_handling').find('.button-next').show();
                    $('#form_manual_handling').find('.button-submit').hide();
                }

                // App.scrollTo($('.page-title'));
                App.scrollTo($('.dummy_div_for_scroll_to'));
            }

            // default form wizard
            $('#form_manual_handling').bootstrapWizard({
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
                    $('#form_manual_handling').find('.progress-bar').css({
                        width: $percent + '%'
                    });
                }
            });

            $('#form_manual_handling').find('.button-previous').hide();
            $('#form_manual_handling .button-submit').click(function () {

            }).hide();

        }

    };

}();

jQuery(document).ready(function () {
    FormWizard.init();
});