using CinemaApp.Data.Context;
using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Implementations
{
    /// <summary>
    /// Имплементация на услугата за управление на билети.
    /// </summary>
    public class TicketService : ITicketService
    {
        private readonly CinemaDbContext _context;

        /// <summary>Инициализира нова инстанция на <see cref="TicketService"/>.</summary>
        public TicketService(CinemaDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Ticket ticket)
        {
            if (ticket == null) throw new ArgumentNullException(nameof(ticket));
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Ticket ticket)
        {
            if (ticket == null) throw new ArgumentNullException(nameof(ticket));
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) throw new KeyNotFoundException($"Билет с ID {id} не е намерен.");
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<decimal> GetTotalTicketsSumAsync()
        {
            return await _context.Clients
                .Include(c => c.Ticket)
                .Where(c => c.Ticket != null)
                .SumAsync(c => c.Ticket!.TicketPrice);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Place>> GetFreePlacesAsync()
        {
            return await _context.Places
                .Include(p => p.Room)
                .Include(p => p.Row)
                .Where(p => !_context.Clients.Any(c => c.TicketId == p.TicketId))
                .OrderBy(p => p.RoomId)
                .ThenBy(p => p.RowsId)
                .ThenBy(p => p.SeatId)
                .ToListAsync();
        }
    }
}
