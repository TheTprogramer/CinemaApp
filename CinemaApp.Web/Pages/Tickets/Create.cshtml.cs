using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Tickets
{
    public class CreateModel : PageModel
    {
        private readonly ITicketService _ticketService;
        public CreateModel(ITicketService s) => _ticketService = s;
        [BindProperty] public Ticket Ticket { get; set; } = new();
        public void OnGet() { }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _ticketService.AddAsync(Ticket);
            TempData["Success"] = $"Билетът с цена {Ticket.TicketPrice:F2} лв. е добавен!";
            return RedirectToPage("Index");
        }
    }
}
