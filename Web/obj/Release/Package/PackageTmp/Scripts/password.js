
; (function ($) {
    'use strict';

    var Password = function ($object, options) {
        var defaults = {
            shortPass: "<span style='color: #e73d4a;'>Low: The password is too short</span>",
            badPass: "<span style='color: #e73d4a;'>Low: try combining upper case, lower case letters & numbers</span>",
            goodPass: "<span style='color: #FFA500'>Medium; try using special characters</span>",
            strongPass: "<span style='color: #22B14C'>Strong: The password is strong</span>",
            containsUsername: "",
            enterPass: "",
            showPercent: false,
            showText: true,
            animate: true,
            animateSpeed: "fast",
            username: false,
            usernamePartialMatch: true,
            minimumLength: 6
        };

        options = $.extend({}, defaults, options);

        function scoreText(score) {

            if (score === -1) {
                return options.shortPass;
            }
            if (score === -2) {
                return options.containsUsername;
            }

            score = score < 0 ? 0 : score;

            if (score < 34) {
                return options.badPass;
            }
            if (score < 68) {
                return options.goodPass;
            }

            return options.strongPass;
        }


        //Test by SF
        function calculateScore(password, username) {
            var passResult = "";
            var score = 0;
            var specialOneRegEx = /[^a-zA-Z0-9]/;
            var numberOneRegEx = /^(?=.*[0-9]).+$/;
            var upperCaseRegEx = /^(?=.*[A-Z]).+$/;
            var lowerCaseRegEx = /^(?=.*[a-z]).+$/;


            score += password.length * 6;
            score += checkRepetition(1, password).length - password.length;
            score += checkRepetition(2, password).length - password.length;
            score += checkRepetition(3, password).length - password.length;
            score += checkRepetition(4, password).length - password.length;
            score += checkRepetition(5, password).length - password.length;
            score += checkRepetition(6, password).length - password.length;

            if (password != "" && password.length >= 6 && upperCaseRegEx.test(password) == true && lowerCaseRegEx.test(password) == true && numberOneRegEx.test(password) == true && specialOneRegEx.test(password) == true) {
                passResult = "strong";
                score += 60;
            } else {
                if (password.length >= 6 && upperCaseRegEx.test(password) == true && lowerCaseRegEx.test(password) == true && numberOneRegEx.test(password) == true) {
                    score = 20;
                }
            }


            if (password != "" && password.length >= 6 && upperCaseRegEx.test(password) == true && lowerCaseRegEx.test(password) == true && numberOneRegEx.test(password) == true ) {
                passResult = "medium";
                score += 30;
            } else {
                passResult = "low";
                if (password.length >= 6) {
                    score = 30;
                }
                
            }

            if (score > 100) {
                score = 100;
            }

            if (score < 0) {
                score = 0;
            }

            return score;
        }


        function checkRepetition(rLen, str) {
            var res = "", repeated = false;
            for (var i = 0; i < str.length; i++) {
                repeated = true;
                for (var j = 0; j < rLen && (j + i + rLen) < str.length; j++) {
                    repeated = repeated && (str.charAt(j + i) === str.charAt(j + i + rLen));
                }
                if (j < rLen) {
                    repeated = false;
                }
                if (repeated) {
                    i += rLen - 1;
                    repeated = false;
                }
                else {
                    res += str.charAt(i);
                }
            }
            return res;
        }


        function init() {
            var shown = true;
            var $text = options.showText;
            var $percentage = options.showPercent;
            var $graybar = $('<div>').addClass('pass-graybar');
            var $colorbar = $('<div>').addClass('pass-colorbar');
            var $insert = $('<div>').addClass('pass-wrapper').append(
                $graybar.append($colorbar)
            );

            $object.parent().addClass('pass-strength-visible');
            if (options.animate) {
                $insert.css('display', 'none');
                shown = false;
                $object.parent().removeClass('pass-strength-visible');
            }

            if (options.showPercent) {
                $percentage = $('<span>').addClass('pass-percent').text('0%');
                $insert.append($percentage);
            }

            if (options.showText) {
                $text = $('<span>').addClass('pass-text').html(options.enterPass);
                $insert.append($text);
            }

            $object.after($insert);

            $object.keyup(function () {
                var username = options.username || '';
                if (username) {
                    username = $(username).val();
                }

                var score = calculateScore($object.val(), username);
                $object.trigger('password.score', [score]);
                var perc = score < 0 ? 0 : score;
                $colorbar.css({
                    backgroundPosition: "0px -" + perc + "px",
                    width: perc + '%'
                });

                if (options.showPercent) {
                    $percentage.html(perc + '%');
                }

                if (options.showText) {
                    var text = scoreText(score);
                    if (!$object.val().length && score <= 0) {
                        text = options.enterPass;
                    }

                    if ($text.html() !== $('<div>').html(text).html()) {
                        $text.html(text);
                        $object.trigger('password.text', [text, score]);
                    }
                }
            });

            if (options.animate) {
                $object.focus(function () {
                    if (!shown) {
                        $insert.slideDown(options.animateSpeed, function () {
                            shown = true;
                            $object.parent().addClass('pass-strength-visible');
                        });
                    }
                });

                $object.blur(function () {
                    if (!$object.val().length && shown) {
                        $insert.slideUp(options.animateSpeed, function () {
                            shown = false;
                            $object.parent().removeClass('pass-strength-visible')
                        });
                    }
                });
            }

            return this;
        }

        return init.call(this);
    }

    // Bind to jquery
    $.fn.password = function (options) {
        return this.each(function () {
            new Password($(this), options);
        });
    };
})(jQuery);
