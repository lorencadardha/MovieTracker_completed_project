const API_BASE = "https://localhost:7226/api/MediaItems";

// Show alert message for success or error
function amShowAlert($el, msg) {
    $el.text(msg).removeClass("d-none");
    setTimeout(() => $el.addClass("d-none"), 2500);
}

// Small helper to extract error text from API
function amGetErrorMessage(jqXHR) {
    if (jqXHR && jqXHR.responseText) return jqXHR.responseText;
    return "Something went wrong. Please try again.";
}

const $form = $("#movieForm");
const $type = $("#type");
const $title = $("#title");
const $genre = $("#genre");
const $status = $("#status");
const $rating = $("#rating");
const $date = $("#date");
const $notes = $("#notes");

const $ok = $("#alertSuccess");  // success alert box
const $err = $("#alertError");   // error alert box

$(function () {
    const today = new Date().toISOString().split("T")[0];
    $date.val(today);

    // Enable rating ONLY when status = "watched" (case-insensitive)
    $status.on("change", function () {
        if (String(this.value).toLowerCase() === "watched") {
            $rating.prop("disabled", false);
        } else {
            $rating.prop("disabled", true).val("");
        }
    });

    $form.on("submit", function (e) {
        e.preventDefault();

        if (
            !$type.val() ||
            !$title.val().trim() ||
            !$genre.val() ||
            !$status.val() ||
            !$date.val()
        ) {
            amShowAlert($err, "Please fill all required fields.");
            return;
        }

        const dto = {
            type: $type.val(),
            title: $title.val().trim(),
            genre: $genre.val(),
            status: $status.val(),
            rating: $rating.val() ? parseInt($rating.val(), 10) : null,
            date: $date.val(),
            notes: $notes.val().trim()
        };

        // normalize status ONLY for validation, without changing your original fields
        const statusLower = String(dto.status).toLowerCase();

        // If not watched, don't send rating (prevents rating=0 or wrong values)
        if (statusLower !== "watched") dto.rating = null;

        if (statusLower === "watched") {
            if (dto.rating == null || dto.rating < 1 || dto.rating > 10) {
                amShowAlert($err, "Rating must be between 1 and 10 for watched items.");
                return;
            }
        }

        $.ajax({
            url: API_BASE,
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(dto)
        })
            .done(function () {
                amShowAlert($ok, "Saved successfully!");
                $form[0].reset();
                $rating.prop("disabled", true);
                $date.val(today);
            })
            .fail(function (jqXHR) {
                amShowAlert($err, amGetErrorMessage(jqXHR));
            });
    });

    $("#btnReset").on("click", function () {
        $form[0].reset();
        $rating.prop("disabled", true);
        $date.val(today);
        $form.removeClass("was-validated");
    });
});
