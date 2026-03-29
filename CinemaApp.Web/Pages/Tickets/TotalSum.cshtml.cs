using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Tickets
{
    public class TotalSumModel : PageModel
    {
        private readonly ITicketService _ticketService;
        public TotalSumModel(ITicketService s) => _ticketService = s;
        public decimal Total { get; set; }
        public async Task OnGetAsync() => Total = await _ticketService.GetTotalTicketsSumAsync();
    }
}
