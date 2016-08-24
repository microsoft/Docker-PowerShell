using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.PowerShell.Support
{
    internal class PipeReadStream : Stream
    {
        public PipeReadStream(Pipe pipe)
        {
            _pipe = pipe;
        }

        private Pipe _pipe;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

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

        public override void Write(byte[] buffer, int offset, int count)
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

        public override int Read(byte[] buffer, int offset, int count)
        {
            var task = _pipe.ReadAsync(buffer, offset, count, CancellationToken.None);
            task.Wait();
            return task.Result;
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _pipe.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public void Close(Exception e)
        {
            _pipe.CloseReader(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pipe.CloseReader();
            }
        }
    }
}