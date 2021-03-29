using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Opyum.Structures.FileSupport
{
    /// <summary>
    /// Used for caching files into memory
    /// </summary>
    public class FileFromMemoryStream : Stream, IDisposable, IFileFromMemoryStream
    {
        protected long _position = 0;
        protected byte[] memoryBuffer = new byte[0];
        protected int tempBufferSize = 1024 * 4;
        protected int writtenBytes = 0;
        protected bool isLoading = false;



        Object load_lock = new Object();

        public BufferingStatus BufferingState { get; private set; } = 0;
        public string FilePath { get; private set; } = String.Empty;
        public FileInfo FileInformation { get; set; }

        CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        /// If set to true will wait till it can read the requested number of data (if possible).
        /// </summary>
        public bool WaitForRead { get; set; } = false;
        /// <summary>
        /// Returns boolen true if stream can be read
        /// </summary>
        public override bool CanRead => true;
        /// <summary>
        /// Returns boolen true if stream can seek
        /// </summary>
        public override bool CanSeek => true;
        /// <summary>
        /// Returns boolen true if stream can be written
        /// </summary>
        public override bool CanWrite => true;
        /// <summary>
        /// Returns the number of bytes buffered in the memory
        /// </summary>
        public override long Length => memoryBuffer == null ? 0 : memoryBuffer.Length;
        /// <summary>
        /// Gets or sets the current position (the current byte) in the memory buffer
        /// </summary>
        public override long Position { get => _position; set => _position = value; }
        /// <summary>
        /// Gets or sets percentage of the current position in the buffer.
        /// <para>The range for this value is from 0 to 1.</para>
        /// <para>Setting this value outside of the range will trigger an <see cref="ArgumentOutOfRangeException"/>.</para>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public double PositionPercentage
        {
            get => Length == 0 || memoryBuffer == null ? 0 : (double)Position / (double)Length;
            set
            {
                if (value > 1 || value < 0)
                {
                    throw new ArgumentOutOfRangeException("The range for this value is from 0 to 1");
                }
                if (memoryBuffer != null && Length != 0)
                {
                    _position = (long)(value * (double)Length);
                }

            }
        }
        /// <summary>
        /// Return true if the buffer is empty or non-existent.
        /// </summary>
        public bool BufferEmpty => memoryBuffer == null || writtenBytes == 0 ? true : memoryBuffer.Length > 1024 * 64 ? false : true;

        protected FileFromMemoryStream()
        {

        }
        protected FileFromMemoryStream(string file) : base()
        {
            FilePath = file;
        }
        public FileFromMemoryStream(byte[] buffer, string file) : base()
        {
            FilePath = file;
            memoryBuffer = buffer;
        }

        ~FileFromMemoryStream()
        {
            Dispose();
        }


        /// <summary>Returns the internal buffer.</summary>
        public virtual byte[] GetBuffer() => memoryBuffer;
        public virtual byte[] GetBufferClone()
        {
            var temp = new byte[memoryBuffer.Length];
            Buffer.BlockCopy(memoryBuffer, 0, temp, 0, memoryBuffer.Length);
            return temp;
        }

        public virtual Stream GetStream() => (Stream)this;



        /// <summary>
        /// Reads the cached data from the memory into the buffer.
        /// </summary>
        /// <param name="buffer">The buffer the data is being coppied to.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (WaitForRead)
            {
                var t = ReadAsync(buffer, offset, count).Result;
                return t;
            }
            else
            {
                if (memoryBuffer == null || buffer == null)
                {
                    throw new ArgumentNullException();
                }
                if (offset < 0 || offset > buffer.Length - 1 || count > buffer.Length || buffer.Length - offset < count)
                {
                    throw new ArgumentOutOfRangeException("The Offset cannot be set to a value less then 0");
                }

                int toCopy = memoryBuffer.Length - _position < (long)count ? (int)(memoryBuffer.Length - _position) : count;
                Array.Copy(memoryBuffer, _position, buffer, offset, toCopy);


                _position += toCopy;

                return toCopy;
            }
        }

        /// <summary>
        /// Reads the cached data from the memory into the buffer.
        /// </summary>
        /// <param name="buffer">The buffer the data is being coppied to.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public new async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            if (memoryBuffer == null || buffer == null)
            {
                throw new ArgumentNullException();
            }
            if (offset < 0 || offset > buffer.Length - 1 || count > buffer.Length || buffer.Length - offset < count)
            {
                throw new ArgumentOutOfRangeException("The Offset cannot be set to a value less then 0");
            }

            if (writtenBytes - _position < count)
            {
                int pos = Int32.Parse(_position.ToString());
                var t = memoryBuffer.Length - pos < count ? memoryBuffer.Length - pos : pos + count;
                if (writtenBytes != memoryBuffer.Length)
                {
                    await WaitForBufferToFill(t); 
                }
            }

            int toCopy = memoryBuffer.Length - _position < (long)count ? (int)(memoryBuffer.Length - _position) : count;
            Array.Copy(memoryBuffer, _position, buffer, offset, toCopy);


            _position += toCopy;


            return toCopy;
        }

        /// <summary>
        /// Waits until the buffer can read the requested number of bytes if possible
        /// </summary>
        /// <param name="minWrittenBytesRequired"></param>
        /// <returns></returns>
        private async Task<int> WaitForBufferToFill(int minWrittenBytesRequired)
        {
            Task t = Task.Run(() =>
            {
                while (writtenBytes < minWrittenBytesRequired) Thread.Sleep(5);
            });

            await t;

            return writtenBytes;
        }


        /// <summary>
        /// Does nothing.
        /// </summary>
        public override void Flush() { }

        /// <summary>Sets the current position of the stream.
        /// <para>Positive and negative offsets are acceptable in differet contexts.</para>
        /// <para>If the position overshoots, it will be corrected to the stream's end.</para>
        /// <para>If the position undershoots, it will be corrected to the stream's start.</para>
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long temp = 0;
            if (origin == SeekOrigin.Begin)
            {
                _position = offset < 0 ? 0 : offset > Length ? Length : offset;
            }
            else if (origin == SeekOrigin.End)
            {
                temp = Length - offset;
                _position = temp > Length ? Length : temp < 0 ? 0 : temp;
            }
            else
            {
                temp = _position + offset;
                _position = temp > Length ? Length : temp < 0 ? 0 : temp;
            }

            return _position;
        }

        /// <summary>
        /// Loads the file into the memory buffer.
        /// </summary>
        /// <param name="file">the file path.</param>
        /// <exception cref="Exception">Unhandled exceptions</exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual void Load(string file)
        {
            if (file == null)
            {
                throw new ArgumentNullException();
            }

            CancellationToken ct = cts.Token;

            FilePath = file;
            FileInformation = new FileInfo(file);

            isLoading = true;

            lock (load_lock)
            {
                int point = 0;
                byte[] tempBuffer = new byte[tempBufferSize];
                int readBytes = 0;

                if (memoryBuffer != null)
                {
                    memoryBuffer = null;
                    GC.Collect();
                }

                memoryBuffer = new byte[FileInformation.Length];

                BufferingState = BufferingStatus.Buffering;

                #region Dynamic_Loading_Into_Memory

                MemoryStream ms = new MemoryStream(memoryBuffer);

                using (Stream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    BufferingState = BufferingStatus.Buffering;
                    do
                    {
                        if (ct.IsCancellationRequested)
                        {
                            break;
                        }
                        readBytes = fs.Read(tempBuffer, 0, tempBufferSize);
                        if (readBytes != 0)
                        {
                            ms.Write(tempBuffer, 0, readBytes);
                            point += readBytes;
                            writtenBytes += readBytes;
                        }
                    } while (readBytes > 0);
                    tempBuffer = null;
                }
                BufferingState = BufferingStatus.Done;

                ms.Dispose();
                GC.Collect();

                #endregion
            }
            isLoading = false;
        }

        /// <summary>
        /// Used to reload if necessary.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="Exception"/>
        public virtual IFileFromMemoryStream Reload()
        {
            if (FilePath == null)
            {
                throw new ArgumentNullException();
            }
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException();
            }
            var info = new FileInfo(FilePath);
            if (info.Length == 0)
            {
                throw new IOException(message: "File is empty");
            }

            if (isLoading)
            {
                CancleLoading();
            }
            int safetyCounter = 0;
            while (safetyCounter < 50)
            {
                try
                {
                    using (Stream s = new FileStream(FilePath, FileMode.Open))
                    {

                    }
                    break;
                }
                catch 
                {
                    Thread.Sleep(100);
                    safetyCounter++;
                    if (safetyCounter == 50)
                    {
                        throw new IOException("5 second timeout: the file cannot be accessed");
                    }
                }
            }
            //ThreadPool.QueueUserWorkItem((i) => this.Load(FilePath));
            Task.Run(() => this.Load(FilePath), cts.Token);
            Thread.Sleep(100);
            return this;
        }

        private void CancleLoading()
        {
            cts.Cancel();
            //cts.Dispose();
            cts = new CancellationTokenSource();
        }


        #region Garbage_Collection

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        /// <summary>
        /// Will be called when the <see cref="Dispose()"/> function is called.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            List<int> iii = new List<int>();

            if (disposing)
            {
                cts.Cancel();
                memoryBuffer = null;
                FilePath = null;
                FileInformation = null;
                load_lock = null;
            }
        }

        #endregion

        #region NOT_SUPPORTED

        /// <summary>
        /// This function is not supported
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="NotSupportedException">Will always throw an exception</exception>
        public override void SetLength(long value) => throw new NotSupportedException("Can't set length of a FileMemoryCacheStream");

        /// <summary>
        /// Function not supported.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException("You cannot alter the source, change/write to the FileMemoryCacheStream.");


        #endregion




        /// <summary>
        /// Creates a new <see cref="FileFromMemoryStream"/> from a given filepath.
        /// </summary>
        /// <param name="file">The audio file. It's not checked for being and actual audio file. That's on you</param>
        /// <returns><see cref="FileFromMemoryStream"/></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="IOException"></exception>
        public static FileFromMemoryStream Create(string file)
        {
            if (file == null)
            {
                throw new ArgumentNullException();
            }
            if (!File.Exists(file))
            {
                throw new FileNotFoundException();
            }
            var info = new FileInfo(file);
            if (info.Length == 0)
            {
                throw new IOException(message: "File is empty");
            }

            var temp = new FileFromMemoryStream(file);
            Task.Run(() => temp.Load(file), temp.cts.Token);
            Thread.Sleep(100);
            return temp;
        }

        /// <summary>
        /// Creates a new <see cref="FileFromMemoryStream"/> from a given filepath and directly loads the memory buffer.
        /// </summary>
        /// <param name="file">The audio file. It's not checked for being and actual audio file. That's on you</param>
        /// <returns><see cref="FileFromMemoryStream"/></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="IOException"></exception>
        public static FileFromMemoryStream Create(byte[] buffer, string file)
        {
            if (file == null)
            {
                throw new ArgumentNullException();
            }
            if (!File.Exists(file))
            {
                throw new FileNotFoundException();
            }
            if (buffer == null)
            {
                throw new ArgumentNullException();
            }
            if (buffer.Length == 0)
            {
                throw new IOException(message: "Memory is empty");
            }

            var temp = new FileFromMemoryStream(buffer, file);
            return temp;
        }
    }
}
