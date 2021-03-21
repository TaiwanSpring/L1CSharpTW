using System;
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
namespace LineageServer.Server.Server.Model
{

	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	public class L1Buddy
	{
		private readonly int _charId;

		private readonly LinkedHashMap<int, string> _buddys = new LinkedHashMap<int, string>();

		public L1Buddy(int charId)
		{
			_charId = charId;
		}

		public virtual int CharId
		{
			get
			{
				return _charId;
			}
		}

		public virtual bool add(int objId, string name)
		{
			if (_buddys.containsKey(objId))
			{
				return false;
			}
			_buddys.put(objId, name);
			return true;
		}

		public virtual bool remove(int objId)
		{
			string result = _buddys.remove(objId);
			return (!string.ReferenceEquals(result, null) ? true : false);
		}

		public virtual bool remove(string name)
		{
			int id = 0;
			foreach (KeyValuePair<int, string> buddy in _buddys.entrySet())
			{
				if (name.Equals(buddy.Value, StringComparison.OrdinalIgnoreCase))
				{
					id = buddy.Key;
					break;
				}
			}
			if (id == 0)
			{
				return false;
			}
			_buddys.remove(id);
			return true;
		}

		public virtual string OnlineBuddyListString
		{
			get
			{
				string result = "";
				foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
				{
					if (_buddys.containsKey(pc.Id))
					{
						result += pc.Name + " ";
					}
				}
				return result;
			}
		}

		public virtual string BuddyListString
		{
			get
			{
				string result = "";
				foreach (string name in _buddys.values())
				{
					result += name + " ";
				}
				return result;
			}
		}

		public virtual bool containsId(int objId)
		{
			return _buddys.containsKey(objId);
		}

		public virtual bool containsName(string name)
		{
			foreach (string buddyName in _buddys.values())
			{
				if (name.Equals(buddyName, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public virtual int size()
		{
			return _buddys.size();
		}
	}

}