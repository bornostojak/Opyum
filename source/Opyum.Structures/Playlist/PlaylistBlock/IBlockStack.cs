using System.Collections.Generic;

namespace Opyum.Structures.Playlist
{
    public interface IBlockStack<T>
    {
        /// <summary>
        /// Returns the number of items in the <see cref="BlockStack{T}"/>.
        /// </summary>
        int Count { get; }

        Stack<T> Elements { get; set; }

        Stack<T> Footer { get; set; }

        Stack<T> Header { get; set; }

        T Separator { get; set; }
        /// <summary>
        /// Returns <see cref="true"/> it the <see cref="Separator"/> should be put in between 2 elements.
        /// </summary>
        bool UseSeparator { get; set; }


        /// <summary>
        /// Returns an <see cref="Array"/> with items from the <see cref="Elements"/> <see cref="Stack"/>.
        /// </summary>
        /// <returns></returns>
        T[] GetElements();
        /// <summary>
        /// Returns an <see cref="Array"/> with items from the <see cref="Footer"/> <see cref="Stack"/>.
        /// </summary>
        /// <returns></returns>
        T[] GetFooter();
        /// <summary>
        /// Returns an <see cref="Array"/> with items from the <see cref="Header"/> <see cref="Stack"/>.
        /// </summary>
        /// <returns></returns>
        T[] GetHeader();
        /// <summary>
        /// Inserts the <paramref name="collection"/> into the <see cref="Elements"/> <see cref="Stack"/>.
        /// </summary>
        /// <param name="collection"></param>
        void InsertElements(IEnumerable<T> collection);
        /// <summary>
        /// Inserts the <paramref name="collection"/> into the <see cref="Footer"/> <see cref="Stack"/>.
        /// </summary>
        /// <param name="collection"></param>
        void InsertFooter(IEnumerable<T> collection);
        /// <summary>
        /// Inserts the <paramref name="collection"/> into the <see cref="Header"/> <see cref="Stack"/>.
        /// </summary>
        /// <param name="collection"></param>
        void InsertHeader(IEnumerable<T> collection);
        /// <summary>
        /// Gets the top element of the stack, or throws <see cref="InvalidOperationException"/> if the stack is empty.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"/>
        T Pop();
        /// <summary>
        /// Pushes the item to the <see cref="Elements"/> Stack.
        /// </summary>
        /// <param name="item"></param>
        void Push(T item);

        void Reset();
    }
}