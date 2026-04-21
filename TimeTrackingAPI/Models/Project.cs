namespace TimeTrackingAPI.Models
{
    /// <summary>
    /// Проект
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Уникальный идентификатор проекта
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название проекта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Уникальный код проекта
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Активен ли проект
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Список задач, принадлежащих проекту
        /// </summary>
        public ICollection<TaskItem>? Tasks { get; set; }
    }
}