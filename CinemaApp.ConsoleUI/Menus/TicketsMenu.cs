using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;

namespace CinemaApp.ConsoleUI.Menus
{
    public class TicketsMenu
    {
        private readonly ITicketService _ticketService;
        public TicketsMenu(ITicketService ticketService) => _ticketService = ticketService;

        public async Task ShowAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear(); ConsoleHelper.PrintLogo();
                ConsoleHelper.PrintTitle("УПРАВЛЕНИЕ НА БИЛЕТИ");
                Console.WriteLine();
                ConsoleHelper.PrintMenuOption(1, "Всички билети");
                ConsoleHelper.PrintMenuOption(2, "Търсене по ID");
                ConsoleHelper.PrintMenuOption(3, "Добавяне на билет");
                ConsoleHelper.PrintMenuOption(4, "Редактиране на билет");
                ConsoleHelper.PrintMenuOption(5, "Изтриване на билет");
                ConsoleHelper.PrintMenuOption(6, "Обща сума на продадените билети");
                ConsoleHelper.PrintMenuOption(7, "Свободни места");
                ConsoleHelper.PrintMenuOption(0, "Обратно");
                ConsoleHelper.PrintMenuFooter();

                switch (ConsoleHelper.ReadChoice())
                {
                    case "1": await ListAllAsync();        break;
                    case "2": await FindByIdAsync();       break;
                    case "3": await AddAsync();            break;
                    case "4": await UpdateAsync();         break;
                    case "5": await DeleteAsync();         break;
                    case "6": await ShowTotalSumAsync();   break;
                    case "7": await ShowFreePlacesAsync(); break;
                    case "0": running = false;             break;
                    default: ConsoleHelper.PrintWarning("Невалиден избор."); ConsoleHelper.Pause(); break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ВСИЧКИ БИЛЕТИ");
            Console.WriteLine();
            PrintTable(await _ticketService.GetAllAsync());
            ConsoleHelper.Pause();
        }

        private async Task FindByIdAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ТЪРСЕНЕ НА БИЛЕТ ПО ID");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            var ticket = await _ticketService.GetByIdAsync(id);
            Console.WriteLine();
            if (ticket == null) ConsoleHelper.PrintWarning("Не е намерен.");
            else
            {
                ConsoleHelper.PrintInfo($"ID: {ticket.TicketId}");
                ConsoleHelper.PrintInfo($"Цена: {ticket.TicketPrice:F2} лв.");
            }
            ConsoleHelper.Pause();
        }

        private async Task AddAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ДОБАВЯНЕ НА БИЛЕТ");
            Console.WriteLine();
            var ticket = new Ticket { TicketPrice = ConsoleHelper.ReadDecimal("Цена (лв.)") };
            try { await _ticketService.AddAsync(ticket); ConsoleHelper.PrintSuccess($"Билет с цена {ticket.TicketPrice:F2} лв. е добавен!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task UpdateAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("РЕДАКТИРАНЕ НА БИЛЕТ");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null) { ConsoleHelper.PrintWarning("Не е намерен."); ConsoleHelper.Pause(); return; }
            Console.WriteLine();
            ConsoleHelper.PrintInfo($"Текуща цена: {ticket.TicketPrice:F2} лв.");
            Console.WriteLine();
            ticket.TicketPrice = ConsoleHelper.ReadDecimal($"Нова цена [{ticket.TicketPrice:F2}]");
            try { await _ticketService.UpdateAsync(ticket); ConsoleHelper.PrintSuccess("Актуализирано!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task DeleteAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ИЗТРИВАНЕ НА БИЛЕТ");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            Console.WriteLine();
            ConsoleHelper.PrintWarning("Потвърждавате ли? (да/не)");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("  › "); Console.ResetColor();
            if (Console.ReadLine()?.ToLower() != "да") { ConsoleHelper.PrintWarning("Отменено."); ConsoleHelper.Pause(); return; }
            try { await _ticketService.DeleteAsync(id); ConsoleHelper.PrintSuccess("Изтрит!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task ShowTotalSumAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ОБЩА СУМА НА БИЛЕТИТЕ");
            Console.WriteLine();
            decimal total = await _ticketService.GetTotalTicketsSumAsync();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ★  Обща сума на всички продадени билети:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"     {total:F2} лв.");
            Console.ResetColor();
            ConsoleHelper.Pause();
        }

        private async Task ShowFreePlacesAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("СВОБОДНИ МЕСТА");
            Console.WriteLine();
            var places = await _ticketService.GetFreePlacesAsync();
            ConsoleHelper.PrintTableHeader("Зала ID", "Тип зала  ", "Ред ID", "Тип ред   ", "Място ID", "Тип място       ");
            foreach (var p in places)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("  ║  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write($"{p.RoomId,-7}  ");
                Console.ForegroundColor = ConsoleColor.Cyan;      Console.Write($"{p.Room?.RoomType,-10}  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write($"{p.RowsId,-6}  ");
                Console.ForegroundColor = p.Row?.RowsType == "VIP" ? ConsoleColor.Yellow : ConsoleColor.White;
                Console.Write($"{p.Row?.RowsType,-10}  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write($"{p.SeatId,-8}  ");
                Console.ForegroundColor = ConsoleColor.White;     Console.WriteLine($"{p.PlaceType}");
                Console.ResetColor();
            }
            if (!places.Any()) ConsoleHelper.PrintWarning("  Няма свободни места.");
            ConsoleHelper.Pause();
        }

        private static void PrintTable(IEnumerable<Ticket> tickets)
        {
            ConsoleHelper.PrintTableHeader("ID    ", "Цена (лв.)  ");
            foreach (var t in tickets)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("  ║  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write($"{t.TicketId,-6}  ");
                Console.ForegroundColor = ConsoleColor.Green;     Console.WriteLine($"{t.TicketPrice:F2}");
                Console.ResetColor();
            }
            if (!tickets.Any()) ConsoleHelper.PrintWarning("  Няма билети.");
        }
    }
}
