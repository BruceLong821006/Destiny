$(function () {
    $("#btnSubmit").click(function () {
        var num = $("#txtNum").val();
        $("#spResult").text("");
        $.ajax({
            type: "post",
            url: "/Account/Submit",
            data: { num: num },
            datatype: "json",
            success: function (data) {
                if (data.Status == "Success")
                    $("#spResult").text(data.Num);
            }
        });

    });

});