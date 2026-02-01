const API_BASE = "https://localhost:7226/api/MediaItems";

const $filterType = $("#filterType");
const $filterStatus = $("#filterStatus");
const $filterSearch = $("#filterSearch");
const $listBody = $("#listBody");

let wlCache = [];

function wlLoadItems() {
    return $.ajax({
        url: API_BASE,
        method: "GET"
    }).then(function (data) {
        wlCache = Array.isArray(data) ? data : [];
        return wlCache;
    });
}

function wlGetFilteredItems() {
    const type = $filterType.val();
    const status = $filterStatus.val();
    const search = $filterSearch.val().toLowerCase().trim();

    return wlCache
        .filter(it => {
            const itType = String(it.type).toLowerCase();
            const itStatus = String(it.status).toLowerCase();

            if (type !== "all" && itType !== String(type).toLowerCase()) return false;
            if (status !== "all" && itStatus !== String(status).toLowerCase()) return false;
            if (search && !String(it.title || "").toLowerCase().includes(search)) return false;
            return true;
        })
        .sort((a, b) => new Date(b.date) - new Date(a.date));
}

function wlRenderTable() {
    const items = wlGetFilteredItems();
    $listBody.html("");

    if (!items.length) {
        $listBody.html('<tr><td colspan="7" class="text-center text-muted">No items found</td></tr>');
        return;
    }

    items.forEach(it => {
        const typeLower = String(it.type).toLowerCase();
        const statusLower = String(it.status).toLowerCase();

        const typeBadgeClass = typeLower === "movie" ? "bg-success" : "bg-info";
        const statusBadgeClass =
            statusLower === "planned" ? "bg-secondary" :
                statusLower === "watching" ? "bg-warning text-dark" :
                    "bg-primary";

        $listBody.append(`
      <tr data-id="${it.id}">
        <td>${it.date || "-"}</td>
        <td>${it.title}</td>
        <td><span class="badge ${typeBadgeClass} text-uppercase">${it.type}</span></td>
        <td>${it.genre}</td>
        <td><span class="badge ${statusBadgeClass} text-capitalize">${it.status}</span></td>
        <td>${(it.rating != null && Number(it.rating) > 0) ? it.rating : "-"}</td>
        <td class="text-end">
          <div class="btn-group btn-group-sm">
            <button class="btn btn-outline-secondary btn-status" data-status="planned">Planned</button>
            <button class="btn btn-outline-secondary btn-status" data-status="watching">Watching</button>
            <button class="btn btn-outline-secondary btn-status" data-status="watched">Watched</button>
          </div>
          <button class="btn btn-sm btn-danger ms-1 btn-delete">Delete</button>
        </td>
      </tr>
    `);
    });
}

function wlUpdateStatus(id, newStatus) {
    $.ajax({
        url: `${API_BASE}/${id}/status`,
        method: "PUT",
        contentType: "application/json",
        data: JSON.stringify({ status: newStatus })
    })
        .done(function () {
            wlLoadItems().then(wlRenderTable);
        })
        .fail(function (jqXHR) {
            alert(jqXHR && jqXHR.responseText ? jqXHR.responseText : "Failed to update status.");
        });
}

function wlDeleteItem(id) {
    $.ajax({
        url: `${API_BASE}/${id}`,
        method: "DELETE"
    })
        .done(function () {
            wlLoadItems().then(wlRenderTable);
        })
        .fail(function (jqXHR) {
            alert(jqXHR && jqXHR.responseText ? jqXHR.responseText : "Failed to delete item.");
        });
}

$(function () {
    wlLoadItems().then(wlRenderTable);

    $filterType.on("change", wlRenderTable);
    $filterStatus.on("change", wlRenderTable);
    $filterSearch.on("input", wlRenderTable);

    $("#btnClearFilters").on("click", function () {
        $filterType.val("all");
        $filterStatus.val("all");
        $filterSearch.val("");
        wlRenderTable();
    });

    $listBody.on("click", ".btn-status", function () {
        const newStatus = $(this).data("status");
        const id = Number($(this).closest("tr").data("id"));
        wlUpdateStatus(id, newStatus);
    });

    $listBody.on("click", ".btn-delete", function () {
        const id = Number($(this).closest("tr").data("id"));
        if (confirm("Delete this item?")) {
            wlDeleteItem(id);
        }
    });
});
