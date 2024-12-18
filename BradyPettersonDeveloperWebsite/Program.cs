using BradyPettersonDeveloperWebsite.Components;
using BradyPettersonDeveloperWebsite.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Retrieve the connection string from configuration (Azure Connection Strings)
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = "Host=autorack.proxy.rlwy.net;Database=railway;Username=postgres;Password=IleihbkJeFYVgwiyrKslhdHgjhwQXoFW;Port=38989;CommandTimeout=600";

// Register AppDbContext with PostgreSQL provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddTransient<DatabaseWarmUpService>();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
