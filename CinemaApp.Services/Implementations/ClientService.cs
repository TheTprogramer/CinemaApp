using CinemaApp.Data.Context;
using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Implementations
{
    /// <summary>
    /// Имплементация на услугата за управление на клиенти.
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly CinemaDbContext _context;

        /// <summary>Инициализира нова инстанция на <see cref="ClientService"/>.</summary>
        public ClientService(CinemaDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients
                .Include(c => c.Ticket)
                .Include(c => c.Food!.Type)
                .Include(c => c.Drink!.Type)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.Ticket)
                .Include(c => c.Food!.Type)
                .Include(c => c.Drink!.Type)
                .FirstOrDefaultAsync(c => c.ClientId == id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Client client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Client client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) throw new KeyNotFoundException($"Клиент с ID {id} не е намерен.");
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Client>> GetByDayAsync(string dayOfWeek)
        {
            if (string.IsNullOrWhiteSpace(dayOfWeek))
                throw new ArgumentException("Денят не може да е празен.", nameof(dayOfWeek));

            return await _context.Clients
                .Include(c => c.Ticket)
                .Include(c => c.Food!.Type)
                .Include(c => c.Drink!.Type)
                .Where(c => c.DayOfWeek == dayOfWeek)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<int> GetSoldTicketsCountByDayAsync(string dayOfWeek)
        {
            return await _context.Clients
                .CountAsync(c => c.DayOfWeek == dayOfWeek);
        }

        /// <inheritdoc/>
        public async Task<decimal> GetRevenueByDayAsync(string dayOfWeek)
        {
            var clients = await _context.Clients
                .Include(c => c.Ticket)
                .Include(c => c.Food!.Type)
                .Include(c => c.Drink!.Type)
                .Where(c => c.DayOfWeek == dayOfWeek)
                .ToListAsync();

            return clients.Sum(c =>
                (c.Ticket?.TicketPrice ?? 0) +
                (c.Food?.Type?.TypePrice ?? 0) +
                (c.Drink?.Type?.TypePrice ?? 0));
        }

        /// <inheritdoc/>
        public async Task<decimal> GetWeeklyRevenueAsync()
        {
            var clients = await _context.Clients
                .Include(c => c.Ticket)
                .Include(c => c.Food!.Type)
                .Include(c => c.Drink!.Type)
                .ToListAsync();

            return clients.Sum(c =>
                (c.Ticket?.TicketPrice ?? 0) +
                (c.Food?.Type?.TypePrice ?? 0) +
                (c.Drink?.Type?.TypePrice ?? 0));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<(decimal Expenses, int Count)>> GetClientsWithSameExpensesAsync()
        {
            var clients = await _context.Clients
                .Include(c => c.Ticket)
                .Include(c => c.Food!.Type)
                .Include(c => c.Drink!.Type)
                .ToListAsync();

            return clients
                .GroupBy(c =>
                    (c.Ticket?.TicketPrice ?? 0) +
                    (c.Food?.Type?.TypePrice ?? 0) +
                    (c.Drink?.Type?.TypePrice ?? 0))
                .Where(g => g.Count() > 1)
                .Select(g => (g.Key, g.Count()));
        }
    }
}
