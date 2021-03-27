using System.Collections.Generic;

/// <summary>
/// License THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). THE WORK IS PROTECTED
/// BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. ANY USE OF THE WORK OTHER THAN AS
/// AUTHORIZED UNDER THIS LICENSE OR COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND AGREE TO
/// BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE MAY BE
/// CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>

namespace LineageServer.Server.Templates
{

	using ListFactory = LineageServer.Utils.ListFactory;

	public class L1MobGroup
	{
		private readonly int _id;

		private readonly int _leaderId;

		private readonly IList<L1NpcCount> _minions = ListFactory.newList();

		private readonly bool _isRemoveGroupIfLeaderDie;

		public L1MobGroup(int id, int leaderId, IList<L1NpcCount> minions, bool isRemoveGroupIfLeaderDie)
		{
			_id = id;
			_leaderId = leaderId;
			((List<L1NpcCount>)_minions).AddRange(minions); // 参照コピーの方が速いが、不変性が保証できない
			_isRemoveGroupIfLeaderDie = isRemoveGroupIfLeaderDie;
		}

		public virtual int Id
		{
			get
			{
				return _id;
			}
		}

		public virtual int LeaderId
		{
			get
			{
				return _leaderId;
			}
		}

		public virtual IList<L1NpcCount> Minions
		{
			get
			{
				return Collections.unmodifiableList(_minions);
			}
		}

		public virtual bool RemoveGroupIfLeaderDie
		{
			get
			{
				return _isRemoveGroupIfLeaderDie;
			}
		}

	}
}