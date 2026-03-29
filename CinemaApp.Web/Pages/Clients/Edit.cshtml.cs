using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaApp.Web.Pages.Clients
{
    public class EditModel : PageModel
    {
        private readonly IClientService _clientService;
        public EditModel(IClientService s) => _clientService = s;

        [BindProperty] public Client Client { get; set; } = new();
        public SelectList DaysList { get; set; } = null!;
        private static readonly string[] Days = ["Понеделник","Вторник","Сряда","Четвъртък","Петък","Събота","Неделя"];

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var c = await _clientService.GetByIdAsync(id);
            if (c == null) return NotFound();
            Client = c;
            DaysList = new SelectList(Days, c.DayOfWeek);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { DaysList = new SelectList(Days); return Page(); }
            await _clientService.UpdateAsync(Client);
            TempData["Success"] = "Клиентът е актуализиран успешно!";
            return RedirectToPage("Index");
        }
    }
}
