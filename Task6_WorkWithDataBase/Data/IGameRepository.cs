using Task6_WorkWithDataBase.Models;

namespace Task6_WorkWithDataBase.Data
{
    /// <summary>
    /// Интерфейс репозитория для выполнения CRUD-операций над сущностью Game
    /// </summary>
    public interface IGameRepository
    {
        /// <summary>
        /// Создаёт новую игру в базе данных
        /// </summary>
        /// <param name="game">Объект Game для добавления игры</param>
        void Create(Game game);

        /// <summary>
        /// Получает игру по её уникальному идентификатору
        /// </summary>
        /// <param name="id">GUID игры</param>
        /// <returns>Объект Game или null, если не найдено</returns>
        Game? Read(Guid id);

        /// <summary>
        /// Возвращает первые N игр из Games
        /// </summary>
        /// <returns>Коллекция из N игр</returns>
        IEnumerable<Game> ReadTop(int count);

        /// <summary>
        /// Обновляет существующую игру
        /// </summary>
        /// <param name="game">Объект Game с изменёнными данными</param>
        void Update(Game game);

        /// <summary>
        /// Удаляет игру по идентификатору
        /// </summary>
        /// <param name="id">GUID игры для удаления</param>
        void Delete(Guid id);
    }
}
