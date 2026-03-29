using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Films
{
    public class DetailsModel : PageModel
    {
        private readonly IFilmService _filmService;
        public DetailsModel(IFilmService filmService) => _filmService = filmService;

        public Film Film { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var film = await _filmService.GetByIdAsync(id);
            if (film == null) return NotFound();
            Film = film;
            return Page();
        }
    }
}
