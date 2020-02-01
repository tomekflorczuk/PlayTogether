// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function startTime() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    m = checkTime(m);
    s = checkTime(s);
    $("#current-time").html(h + ":" + m + ":" + s);
    var t = setTimeout(startTime, 500);
}

function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    };
    return i;
}

$(document).ready(function() {
    //Wyświetlnie form zmiany hasła
    $("#reset-password-button").click(function() {
        $("#reset-password-form").fadeIn();
    });
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
            type: "post",
            dataType: "json",
            success: function(msg) {
                $("#notification").show().html(msg);
                $("#notification").delay(1500).fadeOut("slow");
                location.reload(true);
            }
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
            type: "post",
            dataType: "json",
            success: function(msg) {
                $("#notification").show().html(msg);
                $("#notification").delay(1500).fadeOut("slow");
                location.reload(true);
            }
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
            type: "post",
            dataType: "json",
            success: function(msg) {
                $("#notification").show().html(msg);
                $("#notification").delay(1500).fadeOut("slow");
                location.reload(true);
            }
        });
    });
    //Przełączanie górnego panelu
    $("#top-panel-toogle").click(function() {
        $("#top-panel").toggle();
    });
    //Wybór miasta w górnym panelu
    $("#city-select").change(function(e) {
        e.preventDefault();
        var t = $("input[name='__RequestVerificationToken']").val();
        //Pobieranie id wybranego miasta
        var cityid = $("#top-city-select option:selected").val();

        $.ajax({
            url: "/Main?handler=CityChanged",
            type: "post",
            data: { 'cityid': cityid },
            dataType: "json",
            cache: false,
            success: function(msg) {
                $("#notification").show().html(msg);
                $("#notification").delay(1500).fadeOut("slow");
                location.reload(true);
                //("#select-place-list").options();
            },
            headers:
            {
                "RequestVerificationToken": t
            }
        });
    });
    //Przycisk przeładiowania strony
    $("#reload-button").click(function(e) {
        location.reload(true);
    });
    //Wyświetlenie menu usera
    $("#user-button").click(function() {
        $("#user-menu").fadeIn();
    });
    //Chowanie menu usera
    $(document).click(function(e) {
        if ($(e.target).closest("button").attr("id") !== "user-button" &&
            $(e.target).closest("div").attr("id") !== "user-menu") {
            $("#user-menu").fadeOut();
        }
    });
    //Wyświetlanie szczegółów usera
    $("#user-details-button").click(function() {
        $("#add-event-form").fadeOut();
        $("#user-details-form").fadeIn();
    });
    //Chowanie szczegółów usera
    $("#user-details-close-button").click(function() {
        $("#user-details-form").fadeOut();
    });
    //Zatwierdzenie szczegółów usera
    $("#user-details-confirm").click(function (e) {
        //Zapobiega wywołaniu domyślnej akcji
        e.preventDefault();
        var t = $("input[name='__RequestVerificationToken']").val();
        //Plik zdjęcia profilowego
        var file = $("#file-chooser").get(0).files[0];
        //Dane zawodnika
        var form = new FormData($("#post-user-details").get(0));
        form.append("picture", file);
        //Metoda ajax
        $.ajax({
            url: $(this).attr("formaction"),
            type: "post",
            data: form,
            dataType: "json",
            contentType: false,
            processData: false,
            success: function (msg) {
                $("#user-details-form").hide();
                $("#notification").show().html(msg);
                $("#notification").delay(3000).fadeOut("slow");
                location.reload(true);
            },
            headers: 
            {
                "RequestVerificationToken": t
            }
        });
});
    //Zatwierdzanie dodania nowego miejsca
    $("#confirm-add-place-button").click(function (e) {
        e.preventDefault();
        var t = $("input[name='__RequestVerificationToken']").val();

        $.ajax({
            url: $(this).attr("formaction"),
            type: "post",
            data: $("#post-add-place").serialize(),
            dataType: "json",
            success: function (msg) {
                if (msg === "") { 

                } else {
                    $("#add-place-form").hide();
                    $("#select-place-form").show();
                    $("#notification").show().html(msg);
                    $("#notification").delay(3000).fadeOut("slow");
                }
            },
            headers:
            {
                "RequestVerificationToken": t
            }
        });
    });
    //Zatwierdzenie wyboru miejsca
    $("#confirm-select-place-button").click(function(e) {
        e.preventDefault();
        var t = $("input[name='__RequestVerificationToken']").val();

        var placename = $("#select-place-list option:selected").text();
        var selectedplace = $("#post-select-place");
        selectedplace[0][0].value = placename;
        selectedplace = $("#post-select-place").serialize();

        $.ajax({
            url: $(this).attr("formaction"),
            type: "post",
            data: selectedplace,
            dataType: "json",
            success: function(msg) {
                if (msg === "") {
                } else {
                    $("#select-place-form").hide();
                    $("#add-event-form").show();
                    $("#notification").show().html(msg);
                    $("#notification").delay(3000).fadeOut("slow");
                }
            },
            headers:
            {
                "RequestVerificationToken": t
            }
        });
    });
    //Zatwierdzenie dodania wydarzenia
    $("#confirm-add-game-button").click(function(e) {
        e.preventDefault();
        var t = $("input[name='__RequestVerificationToken']").val();

        $.ajax({
            url: $(this).attr("formaction"),
            type: "post",
            data: $("#post-add-game").serialize(),
            dataType: "json",
            success: function(msg) {
                if (msg === "") {
                } else {
                    $("#add-event-form").hide();
                    $("#notification").show().html(msg);
                    $("#notification").delay(3000).fadeOut("slow");
                    location.reload(true);
                }
            },
            headers:
            {
                "RequestVerificationToken": t
            }
        });
    });
    //Wyświetlanie menu wyboru pliku obrazu
    $("#upload-picture-button").click(function(e) {
        $("#file-chooser").click();
    });
    //Wyświetlanie menu dodawania wydarzenia
    $("#add-event-button").click(function() {
        $("#user-details-form").fadeOut();
        $("#add-event-form").fadeIn();
    });
    //Wyświetlanie menu wyboru miejsca
    $("#place-search").click(function() {
        $("#select-place-form").fadeIn();
        $("#add-event-form").fadeOut();
    });
    //Zamykanie menu wyboru miejsca
    $("#select-place-close-button").click(function() {
        $("#select-place-form").fadeOut();
        $("#add-event-form").fadeIn();
    });
    //Wyświetlanie menu dodawania miejsca
    $("#add-place-button").click(function() {
        $("#add-place-form").fadeIn();
        $("#select-place-form").fadeOut();
    });
    //Zamykanie menu dodawania miejsca
    $("#add-place-close-button").click(function() {
        $("#select-place-form").fadeIn();
        $("#add-place-form").fadeOut();
    });
    //Zamykanie menu dodawania wydarzenia
    $("#add-event-close-button").click(function() {
        $("#add-event-form").fadeOut();
    });
    //Zapisywanie się do wydarzenia
    $(".sign-up-game-button").click(function(e) {
        e.preventDefault();
        var t = $("input[name='__RequestVerificationToken']").val();
        var gameid = $(e.target)[0].value;

        $.ajax({
            url: $(this).attr("formaction"),
            type: "post",
            data: { 'gameid': gameid },
            dataType: "json",
            cache: false,
            success: function (msg) {
                $("#notification").show().html(msg);
                $("#notification").delay(1500).fadeOut("slow");
                location.reload(true);
            },
            headers:
            {
                "RequestVerificationToken": t
            }
        });
    });
    //Wypisanie się z wydarzenia
    $(".sign-out-game-button").click(function(e) {
        e.preventDefault();
        var t = $("input[name='__RequestVerificationToken']").val();
        var gameid = $(e.target)[0].value;

        $.ajax({
            url: $(this).attr("formaction"),
            type: "post",
            data: { 'gameid': gameid },
            dataType: "json",
            cache: false,
            success: function (msg) {
                $("#notification").show().html(msg);
                $("#notification").delay(1500).fadeOut("slow");
                location.reload(true);
            },
            headers:
            {
                "RequestVerificationToken": t
            }
        });
    });
    //Otwarcie informacji o wydarzeniu
    $(".game-details-button").click(function() {
        $(".game-detail-form").show();
    });
    //Zamknięcie informacji o wydarzeniu
    $(".game-details-close-button").click(function() {
        $(".game-detail-form").hide();
    });
    //Ustawienie koloru wydarzenia
    $(".game-component").each(function (index) {
        if ($(this).find(".upcoming-game-type")[0].value === "1") {
            $(this).css("border", "solid greenyellow 0.2rem");
            $(this).find(".form-header").css("background-color", "greenyellow");
        } else if ($(this).find(".upcoming-game-type")[0].value === "2") {
            $(this).css("border", "solid cyan 0.2rem");
            $(this).find(".form-header").css("background-color", "cyan");
        } else if ($(this).find(".upcoming-game-type")[0].value === "3"){
            $(this).css("border", "solid yellow 0.2rem");
            $(this).find(".form-header").css("background-color", "yellow");
        }
    });
});