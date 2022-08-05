using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostArkLogger.LarkCustom
{
    /// <summary>
    /// A log class implemented as a circular queue.
    /// </summary>
    public class LogQueue<T> : IEnumerable<T>, ICollection<T>
    {
        private T[] queue;
        
        private int currentIndex = 0;
        private int start = 0;

        public int Length { get => queue.Length; }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => false;

        public LogQueue(int length)
        {
            queue = new T[length];
        }

        public T Add(T item)
        {
            if (currentIndex == start)
            {
                start++;
            }
            T old = queue[currentIndex];
            queue[currentIndex] = item;
            return old;
        }

        public T Pop()
        {
            return queue[start++];
        }

        public T Peek()
        {
            return queue[start];
        }

        public T[] GetQueueContent()
        {
            T[] content = new T[queue.Length];
            for (int i = start, x = 0 ; i < Length; x++, i++)
            {
                content[x] = queue[i % Length];
            }
            return content;
        }

        public void GetQueueContent(ref T[] outbound, int startInd = 0)
        {
            for (int i = start; i < Length && startInd < outbound.Length; startInd++, i++)
            {
                outbound[startInd] = queue[i % Length];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Add(T item)
        {
            if (currentIndex == start)
            {
                start++;
            }
            queue[currentIndex] = item;
        }

        public void Clear()
        {
            for (int i = 0; i < Length; i++)
            {
                queue[i] = default(T);
            }
            currentIndex = 0;
            start = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < Length; i++)
            {
                if (queue.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = start; i < Length && arrayIndex < array.Length; arrayIndex++, i++)
            {
                array[arrayIndex] = queue[i % Length];
            }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }
    }
}
