using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaApp.Web.Pages.Films
{
    public class CreateModel : PageModel
    {
        private readonly IFilmService  _filmService;
        private readonly IActorService _actorService;

        public CreateModel(IFilmService filmService, IActorService actorService)
        {
            _filmService  = filmService;
            _actorService = actorService;
        }

        [BindProperty]
        public Film Film { get; set; } = new();

        public SelectList ActorsList { get; set; } = null!;

        public async Task OnGetAsync()
        {
            await PopulateAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { await PopulateAsync(); return Page(); }
            await _filmService.AddAsync(Film);
            TempData["Success"] = $"Филмът '{Film.FilmName}' е добавен успешно!";
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
