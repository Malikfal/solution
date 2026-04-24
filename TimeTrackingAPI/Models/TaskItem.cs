using System.Text.Json.Serialization;

namespace TimeTrackingAPI.Models
{
    /// <summary>
    /// Задача
    /// </summary>
    public class TaskItem
    {
        /// <summary>
        /// Уникальный идентификатор задачи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// ID проекта, к которому относится задача
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Активна ли задача
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Проект задачи
        /// </summary>
        [JsonIgnore]
        public Project? Project { get; set; }

        /// <summary>
        /// Проводки времени по этой задаче
        /// </summary>
        [JsonIgnore]
        public ICollection<TimeEntry>? TimeEntries { get; set; }
    }
}