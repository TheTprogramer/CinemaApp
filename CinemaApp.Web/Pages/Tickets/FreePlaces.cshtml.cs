using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Tickets
{
    public class FreePlacesModel : PageModel
    {
        private readonly ITicketService _ticketService;
        public FreePlacesModel(ITicketService s) => _ticketService = s;
        public IEnumerable<Place> Places { get; set; } = [];
        public async Task OnGetAsync() => Places = await _ticketService.GetFreePlacesAsync();
    }
}
