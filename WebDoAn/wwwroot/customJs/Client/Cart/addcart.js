(function () {

    $("#AddTocart").on("click", function () {
        var userid = $("#UserId").val();
        var proid = $("#ProductId").val();
        var quantt = $("#inputQuantt").val();

        $.ajax({
            url: "/Cart/AddToCart",
            type: 'POST',
            data: { idpro: proid, quantt: quantt, userid: userid },
            success: function (rs) {
                if (rs.ishang) {
                    alert("Thêm vào giỏ hàng thành công");
                }
                else {
                    alert("Số lượng sản phẩm không đủ, vui lòng nhập lại số lượng")
                }
            }
        })
    });
})(jQuery);