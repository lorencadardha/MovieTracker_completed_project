const API_BASE = "https://localhost:7226/api/MediaItems";

function fmt(num, digits = 1) {
    return Number.isFinite(num) ? num.toFixed(digits) : "0.0";
}

function loadAllItems() {
    return $.ajax({
        url: API_BASE,
        method: "GET"
    }).then(function (data) {
        return Array.isArray(data) ? data : [];
    });
}

function renderStats(items) {
    const total = items.length;

    const movies = items.filter(i => String(i.type).toLowerCase() === "movie").length;
    const series = items.filter(i => String(i.type).toLowerCase() === "series").length;

    const planned = items.filter(i => String(i.status).toLowerCase() === "planned").length;
    const watching = items.filter(i => String(i.status).toLowerCase() === "watching").length;
    const watched = items.filter(i => String(i.status).toLowerCase() === "watched").length;

    const rated = items.filter(i =>
        String(i.status).toLowerCase() === "watched" &&
        i.rating != null &&
        Number(i.rating) > 0
    );

    const avgRating = rated.length
        ? rated.reduce((sum, it) => sum + Number(it.rating), 0) / rated.length
        : 0;

    $("#statTotal").text(total);
    $("#statMovies").text(movies);
    $("#statSeries").text(series);
    $("#statPlanned").text(planned);
    $("#statWatching").text(watching);
    $("#statWatched").text(watched);
    $("#statRating").text(fmt(avgRating, 1));
}

function renderRecent(items) {
    const recent = items
        .slice()
        .sort((a, b) => new Date(b.date) - new Date(a.date))
        .slice(0, 5);

    const $tbody = $("#recentBody");
    $tbody.html("");

    if (!recent.length) {
        $tbody.html('<tr><td colspan="6" class="text-center text-muted">No items yet</td></tr>');
        return;
    }

    recent.forEach(it => {
        const typeLower = String(it.type).toLowerCase();
        const statusLower = String(it.status).toLowerCase();

        const typeBadgeClass = typeLower === "movie" ? "bg-success" : "bg-info";
        const statusBadgeClass =
            statusLower === "planned" ? "bg-secondary" :
                statusLower === "watching" ? "bg-warning text-dark" :
                    "bg-primary";

        $tbody.append(`
      <tr>
        <td>${it.date || "-"}</td>
        <td><span class="badge ${typeBadgeClass} text-uppercase">${it.type}</span></td>
        <td>${it.title}</td>
        <td>${it.genre}</td>
        <td><span class="badge ${statusBadgeClass} text-capitalize">${it.status}</span></td>
        <td>${(it.rating != null && Number(it.rating) > 0) ? it.rating : "-"}</td>
      </tr>
    `);
    });
}

function renderTopRated(items) {
    const top = items
        .filter(i => String(i.status).toLowerCase() === "watched" && i.rating != null && Number(i.rating) > 0)
        .slice()
        .sort((a, b) => Number(b.rating) - Number(a.rating))
        .slice(0, 3);

    const $list = $("#topRatedList");
    $list.html("");

    if (!top.length) {
        $list.html('<li class="list-group-item text-center text-muted">No rated items yet</li>');
        return;
    }

    top.forEach(it => {
        $list.append(`
      <li class="list-group-item d-flex justify-content-between align-items-center">
        <div>
          <div class="fw-semibold">${it.title}</div>
          <small class="text-muted">${it.type} • ${it.genre}</small>
        </div>
        <span class="badge bg-warning text-dark fs-6">${it.rating}</span>
      </li>
    `);
    });
}

$(function () {
    loadAllItems()
        .then(function (items) {
            renderStats(items);
            renderRecent(items);
            renderTopRated(items);
        })
        .fail(function () {
            renderStats([]);
            renderRecent([]);
            renderTopRated([]);
        });
});
