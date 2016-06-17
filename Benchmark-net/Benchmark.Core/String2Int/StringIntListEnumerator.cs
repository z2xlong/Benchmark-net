using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core
{
    public struct StringIntListEnumerator : IEnumerator<int>, IEnumerator, IEnumerable<int>
    {
        static int _maxTen = int.MaxValue / 10;
        static int _maxMod = int.MaxValue % 10;

        readonly string _source;
        int _index, _current;
        bool _sourceIsEmpty;

        public StringIntListEnumerator(string s)
        {
            _index = 0;
            _current = 0;
            _source = s;
            _sourceIsEmpty = string.IsNullOrWhiteSpace(_source);
        }

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            if (_sourceIsEmpty)
                return false;

            for (; _index < _source.Length && (_source[_index] < '0' || _source[_index] > '9'); _index++) ;
            if (_index >= _source.Length)
                return false;

            int result = 0, n = 0;
            bool negative = _index > 0 && _source[_index - 1] == '-';

            for (; _index < _source.Length; _index++)
            {
                n = _source[_index] - '0';
                if (n < 0 || n > 9)
                    break;

                if (result > _maxTen || (result == _maxTen && n > _maxMod))
                    return false;

                result = result * 10 + n;
            }
            _current = negative ? 0 - result : result;

            return true;
        }

        public void Reset()
        {
            _index = 0;
        }

        public int Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        #region Enumerable

        public StringIntListEnumerator GetEnumerator()
        {
            return this;
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
