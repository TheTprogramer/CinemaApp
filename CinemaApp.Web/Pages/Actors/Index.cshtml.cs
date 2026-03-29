using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Actors
{
    public class IndexModel : PageModel
    {
        private readonly IActorService _actorService;
        public IndexModel(IActorService actorService) => _actorService = actorService;

        public IEnumerable<Actor> Actors { get; set; } = [];

        public async Task OnGetAsync()
        {
            Actors = await _actorService.GetAllAsync();
        }
    }
}
