using System;

namespace OOP
{
    internal partial class Program
    {
        /// <summary>
        /// Товар
        /// </summary>
        public class Product
        {
            private string _name;
            private string _manufacturer;
            private double _price;
            private DateTime _shelfLife;
            private DateTime _productionDate;

            /// <summary>
            /// Срок годности по умолчанию в днях
            /// </summary>
            private const int DEFAULT_SHELF_LIFE_DAYS = 30;

            /// <summary>
            /// Инициализирует продукт со значениями по умолчанию
            /// </summary>
            public Product()
            {
                _name = "Неизвестно";
                _manufacturer = "Неизвестно";
                _price = 1;
                _productionDate = DateTime.Now;
                _shelfLife = DateTime.Now.AddDays(DEFAULT_SHELF_LIFE_DAYS);
            }

            /// <summary>
            /// Инициализирует продукт по заданным значениям
            /// </summary>
            /// <param name="name">Наименование товара</param>
            /// <param name="manufacturer">Производитель</param>
            /// <param name="price">Цена</param>
            /// <param name="productionDate">Дата производства</param>
            /// <param name="shelfLife">Дата окончания срока годности</param>
            public Product(string name, string manufacturer,
                double price, DateTime productionDate, DateTime shelfLife)
            {
                Name = name;
                Manufacturer = manufacturer;
                Price = price;
                ProductionDate = productionDate;
                _shelfLife = shelfLife;
            }

            /// <summary>
            /// Наименование товара
            /// </summary>
            public string Name
            {
                get => _name;
                set
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new ArgumentException("Название не должно быть пустым");
                    }
                    _name = value;
                }
            }

            /// <summary>
            /// Производитель
            /// </summary>
            public string Manufacturer
            {
                get => _manufacturer;
                set
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new ArgumentException("Производетель не должен быть пустым");
                    }
                    _manufacturer = value;
                }
            }

            /// <summary>
            /// Цена товара в рублях
            /// </summary>
            public double Price
            {
                get => _price;
                set
                {
                    if (value <= 0)
                    {
                        throw new ArgumentException("Цена должна быть больше 0");
                    }
                        
                    _price = value;
                }
            }

            /// <summary>
            /// Дата производства товара
            /// </summary>
            public DateTime ProductionDate
            {
                get => _productionDate;
                set
                {
                    if (value.Date > DateTime.Now.Date)
                    {
                        throw new ArgumentException("Дата производства не может быть позже текущего дня");
                    }                
                    
                    _productionDate = value;
                }
            }

            /// <summary>
            /// Дата окончания срока годности
            /// </summary>
            public DateTime ShelfLife
            {
                get => _shelfLife;
                set
                {
                    if (value < _productionDate)
                    {
                        throw new ArgumentException("Дата окончания срока годности не может быть раньше даты производства");
                    }

                    _shelfLife = value;

                }
            }

            /// <summary>
            /// Возвращает строковое представление товара
            /// </summary>
            /// <returns>Строка с информацией о товаре</returns>
            public override string ToString()
            {
                return $"Товар: {Name}\n" +
                       $"Производитель: {Manufacturer}\n" +
                       $"Цена: {Price:F2} руб.\n" +
                       $"Дата производства: {ProductionDate:dd.MM.yyyy}\n" +
                       $"Срок годности: {ShelfLife:dd.MM.yyyy}";
            }
        }
    }
}
