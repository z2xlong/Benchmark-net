using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benchmark.Core.String2Int;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benchmark.Core;

namespace Benchmark.Core.String2Int.Tests
{
    [TestClass()]
    public class StringExtensionTests
    {
        [TestMethod]
        public void TestEmptyStr()
        {
            int count = 0;
            foreach (var num in "".SplitByComma())
            {
                count++;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void TestRedundentSplitor()
        {
            List<string> strs = new List<string>();
            foreach (var s in ",aa,,,bb,".SplitByComma())
            {
                strs.Add(s);
            }
            Assert.AreEqual(2, strs.Count);
            Assert.AreEqual("bb", strs[1]);
        }

        [TestMethod]
        public void TestNormalString()
        {
            List<string> strs = new List<string>();
            foreach (var s in " cc ,aa,11,22,bb".SplitByComma())
            {
                strs.Add(s);
            }
            Assert.AreEqual(5, strs.Count);
            Assert.AreEqual("cc", strs[0]);
            Assert.AreEqual("bb", strs[4]);
        }

        [TestMethod]
        public void TestWithoutSplitor()
        {
            List<string> strs = new List<string>();
            foreach (var s in "cc1122".SplitByComma())
            {
                strs.Add(s);
            }
            Assert.AreEqual(1, strs.Count);
            Assert.AreEqual("cc1122", strs[0]);
        }


        [TestMethod]
        public void TestUnTrimedStr()
        {
            List<string> strs = new List<string>();
            foreach (var s in " , ac da , ,, bb , ".SplitByComma())
            {
                strs.Add(s);
            }
            Assert.AreEqual(2, strs.Count);
            Assert.AreEqual("ac da", strs[0]);
            Assert.AreEqual("bb", strs[1]);
        }

        [TestMethod]
        public void TestSplitors()
        {
            List<string> strs = new List<string>();
            foreach (var s in ",,,,,,".SplitByComma())
            {
                strs.Add(s);
            }
            Assert.AreEqual(0, strs.Count);
        }

    }
}