namespace Benchmark.Core.Enumerator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public struct FlatternTreeEnumerator : IEnumerator<int>, IEnumerator
    {
        private int[] _tree;
        private int _currentIndex;
        private int _current;

        public FlatternTreeEnumerator(AbstractTree tree)
        {
            _tree = tree.InternalTree;
            _currentIndex = 0;
            _current = 0;
        }

        public int Current
        {
            get
            {
                return _current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                Console.WriteLine("boxing");
                return Current;
            }
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            if (_currentIndex < _tree.Length)
            {
                _current = _tree[_currentIndex];
                _currentIndex++;
                return true;
            }
            else
                return false;
        }

        public void Reset()
        {
            _currentIndex = 0;
            _current = 0;
        }
    }
}