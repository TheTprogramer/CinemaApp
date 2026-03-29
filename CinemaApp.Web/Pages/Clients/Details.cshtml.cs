using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Clients
{
    public class DetailsModel : PageModel
    {
        private readonly IClientService _clientService;
        public DetailsModel(IClientService s) => _clientService = s;
        public Client Client { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var c = await _clientService.GetByIdAsync(id);
            if (c == null) return NotFound();
            Client = c;
            return Page();
        }
    }
}
