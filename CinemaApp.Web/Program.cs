using CinemaApp.Data.Context;
using CinemaApp.Services.Implementations;
using CinemaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data.Context;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

// ── Въвеждане на парола от конзолата ─────────────────────────────────────
Console.Write("MySQL парола › ");
string password = "";
ConsoleKeyInfo key;
while ((key = Console.ReadKey(intercept: true)).Key != ConsoleKey.Enter)
{
    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
        password = password[..^1];
    else if (key.Key != ConsoleKey.Backspace)
        password += key.KeyChar; Console.Write("*");
}
Console.WriteLine();

var builder = WebApplication.CreateBuilder(args);

// ── Razor Pages ───────────────────────────────────────────────────────────
builder.Services.AddRazorPages();

// ── Database ──────────────────────────────────────────────────────────────
string connectionString = $"Server=localhost;Database=cinema;User=root;Password={password};";

builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ── Services ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IActorService,   ActorService>();
builder.Services.AddScoped<IFilmService,    FilmService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IClientService,  ClientService>();
builder.Services.AddScoped<ITicketService,  TicketService>();

var app = builder.Build();

// ── Automatically creates the database ────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
