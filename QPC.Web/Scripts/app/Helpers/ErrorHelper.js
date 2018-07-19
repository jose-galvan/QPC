var ErrorHelper = function () {

    var fail = function (err) {
        console.log(err);
    };

    return {
        fail: fail
    };

}();

var Helper = function () {

    var logOff = function () {
        $.post('/Account/LogOff', null, function () {
            console.log('success');
        })
        .done(function (data) {
            window.location('index');
        })
        .fail(function (status) {
            console.log('error' + status);
        });
    };

    return {
        LoggOff: logOff
    };
    
}();