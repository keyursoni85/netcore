$("#AddEditPermission").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            if (d.length > 0) {
                $("#getPermission").html(d);
                $("#modal-action-Permission").modal("show");
                $("#modal-action-Permission").on('shown.bs.modal', function () {
                    $("#PermissionName").focus();
                });
            }
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

$(".EditPermission").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            $("#getPermission").html(d);
            $("#modal-action-Permission").modal("show");
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

$(".DeletePermission").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            $("#DeletePermission").parent().html(d);
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

function PermissionCofirmation(isError, message, isForgotPwdSucc) {

    if (isError && isError == "1") {
        var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-error' });
        jacked.log("<i class='fa fa-times-circle'></i>&nbsp;" + message);
    }

    if (isForgotPwdSucc && isForgotPwdSucc == "1") {
        var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-success' });
        jacked.log("<i class='fa fa-smile-o'></i>&nbsp;" + message);
    }
}