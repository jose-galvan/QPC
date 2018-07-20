var InstructionService = function () {

    var setPerformed = function (instruction, done, fail) {

        $.ajax({
            url: '/api/instruction/' + instruction,
            method: 'PUT'

        })
        .done(done)
        .fail(function (jqXHR, textStatus, err) {
            fail(err);
        });

    };

    return {
        SetPerformed: setPerformed
    };

}();