using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.Structures.Playlist
{
    [Serializable]
    public class BlockStack<T> : Stack, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>, IDisposable, ICloneable, IEnumerator<T>, IBlockStack<T>
    {

        #region Constructor

        public BlockStack()
        {
            Header = new Stack<T>();
            Elements = new Stack<T>();
            Footer = new Stack<T>();
        }

        public BlockStack(int capacity) : base()
        {
            Elements = null;
            Elements = new Stack<T>(capacity);
            GC.Collect();
        }

        /// <summary>
        /// Putse the <paramref name="collection"/> into the elements stack,
        /// so that the first item in <paramref name="collection"/> is
        /// the top item on the stack.
        /// </summary>
        /// <param name="collection"></param>
        public BlockStack(IEnumerable<T> collection) : base()
        {
            Elements = null;
            Elements = new Stack<T>(collection);
            GC.Collect();
        }

        #endregion

        /// <summary>
        /// Returns the number of items in the <see cref="BlockStack{T}"/>.
        /// </summary>
        new public int Count
        {
            get
            {
                int _count = 0;
                if (Header != null)
                {
                    _count += Header.Count;
                }
                if (Elements != null)
                {
                    _count += Elements.Count;
                }
                if (Footer != null)
                {
                    _count += Footer.Count;
                }
                if (UseSeparator && Elements != null && Elements.Count > 0)
                {
                    _count += Elements.Count - 1;
                }
                return _count;
            }
        }

        /// <summary>
        /// Returns <see cref="true"/> it the <see cref="Separator"/> should be put in between 2 elements.
        /// </summary>
        public bool UseSeparator { get; set; } = false;
        protected internal bool nextSeparator = false;

        private int _position = -1;


        /// <summary>
        /// Removes all objects form <see cref="Header"/>, <see cref="Elements"/>, <see cref="Footer"/> and <see cref="Separator"/>.
        /// </summary>
        new public void Clear()
        {
            Header.Clear();
            Elements.Clear();
            Footer.Clear();
            Separator = default(T);
        }


        /// <summary>
        /// Determines whether an element is in the <see cref="BlockStack{T}"/>
        /// </summary>
        /// <param name="item">
        /// The object to locate in the System.Collections.Generic.Stack`1. The value can
        /// be null for reference types.
        /// </param>
        /// <returns></returns>
        public bool Contains(T item) => Header.Contains(item) ? true : Elements.Contains(item) ? true : Footer.Contains(item) ? true & UseSeparator : Separator.Equals(item) ? true : false;

        /// <summary>
        /// Copies the System.Collections.Generic.Stack`1 to an existing one-dimensional
        /// <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array to which the content is being coppied to.</param>
        /// <param name="arrayIndex">The index of the starting position in the <paramref name="array"/>
        /// from which the items are going to start beeing coppied to.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">arrayIndex is less than zero, or excedes the array's length.</exception>
        /// <exception cref="ArgumentException">The number of elements in the <see cref="BlockStack"/> is greater than the available space in the array.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is null.", new Exception());
            }
            if (arrayIndex + Count > array.Length)
            {
                throw new ArgumentException("The numer of items is greater than the available space in the array.", new Exception());
            }
            if (arrayIndex < 0 || arrayIndex > array.Length - 1)
            {
                throw new ArgumentOutOfRangeException("arrayIndex is less than zero, or excedes the array's space.", new Exception());
            }
            Array.Copy(this.ToArray(), 0, array, arrayIndex, Count);
        }

        /// <summary>
        /// Shows the top item in the <see cref="BlockStack{T}"/> without poping it.
        /// </summary>
        /// <returns></returns>
        new public T Peek() => this.ToList().FirstOrDefault();

        /// <summary>
        /// Gets the top element of the stack, or throws <see cref="InvalidOperationException"/> if the stack is empty.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        new public T Pop()
        {
            if (Header != null && Header.Count > 0)
            {
                return Header.Pop();
            }
            else if (Elements != null && Elements.Count > 0)
            {
                if (!UseSeparator)
                {
                    return Elements.Pop();
                }
                else
                {
                    if (nextSeparator)
                    {
                        nextSeparator = false;
                        return Separator;
                    }
                    else
                    {
                        nextSeparator = true;
                        return Elements.Pop();
                    }
                }
            }
            else if (Footer != null && Footer.Count > 0)
            {
                return Footer.Pop();
            }
            throw new InvalidOperationException($"The {this.GetType().ToString()} is empty.", new Exception());
        }

        /// <summary>
        /// Pushes the item to the <see cref="Elements"/> Stack.
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            Elements.Push(item);
        }

        /// <summary>
        /// Copies the <see cref="BlockStack{T}"/> to a new array.
        /// </summary>
        /// <returns>A new array containing copies of the elements of the <see cref="BlockStack{T}"/>.</returns>
        new public T[] ToArray()
        {
            List<T> list = new List<T>();
            bool separation = false;

            foreach (var item in Header.ToArray())
            {
                list.Add(item);
            }
            foreach (var item in Elements.ToArray())
            {
                if (separation)
                {
                    list.Add(Separator);
                }
                if (UseSeparator)
                {
                    separation = true;
                }
                list.Add(item);
            }
            foreach (var item in Footer.ToArray())
            {
                list.Add(item);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public void TrimExcess()
        {

        }

        new public IEnumerator<T> GetEnumerator()
        {
            return this;
        }
        /// <summary>
        ///  Creates a shallow copy of the <see cref="BlockStack{T}"/>
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new BlockStack<T>()
            {
                Header = (Stack<T>)((ICloneable)Header).Clone(),
                Elements = (Stack<T>)((ICloneable)Elements).Clone(),
                Footer = (Stack<T>)((ICloneable)Footer).Clone(),
                Separator = Separator,
                UseSeparator = UseSeparator
            };
        }


        public Stack<T> Header { get; set; } = null;
        public Stack<T> Elements { get; set; } = null;
        public Stack<T> Footer { get; set; } = null;
        public T Separator { get; set; } = default(T);



        /// <summary>
        /// Copies the <see cref="BlockStack{T}"/> to an existing one-dimensional
        /// <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The array to which the content is being coppied to.</param>
        /// <param name="index">The index of the starting position in the <paramref name="array"/>
        /// from which the items are going to start beeing coppied to.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">index is less than zero, or excedes the array's length.</exception>
        /// <exception cref="ArgumentException">The number of elements in the <see cref="BlockStack"/> is greater than the available space in the array.</exception>
        public override void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array is null.", new Exception());
            }
            if (index + Count > array.Length)
            {
                throw new ArgumentException("The numer of items is greater than the available space in the array.", new Exception());
            }
            if (index < 0 || index > array.Length - 1)
            {
                throw new ArgumentOutOfRangeException("arrayIndex is less than zero, or excedes the array's space.", new Exception());
            }
            Array.Copy(this.ToArray(), 0, array, index, Count);
        }

        IEnumerator IEnumerable.GetEnumerator() => this;

        /// <summary>
        /// Get the current item for the <see cref="IEnumerator{T}"/>
        /// </summary>
        public T Current => ToArray()[_position];

        /// <summary>
        /// Get the current object for the <see cref="IEnumerator{T}"/>
        /// </summary>
        object IEnumerator.Current => Current;

        public bool MoveNext() => (++_position) >= Count ? false : true;

        public void Reset() => _position = -1;


        #region BlocStack

        /// <summary>
        /// Inserts the <paramref name="collection"/> into the <see cref="Elements"/> <see cref="Stack"/>.
        /// </summary>
        /// <param name="collection"></param>
        public void InsertElements(IEnumerable<T> collection)
        {
            Elements?.GetEnumerator().Dispose();
            Elements = null;
            Elements = new Stack<T>(collection);
        }

        /// <summary>
        /// Returns an <see cref="Array"/> with items from the <see cref="Elements"/> <see cref="Stack"/>.
        /// </summary>
        /// <returns></returns>
        public T[] GetElements() => Elements.ToArray();


        /// <summary>
        /// Inserts the <paramref name="collection"/> into the <see cref="Header"/> <see cref="Stack"/>.
        /// </summary>
        /// <param name="collection"></param>
        public void InsertHeader(IEnumerable<T> collection)
        {
            Header?.GetEnumerator().Dispose();
            Header = null;
            Header = new Stack<T>(collection);
        }

        /// <summary>
        /// Returns an <see cref="Array"/> with items from the <see cref="Header"/> <see cref="Stack"/>.
        /// </summary>
        /// <returns></returns>
        public T[] GetHeader() => Header.ToArray();


        /// <summary>
        /// Inserts the <paramref name="collection"/> into the <see cref="Footer"/> <see cref="Stack"/>.
        /// </summary>
        /// <param name="collection"></param>
        public void InsertFooter(IEnumerable<T> collection)
        {
            Footer?.GetEnumerator().Dispose();
            Footer = null;
            Footer = new Stack<T>(collection);
        }

        /// <summary>
        /// Returns an <see cref="Array"/> with items from the <see cref="Footer"/> <see cref="Stack"/>.
        /// </summary>
        /// <returns></returns>
        public T[] GetFooter() => Footer.ToArray();

        #endregion

        #region Garbage Collection

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Header.GetEnumerator().Dispose();
                Elements.GetEnumerator().Dispose();
                Footer.GetEnumerator().Dispose();
                Separator = default(T);
            }
        }

        #endregion

        #region Irrelevant

        object ICollection.SyncRoot => null;
        bool ICollection.IsSynchronized => false;

        #endregion


    }
}
