using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.Enumerator
{
    interface ITree<T> : IEnumerable<T>
    {
        T[] InternalTree { get; }
    }
}
