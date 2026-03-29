using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;

namespace CinemaApp.ConsoleUI.Menus
{
    public class ClientsMenu
    {
        private readonly IClientService _clientService;
        private readonly ITicketService _ticketService;

        public ClientsMenu(IClientService clientService, ITicketService ticketService)
        { _clientService = clientService; _ticketService = ticketService; }

        public async Task ShowAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear(); ConsoleHelper.PrintLogo();
                ConsoleHelper.PrintTitle("УПРАВЛЕНИЕ НА КЛИЕНТИ");
                Console.WriteLine();
                ConsoleHelper.PrintMenuOption(1,  "Всички клиенти");
                ConsoleHelper.PrintMenuOption(2,  "Търсене по ID");
                ConsoleHelper.PrintMenuOption(3,  "Добавяне на клиент");
                ConsoleHelper.PrintMenuOption(4,  "Редактиране на клиент");
                ConsoleHelper.PrintMenuOption(5,  "Изтриване на клиент");
                ConsoleHelper.PrintMenuOption(6,  "Клиенти по ден");
                ConsoleHelper.PrintMenuOption(7,  "Продадени билети за ден");
                ConsoleHelper.PrintMenuOption(8,  "Приходи за ден");
                ConsoleHelper.PrintMenuOption(9,  "Приходи за седмицата");
                ConsoleHelper.PrintMenuOption(10, "Клиенти с еднакви разходи");
                ConsoleHelper.PrintMenuOption(0,  "Обратно");
                ConsoleHelper.PrintMenuFooter();

                switch (ConsoleHelper.ReadChoice())
                {
                    case "1":  await ListAllAsync();              break;
                    case "2":  await FindByIdAsync();             break;
                    case "3":  await AddAsync();                  break;
                    case "4":  await UpdateAsync();               break;
                    case "5":  await DeleteAsync();               break;
                    case "6":  await ListByDayAsync();            break;
                    case "7":  await ShowSoldTicketsAsync();      break;
                    case "8":  await ShowRevenueByDayAsync();     break;
                    case "9":  await ShowWeeklyRevenueAsync();    break;
                    case "10": await ShowSameExpensesAsync();     break;
                    case "0":  running = false;                   break;
                    default: ConsoleHelper.PrintWarning("Невалиден избор."); ConsoleHelper.Pause(); break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ВСИЧКИ КЛИЕНТИ");
            Console.WriteLine();
            PrintTable(await _clientService.GetAllAsync());
            ConsoleHelper.Pause();
        }

        private async Task FindByIdAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ТЪРСЕНЕ ПО ID");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            var c = await _clientService.GetByIdAsync(id);
            Console.WriteLine();
            if (c == null) ConsoleHelper.PrintWarning("Не е намерен.");
            else
            {
                ConsoleHelper.PrintInfo($"ID: {c.ClientId}  |  Ден: {c.DayOfWeek}");
                ConsoleHelper.PrintInfo($"Билет: #{c.TicketId}  ({c.Ticket?.TicketPrice:F2} лв.)");
                ConsoleHelper.PrintInfo($"Храна: {c.Food?.FoodTitle} [{c.Food?.Type?.TypeTitle}]");
                ConsoleHelper.PrintInfo($"Напитка: {c.Drink?.DrinkTitle} [{c.Drink?.Type?.TypeTitle}]");
            }
            ConsoleHelper.Pause();
        }

