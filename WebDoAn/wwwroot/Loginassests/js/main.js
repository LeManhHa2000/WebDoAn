(function($) {

    "use strict";
    document.getElementById("PhoneNumber").addEventListener("input", function () {
        var valueChange = funcChanePhoneNumber();
        $("#FormAccess").find('input[name=PhoneNumber]').val(valueChange);
    });
    function funcChanePhoneNumber() {
        var valueChange = $("#FormAccess").find('input[name=PhoneNumber]').val().replace(/[^0-9]/g, '');
        return valueChange;
    }
    $("input#Password").on({
        keydown: function (e) {
            if (e.which === 32)
                return false;
        },
        change: function () {
            this.value = this.value.replace(/\s/g, "");
        }
    });

    $("#checkshow").on('change', function () {
        $(this).prop("checked") ? $("#Password").prop("type", "text") : $("#Password").prop("type", "password");    
    });

})(jQuery);
