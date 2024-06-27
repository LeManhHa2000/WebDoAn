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

    console.log("adad");

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
            Name: {
                required: true
            },
            CategoryId: {
                required: true,
            },
            TypeProduct: {
                required: true,
            },
            Quantity: {
                required: true,
            },
            Price: {
                required: true,
            },
            Photo: {
                required: false,
            },
            Length: {
                required: true,
            },
            Height: {
                required: true,
            },
            Width: {
                required: true,
            },
            Material: {
                required: true,
            },
            ShortDescription: {
                required: true,
            },
            Description: {
                requiredSummernote: true
            }
        },
        messages: {
            Name: "Trường này không được để trống",
            CategoryId: "Trường này không được để trống",
            TypeProduct: "Trường này không được để trống",
            Quantity: "Trường này không được để trống",
            Price: "Trường này không được để trống",
            ShortDescription: "Trường này không được để trống",
            Description: "Trường này không được để trống",
            Length: "Trường này không được để trống",
            Height: "Trường này không được để trống",
            Width: "Trường này không được để trống",
            Material: "Trường này không được để trống",
        }
    });

    //if ($('#summernote').summernote('isEmpty')) {
    //    alert('editor content is empty');
    //}


})(jQuery)
