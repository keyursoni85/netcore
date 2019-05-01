$(document).ready(function () {
    $('.form_submit').submit(function () {
        if ($(this).valid()) {
            $(".btn_f_submit").prop("disabled", true);
            return true;
        }
    });
});