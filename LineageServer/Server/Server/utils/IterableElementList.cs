using System.Collections.Generic;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.utils
{

	using Element = org.w3c.dom.Element;
	using Node = org.w3c.dom.Node;
	using NodeList = org.w3c.dom.NodeList;

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
						_next = (Element) node;
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