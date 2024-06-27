(function () {
    moment.locale("vi");
    var data2 = {};

    var CateTable = $('#data-table').DataTable({
        ajax: {
            url: "/Admin/User/GetAllUserByInput",
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
        //listAction: {
        //    ajaxFunction: "/Admin/Category/GetAllCategory",
        //    inputFilter: function () { }
        //},
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
                    return moment(createTime).format('L');
                }

            },
            {
                targets: 2,
                data: "updateTime",
                render: function (updateTime) {
                    if (moment(updateTime).format('L') == "01/01/0001") {
                        return `<span></span>`;
                    }
                    else {
                        return moment(updateTime).format('L')
                    }
                }
            },
            {
                targets: 3,
                data: "fullName"
            },
            {
                targets: 4,
                data: "role",
                render: function (role) {
                    if (role == 0) {
                        return `<span>Admin</span>`;
                    }
                    else {
                        return `<span>Khách hàng</span>`;
                    }
                }
            },
            {
                targets: 5,
                data: "phoneNumber"
            },
            {
                targets: 6,
                data: "active",
                render: function (status) {
                    if (status) {
                        return `<span class="badge badge-success">Đang hoạt động</span>`;
                    }
                    else {
                        return ` <span class="badge badge-danger">Dừng hoạt động</span>`;
                    }
                }
            },
            {

                targets: 7,
                data: 'id',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `<div class='text-center'>
                                <a href="User/Details/`+ row.id + `" class="btn btn-info m-r-5 text-white">Xem chi tiết</a>
                                <a href="User/Edit/`+ row.id + `" class="btn btn-warning m-r-5 text-white">Sửa</a>
                            </div>`;
                }
            },
        ],
    });

    $("#Search").on("click", function () {
        CateTable.ajax.reload();
    });

})(jQuery)