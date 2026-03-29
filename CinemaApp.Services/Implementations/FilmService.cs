using CinemaApp.Data.Context;
using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Implementations
{
    /// <summary>
    /// Имплементация на услугата за управление на филми.
    /// </summary>
    public class FilmService : IFilmService
    {
        private readonly CinemaDbContext _context;

        /// <summary>Инициализира нова инстанция на <see cref="FilmService"/>.</summary>
        public FilmService(CinemaDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Film>> GetAllAsync()
        {
            return await _context.Films
                .Include(f => f.Actor)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Film?> GetByIdAsync(int id)
        {
            return await _context.Films
                .Include(f => f.Actor)
                .FirstOrDefaultAsync(f => f.FilmId == id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Film film)
        {
            if (film == null) throw new ArgumentNullException(nameof(film));
            await _context.Films.AddAsync(film);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Film film)
        {
            if (film == null) throw new ArgumentNullException(nameof(film));
            _context.Films.Update(film);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null) throw new KeyNotFoundException($"Филм с ID {id} не е намерен.");
            _context.Films.Remove(film);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Film>> GetByGenreAsync(string genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
                throw new ArgumentException("Жанрът не може да е празен.", nameof(genre));

            return await _context.Films
                .Include(f => f.Actor)
                .Where(f => f.FilmGenre == genre)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Film>> GetByMinDurationAsync(double minMinutes)
        {
            return await _context.Films
                .Include(f => f.Actor)
                .Where(f => f.FilmTime >= minMinutes)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Film>> GetShortestAsync(int count)
        {
            return await _context.Films
                .Include(f => f.Actor)
                .OrderBy(f => f.FilmTime)
                .Take(count)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Film?> GetLongestAsync()
        {
            return await _context.Films
                .Include(f => f.Actor)
                .OrderByDescending(f => f.FilmTime)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Film>> GetFilmsWithDetailsAsync()
        {
            return await _context.Films
                .Include(f => f.Actor)
                .Include(f => f.Projects)
                    .ThenInclude(p => p.Room)
                .ToListAsync();
        }
    }
}
