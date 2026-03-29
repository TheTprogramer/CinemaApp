using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;

namespace CinemaApp.ConsoleUI.Menus
{
    public class ActorsMenu
    {
        private readonly IActorService _actorService;
        public ActorsMenu(IActorService actorService) => _actorService = actorService;

        public async Task ShowAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                ConsoleHelper.PrintLogo();
                ConsoleHelper.PrintTitle("УПРАВЛЕНИЕ НА АКТЬОРИ");
                Console.WriteLine();
                ConsoleHelper.PrintMenuOption(1, "Всички актьори");
                ConsoleHelper.PrintMenuOption(2, "Търсене по ID");
                ConsoleHelper.PrintMenuOption(3, "Добавяне на актьор");
                ConsoleHelper.PrintMenuOption(4, "Редактиране на актьор");
                ConsoleHelper.PrintMenuOption(5, "Изтриване на актьор");
                ConsoleHelper.PrintMenuOption(6, "Актьори над определена възраст");
                ConsoleHelper.PrintMenuOption(7, "Актьори с техните филми");
                ConsoleHelper.PrintMenuOption(0, "Обратно");
                ConsoleHelper.PrintMenuFooter();

                switch (ConsoleHelper.ReadChoice())
                {
                    case "1": await ListAllAsync();          break;
                    case "2": await FindByIdAsync();         break;
                    case "3": await AddAsync();              break;
                    case "4": await UpdateAsync();           break;
                    case "5": await DeleteAsync();           break;
                    case "6": await ListByMinAgeAsync();     break;
                    case "7": await ListWithFilmsAsync();    break;
                    case "0": running = false;               break;
                    default: ConsoleHelper.PrintWarning("Невалиден избор."); ConsoleHelper.Pause(); break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ВСИЧКИ АКТЬОРИ");
            Console.WriteLine();
            PrintTable(await _actorService.GetAllAsync());
            ConsoleHelper.Pause();
        }

        private async Task FindByIdAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ТЪРСЕНЕ ПО ID");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            var actor = await _actorService.GetByIdAsync(id);
            Console.WriteLine();
            if (actor == null) ConsoleHelper.PrintWarning("Актьорът не е намерен.");
            else
            {
                ConsoleHelper.PrintInfo($"ID: {actor.ActorId}");
                ConsoleHelper.PrintInfo($"Ime: {actor.FirstName} {actor.LastName}");
                ConsoleHelper.PrintInfo($"Възраст: {actor.Age}");
            }
            ConsoleHelper.Pause();
        }

        private async Task AddAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ДОБАВЯНЕ НА АКТЬОР");
            Console.WriteLine();
            var actor = new Actor
            {
                FirstName = ConsoleHelper.ReadNonEmptyString("Собствено ime"),
                LastName  = ConsoleHelper.ReadNonEmptyString("Фамилно ime"),
                Age       = ConsoleHelper.ReadInt("Възраст"),
            };
            try
            {
                await _actorService.AddAsync(actor);
                ConsoleHelper.PrintSuccess($"Актьорът {actor.FirstName} {actor.LastName} е добавен!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task UpdateAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("РЕДАКТИРАНЕ НА АКТЬОР");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            var actor = await _actorService.GetByIdAsync(id);
            if (actor == null) { ConsoleHelper.PrintWarning("Не е намерен."); ConsoleHelper.Pause(); return; }

            Console.WriteLine();
            ConsoleHelper.PrintInfo($"Текущо: {actor.FirstName} {actor.LastName} | Възраст: {actor.Age}");
            Console.WriteLine();

            actor.FirstName = ConsoleHelper.ReadNonEmptyString($"Собствено [{actor.FirstName}]");
            actor.LastName  = ConsoleHelper.ReadNonEmptyString($"Фамилно [{actor.LastName}]");
            actor.Age       = ConsoleHelper.ReadInt($"Възраст [{actor.Age}]");
            try
            {
                await _actorService.UpdateAsync(actor);
                ConsoleHelper.PrintSuccess("Данните са актуализирани!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task DeleteAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ИЗТРИВАНЕ НА АКТЬОР");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            Console.WriteLine();
            ConsoleHelper.PrintWarning("Потвърждавате ли изтриването? (да/не)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  › ");
            Console.ResetColor();
            if (Console.ReadLine()?.ToLower() != "да") { ConsoleHelper.PrintWarning("Отменено."); ConsoleHelper.Pause(); return; }
            try
            {
                await _actorService.DeleteAsync(id);
                ConsoleHelper.PrintSuccess("Актьорът е изтрит!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task ListByMinAgeAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("АКТЬОРИ ПО МИНИМАЛНА ВЪЗРАСТ");
            Console.WriteLine();
            int minAge = ConsoleHelper.ReadInt("Минимална възраст");
            Console.WriteLine();
            PrintTable(await _actorService.GetByMinAgeAsync(minAge));
            ConsoleHelper.Pause();
        }

        private async Task ListWithFilmsAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("АКТЬОРИ С ФИЛМИ");
            Console.WriteLine();
            var actors = await _actorService.GetActorsWithFilmsAsync();
            foreach (var a in actors)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  ★  {a.FirstName} {a.LastName}  ({a.Age} г.)");
                Console.ResetColor();
                if (!a.Films.Any())
                    ConsoleHelper.PrintWarning("    Няма филми.");
                else
                    foreach (var f in a.Films)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("       › ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(f.FilmName.PadRight(30));
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"[{f.FilmGenre}]  {f.FilmTime} мин.");
                        Console.ResetColor();
                    }
                ConsoleHelper.PrintThinSeparator();
            }
            ConsoleHelper.Pause();
        }

        private static void PrintTable(IEnumerable<Actor> actors)
        {
            ConsoleHelper.PrintTableHeader("ID  ", "Собствено       ", "Фамилно         ", "Възраст");
            foreach (var a in actors)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("  ║  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"{a.ActorId,-4}  ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{a.FirstName,-16}  {a.LastName,-16}  ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{a.Age} г.");
                Console.ResetColor();
            }
            if (!actors.Any()) ConsoleHelper.PrintWarning("  Няма актьори.");
        }
    }
}
