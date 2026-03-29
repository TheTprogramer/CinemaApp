using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaApp.Web.Pages.Films
{
    public class EditModel : PageModel
    {
        private readonly IFilmService  _filmService;
        private readonly IActorService _actorService;

        public EditModel(IFilmService filmService, IActorService actorService)
        {
            _filmService  = filmService;
            _actorService = actorService;
        }

        [BindProperty]
        public Film Film { get; set; } = new();

        public SelectList ActorsList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var film = await _filmService.GetByIdAsync(id);
            if (film == null) return NotFound();
            Film = film;
            await PopulateAsync(film.ActorId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { await PopulateAsync(Film.ActorId); return Page(); }
            await _filmService.UpdateAsync(Film);
            TempData["Success"] = "Филмът е актуализиран успешно!";
            return RedirectToPage("Index");
        }

        private async Task PopulateAsync(int? selectedId = null)
        {
            var actors = await _actorService.GetAllAsync();
            ActorsList = new SelectList(
                actors.Select(a => new { a.ActorId, Name = $"{a.FirstName} {a.LastName}" }),
                "ActorId", "Name", selectedId);
        }
    }
}
