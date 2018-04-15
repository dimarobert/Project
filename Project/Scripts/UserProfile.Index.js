function editName(name) {
    $('#name-form').removeClass('d-none');
    $(name).hide();
}

function cancelEditName() {
    $('#name-form').addClass('d-none');
    $('#profile-name').show();
}

function editAboutme(name) {
    $('#aboutme-form').removeClass('d-none');
    $(name).hide();
}

function cancelEditAboutme() {
    $('#aboutme-form').addClass('d-none');
    $('#profile-aboutme').show();
}