using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    internal class SmartStack<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _count;

        public SmartStack()
        {
            _items = new T[4];
            _count = 0;
        }

        public SmartStack(int capacity)
        {
            _items = new T[capacity];
            _count = 0;
        }

        public SmartStack(IEnumerable<T> collection)
        {
            int count = 0;
            foreach (var item in collection) count++;

            _items = new T[count];
            _count = count;

            int index = 0;
            foreach (var item in collection)
            {
                _items[index] = item;
                index++;
            }
        }

        public int Count => _count;
        public int Capacity => _items.Length;

        public void Push(T item)
        {
            if (_count == _items.Length)
                Array.Resize(ref _items, _items.Length * 2);           

            _items[_count++] = item;
        }

        public void PushRange(IEnumerable<T> collection)
        {
            int countItems = 0;
            foreach (T item in collection)
                countItems++;
            
            int newCount = _count + countItems;

            if (newCount > _items.Length)
            {
                int newCapacity = _items.Length;
                while (newCapacity < newCount) 
                    newCapacity *= 2;
                Array.Resize(ref _items, newCapacity);
            }

            foreach (var item in collection)
                _items[_count++] = item;
        }

        public T Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек пуст");
            
            _count--;
            T item = _items[_count];
            _items[_count] = default(T);

            return item;
        }

        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек пуст");
            
            return _items[_count - 1];
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (Equals(_items[i], item))
                    return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _count - 1; i >= 0; i--)
                yield return _items[i];
        }
    }
}
