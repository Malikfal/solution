using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TimeTrackingAPI.Models;

namespace TimeTrackingAPI.Data
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Набор проектов
        /// </summary>
        public DbSet<Project> Projects { get; set; }

        /// <summary>
        /// Набор задач
        /// </summary>
        public DbSet<TaskItem> Tasks { get; set; }

        /// <summary>
        /// Набор проводок времени
        /// </summary>
        public DbSet<TimeEntry> TimeEntries { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр контекста.
        /// </summary>
        /// <param name="options">Параметры подключения к БД.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        /// <summary>
        /// Настройка модели БД
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Code)
                .IsUnique();

            // Один проект может иметь много задач
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Одна задача моежт иметь много проводок
            modelBuilder.Entity<TimeEntry>()
                .HasOne(te => te.Task)
                .WithMany(t => t.TimeEntries)
                .HasForeignKey(te => te.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimeEntry>()
                .Property(te => te.Hours)
                .HasPrecision(5, 2);
        }
    }
}