using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOP
{
    /// <summary>
    /// Программа для работы с классом Product
    /// </summary>
    internal partial class Program
    {

        /// <summary>
        /// Точка входа в программу
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                Product product = s_ReadProductFromConsole();
                Console.WriteLine("\n" + product.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            
        }

        /// <summary>
        /// Считывает данные о продукте с консоли и создает объект Product
        /// </summary>
        /// <returns>Объект Product с введёнными данными</returns>
        public static Product s_ReadProductFromConsole()
        {
            string name = s_ReadString("Введите наименование товара: ");

            string manufacturer = s_ReadString("Введите производителя: ");

            double price = double.Parse(s_ReadString("Введите цену (в рублях): "));

            DateTime productionDate = DateTime.ParseExact(
                s_ReadString("Введите дату производства (дд.мм.гггг): "),
                "dd.MM.yyyy", null);

            DateTime shelfLife = DateTime.ParseExact(
                s_ReadString("Введите дату окончания срока годности (дд.мм.гггг): "),
                "dd.MM.yyyy", null);

            return new Product(name, manufacturer, price, productionDate, shelfLife);
        }

        /// <summary>
        /// Читает строку с консоли
        /// </summary>
        /// <param name="prompt">Подсказка для пользователя</param>
        /// <returns>Введённая строка</returns>
        private static string s_ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
