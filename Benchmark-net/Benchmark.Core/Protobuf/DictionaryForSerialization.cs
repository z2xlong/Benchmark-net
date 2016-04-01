using ProtoBuf;
using System.Collections.Generic;

namespace Benchmark.Core.Protobuf
{
    [ProtoContract]
    public class DictionaryForSerialization<TKey, TValue>
    {
        public DictionaryForSerialization()
        {
        }
        public DictionaryForSerialization(Dictionary<TKey, TValue> dict)
        {
            InternalDict = dict;
        }
        [ProtoMember(1)]
        public int Capacity
        {
            get
            {
                return InternalDict != null ? InternalDict.Count : 0;
            }
            set
            {
                InternalDict = new Dictionary<TKey, TValue>(value);
            }
        }
        [ProtoMember(2)]
        public Dictionary<TKey, TValue> InternalDict { get; private set; }
    }
}
