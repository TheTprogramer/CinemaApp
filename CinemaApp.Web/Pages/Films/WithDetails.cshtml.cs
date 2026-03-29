using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Films
{
    public class WithDetailsModel : PageModel
    {
        private readonly IFilmService _filmService;
        public WithDetailsModel(IFilmService filmService) => _filmService = filmService;

        public IEnumerable<Film> Films { get; set; } = [];

        public async Task OnGetAsync() => Films = await _filmService.GetFilmsWithDetailsAsync();
    }
}
