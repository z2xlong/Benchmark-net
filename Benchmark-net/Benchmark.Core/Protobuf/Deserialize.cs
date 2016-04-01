using Benchmark.Core.Model;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.Protobuf
{
    public class Deserialize
    {
        public static void RunWithWrapper(Stream source, int size)
        {
            Dictionary<int, Foo> dict = new Dictionary<int, Foo>(size);
            var d = Serializer.Deserialize<DictionaryForSerialization<int, Foo>>(source);
            foreach (var kvp in d.InternalDict)
                dict[kvp.Key + size] = kvp.Value;
        }

        public static void RunWithItems(Stream source)
        {
            Dictionary<int, Foo> dict = new Dictionary<int, Foo>();
            foreach (var kvp in Serializer.DeserializeItems<KeyValuePair<int, Foo>>(source, PrefixStyle.Base128, 0))
            {
                dict[kvp.Key] = kvp.Value;
            }
        }
    }
}
