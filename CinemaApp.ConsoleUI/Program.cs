using CinemaApp.ConsoleUI;
using CinemaApp.ConsoleUI.Menus;
using CinemaApp.Data.Context;
using CinemaApp.Services.Implementations;
using CinemaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CinemaApp.Data.Context;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

// ── Въвеждане на парола ───────────────────────────────────────────────────
Console.ForegroundColor = ConsoleColor.DarkYellow;
Console.WriteLine("  ╔══════════════════════════════════════════╗");
Console.WriteLine("  ║         CINEMA APP — СВЪРЗВАНЕ           ║");
Console.WriteLine("  ╠══════════════════════════════════════════╣");
Console.Write("  ║  MySQL парола › ");
Console.ResetColor();

string password = "";
ConsoleKeyInfo key;
while ((key = Console.ReadKey(intercept: true)).Key != ConsoleKey.Enter)
{
    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
    {
        password = password[..^1];
        Console.Write("\b \b");
    }
    else if (key.Key != ConsoleKey.Backspace)
    {
        password += key.KeyChar;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("*");
        Console.ResetColor();
    }
}

Console.ForegroundColor = ConsoleColor.DarkYellow;
Console.WriteLine();
Console.WriteLine("  ╚══════════════════════════════════════════╝");
Console.ResetColor();
Console.WriteLine();

// ── Dependency Injection ──────────────────────────────────────────────────
var services = new ServiceCollection();

string connectionString = $"Server=localhost;Database=cinema;User=root;Password={password};";

services.AddDbContext<CinemaDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

services.AddScoped<IActorService,   ActorService>();
services.AddScoped<IFilmService,    FilmService>();
services.AddScoped<IProjectService, ProjectService>();
services.AddScoped<IClientService,  ClientService>();
services.AddScoped<ITicketService,  TicketService>();

services.AddScoped<ActorsMenu>();
services.AddScoped<FilmsMenu>();
services.AddScoped<ProjectsMenu>();
services.AddScoped<ClientsMenu>();
services.AddScoped<TicketsMenu>();

var provider = services.BuildServiceProvider();

// ── Automatically creates the database ────────────────────────────────────
using (var scope = provider.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
    db.Database.EnsureCreated();
}

// ── Main loop ─────────────────────────────────────────────────────────────
bool appRunning = true;

while (appRunning)
{
    ConsoleHelper.PrintLogo();
    ConsoleHelper.PrintTitle("ГЛАВНО МЕНЮ");
    Console.WriteLine();
    ConsoleHelper.PrintMenuOption(1, "Актьори");
    ConsoleHelper.PrintMenuOption(2, "Филми");
    ConsoleHelper.PrintMenuOption(3, "Прожекции");
    ConsoleHelper.PrintMenuOption(4, "Клиенти");
    ConsoleHelper.PrintMenuOption(5, "Билети");
    ConsoleHelper.PrintMenuOption(0, "Изход");
    ConsoleHelper.PrintMenuFooter();

    string? choice = ConsoleHelper.ReadChoice();

    switch (choice)
    {
        case "1": await provider.GetRequiredService<ActorsMenu>().ShowAsync();   break;
        case "2": await provider.GetRequiredService<FilmsMenu>().ShowAsync();    break;
        case "3": await provider.GetRequiredService<ProjectsMenu>().ShowAsync(); break;
        case "4": await provider.GetRequiredService<ClientsMenu>().ShowAsync();  break;
        case "5": await provider.GetRequiredService<TicketsMenu>().ShowAsync();  break;
        case "0":
            appRunning = false;
            Console.Clear();
            ConsoleHelper.PrintLogo();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  Благодарим, че използвате Cinema App!");
            Console.WriteLine();
            Console.ResetColor();
            break;
        default:
            ConsoleHelper.PrintWarning("Невалиден избор.");
            ConsoleHelper.Pause();
            break;
    }
}
