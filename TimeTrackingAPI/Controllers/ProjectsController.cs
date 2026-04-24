using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrackingAPI.Data;
using TimeTrackingAPI.Models;

namespace TimeTrackingAPI.Controllers
{
    /// <summary>
    /// Контроллер для проектов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Инициализирует контроллер
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если контекст не задан</exception>
        public ProjectsController(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Контекст БД не задан");
            }
            _context = context;
        }

        /// <summary>
        /// Получить список всех проектов
        /// </summary>
        /// <returns>Список проектов</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        /// <summary>
        /// Получить проект по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор проекта</param>
        /// <returns>Проект, если найден, иначе 404</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        /// <summary>
        /// Обновить существующий проект
        /// </summary>
        /// <param name="id">Идентификатор обновляемого проекта</param>
        /// <param name="project">Новые данные</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (project == null)
            {
                return NotFound();
            }

            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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
        /// Создать новый проект
        /// </summary>
        /// <param name="project">Данные проекта</param>
        /// <returns>Созданные проект</returns>
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            if (project == null)
            {
                return NotFound();
            }
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        /// <summary>
        /// Удаляет проект
        /// </summary>
        /// <param name="id">Идентификатор проекта</param>
        /// <returns>204 No Content при успехе, 409 Conflict если есть задачи</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Проверяет существует ли проект
        /// </summary>
        /// <param name="id">Идентиикатор проекта</param>
        /// <returns>True при существовании</returns>
        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
