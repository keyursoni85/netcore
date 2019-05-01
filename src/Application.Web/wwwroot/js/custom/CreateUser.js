
$(document).ready(function () {

    $('#Username').keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });

    if ($("#UserId").val() == "0") {
        $("#Username").blur(function () {
            if ($(this).val() != "") {
                CheckDuplicateUserName();
            }
            else {
                $("#l_available").hide();
                $("#Username").css('border-color', '#ccc');
            }
        });
    }

    $("#Email").blur(function () {
        if ($(this).val() != "") {
            CheckDuplicateUserEmail();
        }
        else {
            $("#l_EmailAvailable").hide();
            $("#Email").css('border-color', '#ccc');
        }
    });

    $("#Password").blur(function () {
        if ($(this).val() != "") {
            CheckPassword();
        }
    });

    $("#SaveUser").submit(function () {
        return CheckPassword();
    });
});

function CheckDuplicateUserName() {
    var url = "/Manage/CheckUserName";
    var name = $("#Username").val();
    var culture = $("#Culture").text();

    $.get(url, { userName: name }, function (data) {
        if (data != "Available") {

            if (culture != 'en-US') {
                $("#l_available").text("Dieser Benutzername ist bereits vergeben. Bitte versuchen Sie es mit einem anderen.");
            }
            else {
                $("#l_available").text("This Username is already taken. Please Try another.");
            }

            $('#l_available').attr('style', 'color:red;');
            $("#Username").css('border-color', '#e97878');
            $("#Username").focus();
        }
        else {
            $("#l_available").hide();
            $("#Username").css('border-color', '#ccc');
        }
    });
}

function CheckDuplicateUserEmail() {
    var url = "/Manage/CheckUserEmail";
    var email = $("#Email").val();
    var uid = $("#UserId").val();
    var culture = $("#Culture").text();

    $.get(url, { userEmail: email, uId: uid }, function (data) {
        if (data != "Available") {

            if (culture != 'en-US') {
                $("#l_EmailAvailable").text("Diese E-Mail-Adresse ist bereits vergeben. Bitte versuchen Sie es mit einem anderen.");
            }
            else {
                $("#l_EmailAvailable").text("This Email address is already taken. Please Try another.");
            }

            $('#l_EmailAvailable').attr('style', 'color:red;');
            $("#Email").css('border-color', '#e97878');
            $("#Email").focus();
        }
        else {
            $("#l_EmailAvailable").hide();
            $("#Email").css('border-color', '#ccc');
        }
    });
}

function CheckPassword() {
    var url = "/Manage/CheckPassoword";
    var password = $("#Password").val();
    var MinLenght = $("#MinLenght").text();
    var MaxLenght = $("#MaxLenght").text();
    $.get(url, { password: password }, function (data) {
        switch (data) {
            case 1:
                $("#IsValidPassword").hide();
                $("#Password").css('border-color', '#ccc');
                return true;
            case -1:
                $("#IsValidPassword").text("A password must be at least " + MinLenght + " characters long. but not more than " + MaxLenght + " characters in length.");
                $('#IsValidPassword').attr('style', 'color:red;');
                $("#Password").css('border-color', '#e97878');
                $("#Password").focus();
                return false;
            case -2:
                $("#IsValidPassword").text("A password must be one uppercase letter, one special character and alphanumeric characters.");
                $('#IsValidPassword').attr('style', 'color:red;');
                $("#Password").css('border-color', '#e97878');
                $("#Password").focus();
                return false;
            case -3:
                $("#IsValidPassword").text("A password must be one special character and alphanumeric characters.");
                $('#IsValidPassword').attr('style', 'color:red;');
                $("#Password").css('border-color', '#e97878');
                $("#Password").focus();
                return false;
            case -4:
                $("#IsValidPassword").text("A password must be alphanumeric characters.");
                $('#IsValidPassword').attr('style', 'color:red;');
                $("#Password").css('border-color', '#e97878');
                $("#Password").focus();
                return false;
        }
    });
}




