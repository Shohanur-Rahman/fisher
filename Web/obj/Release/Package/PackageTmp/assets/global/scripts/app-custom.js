var appBodyHeight;
var appBodyWidth;
$(document).ready(function () {
    appBodyHeight = $(window).height();
    appBodyWidth = $(window).width();
    $(".page-content").css({ "min-height": appBodyHeight + "px" });
    $(".fisher-hamburger").mouseover(function () {
        $(this).addClass("is-active");

        if ($("body").hasClass("page-sidebar-closed")) {
            $(this).removeClass("hamburger--arrow");
            $(this).addClass("hamburger--arrow-r");
        } else {
            $(this).removeClass("hamburger--arrow-r");
            $(this).addClass("hamburger--arrow");
        }
    });

    $(".fisher-hamburger").mouseleave(function () {
        $(this).removeClass("is-active");
    });

    $('.counter').each(function () {
        var $this = $(this),
            countTo = $this.attr('data-count');

        $({ countNum: $this.text() }).animate({
            countNum: countTo
        },

        {

            duration: 8000,
            easing: 'linear',
            step: function () {
                $this.text(Math.floor(this.countNum));
            },
            complete: function () {
                $this.text(this.countNum);
                //alert('finished');
            }

        });



    });
    
    
    //new WOW().init();

});

$(window).on('resize', function () {
    appBodyHeight = $(window).height();
    appBodyWidth = $(window).width();
    $(".page-content").css({ "min-height": appBodyHeight + "px" });
});

function HtmlEncode(value) {
    //create a in-memory div, set it's inner text(which jQuery automatically encodes)
    //then grab the encoded contents back out.  The div never exists on the page.
    return $('<div/>').text(value).html();
}

function HtmlDecode(value) {
    return $('<div/>').html(value).text();
}

function DecodehtmlDecode(value) {
    return value.replace(/&lt;/g, "<").replace(/&gt;/g, ">");
}
