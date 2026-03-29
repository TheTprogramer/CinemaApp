using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Actors
{
    public class WithFilmsModel : PageModel
    {
        private readonly IActorService _actorService;
        public WithFilmsModel(IActorService actorService) => _actorService = actorService;

        public IEnumerable<Actor> Actors { get; set; } = [];

        public async Task OnGetAsync()
        {
            Actors = await _actorService.GetActorsWithFilmsAsync();
        }
    }
}
