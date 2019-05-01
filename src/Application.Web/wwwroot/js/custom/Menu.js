$("#CreateEditMenuModal").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            if (d.length > 0) {
                $("#getMenu").html(d);
                $("#modal-action-menu").modal("show");
                $("#modal-action-menu").on('shown.bs.modal', function () {
                    $("#MenuName").focus();
                });
            }
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

$(".EditMenu").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            $("#getMenu").html(d);
            $("#modal-action-menu").modal("show");
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

$(".DeleteMenu").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            $("#DeleteMenu").parent().html(d);
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

function MenuCofirmation(isError, message, isForgotPwdSucc) {

    if (isError && isError == "1") {
        var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-error' });
        jacked.log("<i class='fa fa-times-circle'></i>&nbsp;" + message);
    }

    if (isForgotPwdSucc && isForgotPwdSucc == "1") {
        var jacked = humane.create({ timeout: 6000, baseCls: 'humane-jackedup', addnCls: 'humane-jackedup-success' });
        jacked.log("<i class='fa fa-smile-o'></i>&nbsp;" + message);
    }
}