﻿(function () {

    var ProductTable = $('#data-table').DataTable({
        ajax: {
            url: "/Admin/Product/GetAllProduct",
            data: () => {
                var inputSearch = $("#SearchInput").val();
                if (inputSearch == "") {
                    return data2 = {
                        name: "all",
                    };
                }
                else {
                    return data2 = {
                        name: inputSearch,
                    };
                }
            },
            dataSrc: ''
        },
        searching: false,
        processing: true,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "info": "Đang hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
            "infoEmpty": "Đang hiển thị 0 đến 0 của 0 bản ghi",
            "infoFiltered": "(tìm kiếm từ tổng _MAX_ bản ghi)",
            "zeroRecords": "Không tìm thấy bản ghi nào",
            "paginate": {
                "next": "Tiếp",
                "previous": "Quay lại"
            },

        },
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],
        columnDefs: [
            {
                orderable: false,
                targets: 0,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    var stt = parseInt(meta.row) + 1;
                    return '<span>' + stt + '<span>';
                }
            },
            {
                targets: 1,
                data: "createTime",
                render: function (createTime) {
                    var createTimecv = createTime.split("T")[0];
                    var crreturn = createTimecv.split("-")
                    return crreturn[2] + "/" + crreturn[1] + "/" + crreturn[0];
                }

            },
            {
                targets: 2,
                data: "updateTime",
                render: function (updateTime) {
                    if (updateTime == "0001-01-01T00:00:00") {
                        return `<span></span>`;
                    }
                    else {
                        var updateTimecv = updateTime.split("T")[0];
                        var upreturn = updateTimecv.split("-")
                        return upreturn[2] + "/" + upreturn[1] + "/" + upreturn[0];
                    }
                }
            },
            {
                targets: 3,
                data: "image",
                width: "14%",
                render: function (image) {
                    var imgurl = "../images/product/" + image;
                    return `<img class="imgproduct_table" src="` + imgurl + `"/>`;
                }
            },
            {
                targets: 4,
                data: "name"
            },
            {
                targets: 5,
                data: "quantity"
            },
            {

                targets: 6,
                data: 'id',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `<div class='d-flex justify-content-center'>
                                <a href="Product/Details/`+ row.id + `" class="btn btn-info m-r-5 text-white">Xem chi tiết</a>
                                <a href="Product/Edit/`+ row.id + `" class="btn btn-warning m-r-5 text-white">Sửa</a>
                                <a href="Product/Delete/`+ row.id + `" class="btn btn-danger m-r-5 text-white">Xóa</a>
                            </div>`;
                }
            },
        ],
    });

    $("#Search").on("click", function () {
        ProductTable.ajax.reload();
    });
})(jQuery);