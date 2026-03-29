using CinemaApp.Data.Models;

namespace CinemaApp.Services.Interfaces
{
    /// <summary>
    /// Дефинира операциите за управление на филми.
    /// </summary>
    public interface IFilmService
    {
        /// <summary>Връща всички филми.</summary>
        Task<IEnumerable<Film>> GetAllAsync();

        /// <summary>Връща филм по идентификатор.</summary>
        Task<Film?> GetByIdAsync(int id);

        /// <summary>Добавя нов филм.</summary>
        Task AddAsync(Film film);

        /// <summary>Актуализира съществуващ филм.</summary>
        Task UpdateAsync(Film film);

        /// <summary>Изтрива филм по идентификатор.</summary>
        Task DeleteAsync(int id);

        /// <summary>Връща филми по жанр.</summary>
        Task<IEnumerable<Film>> GetByGenreAsync(string genre);

        /// <summary>Връща филми с времетраене над зададен минимум.</summary>
        Task<IEnumerable<Film>> GetByMinDurationAsync(double minMinutes);

        /// <summary>Връща най-кратките N филма.</summary>
        Task<IEnumerable<Film>> GetShortestAsync(int count);

        /// <summary>Връща най-дългия филм.</summary>
        Task<Film?> GetLongestAsync();

        /// <summary>Връща филми с техните актьори и прожекции.</summary>
        Task<IEnumerable<Film>> GetFilmsWithDetailsAsync();
    }
}
