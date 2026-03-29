using CinemaApp.Data.Context;
using CinemaApp.Data.Models;
using CinemaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Implementations
{
    /// <summary>
    /// Имплементация на услугата за управление на прожекции.
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly CinemaDbContext _context;

        private static readonly string[] DayOrder =
            ["Понеделник", "Вторник", "Сряда", "Четвъртък", "Петък", "Събота", "Неделя"];

        /// <summary>Инициализира нова инстанция на <see cref="ProjectService"/>.</summary>
        public ProjectService(CinemaDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Film)
                .Include(p => p.Room)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Film)
                .Include(p => p.Room)
                .FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        /// <inheritdoc/>
        public async Task AddAsync(Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) throw new KeyNotFoundException($"Прожекция с ID {id} не е намерена.");
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Project>> GetByFilmNameAsync(string filmName)
        {
            if (string.IsNullOrWhiteSpace(filmName))
                throw new ArgumentException("Заглавието не може да е празно.", nameof(filmName));

            var list = await _context.Projects
                .Include(p => p.Film)
                .Include(p => p.Room)
                .Where(p => p.Film != null && p.Film.FilmName == filmName)
                .ToListAsync();

            return list
                .OrderBy(p => Array.IndexOf(DayOrder, p.DayOfWeek))
                .ThenBy(p => p.StartTime);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Project>> GetByDayAsync(string dayOfWeek)
        {
            if (string.IsNullOrWhiteSpace(dayOfWeek))
                throw new ArgumentException("Денят не може да е празен.", nameof(dayOfWeek));

            return await _context.Projects
                .Include(p => p.Film)
                .Include(p => p.Room)
                .Where(p => p.DayOfWeek == dayOfWeek)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Project>> GetProjectionsWithDetailsAsync()
        {
            return await _context.Projects
                .Include(p => p.Film)
                    .ThenInclude(f => f!.Actor)
                .Include(p => p.Room)
                .ToListAsync();
        }
    }
}
