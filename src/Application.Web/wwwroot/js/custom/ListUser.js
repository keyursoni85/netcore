$("#createEditCustomerModal").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            if (d.length > 0) {
                $("#getcode").html(d);
                $("#modal-action-customer").modal("show");
                $("#modal-action-customer").on('shown.bs.modal', function () {
                    $("#Username").focus();
                });
            }
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

$(".editCustomer").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            $("#getcode").html(d);
            $("#modal-action-customer").modal("show");
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

$(".deleteUser").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            $("#DeleteUser").parent().html(d);
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

function UserCofirmation(isError, message, isForgotPwdSucc) {

    if (isError && isError == "1") {
        var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-error' });
        jacked.log("<i class='fa fa-times-circle'></i>&nbsp;" + message);
    }

    if (isForgotPwdSucc && isForgotPwdSucc == "1") {
        var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-success' });
        jacked.log("<i class='fa fa-smile-o'></i>&nbsp;" + message);
    }
}




