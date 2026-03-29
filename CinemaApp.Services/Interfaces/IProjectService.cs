using CinemaApp.Data.Models;

namespace CinemaApp.Services.Interfaces
{
    /// <summary>
    /// Дефинира операциите за управление на прожекции.
    /// </summary>
    public interface IProjectService
    {
        /// <summary>Връща всички прожекции.</summary>
        Task<IEnumerable<Project>> GetAllAsync();

        /// <summary>Връща прожекция по идентификатор.</summary>
        Task<Project?> GetByIdAsync(int id);

        /// <summary>Добавя нова прожекция.</summary>
        Task AddAsync(Project project);

        /// <summary>Актуализира съществуваща прожекция.</summary>
        Task UpdateAsync(Project project);

        /// <summary>Изтрива прожекция по идентификатор.</summary>
        Task DeleteAsync(int id);

        /// <summary>Връща прожекции за конкретен филм по заглавие.</summary>
        Task<IEnumerable<Project>> GetByFilmNameAsync(string filmName);

        /// <summary>Връща прожекции за конкретен ден от седмицата.</summary>
        Task<IEnumerable<Project>> GetByDayAsync(string dayOfWeek);

        /// <summary>Връща прожекции с детайли (филм + зала).</summary>
        Task<IEnumerable<Project>> GetProjectionsWithDetailsAsync();
    }
}
