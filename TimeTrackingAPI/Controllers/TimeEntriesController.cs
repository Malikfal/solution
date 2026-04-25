using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrackingAPI.Data;
using TimeTrackingAPI.Models;
using TimeTrackingAPI.Services;

namespace TimeTrackingAPI.Controllers
{
    /// <summary>
    /// Контроллер для проводок
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TimeEntriesController : ControllerBase
    {
        /// <summary>
        /// Контекст БД
        /// </summary>
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Сервис бизнес логики проводок
        /// </summary>
        private readonly ITimeEntryService _timeEntryService;

        /// <summary>
        /// Инициализирует контроллер проводок
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="timeEntryService">Сервис бизнес логики проводок</param>
        /// <exception cref="ArgumentNullException">Если любой из параметров null</exception>
        public TimeEntriesController(ApplicationDbContext context, ITimeEntryService timeEntryService)
        {
            _context = context ?? throw new ArgumentNullException("Контекст Бд не задан");
            _timeEntryService = timeEntryService ?? throw new ArgumentNullException("Сервис проводок не задан");
        }

        /// <summary>
        /// Получить все проводки с пагинацией.
        /// </summary>
        /// <param name="skip">Количество пропущенных записей (по умолчанию 0).</param>
        /// <param name="take">Количество возвращаемых записей (макс. 1000, по умолчанию 100)</param>
        /// <returns>Список проводок</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetTimeEntries(int skip = 0, int take = 100)
        {
            take = Math.Min(take, 1000);
            var entries = await _context.TimeEntries
                .OrderByDescending(te => te.Date)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            return entries;
        }

        /// <summary>
        /// Получить проводку по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор проводки</param>
        /// <returns>Проводка, если найдена, иначе 404</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeEntry>> GetTimeEntry(int id)
        {
            var timeEntry = await _context.TimeEntries.FindAsync(id);

            if (timeEntry == null)
            {
                return NotFound();
            }

            return timeEntry;
        }

        /// <summary>
        /// Получить все проводки за конкретную дату
        /// </summary>
        /// <param name="date">Дата в формате YYYY-MM-DD</param>
        /// <returns>Список проводок за указанный день</returns>
        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetByDate(DateTime date)
        {
            var entries = await _context.TimeEntries
                .Where(te => te.Date.Date == date.Date)
                .OrderBy(te => te.Id)
                .ToListAsync();
            return entries;
        }

        /// <summary>
        /// Получить сводку по дню: сумма часов и цветной стикер (жёлтый < 8, зелёный = 8, красный > 8)
        /// </summary>
        /// <param name="date">Дата в формате YYYY-MM-DD</param>
        /// <returns>Объект с датой, суммой часов, цветом стикера и текстовым сообщением</returns>
        [HttpGet("summary/date/{date}")]
        public async Task<ActionResult<object>> GetDailySummary(DateTime date)
        {
            var total = await _context.TimeEntries
                .Where(te => te.Date.Date == date.Date)
                .SumAsync(te => te.Hours);

            string color, message;
            if (total < 8)
            {
                color = "yellow";
                message = "Внесено недостаточно часов (менее 8)";
            }
            else if (total == 8)
            {
                color = "green";
                message = "Норма часов выполнена (ровно 8)";
            }
            else
            {
                color = "red";
                message = "Часов внесено больше нормы (более 8)";
            }

            return Ok(new { Date = date, TotalHours = total, StickerColor = color, Message = message });
        }

        /// <summary>
        /// Обновить существующую проводку.
        /// Нельзя изменить задачу, если текущая задача неактивна
        /// Также проверяются лимиты часов и сумма за день
        /// </summary>
        /// <param name="id">Идентификатор обновляемой проводки</param>
        /// <param name="timeEntry">Новые данные</param>
        /// <returns>204 No Content при успехе, иначе код ошибки</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeEntry(int id, TimeEntry timeEntry)
        {
            if (id != timeEntry.Id)
                return BadRequest("Id в URL и в теле не совпадают.");

            try
            {
                await _timeEntryService.UpdateAsync(id, timeEntry);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Создать новую проводку
        /// </summary>
        /// <param name="timeEntry">Данные проводки</param>
        /// <returns>Созданная проводка</returns>
        [HttpPost]
        public async Task<ActionResult<TimeEntry>> PostTimeEntry(TimeEntry timeEntry)
        {
            try
            {
                var created = await _timeEntryService.CreateAsync(timeEntry);
                return CreatedAtAction(nameof(GetTimeEntry), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Удалить проводку
        /// </summary>
        /// <param name="id">Идентификатор проводки</param>
        /// <returns>204 No Content при успехе, иначе 404</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeEntry(int id)
        {
            var timeEntry = await _context.TimeEntries.FindAsync(id);
            if (timeEntry == null)
                return NotFound();

            _context.TimeEntries.Remove(timeEntry);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Проверяет существование проводки по Id
        /// </summary>
        /// <param name="id">Идентификатор проводки</param>
        /// <returns>True при существовании</returns>
        private bool TimeEntryExists(int id)
        {
            return _context.TimeEntries.Any(e => e.Id == id);
        }
    }
}
