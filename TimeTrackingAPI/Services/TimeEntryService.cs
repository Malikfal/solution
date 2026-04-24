using Microsoft.EntityFrameworkCore;
using TimeTrackingAPI.Data;
using TimeTrackingAPI.Models;

namespace TimeTrackingAPI.Services
{
    /// <summary>
    /// Реализация сервиса для проводок времени
    /// </summary>
    public class TimeEntryService : ITimeEntryService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Инициализирует сервис с заданным контекстом БД
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public TimeEntryService(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Контекст БД не задан");
            }
            _context = context;
        }

        /// <inheritdoc />
        public async Task<decimal> GetTotalHoursForDateAsync(DateTime date, int? excludeEntryId = null)
        {
            var query = _context.TimeEntries.Where(te => te.Date.Date == date.Date);
            if (excludeEntryId.HasValue)
            {
                query = query.Where(te => te.Id != excludeEntryId.Value);
            }
            return await query.SumAsync(te => te.Hours);
        }

        /// <inheritdoc />
        public async Task<bool> IsTaskActiveAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            return task != null && task.IsActive;
        }

        /// <inheritdoc />
        public async Task<TimeEntry> CreateAsync(TimeEntry entry)
        {
            if (entry.Hours <= 0 || entry.Hours > 24)
            {
                throw new ArgumentException("Часы должны быть от 0 до 24", nameof(entry.Hours));
            }

            var taskActive = await IsTaskActiveAsync(entry.TaskId);
            if (!taskActive)
            {
                throw new InvalidOperationException("Задача неактивна или не существует");
            }

            var totalHours = await GetTotalHoursForDateAsync(entry.Date);
            if (totalHours + entry.Hours > 24)
            {
                throw new InvalidOperationException("Суммарное время за день не может превышать 24 часа");
            }

            entry.Id = 0;
            _context.TimeEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        /// <inheritdoc />
        public async Task<TimeEntry> UpdateAsync(int id, TimeEntry updatedEntry)
        {
            if (updatedEntry == null)
            {
                throw new ArgumentNullException("Данные не заданы");
            }

            var existing = await _context.TimeEntries
                .Include(te => te.Task)
                .FirstOrDefaultAsync(te => te.Id == id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Проводка с id {id} не найдена");
            }

            if (!existing.Task.IsActive && updatedEntry.TaskId != existing.TaskId)
            {
                throw new InvalidOperationException("Нельзя изменить неактивную задачу");
            }

            if (updatedEntry.TaskId != existing.TaskId)
            {
                var newTaskActive = await IsTaskActiveAsync(updatedEntry.TaskId);
                if (!newTaskActive)
                {
                    throw new InvalidOperationException("Новая задача неактивна");
                }
                existing.TaskId = updatedEntry.TaskId;
            }

            if (updatedEntry.Date != default)
            {
                existing.Date = updatedEntry.Date;
            }
            if (updatedEntry.Hours > 0)
            {
                if (updatedEntry.Hours > 24)
                {
                    throw new ArgumentException("Часы не могут быть больше 24", nameof(updatedEntry.Hours));
                }
                existing.Hours = updatedEntry.Hours;
            }
            if (updatedEntry.Description != null)
            {
                existing.Description = updatedEntry.Description;
            }

            var totalHours = await GetTotalHoursForDateAsync(existing.Date, existing.Id);
            if (totalHours + existing.Hours > 24)
            {
                throw new InvalidOperationException("Суммарное время за день не может превышать 24 часа");
            }

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}