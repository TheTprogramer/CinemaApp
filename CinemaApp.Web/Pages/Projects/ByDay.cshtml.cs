using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Projects
{
    public class ByDayModel : PageModel
    {
        private readonly IProjectService _projectService;
        public ByDayModel(IProjectService s) => _projectService = s;

        public string? Day { get; set; }
        public IEnumerable<Project> Projects { get; set; } = [];
        public static string[] Days => ["Понеделник","Вторник","Сряда","Четвъртък","Петък","Събота","Неделя"];

        public async Task OnGetAsync(string? day)
        {
            Day = day;
            if (!string.IsNullOrWhiteSpace(day))
                Projects = await _projectService.GetByDayAsync(day);
        }
    }
}
