using System.Collections.Generic;
namespace LineageServer.Server.Server.utils
{


	public class IterableElementList : IEnumerable<Element>
	{
		internal IterableNodeList _list;

		private class MyIterator : IEnumerator<Element>
		{
			private readonly IterableElementList outerInstance;

			internal IEnumerator<Node> _itr;
			internal Element _next = null;

			public MyIterator(IterableElementList outerInstance, IEnumerator<Node> itr)
			{
				this.outerInstance = outerInstance;
				_itr = itr;
				updateNextElement();
			}

			internal virtual void updateNextElement()
			{
				while (_itr.MoveNext())
				{
					Node node = _itr.Current;
					if (node is Element)
					{
						_next = (Element)node;
						return;
					}
				}
				_next = null;
			}

			public override bool hasNext()
			{
				return _next != null;
			}

			public override Element next()
			{
				//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
				if (!hasNext())
				{
					throw new NoSuchElementException();
				}
				Element result = _next;
				updateNextElement();
				return result;
			}

			public override void remove()
			{
				throw new System.NotSupportedException();
			}
		}

		public IterableElementList(NodeList list)
		{
			_list = new IterableNodeList(list);
		}

		public virtual IEnumerator<Element> GetEnumerator()
		{
			return new MyIterator(this, _list.GetEnumerator());
		}

	}

}