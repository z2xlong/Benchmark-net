using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.Enumerator
{
    public abstract class AbstractTree : ITree<int>
    {
        public abstract int[] InternalTree { get; }

        public FlatternTreeEnumerator GetEnumerator()
        {
            return new FlatternTreeEnumerator(this);
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
