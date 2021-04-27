using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.StandardPlayback
{
    /// <summary>
    /// Used to read a certain number of bytes ahead of the current stream position
    /// </summary>
    /// <exception cref="InvalidOperationException">Ceartain stream functions are not implemented, unnecessary</exception>
    public class ReadAheadStream : Stream
    {
        private readonly Stream sourceStream;
        private long pos; // psuedo-position
        private readonly byte[] readAheadBuffer;
        private int readAheadLength;
        private int readAheadOffset;

        public ReadAheadStream(Stream sourceStream)
        {
            this.sourceStream = sourceStream;
            readAheadBuffer = new byte[1024 * 4]; //4kB buffer
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => pos;

        public override long Position { get => pos; set => throw new InvalidOperationException(); }


        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = 0;
            while (bytesRead < count)
            {
                int readAheadAvailableBytes = readAheadLength - readAheadOffset;
                int bytesRequired = count - bytesRead;
                if (readAheadAvailableBytes > 0)
                {
                    int toCopy = Math.Min(readAheadAvailableBytes, bytesRequired);
                    Array.Copy(readAheadBuffer, readAheadOffset, buffer, offset + bytesRead, toCopy);
                    bytesRead += toCopy;
                    readAheadOffset += toCopy;
                }
                else
                {
                    readAheadOffset = 0;
                    readAheadLength = sourceStream.Read(readAheadBuffer, 0, readAheadBuffer.Length);
                    if (readAheadLength == 0)
                    {
                        break;
                    }
                }
            }
            pos += bytesRead;
            return bytesRead;
        }

        public override void Flush()
        {
            throw new InvalidOperationException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }
    }
}
