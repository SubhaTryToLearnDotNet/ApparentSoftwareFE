function showInfoMsg(msg) {
    Swal.fire({
        icon: 'info',
        title: 'Information!',
        html: msg,
    });
}
function ErrorMsg(msg) {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        html: msg,
    });
}
function showSuccess(msg) {
    Swal.fire({
        icon: 'success',
        title: 'Success',
        html: msg,
    });
}

function showSuccessUrl(msg, url) {
    Swal.fire({
        icon: 'success',
        title: 'Success',
        html: msg,
    }).then(function () {
        window.location.href = url;
    });
}
function validateEmail(email) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    return reg.test(email);
}