using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Tickets
{
    public class DeleteModel : PageModel
    {
        private readonly ITicketService _ticketService;
        public DeleteModel(ITicketService s) => _ticketService = s;
        public Ticket Ticket { get; set; } = new();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var t = await _ticketService.GetByIdAsync(id);
            if (t == null) return NotFound();
            Ticket = t; return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _ticketService.DeleteAsync(id);
            TempData["Success"] = "Билетът е изтрит успешно!";
            return RedirectToPage("Index");
        }
    }
}
