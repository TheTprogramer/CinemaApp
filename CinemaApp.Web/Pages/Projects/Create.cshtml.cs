using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaApp.Web.Pages.Projects
{
    public class CreateModel : PageModel
    {
        private readonly IProjectService _projectService;
        private readonly IFilmService    _filmService;
        public CreateModel(IProjectService ps, IFilmService fs) { _projectService = ps; _filmService = fs; }

        [BindProperty] public Project Project { get; set; } = new();
        public SelectList FilmsList { get; set; } = null!;
        public SelectList RoomsList { get; set; } = null!;
        public SelectList DaysList  { get; set; } = null!;

        public async Task OnGetAsync() => await PopulateAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { await PopulateAsync(); return Page(); }
            await _projectService.AddAsync(Project);
            TempData["Success"] = "Прожекцията е добавена успешно!";
            return RedirectToPage("Index");
        }

        private async Task PopulateAsync(int? filmId = null, int? roomId = null)
        {
            var films = await _filmService.GetAllAsync();
            FilmsList = new SelectList(films.Select(f => new { f.FilmId, f.FilmName }), "FilmId", "FilmName", filmId);
            RoomsList = new SelectList(new[]
            {
                new { Id = 1, Name = "1 – 2D" }, new { Id = 2, Name = "2 – 3D" },
                new { Id = 3, Name = "3 – 4DX" }, new { Id = 4, Name = "4 – ScreenX" },
            }, "Id", "Name", roomId);
            DaysList = new SelectList(new[] { "Понеделник","Вторник","Сряда","Четвъртък","Петък","Събота","Неделя" });
        }
    }
}
