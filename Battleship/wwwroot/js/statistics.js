$(function () {
    let allGamesList = document.getElementById("allGamesContainer");
    let userOnlyGamesList = document.getElementById("userOnlyGamesContainer");
    userOnlyGamesList.hidden = true;
    $('.switch-btn').click(function (e, changeState) {
        if (changeState === undefined) {
            $(this).toggleClass('switch-on');
        }
        if ($(this).hasClass('switch-on')) {
            $(this).trigger('on.switch');
        } else {
            $(this).trigger('off.switch');
        }
    });

    $('.switch-btn').on('on.switch', function () {
        allGamesList.hidden = true;
        userOnlyGamesList.hidden = false;
    });

    $('.switch-btn').on('off.switch', function () {
        allGamesList.hidden = false;
        userOnlyGamesList.hidden = true;
    });

    $('.switch-btn').each(function () {
        $(this).triggerHandler('click', false);
    });
});