        private async Task AddAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ДОБАВЯНЕ НА КЛИЕНТ");
            Console.WriteLine();
            var client = new Client
            {
                DayOfWeek = ConsoleHelper.ReadNonEmptyString("Ден (Понеделник/Вторник/...)"),
                TicketId  = ConsoleHelper.ReadInt("ID на билет"),
                FoodId    = ConsoleHelper.ReadInt("ID на храна"),
                DrinkId   = ConsoleHelper.ReadInt("ID на напитка"),
            };
            try { await _clientService.AddAsync(client); ConsoleHelper.PrintSuccess("Клиентът е добавен!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task UpdateAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("РЕДАКТИРАНЕ НА КЛИЕНТ");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            var client = await _clientService.GetByIdAsync(id);
            if (client == null) { ConsoleHelper.PrintWarning("Не е намерен."); ConsoleHelper.Pause(); return; }
            Console.WriteLine();
            ConsoleHelper.PrintInfo($"Текущо: Ден={client.DayOfWeek}  Билет={client.TicketId}");
            Console.WriteLine();
            client.DayOfWeek = ConsoleHelper.ReadNonEmptyString($"Ден [{client.DayOfWeek}]");
            try { await _clientService.UpdateAsync(client); ConsoleHelper.PrintSuccess("Актуализирано!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task DeleteAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ИЗТРИВАНЕ НА КЛИЕНТ");
            Console.WriteLine();
            int id = ConsoleHelper.ReadInt("ID");
            Console.WriteLine();
            ConsoleHelper.PrintWarning("Потвърждавате ли? (да/не)");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("  › "); Console.ResetColor();
            if (Console.ReadLine()?.ToLower() != "да") { ConsoleHelper.PrintWarning("Отменено."); ConsoleHelper.Pause(); return; }
            try { await _clientService.DeleteAsync(id); ConsoleHelper.PrintSuccess("Изтрит!"); }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task ListByDayAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("КЛИЕНТИ ПО ДЕН");
            Console.WriteLine();
            string day = ConsoleHelper.ReadNonEmptyString("Ден (Понеделник/Вторник/...)");
            Console.WriteLine();
            PrintTable(await _clientService.GetByDayAsync(day));
            ConsoleHelper.Pause();
        }

        private async Task ShowSoldTicketsAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ПРОДАДЕНИ БИЛЕТИ ЗА ДЕН");
            Console.WriteLine();
            string day = ConsoleHelper.ReadNonEmptyString("Ден");
            int count = await _clientService.GetSoldTicketsCountByDayAsync(day);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ★  Продадени билети за {day}:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"     {count} билета");
            Console.ResetColor();
            ConsoleHelper.Pause();
        }

        private async Task ShowRevenueByDayAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ПРИХОДИ ЗА ДЕН");
            Console.WriteLine();
            string day = ConsoleHelper.ReadNonEmptyString("Ден");
            decimal revenue = await _clientService.GetRevenueByDayAsync(day);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ★  Приходи за {day}:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"     {revenue:F2} лв.");
            Console.ResetColor();
            ConsoleHelper.Pause();
        }

        private async Task ShowWeeklyRevenueAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("ПРИХОДИ ЗА СЕДМИЦАТА");
            Console.WriteLine();
            decimal revenue = await _clientService.GetWeeklyRevenueAsync();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ★  Общи приходи за седмицата:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"     {revenue:F2} лв.");
            Console.ResetColor();
            ConsoleHelper.Pause();
        }

        private async Task ShowSameExpensesAsync()
        {
            Console.Clear(); ConsoleHelper.PrintLogo();
            ConsoleHelper.PrintTitle("КЛИЕНТИ С ЕДНАКВИ РАЗХОДИ");
            Console.WriteLine();
            var groups = (await _clientService.GetClientsWithSameExpensesAsync()).ToList();
            ConsoleHelper.PrintTableHeader("Разход (лв.)    ", "Брой клиенти");
            foreach (var (expenses, count) in groups)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("  ║  ");
                Console.ForegroundColor = ConsoleColor.Green;    Console.Write($"{expenses,-16:F2}  ");
                Console.ForegroundColor = ConsoleColor.White;    Console.WriteLine($"{count}");
                Console.ResetColor();
            }
            if (!groups.Any()) ConsoleHelper.PrintWarning("  Няма клиенти с еднакви разходи.");
            ConsoleHelper.Pause();
        }

        private static void PrintTable(IEnumerable<Client> clients)
        {
            ConsoleHelper.PrintTableHeader("ID   ", "Ден         ", "Билет", "Цена     ", "Храна               ", "Напитка           ");
            foreach (var c in clients)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("  ║  ");
                Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write($"{c.ClientId,-5}  ");
                Console.ForegroundColor = ConsoleColor.White;     Console.Write($"{c.DayOfWeek,-12}  ");
                Console.ForegroundColor = ConsoleColor.DarkGray;  Console.Write($"#{c.TicketId,-5}");
                Console.ForegroundColor = ConsoleColor.Green;     Console.Write($"{c.Ticket?.TicketPrice,-9:F2}");
                Console.ForegroundColor = ConsoleColor.White;     Console.Write($"{c.Food?.FoodTitle,-20}  ");
                Console.ForegroundColor = ConsoleColor.Cyan;      Console.WriteLine($"{c.Drink?.DrinkTitle}");
                Console.ResetColor();
            }
            if (!clients.Any()) ConsoleHelper.PrintWarning("  Няма клиенти.");
        }
    }
}
