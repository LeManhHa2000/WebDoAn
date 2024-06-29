(function () {
    
    // setup 
    $.ajax({
        type: "POST",
        url: "/Admin/Home/GetSumTotalOrder",
        data: "",
        contextType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessResult,
        error: OnError
    });

    function OnSuccessResult(dataArray) {
        var _data = dataArray;
        var _chartData = _data[0];

        // setup 
        const data = {
            labels: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
            datasets: [{
                label: 'Thu nhập',
                data: _chartData,
                backgroundColor: [
                    'rgba(54, 162, 235, 0.2)'
                ],
                borderColor: [
                    'rgba(54, 162, 235, 0.2)'
                ],
                borderWidth: 1
            }]
        };

        // config 
        const config = {
            type: 'bar',
            data,
            options: {
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            // forces step size to be 50 units
                            stepSize: 1000000
                        }
                    }
                }
            }
        };

        // render init block
        const myChart = new Chart(
            document.getElementById('myChart'),
            config
        );

        // Instantly assign Chart.js version
        const chartVersion = document.getElementById('chartVersion');
    }

    function OnError(data) {

    }
    $("#btn-test").on("click", function () {
        
    })
})(jQuery)