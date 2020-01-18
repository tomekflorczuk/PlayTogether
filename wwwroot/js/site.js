// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$(document).ready(function() {
    //Zmiana koloru oraz id wybranego sportu
    $("#football-button").click(function(e) {
        $("#football-button").css("background-color", "crimson");
        $("#football-button").css("border", "solid darkred 0.2rem");
        $("#basketball-button").css("background", "transparent");
        $("#basketball-button").css("border", "0");
        $("#volleyball-button").css("background", "transparent");
        $("#volleyball-button").css("border", "0");

        e.preventDefault();

        var t = $("input[name='__RequestVerificationToken']").val();

        $.ajax({
            url: $(this).attr("formaction"),
            headers:
            {
                "RequestVerificationToken": t
            },
            type: "post"
        });
    });
    $("#basketball-button").click(function(e) {
        $("#basketball-button").css("background-color", "crimson");
        $("#basketball-button").css("border", "solid darkred 0.2rem");
        $("#football-button").css("background", "transparent");
        $("#football-button").css("border", "0");
        $("#volleyball-button").css("background", "transparent");
        $("#volleyball-button").css("border", "0");

        e.preventDefault();

        var t = $("input[name='__RequestVerificationToken']").val();

        $.ajax({
            url: $(this).attr("formaction"),
            headers:
            {
                "RequestVerificationToken": t
            },
            type: "post"
        });
    });
    $("#volleyball-button").click(function(e) {
        $("#volleyball-button").css("background-color", "crimson");
        $("#volleyball-button").css("border", "solid darkred 0.2rem");
        $("#basketball-button").css("background", "transparent");
        $("#basketball-button").css("border", "0");
        $("#football-button").css("background", "transparent");
        $("#football-button").css("border", "0");

        e.preventDefault();

        var t = $("input[name='__RequestVerificationToken']").val();

        $.ajax({
            url: $(this).attr("formaction"),
            headers:
            {
                "RequestVerificationToken": t
            },
            type: "post"
        });
    });
    //Wyświetlenie menu usera
    $("#user-button").click(function() {
        $("#user-menu").show();
    });
    //Chowanie menu usera
    $(document).click(function(e) {
        if ($(e.target).closest("button").attr("id") !== "user-button" &&
            $(e.target).closest("div").attr("id") !== "user-menu") {
            $("#user-menu").hide();
        }
    });
    //Wyświetlanie szczegółów usera
    $("#user-details-button").click(function() {
        $("#add-event-form").hide();
        $("#user-details-form").show();
    });
    //Chowanie szczegółów usera
    $("#user-details-close-button").click(function() {
        $("#user-details-form").hide();
    });
    //Wyświetlanie menu dodawania wydarzenia
    $("#add-event-button").click(function() {
        $("#user-details-form").hide();
        $("#add-event-form").show();
    });
    //Zamykanie menu dodawania wydarzenia
    $("#add-event-close-button").click(function() {
        $("#add-event-form").hide();
    });
});