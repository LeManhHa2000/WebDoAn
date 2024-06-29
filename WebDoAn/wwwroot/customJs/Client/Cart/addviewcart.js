(function () {
    SumToTalAll();
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
                    SumToTalAll();
                }
            })
        }
        
    });

    // cập nhật số lượng sản phầm trong giỏ hàng
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
                if (rs.iscre == true) {
                    $("#total-" + cartId).text(texttotal);
                    $("#total-" + cartId).data("total", total);

                    // Cập nhật view số lượng mới
                    $("#Quantt_" + cartId).val(soluongmoi)
                    SumToTalAll();
                }
                else {
                    alert("Số lượng sản phẩm trong kho không đủ, vui lòng nhập lại số lượng");
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
                    alert("Không thể giảm được số lượng sản phẩm nữa");
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
                    alert("Số lượng sản phẩm trong kho không đủ, vui lòng nhập lại số lượng");
                }
            }
        })
    });

    $(".btnUpdate").on("click", function () {
        var cartId = $(this).data("id");
        var soluong = $("#Quantt_" + cartId).val();

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
                    $(".tomuch.name-" + cartId).removeClass("tomuch");
                }
                else {
                    alert("Số lượng sản phẩm trong kho không đủ, vui lòng nhập lại số lượng");
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
        alert("Giỏ hàng trống ! Vui lòng thêm mới sản phẩm vào giỏ trước khi đặt hàng");
    });

    //$("#btn_Dathang").on("click", function () {
    //    $.ajax({
    //        url: "/Cart/CheckBeforCreateOrder",
    //        type: 'POST',
    //        data: {},
    //        success: function (rs) {
    //            if (rs.isNot == true) {
    //                alert("Số lượng đủ");
    //            }
    //            else {
    //                alert("có sản phẩm có só lượng tahy đổi");
    //            }
    //        }
    //    })
    //})

    //function loadToQuantityMax() {
    //    if ($(".tomuch").length > 0) {
    //        $("#btn_Dathang").addClass("notclick");
    //        $("#toMaxtext").text("Có sản phẩm bị thay đổi số lượng và số lượng hàng đặt của bạn đang lớn hơn ( tên sản phẩm màu đỏ ). Vui lòng cập nhật lại !")
    //    }
    //    else {
    //        $("#btn_Dathang").removeClass("notclick");
    //        $("#toMaxtext").text("");
    //    }
    //}
 
    
})(jQuery);