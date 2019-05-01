setTimeout(function () {
    
    var request = { "challenge": challange, "version": version, "appId": appId };
    var registerRequests = [{ version: request.version, challenge: request.challenge }];
    console.log(request);
    console.log(registerRequests);

    u2f.register(request.appId, registerRequests, [],
        function (data) {
            console.log(data);
            if (data.errorCode) {
                return alert(errorMap[data.errorCode]);
            }
            console.log("Register callback", data);
            $('#DeviceResponse').val(JSON.stringify(data));
            $('#registerForm').submit();

            return false;
        });
}, 1000);