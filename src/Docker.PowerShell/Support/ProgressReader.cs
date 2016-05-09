using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.PowerShell.Support
{
    internal class ProgressReader : Stream
    {
        public struct Status
        {
            public bool Complete;
            public long TotalBytesRead;
        }

        public ProgressReader(Stream stream, IProgress<Status> progress, long byteDelta = 0)
        {
            _stream = stream;
            _progress = progress;
            _byteDelta = byteDelta;
        }

        private Stream _stream;
        private IProgress<Status> _progress;
        private Status _status;
        private long _lastReport = 0;
        private long _byteDelta = 0;

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

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = _stream.Read(buffer, offset, count);
            Update(read);
            return read;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int read = await _stream.ReadAsync(buffer, offset, count, cancellationToken);
            Update(read);
            return read;
        }

        private void Update(int read)
        {
            if (read == 0)
            {
                _status.Complete = true;
            }

            _status.TotalBytesRead += read;
            if (_status.Complete || _lastReport + _byteDelta <= _status.TotalBytesRead)
            {
                _progress.Report(_status);
                _lastReport = _status.TotalBytesRead;
            }
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
            throw new NotImplementedException();
        }
    }
}