using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Data.Models;
using MovieCatalog.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//from user-secrets
var connectionString = builder.Configuration.GetConnectionString("SQL");

builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IRepository<Film>>(serviceProvider =>
    new FilmRepository(serviceProvider.GetRequiredService<MovieDbContext>()));
builder.Services.AddScoped<IRepository<Category>>(serviceProvider =>
    new CategoryRepository(serviceProvider.GetRequiredService<MovieDbContext>()));
builder.Services.AddScoped<IRepository<FilmCategory>>(serviceProvider =>
    new FilmCategoryRepository(serviceProvider.GetRequiredService<MovieDbContext>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();