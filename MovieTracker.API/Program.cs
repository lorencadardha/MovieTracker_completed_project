
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MovieTracker.API.Models;
using MovieTracker.API.Data;
using MovieTracker.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the DbContext with MySQL configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Register both services used by the project
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMediaItemsService, MediaItemsService>();

// Add Swagger support for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieTracker API", Version = "v1" });
});

// Allow the app to serve static files like HTML, JS, and CSS from the wwwroot folder
builder.Services.AddDirectoryBrowser();  // Allow browsing static files

var app = builder.Build();

// Enable Swagger UI for API documentation
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieTracker API v1"));
}

// Serve static files from wwwroot
app.UseStaticFiles();

// Enabling CORS (Cross-Origin Resource Sharing) for the frontend to communicate with the API
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

// Use routing and endpoints for controllers
app.MapControllers();

// Start the application
app.Run();