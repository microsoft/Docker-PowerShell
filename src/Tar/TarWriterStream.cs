using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tar
{
    internal class TarWriterStream : Stream
    {
        private readonly TarWriter _writer;

        public TarWriterStream(TarWriter writer)
        {
            _writer = writer;
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanTimeout => false;

        public override bool CanWrite => true;

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
            throw new NotSupportedException();
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
            _writer.WriteCurrentFile(buffer, offset, count);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _writer.WriteCurrentFileAsync(buffer, offset, count, cancellationToken);
        }
    }
}