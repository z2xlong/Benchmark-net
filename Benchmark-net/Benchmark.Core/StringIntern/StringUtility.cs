using System;
using System.Collections.Concurrent;

namespace Benchmark.Core
{
    public static class StringUtility
    {
        /// <summary>
        /// Get intern string for the sourceString.
        /// </summary>
        public static string Intern(string sourceString)
        {
            try
            {
                if (sourceString == null)
                {
                    return null;
                }
                return _dictionary.GetOrAdd(sourceString, sourceString);
            }
            catch (Exception e)
            {
                return sourceString;
            }
        }

        /// <summary>
        /// Remove all intern strings
        /// </summary>
        public static void Clear()
        {
            try
            {
                //回收前记录下来,用于信息统计
                _listConcurrent++;
                _dictionary = new ConcurrentDictionary<string, string>();
            }
            catch (Exception e)
            {
                // ignore exception
            }
        }

        /// <summary>
        /// 获取驻留的字符串个数
        /// </summary>
        /// <returns></returns>
        public static int StringCount()
        {
            return _dictionary.Count;
        }

        /// <summary>
        ///回收的数量
        /// </summary>
        /// <returns></returns>
        public static int ClearCount()
        {
            return _listConcurrent;
        }

        private static int _listConcurrent;

        private static ConcurrentDictionary<string, string> _dictionary = new ConcurrentDictionary<string, string>();
    }
}
