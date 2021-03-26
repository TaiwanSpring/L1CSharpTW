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

	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;

	// Referenced classes of package l1j.server.server.model:
	// L1MobGroupInfo

	public class L1MobGroupInfo
	{
		private readonly IList<L1NpcInstance> _membersList = Lists.newList();

		private L1NpcInstance _leader;

		public L1MobGroupInfo()
		{
		}

		public virtual L1NpcInstance Leader
		{
			set
			{
				_leader = value;
			}
			get
			{
				return _leader;
			}
		}


		public virtual bool isLeader(L1NpcInstance npc)
		{
			return npc.Id == _leader.Id;
		}

		private L1Spawn _spawn;

		public virtual L1Spawn Spawn
		{
			set
			{
				_spawn = value;
			}
			get
			{
				return _spawn;
			}
		}


		public virtual void addMember(L1NpcInstance npc)
		{
			if (npc == null)
			{
				throw new System.NullReferenceException();
			}

			// 最初のメンバーであればリーダーにする
			if (_membersList.Count == 0)
			{
				Leader = npc;
				// リーダーの再ポップ情報を保存する
				if (npc.ReSpawn)
				{
					Spawn = npc.Spawn;
				}
			}

			if (!_membersList.Contains(npc))
			{
				_membersList.Add(npc);
			}
			npc.MobGroupInfo = this;
			npc.MobGroupId = _leader.Id;
		}

		public virtual int removeMember(L1NpcInstance npc)
		{
			lock (this)
			{
				if (npc == null)
				{
					throw new System.NullReferenceException();
				}
        
				if (_membersList.Contains(npc))
				{
					_membersList.Remove(npc);
				}
				npc.MobGroupInfo = null;
        
				// リーダーで他のメンバーがいる場合は、新リーダーにする
				if (isLeader(npc))
				{
					if (RemoveGroup && (_membersList.Count != 0))
					{ // リーダーが死亡したらグループ解除する場合
						foreach (L1NpcInstance minion in _membersList)
						{
							minion.MobGroupInfo = null;
							minion.Spawn = null;
							minion.setreSpawn(false);
						}
						return 0;
					}
					if (_membersList.Count != 0)
					{
						Leader = _membersList[0];
					}
				}
        
				// 残りのメンバー数を返す
				return _membersList.Count;
			}
		}

		public virtual int NumOfMembers
		{
			get
			{
				return _membersList.Count;
			}
		}

		private bool _isRemoveGroup;

		public virtual bool RemoveGroup
		{
			get
			{
				return _isRemoveGroup;
			}
			set
			{
				_isRemoveGroup = value;
			}
		}


	}

}