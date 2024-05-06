(function () {
    console.log("dfdsfsfaa");
    $('#data-table').DataTable({
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "info": "Đang hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
            "infoEmpty": "Đang hiển thị 0 đến 0 của 0 bản ghi",
            "paginate": {
                "next": "Tiếp",
                "previous": "Quay lại"
            },
            "search": "Nhập tên loại:",
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
                    return '<span>' + stt +'<span>';
                }
            },
        ],
    });
    //var listtd = $(".table-category").children("tbody").children("tr").children("td.sorting_1");
    //for (i = 0; i < listtd.length; i++) {
    //    var stt = i + 1;
    //    listtd[i].append(stt);
    //}
    
})(jQuery)