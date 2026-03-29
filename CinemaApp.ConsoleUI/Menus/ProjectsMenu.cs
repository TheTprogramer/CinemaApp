using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;

namespace CinemaApp.ConsoleUI.Menus
{
    public class ProjectsMenu
    {
        private readonly IProjectService _projectService;
        private readonly IFilmService    _filmService;

        public ProjectsMenu(IProjectService projectService, IFilmService filmService)
        { _projectService = projectService; _filmService = filmService; }

        public async Task ShowAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear(); ConsoleHelper.PrintLogo();
                ConsoleHelper.PrintTitle("УПРАВЛЕНИЕ НА ПРОЖЕКЦИИ");
                Console.WriteLine();
                ConsoleHelper.PrintMenuOption(1, "Всички прожекции");
                ConsoleHelper.PrintMenuOption(2, "Търсене по ID");
                ConsoleHelper.PrintMenuOption(3, "Добавяне на прожекция");
                ConsoleHelper.PrintMenuOption(4, "Редактиране на прожекция");
                ConsoleHelper.PrintMenuOption(5, "Изтриване на прожекция");
                ConsoleHelper.PrintMenuOption(6, "Прожекции по заглавие на филм");
                ConsoleHelper.PrintMenuOption(7, "Прожекции по ден");
                ConsoleHelper.PrintMenuOption(8, "Всички прожекции с детайли");
                ConsoleHelper.PrintMenuOption(0, "Обратно");
                ConsoleHelper.PrintMenuFooter();

                switch (ConsoleHelper.ReadChoice())
                {
                    case "1": await ListAllAsync();          break;
                    case "2": await FindByIdAsync();         break;
                    case "3": await AddAsync();              break;
                    case "4": await UpdateAsync();           break;
                    case "5": await DeleteAsync();           break;
                    case "6": await ListByFilmNameAsync();   break;
                    case "7": await ListByDayAsync();        break;
                    case "8": await ListWithDetailsAsync();  break;
                    case "0": running = false;               break;
                    default: ConsoleHelper.PrintWarning("Невалиден избор."); ConsoleHelper.Pause(); break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ВСИЧКИ ПРОЖЕКЦИИ");
            Console.WriteLine();
            PrintTable(await _projectService.GetAllAsync());
            ConsoleHelper.Pause();
        }

        private async Task FindByIdAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ТЪРСЕНЕ ПО ID");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            var p = await _projectService.GetByIdAsync(id);
            Console.WriteLine();
            if (p == null) ConsoleHelper.PrintWarning("Не е намерена.");
            else
            {
                ConsoleHelper.PrintInfo($"ID: {p.ProjectId}");
                ConsoleHelper.PrintInfo($"Ден: {p.DayOfWeek}  {p.StartTime}–{p.EndTime}");
                ConsoleHelper.PrintInfo($"Зала: {p.Room?.RoomType}");
                ConsoleHelper.PrintInfo($"Филм: {p.Film?.FilmName}");
            }
            ConsoleHelper.Pause();
        }

