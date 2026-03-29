using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Projects
{
    public class ByFilmModel : PageModel
    {
        private readonly IProjectService _projectService;
        private readonly IFilmService    _filmService;
        public ByFilmModel(IProjectService ps, IFilmService fs) { _projectService = ps; _filmService = fs; }

        public string? FilmName { get; set; }
        public IEnumerable<Project> Projects { get; set; } = [];
        public IEnumerable<Film>    AllFilms { get; set; } = [];

        public async Task OnGetAsync(string? filmName)
        {
            FilmName = filmName;
            AllFilms = await _filmService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(filmName))
                Projects = await _projectService.GetByFilmNameAsync(filmName);
        }
    }
}
