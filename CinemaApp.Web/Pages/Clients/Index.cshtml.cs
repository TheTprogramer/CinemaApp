using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinemaApp.Web.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly IClientService _clientService;
        public IndexModel(IClientService s) => _clientService = s;
        public IEnumerable<Client> Clients { get; set; } = [];
        public async Task OnGetAsync() => Clients = await _clientService.GetAllAsync();
    }
}
