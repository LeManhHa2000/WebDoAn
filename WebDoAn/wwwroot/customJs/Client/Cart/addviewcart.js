﻿(function () {
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

    $(".btnUpdate").on("click", function () {
        var cartId = $(this).data("id");
        var soluong = $("#Quantt_" + cartId).val();
        var gia = $("#Price-" + cartId).text();
        var total = parseInt(gia) * soluong; 

        $.ajax({
            url: "/Cart/UpdateToCart",
            type: 'POST',
            data: { id: cartId, soluong: soluong },
            success: function (rs) {
                if (rs.isUpdate == true) {
                    $("#total-" + cartId).text(total);
                    SumToTalAll();
                    alert("Cập nhật thành công");
                }
                else {
                    alert("Số lượng sản phẩm trong kho không đủ, vui lòng nhập lại số lượng")
                }
            }
        })
    });

    function SumToTalAll() {
        var tong = 0;
        var classtotal = $(".total-card");
        for (var i = 0; i < classtotal.length; i++) {
            /*console.log("sd", $(classtotal[i]).text());*/
            var sotien = parseInt($(classtotal[i]).text());

            tong += sotien;
        }
        var textTong = tong + " VND";
        $("#TongSum").text(textTong);
    }

    $("#btn_nullcart").on("click", function () {
        alert("Giỏ hàng trống ! Vui lòng thêm mới sản phẩm vào giỏ trước khi đặt hàng");
    })
})(jQuery);