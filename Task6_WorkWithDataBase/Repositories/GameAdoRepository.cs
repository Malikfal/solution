using Microsoft.Data.SqlClient;
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
    /// Репозиторий ADO.NET
    /// </summary>
    public class GameAdoRepository : IGameRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Принимает строку подключения к базе данных
        /// </summary>
        /// <param name="connectionString">Строка подключения SQL Server</param>
        public GameAdoRepository(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Строка подключения не задана");
            }
            _connectionString = connectionString;
        }

        /// <summary>
        /// Вставляет новую запись в таблицу Games
        /// </summary>
        public void Create(Game game)
        {
            if (game == null)
            {
                throw new ArgumentException("Запись не задана");
            }
            const string sql = @"
            INSERT INTO Games (GameID, Title, MinPlayers, MaxPlayers, CreatedDate, IsActive)
            VALUES (@GameID, @Title, @MinPlayers, @MaxPlayers, @CreatedDate, @IsActive)";

            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            // Генерация нового Guid, если он не задан
            cmd.Parameters.AddWithValue("@GameID", game.GameID == Guid.Empty ? Guid.NewGuid() : game.GameID);
            cmd.Parameters.AddWithValue("@Title", game.Title);
            cmd.Parameters.AddWithValue("@MinPlayers", (object?)game.MinPlayers ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaxPlayers", (object?)game.MaxPlayers ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedDate", game.CreatedDate == DateTime.MinValue ? DateTime.Now : game.CreatedDate);
            cmd.Parameters.AddWithValue("@IsActive", game.IsActive);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Читает игру по GUID
        /// </summary>
        public Game? Read(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("GUID не задан");
            }
            const string sql = """
                SELECT 
                    GameID
                    , Title
                    , MinPlayers
                    , MaxPlayers
                    , CreatedDate
                    , IsActive
                FROM Games 
                WHERE GameID = @GameID
                """;
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GameID", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Game
                {
                    GameID = reader.GetGuid(0),
                    Title = reader.GetString(1),
                    MinPlayers = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                    MaxPlayers = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    CreatedDate = reader.GetDateTime(4),
                    IsActive = reader.GetBoolean(5)
                };
            }
            return null;
        }

        /// <summary>
        /// Возвращает первые N игр из таблицы
        /// </summary>
        public IEnumerable<Game> ReadTop(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Нельзя вывести игр меньше 1");
            }

            var games = new List<Game>();
            string sql = $"""
                SELECT TOP ({count}) 
                    GameID
                    , Title
                    , MinPlayers
                    , MaxPlayers
                    , CreatedDate
                    , IsActive
                FROM Games
                """;
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                games.Add(new Game
                {
                    GameID = reader.GetGuid(0),
                    Title = reader.GetString(1),
                    MinPlayers = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                    MaxPlayers = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    CreatedDate = reader.GetDateTime(4),
                    IsActive = reader.GetBoolean(5)
                });
            }
            return games;
        }

        /// <summary>
        /// Обновляет существующую запись
        /// </summary>
        public void Update(Game game)
        {
            if (game == null)
            {
                throw new ArgumentException("Запись не задана");
            }

            const string sql = @"
                UPDATE Games 
                SET 
                    Title = @Title
                    , MinPlayers = @MinPlayers
                    , MaxPlayers = @MaxPlayers
                    , CreatedDate = @CreatedDate
                    , IsActive = @IsActive
                WHERE GameID = @GameID";

            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GameID", game.GameID);
            cmd.Parameters.AddWithValue("@Title", game.Title);
            cmd.Parameters.AddWithValue("@MinPlayers", (object?)game.MinPlayers ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaxPlayers", (object?)game.MaxPlayers ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedDate", game.CreatedDate);
            cmd.Parameters.AddWithValue("@IsActive", game.IsActive);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Удаляет игру по GUID
        /// </summary>
        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("GUID не задан");
            }
            const string sql = @"
                DELETE FROM Games
                WHERE GameID = @GameID";
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GameID", id);
            cmd.ExecuteNonQuery();
        }
    }
}
