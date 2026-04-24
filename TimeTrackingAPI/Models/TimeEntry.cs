using System.Text.Json.Serialization;

namespace TimeTrackingAPI.Models
{
    /// <summary>
    /// Проводка
    /// </summary>
    public class TimeEntry
    {
        /// <summary>
        /// Уникальный идентификатор проводки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата выполнения работ
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Количество часов (от 0 до 24)
        /// </summary>
        public decimal Hours { get; set; }

        /// <summary>
        /// Описание выполненной работы
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ID задачи, на которую списывается время
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Задача проводки
        /// </summary>
        [JsonIgnore]
        public TaskItem? Task { get; set; }
    }
}