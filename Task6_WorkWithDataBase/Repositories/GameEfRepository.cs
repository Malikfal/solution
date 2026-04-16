using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task6_WorkWithDataBase.Data;
using Task6_WorkWithDataBase.Models;

namespace Task6_WorkWithDataBase.Repositories
{
    /// <summary>
    /// Реализация репозитория с использованием Entity Framework
    /// </summary>
    public class GameEfRepository : IGameRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор, создающий контекст EF Core на основе строки подключения
        /// </summary>
        /// <param name="connectionString">Строка подключения к SQL Server</param>
        public GameEfRepository(string connectionString)
        {
            _context = new AppDbContext(connectionString);
        }

        /// <summary>
        /// Добавляет новую игру в базу данных
        /// </summary>
        public void Create(Game game)
        {
            if (game.GameID == Guid.Empty)
            {
                game.GameID = Guid.NewGuid();
            }
            _context.Games.Add(game);
            _context.SaveChanges();
        }

        /// <summary>
        /// Находит игру по её идентификатору
        /// </summary>
        public Game? Read(Guid id)
        {
            return _context.Games.Find(id);
        }

        /// <summary>
        /// Возвращает первые N игр
        /// </summary>
        public IEnumerable<Game> ReadTop(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Нельзя вывести игр меньше 1");
            }

            return _context.Games.Take(count).ToList();
        }

        /// <summary>
        /// Обновляет существующую игру, если игра найдена
        /// </summary>
        public void Update(Game game)
        {
            var existing = _context.Games.Find(game.GameID);
            if (existing != null)
            {
                existing.Title = game.Title;
                existing.MinPlayers = game.MinPlayers;
                existing.MaxPlayers = game.MaxPlayers;
                existing.CreatedDate = game.CreatedDate;
                existing.IsActive = game.IsActive;
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Удаляет игру из базы данных
        /// </summary>
        public void Delete(Guid id)
        {
            var game = _context.Games.Find(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                _context.SaveChanges();
            }
        }
    }
}
