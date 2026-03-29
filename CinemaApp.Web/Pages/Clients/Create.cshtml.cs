using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaApp.Web.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly IClientService _clientService;
        public CreateModel(IClientService s) => _clientService = s;

        [BindProperty] public Client Client { get; set; } = new();
        public SelectList DaysList { get; set; } = null!;
        private static readonly string[] Days = ["Понеделник","Вторник","Сряда","Четвъртък","Петък","Събота","Неделя"];

        public void OnGet() => DaysList = new SelectList(Days);

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { DaysList = new SelectList(Days); return Page(); }
            await _clientService.AddAsync(Client);
            TempData["Success"] = "Клиентът е добавен успешно!";
            return RedirectToPage("Index");
        }
    }
}
