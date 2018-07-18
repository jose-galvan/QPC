var InstructionController = function (instructionService, helper) {
    var button;
    var fail = helper.fail;
    var done = function () {
        var text = button.text() === "Performed" ? "Performed?" : "Performed";
        button.toggleClass('btn-performed').text(text);
    };

    var toggleStatus = function (e) {
        console.log('working.....');
        button = $(e.target);
        var instructionId = button.attr('id');
        if (button.hasClass('btn-pending'))
            instructionService.SetPerformed(instructionId, done, fail);
    };
    //Receives container of instructions in order to add metod to buttons
    var init = function (container) {
        $(container).click(toggleStatus);
    };

    return {
        Init: init
    };


}(InstructionService, ErrorHelper);