using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Actors
{
    public class DeleteModel : PageModel
    {
        private readonly IActorService _actorService;
        public DeleteModel(IActorService actorService) => _actorService = actorService;

        public Actor Actor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var actor = await _actorService.GetByIdAsync(id);
            if (actor == null) return NotFound();
            Actor = actor;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _actorService.DeleteAsync(id);
            TempData["Success"] = "Актьорът е изтрит успешно!";
            return RedirectToPage("Index");
        }
    }
}
