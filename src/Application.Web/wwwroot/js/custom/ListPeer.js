$("#CreateEditPeerModal").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            if (d.length > 0) {
                $("#GetPeers").html(d);
                $("#modal-action-peer").modal("show");
                $("#modal-action-peer").on('shown.bs.modal', function () {
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

$(".EditPeer").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            $("#GetPeers").html(d);
            $("#modal-action-peer").modal("show");
            $(".loading").hide();
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });
});

$(".DeletePeer").unbind().click(function () {
    $(".loading").show();
    $.ajax({
        url: this.href,
        type: 'GET',
        dataType: '',
        success: function (d) {
            $("#DeletePeer").parent().html(d);
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
