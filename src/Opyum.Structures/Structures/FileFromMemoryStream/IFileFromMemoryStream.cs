using System.IO;
using System;
using System.Threading.Tasks;

namespace Opyum.Structures.FileSupport
{
    public interface IFileFromMemoryStream : IDisposable
    {
        /// <summary>
        /// The location of the file loaded into the memory.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Informatin about the file loaded into the memory.
        /// </summary>
        FileInfo FileInformation { get; set; }

        BufferingStatus BufferingState { get; }

        /// <summary>
        /// Return true if the buffer is empty or non-existent.
        /// </summary>
        bool BufferEmpty { get; }

        /// <summary>
        /// The number of bytes in the buffer. (Length of the buffer)
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Gets or sets percentage of the current position in the buffer.
        /// <para>The range for this value is from 0 to 1.</para>
        /// <para>Setting this value outside of the range will trigger an <see cref="ArgumentOutOfRangeException"/>.</para>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        double PositionPercentage { get; set; }

        /// <summary>
        /// Returns the current position (the current byte) in the buffer.
        /// </summary>
        long Position { get; set; }

        /// <summary>
        /// If set to true will wait till it can read the requested number of data (if possible).
        /// </summary>
        bool WaitForRead { get; set; }

        /// <summary>
        /// Used to reload if necessary.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="Exception"/>
        IFileFromMemoryStream Reload();

        Stream GetStream();

        /// <summary>
        /// Returns the internal buffer.
        /// </summary>
        byte[] GetBuffer();

        /// <summary>
        /// Coppies the intermal memory buffer into a new array and returns that.
        /// </summary>
        byte[] GetBufferClone();

        /// <summary>
        /// Reads the cached data from the memory into the buffer.
        /// </summary>
        /// <param name="buffer">The buffer the data is being coppied to.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        int Read(byte[] buffer, int offset, int count);

        /// <summary>
        /// Reads the cached data from the memory into the buffer.
        /// </summary>
        /// <param name="buffer">The buffer the data is being coppied to.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        Task<int> ReadAsync(byte[] buffer, int offset, int count);
    }
}