        private async Task AddAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ДОБАВЯНЕ НА ПРОЖЕКЦИЯ");
            Console.WriteLine();
            var films = (await _filmService.GetAllAsync()).ToList();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  Налични филми:");
            foreach (var f in films)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("    › ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[{f.FilmId}] {f.FilmName}  ({f.FilmGenre})  {f.FilmTime} мин.");
            }
            Console.ResetColor(); Console.WriteLine();
            var project = new Project
            {
                DayOfWeek = ConsoleHelper.ReadNonEmptyString("Ден (Понеделник/Вторник/...)"),
                StartTime = ConsoleHelper.ReadNonEmptyString("Начален час (напр. 10:30)"),
                EndTime   = ConsoleHelper.ReadNonEmptyString("Краен час (напр. 13:00)"),
                RoomId    = ConsoleHelper.ReadInt("Зала ID (1=2D, 2=3D, 3=4DX, 4=ScreenX)"),
                FilmId    = ConsoleHelper.ReadInt("Филм ID"),
            };
            try { await _projectService.AddAsync(project); ConsoleHelper.PrintSuccess("Прожекцията е добавена!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task UpdateAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("РЕДАКТИРАНЕ НА ПРОЖЕКЦИЯ");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) { ConsoleHelper.PrintWarning("Не е намерена."); ConsoleHelper.Pause(); return; }
            Console.WriteLine();
            ConsoleHelper.PrintInfo($"Текущо: {project.DayOfWeek}  {project.StartTime}–{project.EndTime}  Зала: {project.RoomId}");
            Console.WriteLine();
            project.DayOfWeek = ConsoleHelper.ReadNonEmptyString($"Ден [{project.DayOfWeek}]");
            project.StartTime = ConsoleHelper.ReadNonEmptyString($"Начален час [{project.StartTime}]");
            project.EndTime   = ConsoleHelper.ReadNonEmptyString($"Краен час [{project.EndTime}]");
            try { await _projectService.UpdateAsync(project); ConsoleHelper.PrintSuccess("Актуализирано!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task DeleteAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ИЗТРИВАНЕ НА ПРОЖЕКЦИЯ");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            Console.WriteLine();
            ConsoleHelper.PrintWarning("Потвърждавате ли? (да/не)");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("  › "); Console.ResetColor();
            if (Console.ReadLine()?.ToLower() != "да") { ConsoleHelper.PrintWarning("Отменено."); ConsoleHelper.Pause(); return; }
            try { await _projectService.DeleteAsync(id); ConsoleHelper.PrintSuccess("Изтрита!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task ListByFilmNameAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ПРОЖЕКЦИИ ПО ФИЛМ");
            Console.WriteLine();
            string name = ConsoleHelper.ReadNonEmptyString("Заглавие на филма");
            Console.WriteLine();
            PrintTable(await _projectService.GetByFilmNameAsync(name));
            ConsoleHelper.Pause();
        }

        private async Task ListByDayAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ПРОЖЕКЦИИ ПО ДЕН");
            Console.WriteLine();
            string day = ConsoleHelper.ReadNonEmptyString("Ден (Понеделник/Вторник/...)");
            Console.WriteLine();
            PrintTable(await _projectService.GetByDayAsync(day));
            ConsoleHelper.Pause();
        }

        private async Task ListWithDetailsAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ПРОЖЕКЦИИ С ДЕТАЙЛИ");
            Console.WriteLine();
            var projects = await _projectService.GetProjectionsWithDetailsAsync();
            foreach (var p in projects)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  ★  {p.DayOfWeek}  {p.StartTime}–{p.EndTime}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"     Филм:   {p.Film?.FilmName}  [{p.Film?.FilmGenre}]  {p.Film?.FilmTime} мин.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"     Актьор: {p.Film?.Actor?.FirstName} {p.Film?.Actor?.LastName}");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"     Зала:   {p.Room?.RoomType}");
                Console.ResetColor();
                ConsoleHelper.PrintThinSeparator();
            }
            ConsoleHelper.Pause();
        }

        private static void PrintTable(IEnumerable<Project> projects)
        {
            ConsoleHelper.PrintTableHeader("ID  ", "Ден         ", "Начало", "Край  ", "Зала      ", "Филм                          ");
            foreach (var p in projects)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("  ║  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write($"{p.ProjectId,-4}  ");
                Console.ForegroundColor = ConsoleColor.White;     Console.Write($"{p.DayOfWeek,-12}  {p.StartTime,-6}  {p.EndTime,-6}  ");
                Console.ForegroundColor = ConsoleColor.Cyan;      Console.Write($"{p.Room?.RoomType,-10}  ");
                Console.ForegroundColor = ConsoleColor.White;     Console.WriteLine($"{p.Film?.FilmName}");
                Console.ResetColor();
            }
            if (!projects.Any()) ConsoleHelper.PrintWarning("  Няма прожекции.");
        }
    }
}
