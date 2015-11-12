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


$(document).ready(function() {
    $(document).on("scroll", onScroll);
    $("a[href^=\"#\"]").on("click", function(e) {
        e.preventDefault();
        $(document).off("scroll");
        $("a").each(function() {
            $(this).removeClass("active");
        });
        $(this).addClass("active");
        var target = this.hash;
        $target = $(target);
        $("html, body").stop().animate({
            'scrollTop': $target.offset().top

        }, 500, "swing", function() {
            window.location.hash = target;
            $(document).on("scroll", onScroll);
        });
    });
});


function onScroll(event) {
    var scrollPosition = $(document).scrollTop();
    $(".nav li a").each(function() {
        var currentLink = $(this);
        var refElement = $(currentLink.attr("href"));

        if (refElement && refElement.position() && (refElement.position().top <= scrollPosition && refElement.position().top + refElement.height() > scrollPosition)) {
            $(".nav li a").removeClass("active");
            currentLink.addClass("active");

        } else {
            currentLink.removeClass("active");
        }
    });
}

function stickyEx(headerId, contentId) {
    var _headerEl = $("#" + headerId);
    var _contentEl = $("#" + contentId);
    var _topMenu = $("#menu");
    var startOffset = _headerEl.position().top;
    _headerEl.css("position", "relative");

    var _borderWidth = 4;

    $(window).scroll(function() {
        var topHeaderHeight = _topMenu.outerHeight();
        var fullHeight = _headerEl.outerHeight() + _contentEl.outerHeight();
        var scrollPosition = $(window).scrollTop();

        if ((scrollPosition) > (startOffset - topHeaderHeight) && ((scrollPosition) < (startOffset + _contentEl.outerHeight() - _borderWidth - topHeaderHeight))) {
            _headerEl.css("position", "fixed");
            _headerEl.css("top", topHeaderHeight + "px");
            _headerEl.css("z-index", "2");
        } else if ((scrollPosition < (startOffset - topHeaderHeight))) {
            _headerEl.css("position", "static");
            _headerEl.css("z-index", "2");
        } else if (scrollPosition > (startOffset - topHeaderHeight + _contentEl.outerHeight() + _borderWidth)) {
            if (_headerEl.css("position") != "absolute") {
                _headerEl.css("position", "absolute");
                _headerEl.css("z-index", "1");
                _headerEl.css("top", (_contentEl.outerHeight() + startOffset + topHeaderHeight + _borderWidth - topHeaderHeight) + "px");

            }
        }
    });
};

function initMenu() {
    window.setTimeout(function() {
        var days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Favorite", "History"];
        $("#menu").sticky({ topSpacing: 0 });

        for (var i = 0, len = days.length; i < len; ++i) {
            new stickyEx("_" + days[i] + "Header", "_" + days[i]);
        }
    }, 1);
};