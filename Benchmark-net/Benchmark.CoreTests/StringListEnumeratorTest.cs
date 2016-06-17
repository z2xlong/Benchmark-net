using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Benchmark.Core;
using System.Collections.Generic;

namespace Benchmark.CoreTests
{
    public class StringListEnumeratorTest
    {
        [Fact]
        public void TestEmptyStr()
        {
            int count = 0;
            foreach (var num in new StringListEnumerator(""))
            {
                count++;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void TestRedundentSplitor()
        {
            List<string> strs = new List<string>();
            foreach (var s in new StringListEnumerator(",aa,,,bb,"))
            {
                strs.Add(s);
            }
            Assert.Equal(2, strs.Count);
            Assert.Equal("bb", strs[1]);
        }

        [Fact]
        public void TestUnTrimedStr()
        {
            List<string> strs = new List<string>();
            foreach (var s in new StringListEnumerator(" ,acda,,,bb, "))
            {
                strs.Add(s);
            }
            Assert.Equal(4, strs.Count);
            Assert.Equal("acda", strs[1]);
            Assert.Equal("bb", strs[2]);
        }

        [Fact]
        public void TestSplitors()
        {
            List<string> strs = new List<string>();
            foreach (var s in new StringListEnumerator(",,,,,,"))
            {
                strs.Add(s);
            }
            Assert.Equal(0, strs.Count);
        }

    }
}
