namespace Benchmark.Core
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [Serializable]
    [ComVisible(true)]
    public class LeanMemoryStream : Stream
    {
        #region fields

        const int MaxByteArrayLength = 0x7FFFFFC7;
        const int MemStreamMaxLength = Int32.MaxValue;

        private byte[] _buffer;    // Either allocated internally or externally.
        private int _origin;       // For user-provided arrays, start at this origin
        private int _position;     // read/write head.
        private int _length;       // Number of bytes within the memory stream
        private int _capacity;     // length of usable portion of buffer for stream
        // Note that _capacity == _buffer.Length for non-user-provided byte[]'s

        private bool _expandable;  // User-provided buffers aren't expandable.
        private bool _writable;    // Can user write to this stream?
        private bool _exposable;   // Whether the array can be returned to the user.
        private bool _disposed;      // Is this stream open or closed?
        private bool _recyclable = true;  // User-provided && handle leaked via GetBuffer() method buffers aren't recyclable.


        #endregion

        #region ctor

        public LeanMemoryStream() : this(0) { }

        public LeanMemoryStream(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", "ArgumentOutOfRange_NegativeCapacity");
            }

            _buffer = ByteArrayPool.Get(capacity);
            _capacity = capacity;
            _expandable = true;
            _writable = true;
            _exposable = true;
            _origin = 0;      // Must be 0 for byte[]'s created by MemoryStream
        }

        public LeanMemoryStream(byte[] buffer)
            : this(buffer, true)
        {
        }

        public LeanMemoryStream(byte[] buffer, bool writable)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "ArgumentNull_Buffer");

            _buffer = buffer;
            _length = _capacity = buffer.Length;
            _writable = writable;
            _exposable = false;
            _origin = 0;
            _recyclable = false;
        }

        public LeanMemoryStream(byte[] buffer, int index, int count)
            : this(buffer, index, count, true, false)
        {
        }

        public LeanMemoryStream(byte[] buffer, int index, int count, bool writable)
            : this(buffer, index, count, writable, false)
        {
        }

        public LeanMemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "ArgumentNull_Buffer");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "ArgumentOutOfRange_NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_NeedNonNegNum");
            if (buffer.Length - index < count)
                throw new ArgumentException("Argument_InvalidOffLen");

            _buffer = buffer;
            _origin = _position = index;
            _length = _capacity = index + count;
            _writable = writable;
            _exposable = publiclyVisible;  // Can TryGetBuffer/GetBuffer return the array?
            _expandable = false;
            _recyclable = false;
        }

        #endregion

        #region Status Properties
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
            get { return !_disposed && _writable; }
        }

        public virtual bool CanRecycledOutside
        {
            get { return !_recyclable; }
        }

        #endregion

        public override void Flush()
        {
        }

        public virtual byte[] GetBuffer()
        {
            if (!_exposable)
                throw new UnauthorizedAccessException("UnauthorizedAccess_MemStreamBuffer");
            _recyclable = false;
            return _buffer;
        }

        public virtual bool TryGetBuffer(out ArraySegment<byte> buffer)
        {
            if (!_exposable)
            {
                buffer = default(ArraySegment<byte>);
                return false;
            }

            _recyclable = false;
            buffer = new ArraySegment<byte>(_buffer, offset: _origin, count: (_length - _origin));
            return true;
        }

        // Gets & sets the capacity (number of bytes allocated) for this stream.
        // The capacity cannot be set to a value less than the current length
        // of the stream.
        // 
        public virtual int Capacity
        {
            get
            {
                CheckDisposed();
                return _capacity - _origin;
            }
            set
            {
                // Only update the capacity if the MS is expandable and the value is different than the current capacity.
                // Special behavior if the MS isn't expandable: we don't throw if value is the same as the current capacity
                if (value < Length) throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_SmallCapacity");

                CheckDisposed();
                if (!_expandable && (value != Capacity))
                    throw new InvalidOperationException("LeanMemoryStream not expandable.");

                // MemoryStream has this invariant: _origin > 0 => !expandable (see ctors)
                if (_expandable && value != _capacity)
                {
                    ByteArrayPool.Recycle(ref _buffer);
                    if (value > 0)
                    {
                        byte[] newBuffer = ByteArrayPool.Get(value);
                        if (_length > 0) Buffer.BlockCopy(_buffer, 0, newBuffer, 0, _length);
                        _buffer = newBuffer;
                    }
                    else {
                        _buffer = null;
                    }
                    _capacity = value;
                }
            }
        }

        public override long Length
        {
            get
            {
                CheckDisposed();
                return _length - _origin;
            }
        }

        public override long Position
        {
            get
            {
                CheckDisposed();
                return _position - _origin;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_NeedNonNegNum");

                CheckDisposed();

                if (value > MemStreamMaxLength)
                    throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_StreamLength");
                _position = _origin + (int)value;
            }
        }

        public override int Read([In, Out] byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "ArgumentNull_Buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_NeedNonNegNum");
            if (buffer.Length - offset < count)
                throw new ArgumentException("Argument_InvalidOffLen");

            CheckDisposed();

            int n = _length - _position;
            if (n > count) n = count;
            if (n <= 0)
                return 0;

            if (n <= 8)
            {
                int byteCount = n;
                while (--byteCount >= 0)
                    buffer[offset + byteCount] = _buffer[_position + byteCount];
            }
            else
                Buffer.BlockCopy(_buffer, _position, buffer, offset, n);
            _position += n;

            return n;
        }

        public override int ReadByte()
        {
            CheckDisposed();

            if (_position >= _length) return -1;

            return _buffer[_position++];
        }

        public override long Seek(long offset, SeekOrigin loc)
        {
            CheckDisposed();

            if (offset > MemStreamMaxLength)
                throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_StreamLength");
            switch (loc)
            {
                case SeekOrigin.Begin:
                    {
                        int tempPosition = unchecked(_origin + (int)offset);
                        if (offset < 0 || tempPosition < _origin)
                            throw new IOException("IO.IO_SeekBeforeBegin");
                        _position = tempPosition;
                        break;
                    }
                case SeekOrigin.Current:
                    {
                        int tempPosition = unchecked(_position + (int)offset);
                        if (unchecked(_position + offset) < _origin || tempPosition < _origin)
                            throw new IOException("IO.IO_SeekBeforeBegin");
                        _position = tempPosition;
                        break;
                    }
                case SeekOrigin.End:
                    {
                        int tempPosition = unchecked(_length + (int)offset);
                        if (unchecked(_length + offset) < _origin || tempPosition < _origin)
                            throw new IOException("IO.IO_SeekBeforeBegin");
                        _position = tempPosition;
                        break;
                    }
                default:
                    throw new ArgumentException("Argument_InvalidSeekOrigin");
            }

            return _position;
        }

        // Sets the length of the stream to a given value.  The new
        // value must be nonnegative and less than the space remaining in
        // the array, Int32.MaxValue - origin
        // Origin is 0 in all cases other than a MemoryStream created on
        // top of an existing array and a specific starting offset was passed 
        // into the MemoryStream constructor.  The upper bounds prevents any 
        // situations where a stream may be created on top of an array then 
        // the stream is made longer than the maximum possible length of the 
        // array (Int32.MaxValue).
        // 
        public override void SetLength(long value)
        {
            if (value < 0 || value > Int32.MaxValue)
            {
                throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_StreamLength");
            }
            EnsureWriteable();

            // Origin wasn't publicly exposed above.
            if (value > (Int32.MaxValue - _origin))
            {
                throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_StreamLength");
            }

            int newLength = _origin + (int)value;
            bool allocatedNewArray = EnsureCapacity(newLength);
            if (!allocatedNewArray && newLength > _length)
                Array.Clear(_buffer, _length, newLength - _length);
            _length = newLength;
            if (_position > newLength) _position = newLength;

        }

        [Obsolete("This method has degraded performance vs. GetBuffer and should be avoided.")]
        public virtual byte[] ToArray()
        {
            if (!_expandable)
                throw new InvalidOperationException("MemoryStream::GetBuffer will let you avoid a copy.");
            byte[] copy = ByteArrayPool.Get(_length - _origin);
            Buffer.BlockCopy(_buffer, _origin, copy, 0, _length - _origin);
            return copy;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "ArgumentNull_Buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_NeedNonNegNum");
            if (buffer.Length - offset < count)
                throw new ArgumentException("Argument_InvalidOffLen");

            CheckDisposed();
            EnsureWriteable();

            int i = _position + count;
            // Check for overflow
            if (i < 0)
                throw new IOException("IO.IO_StreamTooLong");

            if (i > _length)
            {
                bool mustZero = _position > _length;
                if (i > _capacity)
                {
                    bool allocatedNewArray = EnsureCapacity(i);
                    if (allocatedNewArray)
                        mustZero = false;
                }
                if (mustZero)
                    Array.Clear(_buffer, _length, i - _length);
                _length = i;
            }
            if ((count <= 8) && (buffer != _buffer))
            {
                int byteCount = count;
                while (--byteCount >= 0)
                    _buffer[_position + byteCount] = buffer[offset + byteCount];
            }
            else
                Buffer.BlockCopy(buffer, offset, _buffer, _position, count);
            _position = i;

        }

        public override void WriteByte(byte value)
        {
            CheckDisposed();
            EnsureWriteable();

            if (_position >= _length)
            {
                int newLength = _position + 1;
                bool mustZero = _position > _length;
                if (newLength >= _capacity)
                {
                    bool allocatedNewArray = EnsureCapacity(newLength);
                    if (allocatedNewArray)
                        mustZero = false;
                }
                if (mustZero)
                    Array.Clear(_buffer, _length, _position - _length);
                _length = newLength;
            }
            _buffer[_position++] = value;

        }

        // Writes this MemoryStream to another stream.
        public virtual void WriteTo(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "ArgumentNull_Stream");

            CheckDisposed();
            stream.Write(_buffer, _origin, _length - _origin);
        }

        #region help methods

        private void CheckDisposed()
        {
            if (_disposed)
                throw new InvalidOperationException("LeanMemoryStream is disposed.");
        }

        private void EnsureWriteable()
        {
            if (!CanWrite)
                throw new InvalidOperationException("LeanMemoryStream cannot be writed.");
        }

        // returns a bool saying whether we allocated a new array.
        private bool EnsureCapacity(int value)
        {
            // Check for overflow
            if (value < 0)
                throw new IOException("IO.IO_StreamTooLong");
            if (value > _capacity)
            {
                int newCapacity = value;
                if (newCapacity < 256)
                    newCapacity = 256;
                // We are ok with this overflowing since the next statement will deal
                // with the cases where _capacity*2 overflows.
                if (newCapacity < _capacity * 2)
                    newCapacity = _capacity * 2;
                // We want to expand the array up to Array.MaxArrayLengthOneDimensional
                // And we want to give the user the value that they asked for
                if ((uint)(_capacity * 2) > MaxByteArrayLength)
                    newCapacity = value > MaxByteArrayLength ? value : MaxByteArrayLength;

                Capacity = newCapacity;
                return true;
            }
            return false;
        }

        #endregion

        #region IDispose

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _disposed = true;
                    _writable = false;
                    _expandable = false;

                    GC.SuppressFinalize(this);
                }
            }
            finally
            {
                if (_recyclable)
                    ByteArrayPool.Recycle(ref _buffer);
                base.Dispose(disposing);
            }
        }

        #endregion

    }
}

