using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{

    class L1ChatParty
    {
        private readonly IList<L1PcInstance> _membersList = Lists.newList<L1PcInstance>();

        private L1PcInstance _leader = null;

        public virtual void addMember(L1PcInstance pc)
        {
            if (pc == null)
            {
                throw new System.NullReferenceException();
            }
            if (((_membersList.Count == Config.MAX_CHAT_PT) && !_leader.Gm) || _membersList.Contains(pc))
            {
                return;
            }

            if (_membersList.Count == 0)
            {
                // 最初のPTメンバーであればリーダーにする
                Leader = pc;
            }

            _membersList.Add(pc);
            pc.ChatParty = this;
        }

        private void removeMember(L1PcInstance pc)
        {
            if (!_membersList.Contains(pc))
            {
                return;
            }

            _membersList.Remove(pc);
            pc.ChatParty = null;
        }

        public virtual bool Vacancy
        {
            get
            {
                return _membersList.Count < Config.MAX_CHAT_PT;
            }
        }

        //public virtual int Vacancy
        //{
        //	get
        //	{
        //		return Config.MAX_CHAT_PT - _membersList.Count;
        //	}
        //}

        public virtual bool isMember(L1PcInstance pc)
        {
            return _membersList.Contains(pc);
        }

        public L1PcInstance Leader
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


        public virtual bool isLeader(L1PcInstance pc)
        {
            return pc.Id == _leader.Id;
        }

        public virtual string MembersNameList
        {
            get
            {
                string _result = "";
                foreach (L1PcInstance pc in _membersList)
                {
                    _result = _result + pc.Name + " ";
                }
                return _result;
            }
        }

        private void breakup()
        {
            L1PcInstance[] members = Members;

            foreach (L1PcInstance member in members)
            {
                removeMember(member);
                member.sendPackets(new S_ServerMessage(418)); // パーティーを解散しました。
            }
        }

        public virtual void leaveMember(L1PcInstance pc)
        {
            L1PcInstance[] members = Members;
            if (isLeader(pc))
            {
                // パーティーリーダーの場合
                breakup();
            }
            else
            {
                // パーティーリーダーでない場合
                if (NumOfMembers == 2)
                {
                    // パーティーメンバーが自分とリーダーのみ
                    removeMember(pc);
                    L1PcInstance leader = Leader;
                    removeMember(leader);

                    sendLeftMessage(pc, pc);
                    sendLeftMessage(leader, pc);
                }
                else
                {
                    // 残りのパーティーメンバーが２人以上いる
                    removeMember(pc);
                    foreach (L1PcInstance member in members)
                    {
                        sendLeftMessage(member, pc);
                    }
                    sendLeftMessage(pc, pc);
                }
            }
        }

        public virtual void kickMember(L1PcInstance pc)
        {
            if (NumOfMembers == 2)
            {
                // パーティーメンバーが自分とリーダーのみ
                removeMember(pc);
                L1PcInstance leader = Leader;
                removeMember(leader);
            }
            else
            {
                // 残りのパーティーメンバーが２人以上いる
                removeMember(pc);
            }
            pc.sendPackets(new S_ServerMessage(419)); // パーティーから追放されました。
        }

        public virtual L1PcInstance[] Members
        {
            get
            {
                return ((List<L1PcInstance>)_membersList).ToArray();
            }
        }

        public virtual int NumOfMembers
        {
            get
            {
                return _membersList.Count;
            }
        }

        private void sendLeftMessage(L1PcInstance sendTo, L1PcInstance left)
        {
            // %0がパーティーから去りました。
            sendTo.sendPackets(new S_ServerMessage(420, left.Name));
        }

    }

}