using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var stack = new SmartStack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            Console.WriteLine("Данные в стеке (конструктор по умолчанию): ");
            foreach (var item in stack)
                Console.Write(item + " ");


            var stack2 = new SmartStack<int>(4);
            stack2.Push(4);
            stack2.Push(5);
            stack2.Push(6);
            stack2.Push(9);
            Console.WriteLine("\nДанные в стеке2 (с указанием ёмкости): ");
            foreach (var item in stack2)
                Console.Write(item + " ");


            var list = new List<int> { 1, 2, 3, 4 };
            var stack3 = new SmartStack<int>(list);
            Console.WriteLine("\nДанные в стеке3 (Из коллеции): ");
            foreach (var item in stack3)
                Console.Write(item + " ");


            stack.PushRange(new int[] { 10, 20, 30, 40 });
            Console.WriteLine("\nДанные в стеке (конструктор по умолчанию после PushRange): ");
            foreach (var item in stack)
                Console.Write(item + " ");

            Console.WriteLine("\n\nРабота команд с первым стеком:");
            foreach (var item in stack)
                Console.Write(item + " ");
            Console.WriteLine($"\nPop: {stack.Pop()}");
            Console.WriteLine($"Peek: {stack.Peek()}");
            Console.WriteLine($"Contains 5: {stack.Contains(5)}");
            Console.WriteLine($"Count: {stack.Count}, Capacity: {stack.Capacity}");
        }
    }
}
