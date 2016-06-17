using ProtoBuf;

namespace Benchmark.Core.Model
{
    [ProtoContract]
    public class Foo
    {
        public string Name;

        [ProtoMember(1)]
        public int Id { get; private set; }

        public Foo() { }

        public Foo(int id)
        {
            Id = id;
        }
    }
}
