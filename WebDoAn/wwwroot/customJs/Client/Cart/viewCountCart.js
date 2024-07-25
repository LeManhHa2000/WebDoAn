(function () {

    // hàm load số lượng cart
    loadCountcart();
    SumToTalAll();

    // hàm xử lý thời gian hiện notiffi
    function hiddentNotiCt() {
        $("#successNumber").css("display", "none");
        $("#errorNumber").css("display", "none"); 
    }

    // Thêm vào giỏ hàng
    $("#AddTocart").on("click", function () {
        var userid = $("#UserId").val();
        var proid = $("#ProductId").val();
        var quantt = $("#inputQuantt").val();

        /*alert(userid + " " + proid + " " + quantt);*/

        $.ajax({
            url: "/Cart/AddToCart",
            type: 'POST',
            data: { idpro: proid, quantt: quantt, userid: userid },
            success: function (rs) {
                if (rs.ishang) {
                    $("#Textsuccesscart").text("Thêm vào giỏ hàng thành công!");
                    $("#successNumber").css("display", "block");
                    loadCountcart();
                    setTimeout(function () {
                        hiddentNotiCt();
                    }, 3000);
                    
                    
                }
                else {
                    $("#Textwaningcart").text("Số lượng sản phẩm không đủ, vui lòng nhập lại số lượng!");
                    $("#errorNumber").css("display", "block");
                    setTimeout(function () {
                        hiddentNotiCt();
                    }, 3000);
                }
            }
        })
    });

    // Xóa cart
    $(".btnDelete").on("click", function () {
        var cartId = $(this).data("id");

        var conf = confirm("Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng không?");
        if (conf == true) {
            $.ajax({
                url: "/Cart/DeleteToCart",
                type: 'POST',
                data: { id: cartId },
                success: function (rs) {
                    $("#row-" + cartId).remove();
                    loadCountcart();
                    SumToTalAll();
                    $("#Textsuccesscart").text("Xóa thành công!");
                    $("#successNumber").css("display", "block");
                    setTimeout(function () {
                        hiddentNotiCt();
                    }, 3000);
                }
            })
        }

    });

    function loadCountcart() {
        $.ajax({
            type: "POST",
            url: "/Cart/ShowCount",
            data: "",
            contextType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $(".countcart-ct").text(data.countCart)
            },
        });
    }

    // Thao tác trong giỏ hàng
    // nhấn nút +
    $(".btn-cre_custom").on("click", function () {
        var cartId = $(this).data("id");
        var proId = $(this).data("proid");
        var soluong = $("#Quantt_" + cartId).val();

        var gia = $("#Price-" + cartId).data("gia");

        var soluongmoi = parseInt($("#Quantt_" + cartId).val()) + 1;
        var total = parseInt(gia) * soluongmoi;
        var texttotal = Intl.NumberFormat('en-US').format(total);

        $.ajax({
            url: "/Cart/CreToCart",
            type: 'POST',
            data: { id: cartId, soluong: soluong, proid: proId },
            success: function (rs) {
                if (rs.iscre == 2) {
                    $("#total-" + cartId).text(texttotal);
                    $("#total-" + cartId).data("total", total);

                    // Cập nhật view số lượng mới
                    $("#Quantt_" + cartId).val(soluongmoi)
                    SumToTalAll();
                }
                else if (rs.iscre == 1) {
                    $("#Textwaningcart").text("Số lượng sản phẩm trong kho không đủ, vui lòng nhập lại số lượng!");
                    $("#errorNumber").css("display", "block");
                    setTimeout(function () {
                        hiddentNotiCt();
                    }, 3000);
                }
                else if (rs.iscre == 0) {
                    $("#Textwaningcart").text("Giá của sản phẩm này đã thay đổi , không thể thêm sản phẩm nữa!");
                    $("#errorNumber").css("display", "block");
                    setTimeout(function () {
                        hiddentNotiCt();
                    }, 3000);
                }
            }
        })
    });

    // Nhấn nút -
    $(".btn-des_custom").on("click", function () {
        var cartId = $(this).data("id");
        var proId = $(this).data("proid");
        var soluong = $("#Quantt_" + cartId).val();

        var gia = $("#Price-" + cartId).data("gia");

        var soluongmoi = parseInt($("#Quantt_" + cartId).val()) - 1;
        var total = parseInt(gia) * soluongmoi;
        var texttotal = Intl.NumberFormat('en-US').format(total);

        $.ajax({
            url: "/Cart/DesToCart",
            type: 'POST',
            data: { id: cartId, soluong: soluong, proid: proId },
            success: function (rs) {
                if (rs.isdes == true) {
                    $("#total-" + cartId).text(texttotal);
                    $("#total-" + cartId).data("total", total);

                    // Cập nhật view số lượng mới
                    $("#Quantt_" + cartId).val(soluongmoi)
                    SumToTalAll();
                }
                else {

                    $("#Textwaningcart").text("Không thể giảm được số lượng sản phẩm nữa!");
                    $("#errorNumber").css("display", "block");
                    setTimeout(function () {
                        hiddentNotiCt();
                    }, 3000);
                }
            }
        })
    });

    // Nhập ô input
    $(".quantity-input-ct").on("blur", function () {
        var cartId = $(this).data("id");
        /*var proId = $(this).data("proid");*/
        var soluong = $(this).val();

        var gia = $("#Price-" + cartId).data("gia");
        var total = parseInt(gia) * soluong;
        var texttotal = Intl.NumberFormat('en-US').format(total);

        $.ajax({
            url: "/Cart/UpdateToCart",
            type: 'POST',
            data: { id: cartId, soluong: soluong },
            success: function (rs) {
                if (rs.isUpdate == true) {
                    $("#total-" + cartId).text(texttotal);
                    $("#total-" + cartId).data("total", total);
                    SumToTalAll();
                }
                else {
                    $("#Quantt_" + cartId).val(rs.soluong);

                    $("#Textwaningcart").text("Số lượng sản phẩm trong kho không đủ, vui lòng nhập lại số lượng!");
                    $("#errorNumber").css("display", "block");
                    setTimeout(function () {
                        hiddentNotiCt();
                    }, 3000);
                }
            }
        })
    });


    function SumToTalAll() {
        var tong = 0;
        var classtotal = $(".total-card");
        for (var i = 0; i < classtotal.length; i++) {
            /*console.log("sd", $(classtotal[i]).text());*/
            var sotien = parseInt($(classtotal[i]).data("total"));

            tong += sotien;
        }
        console.log("tt", tong);
        var textTong = Intl.NumberFormat('en-US').format(tong) + " VND";
        $("#TongSum").text(textTong);
    }

    $("#btn_nullcart").on("click", function () {
        
        $("#Textwaningcart").text("Giỏ hàng trống ! Vui lòng thêm mới sản phẩm vào giỏ trước khi đặt hàng!");
        $("#errorNumber").css("display", "block");
        setTimeout(function () {
            hiddentNotiCt();
        }, 3000);
    });

    // View hình ảnh
    $(".img-child-ct").on("click", function () {
        var imgurl = $(this).data('imgbigurl');
        var bigImg = $('.product__details__pic__item--large').attr('src');
        if (imgurl != bigImg) {
            $('.product__details__pic__item--large').attr({
                src: imgurl
            });
        }
    });
    
})(jQuery);