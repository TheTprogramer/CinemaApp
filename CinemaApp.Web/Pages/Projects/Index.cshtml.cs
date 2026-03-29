using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Projects
{
    public class IndexModel : PageModel
    {
        private readonly IProjectService _projectService;
        public IndexModel(IProjectService s) => _projectService = s;
        public IEnumerable<Project> Projects { get; set; } = [];
        public async Task OnGetAsync() => Projects = await _projectService.GetProjectionsWithDetailsAsync();
    }
}
