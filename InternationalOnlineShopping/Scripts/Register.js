function SignUpVendor() {
    $('#ShowModalVendor').modal();
    $('#SuccessMessage').hide();
    $('#ErrorMessage').hide();
}

function SignUpCustomer() {
    $('#ShowModalCustomer').modal();
}

function SaveVendor(form) {
    debugger;
    //var email = $('#txtEmail').val();

    //if (email == "") {
    //    $('#SuccessMessage').hide();
    //    $('#ErrorMessage').show();
    //    return false;
    //}
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        $.ajax({
            type: "post",
            url: form.action,
            data: new FormData(form),

            success: function (result) {
              //  alertify(result, "success");
                $('#SuccessMessage').hide();
                $('#ErrorMessage').hide();
                

            },
            error: function () {
                $('#SuccessMessage').hide();
                $('#ErrorMessage').show();
            }

        });
    }
}

function Login(form) {
   
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        $.ajax({
            type: "post",
            url: form.action,
            data: new FormData(form),

            success: function (result) {

                if (result === "Fail") {
                    $('#loginForm')[0].reset();
                    $('#msg').show();
                }
                else {

                    window.location.href = "/Product/Index";
                    $('#msg').hide();
                }
               


            }
           

        });
    }
}
