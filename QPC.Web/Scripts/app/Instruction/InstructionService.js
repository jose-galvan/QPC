var InstructionService = function () {

    var setPerformed = function (instruction, done, fail) {

        $.ajax({
            url: '/api/instruction/' + instruction,
            method: 'PUT',
            headers: { "Authorization": localStorage.getItem('token') }

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