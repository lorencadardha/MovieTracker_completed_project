# Movie & Series Tracker
A full-stack app for tracking movies and TV series you've watched, are
watching, or plan to watch — with ratings, genres, and notes.
## Stack
- **Backend:** ASP.NET Core Web API (C#) with Entity Framework Core
- **Frontend:** Static HTML pages styled with Bootstrap 5, using jQuery to call the API
- **Data access:** `AppDbContext` (EF Core) — `MediaItems` table
## Features
- Add movies or series with type, title, genre, status, and notes
- Rate watched items on a 1–10 scale
- Track status per item: Planned, Watching, or Watched
- Dashboard overview: total items, movie/series split, average rating, status breakdown, recent additions, top-rated list
- Watchlist page with filtering by type, status, and title search
- Full CRUD via REST API
## Business Rules
- `Rating` is only required when `Status` is `"watched"`.
- Clearing an item's `"watched"` status also clears its rating (service-layer logic, not yet exposed as its own route).
- Items are returned ordered by date, most recent first.

