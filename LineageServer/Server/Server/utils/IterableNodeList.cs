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

	using Node = org.w3c.dom.Node;
	using NodeList = org.w3c.dom.NodeList;

	/// <summary>
	/// org.w3c.dom.NodeListにIterableを付加するためのアダプタ。
	/// </summary>
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