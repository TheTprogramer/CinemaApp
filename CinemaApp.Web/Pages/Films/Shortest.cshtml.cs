using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Films
{
    public class ShortestModel : PageModel
    {
        private readonly IFilmService _filmService;
        public ShortestModel(IFilmService filmService) => _filmService = filmService;

        public IEnumerable<Film> Films { get; set; } = [];

        public async Task OnGetAsync() => Films = await _filmService.GetShortestAsync(3);
    }
}
