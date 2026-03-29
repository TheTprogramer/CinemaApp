using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;

namespace CinemaApp.ConsoleUI.Menus
{
    public class FilmsMenu
    {
        private readonly IFilmService  _filmService;
        private readonly IActorService _actorService;

        public FilmsMenu(IFilmService filmService, IActorService actorService)
        { _filmService = filmService; _actorService = actorService; }

        public async Task ShowAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                ConsoleHelper.PrintLogo();
                ConsoleHelper.PrintTitle("УПРАВЛЕНИЕ НА ФИЛМИ");
                Console.WriteLine();
                ConsoleHelper.PrintMenuOption(1,  "Всички филми");
                ConsoleHelper.PrintMenuOption(2,  "Търсене по ID");
                ConsoleHelper.PrintMenuOption(3,  "Добавяне на филм");
                ConsoleHelper.PrintMenuOption(4,  "Редактиране на филм");
                ConsoleHelper.PrintMenuOption(5,  "Изтриване на филм");
                ConsoleHelper.PrintMenuOption(6,  "Филтриране по жанр");
                ConsoleHelper.PrintMenuOption(7,  "Филми над N минути");
                ConsoleHelper.PrintMenuOption(8,  "Най-кратките 3 филма");
                ConsoleHelper.PrintMenuOption(9,  "Най-дългият филм");
                ConsoleHelper.PrintMenuOption(10, "Филми с актьори и прожекции");
                ConsoleHelper.PrintMenuOption(0,  "Обратно");
                ConsoleHelper.PrintMenuFooter();

                switch (ConsoleHelper.ReadChoice())
                {
                    case "1":  await ListAllAsync();          break;
                    case "2":  await FindByIdAsync();         break;
                    case "3":  await AddAsync();              break;
                    case "4":  await UpdateAsync();           break;
                    case "5":  await DeleteAsync();           break;
                    case "6":  await FilterByGenreAsync();    break;
                    case "7":  await FilterByDurationAsync(); break;
                    case "8":  await ListShortestAsync();     break;
                    case "9":  await ShowLongestAsync();      break;
                    case "10": await ListWithDetailsAsync();  break;
                    case "0":  running = false;               break;
                    default: ConsoleHelper.PrintWarning("Невалиден избор."); ConsoleHelper.Pause(); break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ВСИЧКИ ФИЛМИ");
            Console.WriteLine();
            PrintTable(await _filmService.GetAllAsync());
            ConsoleHelper.Pause();
        }

        private async Task FindByIdAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ТЪРСЕНЕ НА ФИЛМ ПО ID");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            var film = await _filmService.GetByIdAsync(id);
            Console.WriteLine();
            if (film == null) ConsoleHelper.PrintWarning("Филмът не е намерен.");
            else
            {
                ConsoleHelper.PrintInfo($"ID: {film.FilmId}");
                ConsoleHelper.PrintInfo($"Заглавие: {film.FilmName}");
                ConsoleHelper.PrintInfo($"Жанр: {film.FilmGenre}");
                ConsoleHelper.PrintInfo($"Времетраене: {film.FilmTime} мин.");
                ConsoleHelper.PrintInfo($"Актьор: {film.Actor?.FirstName} {film.Actor?.LastName}");
            }
            ConsoleHelper.Pause();
        }

