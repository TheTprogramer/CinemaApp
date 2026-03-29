using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Projects
{
    public class DeleteModel : PageModel
    {
        private readonly IProjectService _projectService;
        public DeleteModel(IProjectService s) => _projectService = s;
        public Project Project { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var p = await _projectService.GetByIdAsync(id);
            if (p == null) return NotFound();
            Project = p;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _projectService.DeleteAsync(id);
            TempData["Success"] = "Прожекцията е изтрита успешно!";
            return RedirectToPage("Index");
        }
    }
}
