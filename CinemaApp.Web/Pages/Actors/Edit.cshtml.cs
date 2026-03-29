using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Actors
{
    public class EditModel : PageModel
    {
        private readonly IActorService _actorService;
        public EditModel(IActorService actorService) => _actorService = actorService;

        [BindProperty]
        public Actor Actor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var actor = await _actorService.GetByIdAsync(id);
            if (actor == null) return NotFound();
            Actor = actor;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _actorService.UpdateAsync(Actor);
            TempData["Success"] = "Актьорът е актуализиран успешно!";
            return RedirectToPage("Index");
        }
    }
}
