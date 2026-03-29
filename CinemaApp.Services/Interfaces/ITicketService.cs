using CinemaApp.Data.Models;

namespace CinemaApp.Services.Interfaces
{
    /// <summary>
    /// Дефинира операциите за управление на билети.
    /// </summary>
    public interface ITicketService
    {
        /// <summary>Връща всички билети.</summary>
        Task<IEnumerable<Ticket>> GetAllAsync();

        /// <summary>Връща билет по идентификатор.</summary>
        Task<Ticket?> GetByIdAsync(int id);

        /// <summary>Добавя нов билет.</summary>
        Task AddAsync(Ticket ticket);

        /// <summary>Актуализира съществуващ билет.</summary>
        Task UpdateAsync(Ticket ticket);

        /// <summary>Изтрива билет по идентификатор.</summary>
        Task DeleteAsync(int id);

        /// <summary>Връща обща сума на всички продадени билети.</summary>
        Task<decimal> GetTotalTicketsSumAsync();

        /// <summary>Връща свободни места (без закупен билет от клиент).</summary>
        Task<IEnumerable<Place>> GetFreePlacesAsync();
    }
}
