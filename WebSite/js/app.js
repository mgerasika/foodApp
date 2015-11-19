$(function() {
    var windowH = $(window).height();
    var bannerH = $("#banner").height();
    if (windowH > bannerH) {
        $("#banner").css({ 'height': ($(window).height() - 68) + "px" });
        $("#bannertext").css({ 'height': ($(window).height() - 68) + "px" });
    }
    $(window).resize(function() {
        var windowH = $(window).height();
        var bannerH = $("#banner").height();
        var differenceH = windowH - bannerH;
        var newH = bannerH + differenceH;
        var truecontentH = $("#bannertext").height();
        if (windowH < truecontentH) {
            $("#banner").css({ 'height': (newH - 68) + "px" });
            $("#bannertext").css({ 'height': (newH - 68) + "px" });
        }
        if (windowH > truecontentH) {
            $("#banner").css({ 'height': (newH - 68) + "px" });
            $("#bannertext").css({ 'height': (newH - 68) + "px" });
        }

    });
});


$(function() {
    $("#galleryimg").mixItUp();
});
/*$('.timer').each(count);*/
jQuery(function($) {
    // custom formatting example
    $(".timer").data("countToOptions", {
        formatter: function(value, options) {
            return value.toFixed(options.decimals).replace(/\B(?=(?:\d{3})+(?!\d))/g, ",");
        }
    });

    // start all the timers
    $("#gallery").waypoint(function() {
        $(".timer").each(count);
    });

    function count(options) {
        var $this = $(this);
        options = $.extend({}, options || {}, $this.data("countToOptions") || {});
        $this.countTo(options);
    }
});


$(".quotes").quovolver({
    equalHeight: true
});




function initMenu() {
   
};