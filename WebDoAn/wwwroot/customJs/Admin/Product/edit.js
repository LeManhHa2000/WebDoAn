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
    var listimg = [];

    $("#Image").on("change", function (e) {
        /*var image = URL.createObjectURL(e.target.files[0]);*/
        //Chuỗi ảnh
        var images = e.target.files;
        console.log("file", images);

        for (var i = 0; i < images.length; i++) {
            listimg.push(images[i]);
            var image = URL.createObjectURL(images[i]);

            var imageItem = `
                <div class="col-2 mt-2">
                    <div class="privew-item-ct">
                        <span class="remove-img-ct" data-nameimg="`+ images[i].name + `">
                            <i class="anticon anticon-close"></i>
                        </span>
                        <img src="`+ image + `" style="width: 100%; height: 200px;" />
                    </div>
                </div>
        `;

            $("#priVewOrther").append(imageItem);
        }

        var filderLabel = $(this).next(".custom-file-label");
        if (listimg.length >= 1) {
            filderLabel.html(listimg.length + " ảnh được chọn");
        }
        else if (listimg.length == 0) {
            filderLabel.html("Chọn ảnh");
        }

        $(".remove-img-ct").on("click", function () {
            var dataremove = $(this).data("nameimg");
            $(this).parent("div").parent("div").remove();
            for (var z = 0; z < listimg.length; z++) {
                if (listimg[z].name == dataremove) {
                    listimg.splice(z, 1);

                }
            }

            if (listimg.length >= 1) {
                filderLabel.html(listimg.length + " ảnh được chọn");
            }
            else if (listimg.length == 0) {
                filderLabel.html("Chọn ảnh");
            }
            console.log("list", listimg);
            const dataTransfer = new DataTransfer();
            for (var x = 0; x < listimg.length; x++) {
                dataTransfer.items.add(listimg[x]);
            }

            fileInputElement.files = dataTransfer.files;

            //$("#ImageOrther").attr("data-title", "No file chosen");
        });

        let fileInputElement = document.getElementById('ImageImpor');

        // Now let's create a DataTransfer to get a FileList
        const dataTransfer = new DataTransfer();
        for (var x = 0; x < listimg.length; x++) {
            dataTransfer.items.add(listimg[x]);
        }

        /*$("#Image").files = listimg;*/
        fileInputElement.files = dataTransfer.files;
        console.log("list", dataTransfer.items);

    });

    $(".remove-imged-ct").on("click", function () {
        var proimgId = $(this).data("proimgid");
        $(this).parent("div").parent("div").remove();

        $.ajax({
            url: "/Admin/Product/DeleteProImg",
            type: 'POST',
            data: { id: proimgId },
            success: function (rs) {
                
            }
        })

    });

})(jQuery)
