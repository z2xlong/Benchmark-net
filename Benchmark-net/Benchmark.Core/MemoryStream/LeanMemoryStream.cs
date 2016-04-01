namespace Benchmark.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    [Serializable]
    public class LeanMemoryStream : Stream
    {
        const int MaxStreamLength = Int32.MaxValue;

        byte[] _buffer;
        int _origin;    // For user-provided arrays, start at this origin
        int _position;  // read/write head.
        int _length;    // Number of bytes within the memory stream
        int _capacity;  // length of usable portion of buffer for stream
        // Note that _capacity == _buffer.Length for non-user-provided byte[]'s

        bool _disposed;
        bool _writable;     // Can user write to this stream?
        bool _exposable;    // Whether the array can be returned to the user.
        bool _recyclable;   // User-provided buffer aren't recyclable.

        #region Ctor

        public LeanMemoryStream() : this(0) { }

        public LeanMemoryStream(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", "capacity can not be negative.");

            _buffer = ByteArrayPool.Get(capacity);
            _capacity = _buffer.Length;
            _origin = 0;      // Must be 0 for byte[]'s created by MemoryStream

            _writable = true;
            _exposable = true;
            //_expandable = true;
            //_isOpen = true;
        }

        public LeanMemoryStream(byte[] buffer)
            : this(buffer, true)
        { }

        public LeanMemoryStream(byte[] buffer, bool writable)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "buffer can not be null.");

            _buffer = buffer;
            _length = _capacity = buffer.Length;

            _writable = writable;
            _origin = 0;
            //_exposable = false;
            //_isOpen = true;
        }

        public LeanMemoryStream(byte[] buffer, int index, int count)
            : this(buffer, index, count, true, false)
        { }

        public LeanMemoryStream(byte[] buffer, int index, int count, bool writable)
            : this(buffer, index, count, writable, false)
        { }

        public LeanMemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "buffer can not be null.");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "index can not be less than 0.");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "count can not be less than 0.");
            if (buffer.Length - index < count)
                throw new ArgumentException("index plus count should be less than buffer.Length.");

            _buffer = buffer;
            _origin = _position = index;
            _length = _capacity = index + count;
            _writable = writable;
            _exposable = publiclyVisible;  // Can TryGetBuffer/GetBuffer return the array?
            //_expandable = false;
            //_isOpen = true;
        }
        #endregion

        #region Stream Members

        public override bool CanRead
        {
            get { return !_disposed; }
        }

        public override bool CanSeek
        {
            get { return !_disposed; }
        }

        public override bool CanWrite
        {
            get { return !_disposed; }
        }

        public override long Length
        {
            get
            {
                this.CheckDisposed();
                return _length;
            }
        }

        public override long Position
        {
            get
            {
                CheckDisposed();
                return _position;
            }
            set
            {
                CheckDisposed();
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "value must be non-negative");

                if (value > MaxStreamLength)
                    throw new ArgumentOutOfRangeException("value", "value cannot be more than " + MaxStreamLength);

                _position = (int)value;
            }
        }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            CheckDisposed();
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "offset cannot be negative");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "count cannot be negative");
            if (buffer.Length - offset < count)
                throw new ArgumentException("buffer length must be at least offset + count");

            int remaining = _length - _position;
            if (remaining > count) remaining = count;
            if (remaining <= 0)
                return 0;


            if (remaining <= 8)
            {
                int byteCount = remaining;
                while (--byteCount >= 0)
                    buffer[offset + byteCount] = _buffer[_position + byteCount];
            }
            else
                Buffer.BlockCopy(_buffer, _position, buffer, offset, remaining);
            _position += remaining;

            return remaining;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            _length = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBuffer()
        {
            CheckDisposed();

        }

        #endregion

        #region IDispose

        ~LeanMemoryStream()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
            }

            // We do not currently use unmanaged resources
            // but we have to recycle buffer.
            ByteArrayPool.Recycle(_buffer);
            _disposed = true;
            base.Dispose(disposing);
        }

        #endregion

        #region helper
        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("The LeanMemoryStream is disposed.");
        }
    }

    #endregion

}
}
