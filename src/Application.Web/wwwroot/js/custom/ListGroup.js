﻿$("#CreateEditGroupModal").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            if (d.length > 0) {
                $("#getGroup").html(d);
                $("#modal-action-group").modal("show");
                $("#modal-action-group").on('shown.bs.modal', function () {
                    $("#GroupName").focus();
                });
            }
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

$(".EditGroup").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            $("#getGroup").html(d);
            $("#modal-action-group").modal("show");
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

$(".DeleteGroup").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            $("#DeleteGroup").parent().html(d);
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

function GroupCofirmation(isError, message, isForgotPwdSucc) {

    if (isError && isError == "1") {
        var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-error' });
        jacked.log("<i class='fa fa-times-circle'></i>&nbsp;" + message);
    }

    if (isForgotPwdSucc && isForgotPwdSucc == "1") {
        var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-success' });
        jacked.log("<i class='fa fa-smile-o'></i>&nbsp;" + message);
    }
}