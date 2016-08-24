using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.PowerShell.Support
{
    internal class PipeWriteStream : Stream
    {
        public PipeWriteStream(Pipe pipe)
        {
            _pipe = pipe;
        }

        private Pipe _pipe;

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length { get { throw new NotImplementedException(); } }

        public override long Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _pipe.WriteAsync(buffer, offset, count, CancellationToken.None).Wait();
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _pipe.WriteAsync(buffer, offset, count, cancellationToken);
        }
        public void Close(Exception e)
        {
            _pipe.CloseWriter(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pipe.CloseWriter();
            }
            base.Dispose(disposing);
        }
    }
}