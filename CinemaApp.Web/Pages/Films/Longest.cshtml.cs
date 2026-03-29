using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Films
{
    public class LongestModel : PageModel
    {
        private readonly IFilmService _filmService;
        public LongestModel(IFilmService filmService) => _filmService = filmService;

        public Film? Film { get; set; }

        public async Task OnGetAsync() => Film = await _filmService.GetLongestAsync();
    }
}
