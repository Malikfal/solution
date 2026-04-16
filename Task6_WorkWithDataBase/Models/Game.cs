using System;

namespace Task6_WorkWithDataBase.Models
{
    /// <summary>
    /// Представляет сущность "Игра" из таблицы Games базы данных BoardGamesClub
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Уникальный идентификатор игры (первичный ключ)
        /// </summary>
        public Guid GameID { get; set; }

        /// <summary>
        /// Название игры
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Минимальное количество игроков
        /// </summary>
        public int? MinPlayers { get; set; }

        /// <summary>
        /// Максимальное количество игроков
        /// </summary>
        public int? MaxPlayers { get; set; }

        /// <summary>
        /// Дата и время создания записи
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Флаг активности записи
        /// </summary>
        public bool IsActive { get; set; }
    }
}
