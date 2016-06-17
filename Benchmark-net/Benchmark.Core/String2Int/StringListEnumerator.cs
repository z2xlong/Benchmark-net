using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core
{
    public struct StringListEnumerator : IEnumerator<string>, IEnumerator, IEnumerable<string>
    {
        int _start, _end;

        readonly int _len;
        readonly string _str;
        readonly char[] _splitors;

        public StringListEnumerator(string s, char[] splitors)
        {
            _start = 0;
            _end = -1;

            _str = s;
            _len = string.IsNullOrWhiteSpace(_str) ? 0 : _str.Length;

            _splitors = splitors;
        }

        public StringListEnumerator(string s) : this(s, new char[] { ',' }) { }

        public StringListEnumerator(string s, char splitor) : this(s, new char[] { splitor }) { }

        public bool MoveNext()
        {
            if (_len <= 0)
                return false;

            for (_start = _end + 1; _start < _len && IsSplitor(_str[_start]); _start++) ;

            for (_end = _start; _end < _len && !IsSplitor(_str[_end]); _end++) ;

            return _end > _start;
        }

        bool IsSplitor(char ch)
        {
            for (int i = 0; i < _splitors.Length; i++)
            {
                if (_splitors[i] == ch)
                    return true;
            }
            return false;
        }


        public void Reset()
        {
            _start = 0;
            _end = -1;
        }

        public string Current
        {
            get
            {
                return _str.Substring(_start, _end - _start);
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        #region Enumerable

        public StringListEnumerator GetEnumerator()
        {
            this.Reset();
            return this;
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {

        }
        #endregion

    }
}
