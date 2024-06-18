(function () {
    $('#summernote').summernote({
        height: 200,
        toolbar: [
            ['style', ['bold', 'italic', 'underline',]],
            ['para', ['ul', 'ol', 'paragraph']],
        ]
    });
    jQuery.validator.addMethod("requiredSummernote", function () {
        // true to pass validation
        // false to FAIL validation
        return !($("#summernote").summernote('isEmpty'));
    }, 'Summernote field is required');

    $("#form-validation").validate({
        ignore: ':hidden:not(:checkbox)',
        errorElement: 'label',
        errorClass: 'is-invalid',
        validClass: 'is-valid',
        rules: {
            Title: {
                required: true
            },
            SubDescription: {
                required: true,
            },
            Description: {
                required: true
            }
        },
        messages: {
            Title: "Trường này không được để trống",
            SubDescription: "Trường này không được để trống",
            Description: "Trường này không được để trống",
        }
    });


})(jQuery)
