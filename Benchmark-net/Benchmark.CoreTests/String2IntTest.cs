using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Benchmark.Core;
using System.Collections.Generic;

namespace Benchmark.CoreTests
{
    public class String2IntTest
    {
        [FactAttribute]
        public void TestEmptyStr()
        {
            int count = 0;
            foreach (var num in new StringIntListEnumerator(""))
            {
                count++;
            }
            Assert.Equal(0, count);
        }

        [FactAttribute]
        public void TestOneNum()
        {
            List<int> nums = new List<int>();
            foreach (var num in new StringIntListEnumerator("100"))
            {
                nums.Add(num);
            }
            Assert.Equal(1, nums.Count);
            Assert.Equal(100, nums[0]);
        }

        [FactAttribute]
        public void TestUnTrim()
        {
            List<int> nums = new List<int>();
            foreach (var num in new StringIntListEnumerator(" 1 54 0 "))
            {
                nums.Add(num);
            }
            Assert.Equal(3, nums.Count);
            Assert.Equal(54, nums[1]);
        }

        [FactAttribute]
        public void TestNegativeSymbo()
        {
            List<int> nums = new List<int>();
            foreach (var num in new StringIntListEnumerator("+25-621"))
            {
                nums.Add(num);
            }
            Assert.Equal(2, nums.Count);
            Assert.Equal(25, nums[0]);
            Assert.Equal(-621, nums[1]);
        }

        [FactAttribute]
        public void TestMultiSymbol()
        {
            List<int> nums = new List<int>();
            foreach (var num in new StringIntListEnumerator("-+77"))
            {
                nums.Add(num);
            }
            Assert.Equal(1, nums.Count);
            Assert.Equal(77, nums[0]);
        }

        [FactAttribute]
        public void TestMaxValue()
        {
            List<int> nums = new List<int>();
            foreach (var num in new StringIntListEnumerator("2 a 2147483647 "))
            {
                nums.Add(num);
            }
            Assert.Equal(2, nums.Count);
            Assert.Equal(2147483647, nums[1]);
        }


        [FactAttribute]
        public void TestLeakOfIntMaxValue()
        {
            List<int> nums = new List<int>();
            foreach (var num in new StringIntListEnumerator("2 a 2147483649 "))
            {
                nums.Add(num);
            }
            Assert.Equal(1, nums.Count);
            Assert.Equal(2, nums[0]);
        }
    }
}
