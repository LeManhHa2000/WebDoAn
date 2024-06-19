(function () {
    $('#summernote').summernote({
        height: 200,
        toolbar: [
            ['style', ['style']],
            ['font', ['bold', 'underline', 'clear']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['table', ['table']],
            ['view', ['fullscreen', 'codeview', 'help']]
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
            Photo: {
                required: true
            }
        },
        messages: {
            Title: "Trường này không được để trống",
            SubDescription: "Trường này không được để trống",
            Photo: "Trường này không được để trống",
        }
    });


})(jQuery)
