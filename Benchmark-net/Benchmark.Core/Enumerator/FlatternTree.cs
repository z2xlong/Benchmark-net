namespace Benchmark.Core.Enumerator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class FlatternTree : AbstractTree
    {
        private int _count;
        private int[] _tree;

        public int Count
        {
            get { return _count; }
        }

        public int this[int i]
        {
            get { return _tree[i]; }
            set { _tree[i] = value; }
        }

        public override int[] InternalTree
        {
            get
            {
                return _tree;
            }
        }
        public FlatternTree(int capacity)
        {
            _tree = new int[capacity];
            _count = 0;
        }

        public void Set(int index, int value)
        {
            _tree[index] = value;
        }
    }
}