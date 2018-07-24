var ErrorHelper = function () {

    var fail = function (err) {
        console.log(err);
    };

    return {
        fail: fail
    };

}();

$(document).ready(function () {
    console.log('init....');

    $('#parent').click(function () {
        var isVisible = $('.admin-menu').is(":visible");

        var adminmenu = $('.admin-menu');
        if (isVisible)
            adminmenu.fadeOut();
        else
            adminmenu.fadeIn();
    });
});
