using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Actors
{
    public class CreateModel : PageModel
    {
        private readonly IActorService _actorService;
        public CreateModel(IActorService actorService) => _actorService = actorService;

        [BindProperty]
        public Actor Actor { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _actorService.AddAsync(Actor);
            TempData["Success"] = $"Актьорът {Actor.FirstName} {Actor.LastName} е добавен успешно!";
            return RedirectToPage("Index");
        }
    }
}
