using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tar
{
    internal class TarReaderStream : Stream
    {
        private readonly TarReader _reader;

        public TarReaderStream(TarReader reader)
        {
            _reader = reader;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanTimeout => false;

        public override bool CanWrite => false;

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _reader.ReadCurrentFile(buffer, offset, count);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _reader.ReadCurrentFileAsync(buffer, offset, count, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}