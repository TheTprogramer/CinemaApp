using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Tickets
{
    public class IndexModel : PageModel
    {
        private readonly ITicketService _ticketService;
        public IndexModel(ITicketService s) => _ticketService = s;
        public IEnumerable<Ticket> Tickets { get; set; } = [];
        public async Task OnGetAsync() => Tickets = await _ticketService.GetAllAsync();
    }
}
