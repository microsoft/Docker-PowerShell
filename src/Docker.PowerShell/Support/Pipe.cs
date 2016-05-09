using System;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.PowerShell.Support
{
    internal class Pipe
    {
        // The ring buffer.
        private byte[] _buffer;
        // The current insertion and removal pointers in the ring buffer.
        int _in, _out;
        // The endpoints.
        Endpoint _reader, _writer;
        bool _eof;
        // The lock protecting all the internal state.
        object _mutex = new object();

        private struct Endpoint
        {
            public TaskCompletionSource<int> PendingTask;
            public ArraySegment<byte> PendingBuffer;
            public Exception Failure;

            public TaskCompletionSource<int> ReleaseTask()
            {
                var task = PendingTask;
                PendingTask = null;
                PendingBuffer = new ArraySegment<byte>();
                return task;
            }
        }

        public Pipe(int bufferSize = 65536)
        {
            if (bufferSize == 0)
            {
                // SpaceLeft() needs a buffer size of at least 1.
                bufferSize = 1;
            }

            _buffer = new byte[bufferSize];
        }

        // Number of free bytes in the ring.
        private int SpaceLeft()
        {
            // The ring buffer cannot be filled completely since that state would
            // be indistinguishable from an empty ring, so subtract one byte from
            // the total capacity.
            return _buffer.Length - DataLeft() - 1;
        }

        // Number of data bytes in the ring.
        private int DataLeft()
        {
            return (_in >= _out) ? (_in - _out) : (_buffer.Length - (_out - _in));
        }

        // Copy between two array segments and return the number of bytes copied.
        private static int Copy(ArraySegment<byte> dest, ArraySegment<byte> source)
        {
            int count = Math.Min(dest.Count, source.Count);
            for (int i = 0; i < count; i++)
            {
                dest.Array[dest.Offset + i] = source.Array[source.Offset + i];
            }

            return count;
        }

        private static ArraySegment<byte> Advance(ArraySegment<byte> segment, int count)
        {
            return new ArraySegment<byte>(segment.Array, segment.Offset + count, segment.Count - count);
        }

        // Copies data into the ring and returns the number of bytes copied.
        private int CopyIn(ArraySegment<byte> source)
        {
            int i = 0;
            var inp = _in;
            var outp = _out;
            if (inp >= outp)
            {
                while (inp < _buffer.Length && i < source.Count)
                {
                    _buffer[inp] = source.Array[source.Offset + i];
                    i++;
                    inp++;
                }

                if (inp == _buffer.Length)
                {
                    inp = 0;
                }
            }

            while (inp + 1 < outp && i < source.Count)
            {
                _buffer[inp] = source.Array[source.Offset + i];
                i++;
                inp++;
            }

            _in = inp;
            return i;
        }

        // Copy bytes out of the ring and return the number of bytes copied.
        private int CopyOut(ArraySegment<byte> dest)
        {
            int i = 0;
            var inp = _in;
            var outp = _out;
            if (outp > inp)
            {
                while (outp < _buffer.Length && i < dest.Count)
                {
                    dest.Array[dest.Offset + i] = _buffer[outp];
                    i++;
                    outp++;
                }

                if (outp == _buffer.Length)
                {
                    outp = 0;
                }
            }

            while (outp < inp && i < dest.Count)
            {
                dest.Array[dest.Offset + i] = _buffer[outp];
                i++;
                outp++;
            }

            _out = outp;
            return i;
        }

        private Task WriteInternal(ArraySegment<byte> data, CancellationToken cancellationToken, out TaskCompletionSource<int> signaledTask, out int bytesCopied)
        {
            signaledTask = null;
            bytesCopied = 0;
            lock (_mutex)
            {
                if (_reader.Failure != null)
                {
                    throw _reader.Failure;
                }

                if (_writer.Failure != null || _eof)
                {
                    throw new InvalidOperationException("pipe already closed");
                }

                if (_writer.PendingTask != null)
                {
                    throw new InvalidOperationException("multiple concurrent writes not supported");
                }

                // Satisfy the pending reader first.
                if (_reader.PendingTask != null)
                {
                    int copied = Copy(_reader.PendingBuffer, data);
                    signaledTask = _reader.ReleaseTask();
                    bytesCopied = copied;
                    if (copied == data.Count)
                    {
                        return Task.CompletedTask;
                    }

                    // There is more data than fit in the reader's buffer.
                    data = Advance(data, copied);
                }

                // Copy data into the ring only if all of it fits. There is no sense buffering if
                // we have to block anyway.
                if (SpaceLeft() >= data.Count)
                {
                    CopyIn(data);
                    return Task.CompletedTask;
                }

                // We must block. Allocate a new task to do the blocking.
                var taskSource = new TaskCompletionSource<int>();

                // Register a cancellation callback that dequeues the task.
                if (cancellationToken.CanBeCanceled)
                {
                    cancellationToken.Register(() => CancelTask(ref _writer, taskSource));
                }

                _writer.PendingTask = taskSource;
                _writer.PendingBuffer = data;
                return taskSource.Task;
            }
        }

        public Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int copied;
            TaskCompletionSource<int> signaledTask;
            var data = new ArraySegment<byte>(buffer, offset, count);
            var task = WriteInternal(data, cancellationToken, out signaledTask, out copied);
            if (signaledTask != null)
            {
                signaledTask.SetResult(copied);
            }

            return task;
        }

        private Task<int> ReadInternal(ArraySegment<byte> data, CancellationToken cancellationToken, out TaskCompletionSource<int> signaledTask)
        {
            signaledTask = null;
            lock (_mutex)
            {
                if (_writer.Failure != null)
                {
                    throw _writer.Failure;
                }

                if (_reader.Failure != null)
                {
                    throw new InvalidOperationException("pipe already closed");
                }

                if (_reader.PendingTask != null)
                {
                    throw new InvalidOperationException("multiple concurrent reads not supported");
                }

                // If there is data in the ring, consume it directly. For simplicity, do not
                // consume writer data if there is ring data; the caller can call back again.
                if (_in != _out)
                {
                    return Task.FromResult(CopyOut(data));
                }

                if (_eof)
                {
                    // Return 0 to indicate EOF.
                    return Task.FromResult(0);
                }

                // If there is a writer, copy its data into the buffer.
                if (_writer.PendingTask != null)
                {
                    int copied = Copy(data, _writer.PendingBuffer);
                    if (copied == _writer.PendingBuffer.Count)
                    {
                        signaledTask = _writer.ReleaseTask();
                    }
                    else
                    {
                        // The writer still has data left. Copy it into the ring buffer if
                        // doing so would unblock the writer.
                        var remainingData = Advance(_writer.PendingBuffer, copied);
                        if (remainingData.Count <= SpaceLeft())
                        {
                            CopyIn(remainingData);
                            signaledTask = _writer.ReleaseTask();
                        }
                        else
                        {
                            _writer.PendingBuffer = remainingData;
                        }
                    }

                    return Task.FromResult(copied);
                }

                var taskSource = new TaskCompletionSource<int>();

                // Register a cancellation callback that dequeues the task.
                if (cancellationToken.CanBeCanceled)
                {
                    cancellationToken.Register(() => CancelTask(ref _reader, taskSource));
                }

                _reader.PendingTask = taskSource;
                _reader.PendingBuffer = data;
                return taskSource.Task;
            }
        }

        public Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var data = new ArraySegment<byte>(buffer, offset, count);
            TaskCompletionSource<int> signaledTask;
            var task = ReadInternal(data, cancellationToken, out signaledTask);
            if (signaledTask != null)
            {
                signaledTask.SetResult(0);
            }

            return task;
        }

        public void CloseReader(Exception e = null)
        {
            // Any writes after the reader has closed should cause a failure.
            if (e == null)
            {
                e = new InvalidOperationException("pipe closed");
            }

            TaskCompletionSource<int> readTask, writeTask;

            lock (_mutex)
            {
                readTask = _reader.ReleaseTask();
                writeTask = _writer.ReleaseTask();
                if (_reader.Failure == null)
                {
                    _reader.Failure = e;
                }
            }

            if (writeTask != null)
            {
                writeTask.SetException(e);
            }

            if (readTask != null)
            {
                readTask.SetException(new InvalidOperationException("pipe closed"));
            }
        }

        public void CloseWriter(Exception e = null)
        {
            TaskCompletionSource<int> readTask, writeTask;

            lock (_mutex)
            {
                readTask = _reader.ReleaseTask();
                writeTask = _writer.ReleaseTask();
                if (_writer.Failure == null)
                {
                    _writer.Failure = e;
                }

                _eof = true;
            }

            if (readTask != null)
            {
                if (e != null)
                {
                    readTask.SetException(e);
                }
                else
                {
                    readTask.SetResult(0);
                }
            }

            if (writeTask != null)
            {
                writeTask.SetException(new InvalidOperationException("pipe closed"));
            }
        }

        private void CancelTask(ref Endpoint endpoint, TaskCompletionSource<int> taskSource)
        {
            bool cancel = false;
            lock (_mutex)
            {
                if (taskSource == endpoint.PendingTask)
                {
                    endpoint.ReleaseTask();
                    cancel = true;
                }
            }

            if (cancel)
            {
                taskSource.SetCanceled();
            }
        }
    }
}