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
            Description: {
                required: true,
            },
        },
        messages: {
            Name: "Trường này không được để trống",
            Description: "Trường này không được để trống",
        }
    });

})(jQuery)