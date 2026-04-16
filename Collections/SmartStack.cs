using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    /// <summary>
    /// Класс реализующий структуру данных "Умный стек"
    /// </summary>
    /// <typeparam name="T">Тип элементов стека</typeparam>
    internal class SmartStack<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _count;

        /// <summary>
        /// Конструктор по умолчанию, начальная емкость 4
        /// </summary>
        public SmartStack()
        {
            _items = new T[4];
            _count = 0;
        }

        /// <summary>
        /// Конструктор с указанием начальной емкости
        /// </summary>
        /// <param name="capacity">Начальная емкость стека</param>
        public SmartStack(int capacity)
        {
            _items = new T[capacity];
            _count = 0;
        }

        /// <summary>
        /// Конструктор, заполняющий стек элементами из коллекции
        /// </summary>
        /// <param name="collection">Коллекция элементов для добавления</param>
        public SmartStack(IEnumerable<T> collection)
        {
            int count = 0;
            foreach (var item in collection) { 
                count++;
            }

            _items = new T[count];
            _count = count;

            int index = 0;
            foreach (var item in collection)
            {
                _items[index] = item;
                index++;
            }
        }

        /// <summary>
        /// Количество элементов в стеке
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Текущая емкость стека
        /// </summary>
        public int Capacity => _items.Length;

        /// <summary>
        /// Добавляет элемент в вершину стека
        /// </summary>
        /// <param name="item">Добавляемый элемент</param>
        public void Push(T item)
        {
            if (_count == _items.Length)
            {
                Array.Resize(ref _items, _items.Length * 2);
            }
            _items[_count++] = item;
        }

        /// <summary>
        /// Добавляет несколько элементов в вершину стека
        /// </summary>
        /// <param name="collection">Коллекция добавляемых элементов</param>
        public void PushRange(IEnumerable<T> collection)
        {
            int countItems = 0;
            foreach (T item in collection) {  
                countItems++;
            }
            
            int newCount = _count + countItems;

            if (newCount > _items.Length)
            {
                int newCapacity = _items.Length;
                while (newCapacity < newCount)
                {
                    newCapacity *= 2;
                }
                    
                Array.Resize(ref _items, newCapacity);
            }

            foreach (var item in collection)
            {
                _items[_count++] = item;
            }
                
        }

        /// <summary>
        /// Удаляет и возвращает элемент из вершины стека
        /// </summary>
        /// <returns>Верхний элемент стека</returns>
        /// <exception cref="InvalidOperationException">Стек пуст</exception>
        public T Pop()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек пуст");
            }
                
            
            _count--;
            T item = _items[_count];
            _items[_count] = default(T);

            return item;
        }

        /// <summary>
        /// Возвращает верхний элемент стека без удаления
        /// </summary>
        /// <returns>Верхний элемент стека</returns>
        /// <exception cref="InvalidOperationException">Стек пуст</exception>
        public T Peek()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек пуст");
            }
                
            
            return _items[_count - 1];
        }

        /// <summary>
        /// Проверяет наличие элемента в стеке
        /// </summary>
        /// <param name="item">Искомый элемент</param>
        /// <returns>true, если элемент найден, иначе false</returns>
        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (Equals(_items[i], item))
                {
                    return true;
                }
                    
            }
            return false;
        }

        /// <summary>
        /// Реализация необобщенного интерфейса IEnumerable
        /// </summary>
        /// <returns>Перечислитель элементов стека</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Возвращает перечислитель для обхода стека сверху вниз
        /// </summary>
        /// <returns>Перечислитель элементов стека</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _count - 1; i >= 0; i--)
            {
                yield return _items[i];
            }
                
        }
    }
}
