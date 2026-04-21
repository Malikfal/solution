using Microsoft.EntityFrameworkCore;
using Task6_WorkWithDataBase.Models;

namespace Task6_WorkWithDataBase.Data
{
    /// <summary>
    /// Контекст Entity Framework Core для работы с базой данных BoardGamesClub
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует новый экземпляр контекста с заданной строкой подключения
        /// </summary>
        /// <param name="connectionString">Строка подключения к SQL Server</param>
        public AppDbContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Строка подключения не задана");
            }
            _connectionString = connectionString;
        }

        /// <summary>
        /// Набор сущностей Games, отображаемый на таблицу Games
        /// </summary>
        public DbSet<Game> Games { get; set; }

        /// <summary>
        /// Настраивает подключение к базе данных, используя SQL Server
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        /// <summary>
        /// Настраивает модель таблицы Games
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Games");
                entity.HasKey(e => e.GameID);
                entity.Property(e => e.GameID).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.MinPlayers);
                entity.Property(e => e.MaxPlayers);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });
        }
    }
}
