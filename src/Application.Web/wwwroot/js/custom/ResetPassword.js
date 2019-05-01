$(document).ready(function () {
    $("#NewPassword").focus();

    $("#NewPassword").blur(function () {
        if ($(this).val() != "") {
            CheckPassword();
        }
    });

    $("#restpasswordsubmit").submit(function () {
        return CheckPassword();
        return PasswordValidation($("#NewPassword").val(), $("#ConfirmPassword").val());
    });

});

function PasswordValidation(newPassword, confirmPassword) {

    if (newPassword != "" && confirmPassword != "") {
        if (newPassword != confirmPassword) {
            var message = "Your password and confirmation password do not match.";
            var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-error' });
            jacked.log("<i class='fa fa-times-circle'></i>&nbsp;" + message);
            return false;
        }
        else {
            return true;
        }
    }
}


function UserValidation(isError, message, isForgotPwdSucc) {

    if (isError && isError == "1") {
        var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-error' });
        jacked.log("<i class='fa fa-times-circle'></i>&nbsp;" + message);
    }


    if (isForgotPwdSucc && isForgotPwdSucc == "1") {
        var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-success' });
        jacked.log("<i class='fa fa-smile-o'></i>&nbsp;" + message);
    }
}

function CheckPassword() {
    var url = "/Manage/CheckPassoword";
    var password = $("#NewPassword").val();
    var MinLenght = $("#MinLenght").text();
    var MaxLenght = $("#MaxLenght").text();
    var culture = $("#Culture").val();
    
    $.get(url, { password: password }, function (data) {
        switch (data) {
            case 1:
                $("#IsValidPassword").hide();
                $("#Password").css('border-color', '#ccc');
                return true;
            case -1:
                if (culture != 'en-US') {
                    $("#IsValidPassword").text("Ein Passwort muss mindestens sein " + MinLenght + " Zeichen lang. aber nicht mehr als " + MaxLenght + " Zeichen in der Länge.");
                }
                else {
                    $("#IsValidPassword").text("A password must be at least " + MinLenght + " characters long. but not more than " + MaxLenght + " characters in length.");
                }

                $('#IsValidPassword').attr('style', 'color:red;');
                $("#NewPassword").focus();
                return false;
            case -2:

                if (culture != 'en-US') {
                    $("#IsValidPassword").text("Ein Passwort muss ein Großbuchstabe, ein Sonderzeichen und alphanumerische Zeichen sein.");
                }
                else {
                    $("#IsValidPassword").text("A password must be one uppercase letter, one special character, and alphanumeric characters.");
                }

                $('#IsValidPassword').attr('style', 'color:red;');
                $("#NewPassword").focus();
                return false;
            case -3:
                if (culture != 'en-US') {
                    $("#IsValidPassword").text("Ein Passwort muss ein Sonderzeichen und alphanumerische Zeichen sein.");
                }
                else {
                    $("#IsValidPassword").text("A password must be one special character and alphanumeric characters.");
                }
                $('#IsValidPassword').attr('style', 'color:red;');
                $("#NewPassword").focus();
                return false;
            case -4:
                if (culture != 'en-US') {
                    $("#IsValidPassword").text("Ein Passwort muss aus einem Großbuchstaben und alphanumerischen Zeichen bestehen.");
                }
                else {
                    $("#IsValidPassword").text("A password must be one uppercase letter and alphanumeric characters.");
                }

                $('#IsValidPassword').attr('style', 'color:red;');
                $("#Password").css('border-color', '#e97878');
                $("#Password").focus();
                return false;
            case -5:
                if (culture != 'en-US') {
                    $("#IsValidPassword").text("Ein Passwort muss aus alphanumerischen Zeichen bestehen.");
                }
                else {
                    $("#IsValidPassword").text("A password must be alphanumeric characters.");
                }
                $('#IsValidPassword').attr('style', 'color:red;');
                $("#NewPassword").focus();
                return false;
        }
    });
}