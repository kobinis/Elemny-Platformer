using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.XnaUtils
{
    class SmartArray<T>: IList<T>
    {
        private T[] _data;
        private int _count;
        private int _currentIndex;
        private int _maxCapacity;

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public T this[int index] { get => _data[index]; set => _data[index] = value; }

        public SmartArray(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
            _data = new T[_maxCapacity];
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(T item)
        {
            //TODO: add check that there are free elements (count < capacity)
            while(_data[_currentIndex] != null)
            {
                _currentIndex = (_currentIndex + 1) % _maxCapacity;
            }
            _data[_currentIndex] = item;
            _count++;
        }

        public void Clear()
        {
            _data = new T[_maxCapacity];
            _count = 0;
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }
    }
}
