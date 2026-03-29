using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaApp.Web.Pages.Clients
{
    public class QueriesModel : PageModel
    {
        private readonly IClientService _clientService;
        public QueriesModel(IClientService s) => _clientService = s;

        public string? QueryType { get; set; }
        public string? Day       { get; set; }
        public SelectList DaysList { get; set; } = null!;
        private static readonly string[] Days = ["Понеделник","Вторник","Сряда","Четвъртък","Петък","Събота","Неделя"];

        public int?     SoldCount     { get; set; }
        public decimal? Revenue       { get; set; }
        public decimal? WeeklyRevenue { get; set; }
        public IEnumerable<(decimal Expenses, int Count)> SameExpenses { get; set; } = [];

        public async Task OnGetAsync(string? queryType, string? day)
        {
            DaysList  = new SelectList(Days);
            QueryType = queryType;
            Day       = day;

            switch (queryType)
            {
                case "sold" when !string.IsNullOrWhiteSpace(day):
                    SoldCount = await _clientService.GetSoldTicketsCountByDayAsync(day);
                    break;
                case "revenue_day" when !string.IsNullOrWhiteSpace(day):
                    Revenue = await _clientService.GetRevenueByDayAsync(day);
                    break;
                case "revenue_week":
                    WeeklyRevenue = await _clientService.GetWeeklyRevenueAsync();
                    break;
                case "same_expenses":
                    SameExpenses = await _clientService.GetClientsWithSameExpensesAsync();
                    break;
            }
        }
    }
}
