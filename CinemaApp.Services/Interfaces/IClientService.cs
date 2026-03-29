using CinemaApp.Data.Models;

namespace CinemaApp.Services.Interfaces
{
    /// <summary>
    /// Дефинира операциите за управление на клиенти.
    /// </summary>
    public interface IClientService
    {
        /// <summary>Връща всички клиенти.</summary>
        Task<IEnumerable<Client>> GetAllAsync();

        /// <summary>Връща клиент по идентификатор.</summary>
        Task<Client?> GetByIdAsync(int id);

        /// <summary>Добавя нов клиент.</summary>
        Task AddAsync(Client client);

        /// <summary>Актуализира съществуващ клиент.</summary>
        Task UpdateAsync(Client client);

        /// <summary>Изтрива клиент по идентификатор.</summary>
        Task DeleteAsync(int id);

        /// <summary>Връща клиенти по ден от седмицата.</summary>
        Task<IEnumerable<Client>> GetByDayAsync(string dayOfWeek);

        /// <summary>Връща брой продадени билети за ден.</summary>
        Task<int> GetSoldTicketsCountByDayAsync(string dayOfWeek);

        /// <summary>Връща общи приходи за ден (билет + храна + напитка).</summary>
        Task<decimal> GetRevenueByDayAsync(string dayOfWeek);

        /// <summary>Връща общи приходи за цялата седмица.</summary>
        Task<decimal> GetWeeklyRevenueAsync();

        /// <summary>Връща клиенти с еднакви общи разходи (групирани).</summary>
        Task<IEnumerable<(decimal Expenses, int Count)>> GetClientsWithSameExpensesAsync();
    }
}
