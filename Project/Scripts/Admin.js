function ValidateAjaxForm(elementId, data) {

    if (data.location) {
        window.location = data.location;
    } else {
        // we have a string which means an error
        var rootElement = $('#' + elementId);
        rootElement.html(data);
    }

}