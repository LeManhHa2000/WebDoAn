(function () {

    $("#form-validation").validate({
        ignore: ':hidden:not(:checkbox)',
        errorElement: 'label',
        errorClass: 'is-invalid',
        validClass: 'is-valid',
        rules: {
            Name: {
                required: true
            },
            PhoneNumber: {
                required: true,
                minlength: 10,
            },
        },
        messages: {
            Name: "Trường này không được để trống",
            PhoneNumber: {
                required: "Trường này không được để trống",
                minlength: "Vui lòng nhập đủ 10 số",
            }
        
        }
    });

    document.getElementById("PhoneNumber").addEventListener("input", function () {
        var valueChange = funcChanePhoneNumber();
        $("#form-validation").find('input[name=PhoneNumber]').val(valueChange);
    });
    function funcChanePhoneNumber() {
        var valueChange = $("#form-validation").find('input[name=PhoneNumber]').val().replace(/[^0-9]/g, '');  
        return valueChange;
    }

    console.log("chayj vaof ddaay");

})(jQuery)