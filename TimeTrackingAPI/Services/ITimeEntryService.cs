using TimeTrackingAPI.Models;

namespace TimeTrackingAPI.Services
{
    /// <summary>
    /// Сервис для работы с проводками
    /// </summary>
    public interface ITimeEntryService
    {
        /// <summary>
        /// Возвращает суммарное количество часов за указанный день
        /// </summary>
        /// <param name="date">День, за который суммируются часы</param>
        /// <param name="excludeEntryId">Id проводки, которую следует исключить из суммы</param>
        /// <returns>Общее количество часов</returns>
        Task<decimal> GetTotalHoursForDateAsync(DateTime date, int? excludeEntryId = null);

        /// <summary>
        /// Проверяет, существует ли задача и активна ли она
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <returns>True, если задача существует и активна</returns>
        Task<bool> IsTaskActiveAsync(int taskId);

        /// <summary>
        /// Создаёт новую проводку
        /// </summary>
        /// <param name="entry">Объект проводки</param>
        /// <returns>Созданная проводка с заполненным Id</returns>
        /// <exception cref="ArgumentException">Если количество часов не в диапазоне (0,24]</exception>
        /// <exception cref="InvalidOperationException">Если задача неактивна или превышен лимит 24 часов за день</exception>
        Task<TimeEntry> CreateAsync(TimeEntry entry);

        /// <summary>
        /// Обновляет существующую проводку
        /// </summary>
        /// <param name="id">Идентификатор обновляемой проводки</param>
        /// <param name="updatedEntry">Объект с новыми значениями</param>
        /// <returns>Обновлённая проводка.</returns>
        /// <exception cref="ArgumentNullException">Если данные для изменения не заданы</exception>
        /// <exception cref="KeyNotFoundException">Если проводка с таким Id не найдена</exception>
        /// <exception cref="ArgumentException">Если новое количество часов некорректно</exception>
        /// <exception cref="InvalidOperationException">Если нарушено правило активности задачи или лимит часов за день</exception>
        Task<TimeEntry> UpdateAsync(int id, TimeEntry updatedEntry);

        
    }
}