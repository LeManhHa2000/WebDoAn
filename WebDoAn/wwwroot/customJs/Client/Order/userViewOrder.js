(function () {
    moment.locale("vi");
    var data2 = {};

    var OrderTable = $('#OrderView').DataTable({
        ajax: {
            url: "/OrderUser/GetAllOrder",
            data: () => {
                return data2 = {
                    orderId : 0,
                    idUser: $("#UserId").val()
                };
                
                //var inputSearch = $("#SearchInput").val();
                //if (inputSearch == "") {
                //    return data2 = {
                //        name: "all",
                //    };
                //}
                //else {
                //    return data2 = {
                //        name: inputSearch,
                //    };
                //}
            },
            dataSrc: ''
        },
        //listAction: {
        //    ajaxFunction: "/Admin/Category/GetAllCategory",
        //    inputFilter: function () { }
        //},
        searching: false,
        processing: true,
        "bLengthChange": false,
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
            [10, 20, 30, 50, -1],
            [10, 20, 30, 50, 'Tất cả'],
        ],
        columnDefs: [
            {
                orderable: true,
                targets: 0,
                data: "id",
                className: 'text-center',
            },
            {
                orderable: false,
                targets: 1,
                data: "createTime",
                className: 'text-center',
                render: function (createTime) {
                    return moment(createTime).format('L');
                }

            },
            {
                orderable: false,
                targets: 2,
                data: "shipDate",
                className: 'text-center',
                render: function (shipDate) {
                    if (moment(shipDate).format('L') == "01/01/0001") {
                        return `<span></span>`;
                    }
                    else {
                        return moment(shipDate).format('L')
                    }
                }
            },
            {
                orderable: false,
                targets: 3,
                data: "status",
                className: 'text-center',
                render: function (status) {
                    if (status == 0) {
                        return `<span class="badge badge-warning">Chờ xác nhận</span>`;
                    }
                    else if (status == 1) {
                        return `<span class="badge badge-info">Được xác nhận</span>`;
                    }
                    else if (status == 2) {
                        return `<span class="badge badge-secondary">Đang giao hàng</span>`;
                    }
                    else if (status == 3) {
                        return `<span class="badge badge-primary">Giao hàng thành công</span>`;
                    }
                    else if (status == 4) {
                        return `<span class="badge badge-success">Đơn hàng hoàn thành</span>`;
                    }
                    else if (status == 5) {
                        return `<span class="badge badge-danger">Đơn hàng bị hủy</span>`;
                    }
                }
            },
            {

                targets: 4,
                data: 'id',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `<div class='text-center'>
                                <a href="https://localhost:44325/OrderUser/Details/`+ row.id + `" class="btn btn-info m-r-5 text-white">Xem chi tiết</a>
                            </div>`;
                }
            },
        ],
    });

    $("#Search").on("click", function () {
        CateTable.ajax.reload();
    });


})(jQuery);