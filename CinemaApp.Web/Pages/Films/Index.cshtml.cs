using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Films
{
    public class IndexModel : PageModel
    {
        private readonly IFilmService _filmService;
        public IndexModel(IFilmService filmService) => _filmService = filmService;

        public IEnumerable<Film> Films { get; set; } = [];

        public async Task OnGetAsync()
        {
            Films = await _filmService.GetAllAsync();
        }
    }
}
