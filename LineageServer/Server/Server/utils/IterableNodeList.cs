using System.Collections.Generic;
namespace LineageServer.Server.Server.Utils
{
    // 標準ライブラリに同じものが用意されているようなら置換してください。
    public class IterableNodeList : IEnumerable<Node>
    {
        private readonly NodeList _list;

        private class MyIterator : IEnumerator<Node>
        {
            private readonly IterableNodeList outerInstance;

            public MyIterator(IterableNodeList outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            internal int _idx = 0;

            public override bool hasNext()
            {
                return _idx < outerInstance._list.Length;
            }

            public override Node next()
            {
                //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
                if (!hasNext())
                {
                    throw new NoSuchElementException();
                }
                return outerInstance._list.item(_idx++);
            }

            public override void remove()
            {
                throw new System.NotSupportedException();
            }
        }

        public IterableNodeList(NodeList list)
        {
            _list = list;
        }

        public virtual IEnumerator<Node> GetEnumerator()
        {
            return new MyIterator(this);
        }

    }

}