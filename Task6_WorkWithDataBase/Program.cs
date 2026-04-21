using Microsoft.Extensions.Configuration;
using Task6_WorkWithDataBase.Data;
using Task6_WorkWithDataBase.Models;
using Task6_WorkWithDataBase.Repositories;
using Bogus;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            // Проверка
            IGameRepository? repository = new GameAdoRepository(connectionString);
            //repository = new GameEfRepository(connectionString);


            // 1) Добавление новой игры
            var faker = new Faker();
            var gameFaker = new Faker<Game>()
                .StrictMode(true)
                .RuleFor(g => g.GameID, f => f.Random.Guid())                     
                .RuleFor(g => g.Title, f => f.Commerce.ProductName())
                .RuleFor(g => g.MinPlayers, f => f.Random.Int(1, 8))
                .RuleFor(g => g.MaxPlayers, f => (f.Random.Int(2, 10)))
                .RuleFor(g => g.CreatedDate, f => f.Date.Past(5))
                .RuleFor(g => g.IsActive, f => f.Random.Bool());

            var fakeGame = gameFaker.Generate();
            repository.Create(fakeGame);
            Console.WriteLine($"Игра '{fakeGame.Title}' добавлена. ID: {fakeGame.GameID}");

            // 2) Вывод указанного кол-ва игр

            Random random = new Random();
            int gamesCount = random.Next(1, 10);
            var games = repository.ReadTop(gamesCount);
            Console.WriteLine($"Первые {gamesCount} игр");
            if (!games.Any())
            {
                Console.WriteLine("Игры отсутствуют.");
            }

            else
            {
                foreach (var g in games)
                {
                    Console.WriteLine(g.ToString());
                }

            }

            // 3) Вывод игры по GUID
            Console.WriteLine("Вывод игры по Guid: A0A24A16-EEED-40FE-9D5C-12700597995B");
            Guid readId = Guid.Parse("A0A24A16-EEED-40FE-9D5C-12700597995B");
            var game = repository.Read(readId);
            if (game == null)
            {
                Console.WriteLine("Игра не найдена.");
            }
            else
            {
                Console.WriteLine(game.ToString());
            }


            // 4) Обновление игры по GUID
            var existing = repository.Read(readId);
            Console.WriteLine("Была игра: " + existing.ToString());
            if (existing == null)
            {
                Console.WriteLine("Игра не найдена.");
            }
            Console.WriteLine($"Текущее название: {existing.Title}");
            var updated = gameFaker.Generate();
            updated.GameID = existing.GameID;
            repository.Update(updated);
            Console.WriteLine($"Игра обновлена: {updated.ToString()}");


        }


        /// <summary>
        /// Считывает с консоли данные для новой игры
        /// </summary>
        /// <returns>Заполненный объект Game</returns>
        public static Game s_ReadGameFromConsole()
        {
            var game = new Game();
            game.Title = s_ReadString("Название игры для добавления: ") ?? "";
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
