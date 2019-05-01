
$(document).ready(function () {

    $('body').append('<div class="loader-block loading" style="display:none"><svg id="loader2" viewBox="0 0 100 100"><circle id="circle-loader2" cx="50" cy="50" r="45"></circle></svg></div>');

    $(".form-loading").submit(function () {
        $(".loading").css({ "display": "block" });
        return true;
    });

    $(".btn-loading").submit(function () {
        $(".loading").css({ "display": "block" });
        return true;
    });
});



