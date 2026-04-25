using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TimeTrackingAPI.Data;
using TimeTrackingAPI.Models;

namespace TimeTrackingAPI.Controllers
{
    /// <summary>
    /// Контроллер для задач
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        /// <summary>
        /// Контекст БД
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Инициализирует контроллер задач
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если контекст не задан</exception>
        public TasksController(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Контекст БД не задан");
            }
            _context = context;
        }

        /// <summary>
        /// Получить список всех задач
        /// </summary>
        /// <returns>Список задач</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        /// <summary>
        /// Получить задачу по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Залача, если найдена, иначе 404</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return taskItem;
        }

        /// <summary>
        /// Обновить существующую задачу
        /// </summary>
        /// <param name="id">Идентификатор обновляемой задачи</param>
        /// <param name="taskItem">Новые данные</param>
        /// <returns>204 No Content при успехе, иначе код ошибки</returns>
        /// <exception cref="ArgumentNullException">Если новые данные не заданы</exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(int id, TaskItem taskItem)
        {
            if (taskItem == null)
            {
                throw new ArgumentNullException("Задача не найдена");
            }

            if (id != taskItem.Id)
            {
                return BadRequest("Id в URL и в теле не совпадают");
            }

            _context.Entry(taskItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Создать новую задачу
        /// </summary>
        /// <param name="taskItem">Данные задачи</param>
        /// <returns>Созданная задача</returns>
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTaskItem(TaskItem taskItem)
        {
            _context.Tasks.Add(taskItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskItem", new { id = taskItem.Id }, taskItem);
        }

        /// <summary>
        /// Удалить задачу. Удаление возможно только если у задачи нет связанных проводок
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>204 No Content при успехе, 409 Conflict если есть проводки</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var taskItem = await _context.Tasks
                .Include(t => t.TimeEntries)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            if (taskItem.TimeEntries != null && taskItem.TimeEntries.Any())
            {
                return Conflict("Нельзя удалить задачу, так как на неё есть связанные проводки");
            }

            _context.Tasks.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Проверяет существование задачи по Id
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>True при существовании</returns>
        private bool TaskItemExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
