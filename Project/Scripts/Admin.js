function ValidateAjaxForm(elementId, data) {

    if (data.location) {
        window.location = data.location;
    } else {
        // we have a string which means an error
        var rootElement = $('#' + elementId);
        rootElement.html(data);
    }

}

function MakeSelection(userId, formId, rowIdPrefix, submitButtonId) {

    var form = $('#' + formId);
    var existingSelection = form.children('#UserId').val();

    if (existingSelection) {
        $('#' + rowIdPrefix + existingSelection).removeClass('table-active');
    }

    form.children('#UserId').val(userId);
    $('#' + rowIdPrefix + userId).addClass('table-active');

    $('#' + submitButtonId).removeClass('invisible');
}
