
let RegisterForm = document.querySelector("#register");
RegisterForm.addEventListener("submit", (event) => {


    if ($("#register").valid()) {
        let proceed = document.querySelector("#register-btn");
        proceed.disabled = true;
        proceed.innerText = "loading...";

    } else {
        $('.spinner').css('display', 'none');
    }
});

$(function () {
    $.validator.addMethod("noSpace", function (value, element) {

        return value == "" || value.trim().length != 0;
    }, "spaces are not allowed");

    var value = $("#password").val();

    $.validator.addMethod("checklower", function (value) {
        return /[a-z]/.test(value);
    });
    $.validator.addMethod("checkupper", function (value) {
        return /[A-Z]/.test(value);
    });
    $.validator.addMethod("checkdigit", function (value) {
        return /[0-9]/.test(value);
    });
    $.validator.addMethod("pwcheck", function (value) {
        return /[!"#$&'()*+%,-./:;<=>?@\[\\\]^_`{|}~]/.test(value);
    });

    var $registerForm = $("#register");



    if ($registerForm.length) {

        $registerForm.validate({
            rules: {

                name: {
                    required: true
                    ,
                    noSpace: true,

                    minlength: 4

                },



                Email: {
                    required: true
                    ,
                    noSpace: true,
                    email: true
                },

                password: {
                    required: true
                    ,
                    noSpace: true,
                    minlength: 8,

                    maxlength: 30,

                    pwcheck: true,
                    checklower: true,
                    checkupper: true,
                    checkdigit: true




                }
                ,
                confirmPassword: {
                    equalTo: "#password"
                }


            },
            messages: {

                name: {
                    required: "Name is  a required field!",
                    minlength: "Name  must be 4 characters atleast!",



                },
                Email: {
                    required: "Email is  a required field!",
                    email: "Must be a valid mail!",




                },
                password: {
                    required: "password is  a required field!",
                    minlength: "Password must be 8 characters atleast!",
                    maxlength: "Password must be 30 characters atmost!",
                    pwcheck: "Need atleast 1  Special Character",
                    checklower: "Need atleast 1 lowercase alphabet",
                    checkupper: "Need atleast 1 uppercase alphabet",
                    checkdigit: "Need atleast 1 Number"

                },
                confirmPassword: {
                    equalTo: "Must Match password!"
                },
                
            },
            errorElement: "div",
            errorPlacement: function (error, element) {

                error.insertAfter(element);

            }

        });
    }





});





