using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Clients
{
    public class DeleteModel : PageModel
    {
        private readonly IClientService _clientService;
        public DeleteModel(IClientService s) => _clientService = s;
        public Client Client { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var c = await _clientService.GetByIdAsync(id);
            if (c == null) return NotFound();
            Client = c;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _clientService.DeleteAsync(id);
            TempData["Success"] = "Клиентът е изтрит успешно!";
            return RedirectToPage("Index");
        }
    }
}
