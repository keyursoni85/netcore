
$(document).ready(function () {
    $("#Username").focus();
    $("#Username").keydown(function (e) {
        if ((e.shiftKey && ((e.which >= 48 && e.which <= 57) && e.which != 50 && e.which != 55))) {
            e.preventDefault();
        }
        else if ((e.shiftKey && ((e.which >= 186 && e.which <= 222) && e.which != 189))) {
            e.preventDefault();
        }
        else if ((e.which >= 186 && e.which <= 222) && e.which != 189 && e.which != 190) {
            e.preventDefault();
        }
        else if ((e.which >= 106 && e.which <= 111) && e.which != 110) {
            e.preventDefault();
        }
        else {
            return true;
        }
    });
});

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

