(function () {
    moment.locale("vi");
    var data2 = {};
    //$.ajax({
    //    type: "GET",
    //    url: "/Admin/Category/GetAlltestCategory",
    //    dataType: 'json',
    //    data: data2,
    //    success: function (data) {
    //        console.log("assadsa",data);
    //    }
    //});
     

    var CateTable = $('#data-table').DataTable({
        ajax: {
            url: "/Admin/Order/GetAllOrder",
            data: () => {
                var inputSearch = $("#SearchInput").val();
                var selectstatus = $("#SelectStatus").val();
                if (inputSearch == "") {
                    return data2 = {
                        code: "all",
                        status: selectstatus
                    };
                }
                else {
                    return data2 = {
                        code: inputSearch,
                        status: selectstatus
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

            },
            {
                targets: 2,
                data: "code"
            },
            {
                targets: 3,
                data: "updateTime",
                render: function (updateTime) {
                    if (updateTime == "1/1/0001") {
                        return `<span></span>`;
                    }
                    else {
                        return updateTime;
                    }
                }
            },
            {
                targets: 4,
                data: "nameUser"
            },
            {
                targets: 5,
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

                targets: 6,
                data: 'id',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    if (row.status == 5 || row.status == 4) {
                        return `<div class='text-center'>
                                <a href="Order/Details/`+ row.id + `" class="btn btn-info m-r-5 text-white">Xem chi tiết</a>
                            </div>`;
                    }
                    else {
                        return `<div class='text-center'>
                                <a href="Order/Details/`+ row.id + `" class="btn btn-info m-r-5 text-white">Xem chi tiết</a>
                                <a href="Order/Edit/`+ row.id + `" class="btn btn-warning m-r-5 text-white">Cập nhật</a>
                            </div>`;
                    }
                    
                }
            },
        ],
    });

    $("#Search").on("click", function () {
        CateTable.ajax.reload();
    });

})(jQuery)