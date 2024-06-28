(function () {
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
                    $(".tomuch.name-" + cartId).removeClass("tomuch");
                    loadToQuantityMax();
                    alert("Cập nhật thành công");
                }
                else {
                    alert("Số lượng sản phẩm trong kho không đủ, vui lòng nhập lại số lượng")
                }
            }
        })
    });

})(jQuery);