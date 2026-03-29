using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Tickets
{
    public class EditModel : PageModel
    {
        private readonly ITicketService _ticketService;
        public EditModel(ITicketService s) => _ticketService = s;
        [BindProperty] public Ticket Ticket { get; set; } = new();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var t = await _ticketService.GetByIdAsync(id);
            if (t == null) return NotFound();
            Ticket = t; return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _ticketService.UpdateAsync(Ticket);
            TempData["Success"] = "Билетът е актуализиран успешно!";
            return RedirectToPage("Index");
        }
    }
}
