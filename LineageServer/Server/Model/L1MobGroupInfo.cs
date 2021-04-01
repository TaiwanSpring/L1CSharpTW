using LineageServer.Server.Model.Instance;
using LineageServer.Utils;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
    class L1MobGroupInfo
    {
        private readonly IList<L1NpcInstance> _membersList = ListFactory.NewList<L1NpcInstance>();

        public virtual L1NpcInstance Leader { get; set; }

        public virtual L1Spawn Spawn { get; set; }

        public bool isLeader(L1NpcInstance npc)
        {
            return npc.Id == Leader.Id;
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
            npc.MobGroupId = Leader.Id;
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

        public int NumOfMembers
        {
            get
            {
                return _membersList.Count;
            }
        }

        private bool _isRemoveGroup;

        public bool RemoveGroup
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