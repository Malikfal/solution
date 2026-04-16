using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    /// <summary>
    /// Программа для демонстрации работы SmartStack
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Точка входа в программу
        /// </summary>
        /// <param name="args">Аргументы командной строки</param>
        static void Main(string[] args)
        {
            var stack = new SmartStack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            DisplayCollection("\nДанные в стеке (конструктор по умолчанию): ", stack);

            var stack2 = new SmartStack<int>(4);
            stack2.Push(4);
            stack2.Push(5);
            stack2.Push(6);
            stack2.Push(9);
            DisplayCollection("\nДанные в стеке2 (с указанием ёмкости): ", stack2);


            var list = new List<int> { 1, 2, 3, 4 };
            var stack3 = new SmartStack<int>(list);
            DisplayCollection("\nДанные в стеке3 (Из коллеции): ", stack3);


            stack.PushRange(new int[] { 10, 20, 30, 40 });
            DisplayCollection("\nДанные в стеке (конструктор по умолчанию после PushRange): ", stack);

            Console.WriteLine("\n\nРабота команд с первым стеком:");
            DisplayCollection("", stack);
            Console.WriteLine($"\nPop: {stack.Pop()}");
            Console.WriteLine($"Peek: {stack.Peek()}");
            Console.WriteLine($"Contains 5: {stack.Contains(5)}");
            Console.WriteLine($"Count: {stack.Count}, Capacity: {stack.Capacity}");
        }

        /// <summary>
        /// Выводит информацию пользователя и все элементы коллекции в консоль
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="prompt">Текст заданные пользователем</param>
        /// <param name="collection">Коллекция для отображения</param>
        public static void DisplayCollection<T>(string prompt, IEnumerable<T> collection)
        {
            Console.Write(prompt);
            foreach (var item in collection)
            {
                Console.Write(item + " ");
            }          
        }
    }
}
