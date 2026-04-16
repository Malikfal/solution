using Microsoft.Extensions.Configuration;
using Task6_WorkWithDataBase.Data;
using Task6_WorkWithDataBase.Models;
using Task6_WorkWithDataBase.Repositories;

namespace Task6_WorkWithDataBase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Загрузка конфигурации из appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string? connectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Ошибка: строка подключения не найдена в appsettings.json");
                return;
            }


            Console.WriteLine($"Сервер: {connectionString.Split(';')[0]}\n");

            while (true)
            {
                Console.WriteLine("Выберите технологию доступа:");
                Console.WriteLine("1 - ADO.NET");
                Console.WriteLine("2 - Entity Framework");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");
                string? choiceTech = Console.ReadLine();
                string technologyName = "";
                IGameRepository? repository = null;
                if (choiceTech == "1")
                {
                    repository = new GameAdoRepository(connectionString);
                    technologyName = "ADO.NET";
                }
                if (choiceTech == "2")
                {
                    repository = new GameEfRepository(connectionString);
                    technologyName = "Entity Framework";
                }
                if (choiceTech == "0")
                {
                    break;
                }


                if (repository == null)
                {
                    Console.WriteLine("Неверный выбор.\n");
                    continue;
                }

                // Меню CRUD операций
                while (true)
                {
                    Console.WriteLine($"\n--- Текущая технология ({technologyName}) ---");
                    Console.WriteLine("--- Операции с играми (Games) ---");
                    Console.WriteLine("1 - Добавить игру");
                    Console.WriteLine("2 - Показать N игр");
                    Console.WriteLine("3 - Найти игру по ID");
                    Console.WriteLine("4 - Обновить игру");
                    Console.WriteLine("5 - Удалить игру");
                    Console.WriteLine("0 - Назад (сменить технологию)");
                    Console.Write("Ваш выбор: ");
                    string? choiceCrud = Console.ReadLine();

                    if (choiceCrud == "0") break;

                    switch (choiceCrud)
                    {
                        case "1": // Create
                            var newGame = s_ReadGameFromConsole();
                            repository.Create(newGame);
                            Console.WriteLine($"Игра '{newGame.Title}' добавлена. ID: {newGame.GameID}");
                            break;

                        case "2": // ReadTop
                            var games = repository.ReadTop(int.Parse(s_ReadString("Введите кол-во игр для вывода: ")));
                            if (!games.Any())
                            {
                                Console.WriteLine("Игры отсутствуют.");
                            }

                            else
                            {
                                foreach (var g in games)
                                {
                                    s_PrintGameInfo(g);
                                }

                            }
                                
                            break;

                        case "3": // Read by ID
                            if (Guid.TryParse(s_ReadString("Введите GUID игры: "), out Guid readId))
                            {
                                var game = repository.Read(readId);
                                if (game == null)
                                {
                                    Console.WriteLine("Игра не найдена.");
                                }
                                else
                                {
                                    s_PrintGameInfo(game);
                                }
                                    
                            }
                            else
                            {
                                Console.WriteLine("Неверный формат GUID.");
                            }
                                
                            break;

                        case "4": // Update
                            if (Guid.TryParse(s_ReadString("Введите GUID игры для обновления: "), out Guid updateId))
                            {
                                var existing = repository.Read(updateId);
                                if (existing == null)
                                {
                                    Console.WriteLine("Игра не найдена.");
                                    break;
                                }
                                Console.WriteLine($"Текущее название: {existing.Title}");
                                var updated = s_ReadGameFromConsole();
                                updated.GameID = updateId;
                                repository.Update(updated);
                                Console.WriteLine("Игра обновлена.");
                            }
                            else
                            {
                                Console.WriteLine("Неверный GUID.");
                            }
                                
                            break;

                        case "5": // Delete
                            if (Guid.TryParse(s_ReadString("Введите GUID игры для удаления: "), out Guid deleteId))
                            {
                                repository.Delete(deleteId);
                                Console.WriteLine("Игра удалена (если существовала).");
                            }
                            else
                            {
                                Console.WriteLine("Неверный GUID.");
                            }
                                
                            break;

                        default:
                            Console.WriteLine("Неверная операция.");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Выводит информацию о игре
        /// </summary>
        /// <param name="game">Заполненный объект Game</param>
        private static void s_PrintGameInfo(Game game)
        {
            Console.WriteLine($"ID: {game.GameID}," +
                            $" Название: {game.Title}," +
                            $" Игроков: {game.MinPlayers}–{game.MaxPlayers}," +
                            $" Активна: {game.IsActive}");
        }

        /// <summary>
        /// Считывает с консоли данные для новой игры
        /// </summary>
        /// <returns>Заполненный объект Game</returns>
        public static Game s_ReadGameFromConsole()
        {
            var game = new Game();
            game.Title = s_ReadString("Название игры: ") ?? "";
            game.MinPlayers = int.TryParse(s_ReadString("Минимум игроков: "), out int min) ? min : null;
            game.MaxPlayers = int.TryParse(s_ReadString("Максимум игроков: "), out int max) ? max : null;
            game.CreatedDate = DateTime.Now;
            game.IsActive = true;
            return game;
        }

        /// <summary>
        /// Ввод строки пользователем через консоль с указанной для него информацией
        /// </summary>
        /// <param name="prompt">Информация для пользователя</param>
        /// <returns>Введённая пользователем строка</returns>
        public static string s_ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
