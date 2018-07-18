var ErrorHelper = function () {

    var fail = function (err) {
        console.log(err);
    };

    return {
        fail: fail
    };

}();