        private async Task AddAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ДОБАВЯНЕ НА ФИЛМ");
            Console.WriteLine();
            var actors = (await _actorService.GetAllAsync()).ToList();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  Налични актьори:");
            foreach (var a in actors)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("    › ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[{a.ActorId}] {a.FirstName} {a.LastName}");
            }
            Console.ResetColor();
            Console.WriteLine();
            var film = new Film
            {
                FilmName  = ConsoleHelper.ReadNonEmptyString("Заглавие"),
                FilmGenre = ConsoleHelper.ReadNonEmptyString("Жанр"),
                FilmTime  = ConsoleHelper.ReadDouble("Времетраене (мин.)"),
                ActorId   = ConsoleHelper.ReadInt("ID на актьора"),
            };
            try { await _filmService.AddAsync(film); ConsoleHelper.PrintSuccess($"Филмът '{film.FilmName}' е добавен!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task UpdateAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("РЕДАКТИРАНЕ НА ФИЛМ");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            var film = await _filmService.GetByIdAsync(id);
            if (film == null) { ConsoleHelper.PrintWarning("Не е намерен."); ConsoleHelper.Pause(); return; }
            Console.WriteLine();
            ConsoleHelper.PrintInfo($"Текущо: {film.FilmName} | {film.FilmGenre} | {film.FilmTime} мин.");
            Console.WriteLine();
            film.FilmName  = ConsoleHelper.ReadNonEmptyString($"Заглавие [{film.FilmName}]");
            film.FilmGenre = ConsoleHelper.ReadNonEmptyString($"Жанр [{film.FilmGenre}]");
            film.FilmTime  = ConsoleHelper.ReadDouble($"Времетраене [{film.FilmTime}]");
            try { await _filmService.UpdateAsync(film); ConsoleHelper.PrintSuccess("Актуализирано!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task DeleteAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ИЗТРИВАНЕ НА ФИЛМ");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            Console.WriteLine();
            ConsoleHelper.PrintWarning("Потвърждавате ли? (да/не)");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("  › "); Console.ResetColor();
            if (Console.ReadLine()?.ToLower() != "да") { ConsoleHelper.PrintWarning("Отменено."); ConsoleHelper.Pause(); return; }
            try { await _filmService.DeleteAsync(id); ConsoleHelper.PrintSuccess("Изтрит!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task FilterByGenreAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ФИЛТРИРАНЕ ПО ЖАНР");
            Console.WriteLine();
            string genre = ConsoleHelper.ReadNonEmptyString("Жанр");
            Console.WriteLine();
            PrintTable(await _filmService.GetByGenreAsync(genre));
            ConsoleHelper.Pause();
        }

        private async Task FilterByDurationAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ФИЛМИ НАД N МИНУТИ");
            Console.WriteLine();
            double min = ConsoleHelper.ReadDouble("Минимум минути");
            Console.WriteLine();
            PrintTable(await _filmService.GetByMinDurationAsync(min));
            ConsoleHelper.Pause();
        }

        private async Task ListShortestAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("НАЙ-КРАТКИТЕ 3 ФИЛМА");
            Console.WriteLine();
            PrintTable(await _filmService.GetShortestAsync(3));
            ConsoleHelper.Pause();
        }

        private async Task ShowLongestAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("НАЙ-ДЪЛГИЯТ ФИЛМ");
            Console.WriteLine();
            var film = await _filmService.GetLongestAsync();
            if (film == null) ConsoleHelper.PrintWarning("Няма филми.");
            else PrintTable(new[] { film });
            ConsoleHelper.Pause();
        }

        private async Task ListWithDetailsAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ФИЛМИ С АКТЬОРИ И ПРОЖЕКЦИИ");
            Console.WriteLine();
            var films = await _filmService.GetFilmsWithDetailsAsync();
            foreach (var f in films)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  ★  {f.FilmName}  [{f.FilmGenre}]  {f.FilmTime} мин.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"     Актьор: {f.Actor?.FirstName} {f.Actor?.LastName}");
                Console.ResetColor();
                if (!f.Projects.Any()) ConsoleHelper.PrintWarning("     Няма прожекции.");
                else foreach (var p in f.Projects)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("       › ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{p.DayOfWeek}  {p.StartTime}–{p.EndTime}  (зала: {p.Room?.RoomType})");
                    Console.ResetColor();
                }
                ConsoleHelper.PrintThinSeparator();
            }
            ConsoleHelper.Pause();
        }

        private static void PrintTable(IEnumerable<Film> films)
        {
            ConsoleHelper.PrintTableHeader("ID  ", "Заглавие                      ", "Жанр          ", "Мин. ", "Актьор              ");
            foreach (var f in films)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("  ║  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"{f.FilmId,-4}  ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{f.FilmName,-30}  ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{f.FilmGenre,-14}  ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{f.FilmTime,-5}  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"{f.Actor?.FirstName} {f.Actor?.LastName}");
                Console.ResetColor();
            }
            if (!films.Any()) ConsoleHelper.PrintWarning("  Няма филми.");
        }
    }
}
