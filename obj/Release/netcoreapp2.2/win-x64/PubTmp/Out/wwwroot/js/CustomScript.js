

$(document).ready(function () {

    /*for sticky navigation*/
    $('.js--section-features').waypoint(function (direction) {
        if (direction == "down") {
            $('nav').addClass('sticky');
        } else {
            $('nav').removeClass('sticky');
        }
    }, { offset: '60px;' }
    );

    /*for scroll buttons*/
    $('.js--scroll-to-subscribe').click(function () {
        $('html,body').animate({ scrollTop: $('.js--section-form').offset().top }, 1000);
    });

    $('.js--scroll-to-pics').click(function () {
        $('html,body').animate({ scrollTop: $('.js--section-pics').offset().top }, 1000);
    });

});