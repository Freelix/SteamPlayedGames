$(document).ready(function () {
    // If we click on a green check, we change the status to "Finished"
    $(".greenCheck").on("click", function () {
        var id = this.id;
        $(function () {
            $.ajax({
                type: 'POST',
                url: 'MainPage.aspx/SaveStatus', // Call the function here
                data: JSON.stringify({ strId: id }), // Put the parameters here
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    // response.d is used to retrieve the result object
                    $("#green_" + response.d).css("opacity", "1.0");

                    $("#red_" + response.d).css("opacity", "0.1");

                    /*$("#red_" + response.d).on("mouseover", function () {
                        $(this).css("opacity", "1.0"); 
                    });

                    $("#red_" + response.d).on("mouseleave", function () {
                        $(this).css("opacity", "0.1");
                    });*/
                }
            });
        });
    });

    // If we click on a red X, we change the status to "NotPlayed"
    $(".redX").on("click", function () {
        var id = this.id;
        $(function () {
            $.ajax({
                type: 'POST',
                url: 'MainPage.aspx/SaveStatus',
                data: JSON.stringify({ strId: id }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    // Notice that msg.d is used to retrieve the result object
                    $("#red_" + response.d).css("opacity", "1.0");

                    $("#green_" + response.d).css("opacity", "0.1");

                    /*$("#green_" + response.d).on("mouseover", function () {
                        $(this).css("opacity", "1.0");
                    });

                    $("#green_" + response.d).on("mouseleave", function () {
                        $(this).css("opacity", "0.1");
                    });*/
                }
            });
        });
    });
});