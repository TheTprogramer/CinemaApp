using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Films
{
    public class DeleteModel : PageModel
    {
        private readonly IFilmService _filmService;
        public DeleteModel(IFilmService filmService) => _filmService = filmService;

        public Film Film { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var film = await _filmService.GetByIdAsync(id);
            if (film == null) return NotFound();
            Film = film;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _filmService.DeleteAsync(id);
            TempData["Success"] = "Филмът е изтрит успешно!";
            return RedirectToPage("Index");
        }
    }
}
