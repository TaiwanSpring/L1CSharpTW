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
namespace LineageServer.Server.Server.Model.npc
{
	public class L1NpcHtml
	{
		private readonly string _name;
		private readonly string[] _args;

		public static readonly L1NpcHtml HTML_CLOSE = new L1NpcHtml("");

		public L1NpcHtml(string name) : this(name, new string[] {})
		{
		}

		public L1NpcHtml(string name, params string[] args)
		{
			if (string.ReferenceEquals(name, null) || args == null)
			{
				throw new System.NullReferenceException();
			}
			_name = name;
			_args = args;
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}
		}

		public virtual string[] Args
		{
			get
			{
				return _args;
			}
		}
	}

}