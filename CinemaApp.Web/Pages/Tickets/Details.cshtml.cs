using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Tickets
{
    public class DetailsModel : PageModel
    {
        private readonly ITicketService _ticketService;
        public DetailsModel(ITicketService s) => _ticketService = s;
        public Ticket Ticket { get; set; } = new();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var t = await _ticketService.GetByIdAsync(id);
            if (t == null) return NotFound();
            Ticket = t; return Page();
        }
    }
}
