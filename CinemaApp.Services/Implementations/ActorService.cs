using CinemaApp.Data.Context;
using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Implementations
{
    /// <summary>
    /// Имплементация на услугата за управление на актьори.
    /// </summary>
    public class ActorService : IActorService
    {
        private readonly CinemaDbContext _context;

        /// <summary>Инициализира нова инстанция на <see cref="ActorService"/>.</summary>
        public ActorService(CinemaDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Actor>> GetAllAsync()
        {
            return await _context.Actors.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Actor?> GetByIdAsync(int id)
        {
            return await _context.Actors.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Actor actor)
        {
            if (actor == null) throw new ArgumentNullException(nameof(actor));
            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Actor actor)
        {
            if (actor == null) throw new ArgumentNullException(nameof(actor));
            _context.Actors.Update(actor);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) throw new KeyNotFoundException($"Актьор с ID {id} не е намерен.");
            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Actor>> GetByMinAgeAsync(int minAge)
        {
            return await _context.Actors
                .Where(a => a.Age >= minAge)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Actor>> GetActorsWithFilmsAsync()
        {
            return await _context.Actors
                .Include(a => a.Films)
                .ToListAsync();
        }
    }
}
