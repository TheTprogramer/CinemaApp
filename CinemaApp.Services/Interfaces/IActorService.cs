using CinemaApp.Data.Models;

namespace CinemaApp.Services.Interfaces
{
    /// <summary>
    /// Дефинира операциите за управление на актьори.
    /// </summary>
    public interface IActorService
    {
        /// <summary>Връща всички актьори.</summary>
        Task<IEnumerable<Actor>> GetAllAsync();

        /// <summary>Връща актьор по идентификатор.</summary>
        Task<Actor?> GetByIdAsync(int id);

        /// <summary>Добавя нов актьор.</summary>
        Task AddAsync(Actor actor);

        /// <summary>Актуализира съществуващ актьор.</summary>
        Task UpdateAsync(Actor actor);

        /// <summary>Изтрива актьор по идентификатор.</summary>
        Task DeleteAsync(int id);

        /// <summary>Връща актьори над зададена минимална възраст.</summary>
        Task<IEnumerable<Actor>> GetByMinAgeAsync(int minAge);

        /// <summary>Връща актьори с техните филми.</summary>
        Task<IEnumerable<Actor>> GetActorsWithFilmsAsync();
    }
}
