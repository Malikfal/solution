using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOP
{
    internal class Program
    {
        public class Product
        {
            private string _name;
            private string _manufacturer;
            private double _price;
            private DateTime _shelfLife;
            private DateTime _productionDate;

            public Product()
            {
                _name = "Неизвестно";
                _manufacturer = "Неизвестно";
                _price = 0;
                _productionDate = DateTime.Now;
                _shelfLife = DateTime.Now.AddDays(30);
            }

            public Product(string name, string manufacturer,
                double price, DateTime productionDate, DateTime shelfLife)
            {
                Name = name;
                Manufacturer = manufacturer;
                Price = price;
                ProductionDate = productionDate;
                _shelfLife = shelfLife;
            }

            public string Name { get; set; }
            public string Manufacturer { get; set; }

            public double Price
            {
                get => _price;
                set
                {
                    if (value <= 0) 
                        throw new ArgumentException("Цена должна быть больше 0");
                    _price = value;
                }
            }

            public DateTime ProductionDate
            {
                get => _productionDate;
                set
                {
                    if (value.Date > DateTime.Now.Date)
                        throw new ArgumentException("Дата производства не может быть позже текущего дня");
                    
                    _productionDate = value;
                }
            }

            public DateTime ShelfLife
            {
                get => _shelfLife;
                set
                {
                    if (value >= _productionDate)
                        _shelfLife = value;
                    else
                        throw new ArgumentException("Дата окончания срока годности не может быть раньше даты производства");
                }
            }

            public override string ToString()
            {
                return $"Товар: {Name}\n" +
                       $"Производитель: {Manufacturer}\n" +
                       $"Цена: {Price:F2} руб.\n" +
                       $"Дата производства: {ProductionDate:dd.MM.yyyy}\n" +
                       $"Срок годности: {ShelfLife:dd.MM.yyyy}";
            }
        }


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

        public static Product s_ReadProductFromConsole()
        {
            Console.Write("Введите наименование товара: ");
            string name = Console.ReadLine();

            Console.Write("Введите производителя: ");
            string manufacturer = Console.ReadLine();

            Console.Write("Введите цену (в рублях): ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Введите дату производства (дд.мм.гггг): ");
            DateTime productionDate = DateTime.ParseExact(
                Console.ReadLine(), "dd.MM.yyyy", null);

            Console.Write("Введите дату окончания срока годности (дд.мм.гггг): ");
            DateTime shelfLife = DateTime.ParseExact(
                Console.ReadLine(), "dd.MM.yyyy", null);

            Product product = new Product(name,manufacturer, price, productionDate, shelfLife);
            return product;
        }
    }
}
