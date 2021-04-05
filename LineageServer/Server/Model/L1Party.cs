using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
    class L1Party
    {
        private readonly IList<L1PcInstance> _membersList = ListFactory.NewList<L1PcInstance>();

        private L1PcInstance _leader = null;

        public virtual void addMember(L1PcInstance pc)
        {
            if (pc == null)
            {
                throw new System.NullReferenceException();
            }
            if (((_membersList.Count == Config.MAX_PT) && !_leader.Gm) || _membersList.Contains(pc))
            {
                return;
            }

            if (_membersList.Count == 0)
            {
                // 最初のPTメンバーであればリーダーにする
                Leader = pc;
            }
            else
            {
                createMiniHp(pc);
            }

            _membersList.Add(pc);
            pc.Party = this;
            showAddPartyInfo(pc);
            pc.startRefreshParty();
        }

        private void removeMember(L1PcInstance pc)
        {
            if (!_membersList.Contains(pc))
            {
                return;
            }
            pc.stopRefreshParty();
            _membersList.Remove(pc);
            pc.Party = null;
            if (_membersList.Count > 0)
            {
                deleteMiniHp(pc);
            }
        }

        public virtual bool Vacancy
        {
            get
            {
                return _membersList.Count < Config.MAX_PT;
            }
        }

        //public virtual int Vacancy
        //{
        //	get
        //	{
        //		return Config.MAX_PT - _membersList.Count;
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

        private void createMiniHp(L1PcInstance pc)
        {
            // パーティー加入時、相互にHPを表示させる
            L1PcInstance[] members = Members;

            foreach (L1PcInstance member in members)
            {
                member.sendPackets(new S_HPMeter(pc.Id, 100 * pc.CurrentHp / pc.getMaxHp()));
                pc.sendPackets(new S_HPMeter(member.Id, 100 * member.CurrentHp / member.getMaxHp()));
            }
        }

        private void deleteMiniHp(L1PcInstance pc)
        {
            // 隊員離隊時，頭頂的Hp血條消除。
            L1PcInstance[] members = Members;

            foreach (L1PcInstance member in members)
            {
                member.sendPackets(new S_HPMeter(pc.Id, 0xff));
                pc.sendPackets(new S_HPMeter(member.Id, 0xff));
            }
            pc.sendPackets(new S_HPMeter(pc.Id, 0xff));
        }

        public virtual void updateMiniHP(L1PcInstance pc)
        {
            L1PcInstance[] members = Members;

            foreach (L1PcInstance member in members)
            { // パーティーメンバー分更新
                member.sendPackets(new S_HPMeter(pc.Id, 100 * pc.CurrentHp / pc.getMaxHp()));
            }
        }

        private void breakup()
        {
            L1PcInstance[] members = Members;

            foreach (L1PcInstance member in members)
            {
                if (!isLeader(member))
                {
                    sendLeftMessage(Leader, member);
                    removeMember(member);
                    member.sendPackets(new S_ServerMessage(418)); // 您解散您的隊伍了!!
                }
                else
                {
                    member.sendPackets(new S_ServerMessage(418)); // 您解散您的隊伍了!!
                    removeMember(member);
                }
            }
        }

        public virtual void passLeader(L1PcInstance pc)
        {
            pc.Party.Leader = pc;
            foreach (L1PcInstance member in Members)
            {
                member.sendPackets(new S_Party(0x6A, pc));
            }
        }

        public virtual void leaveMember(L1PcInstance pc)
        {
            if (isLeader(pc) || (NumOfMembers == 2))
            {
                // パーティーリーダーの場合
                breakup();
            }
            else
            {
                removeMember(pc);
                foreach (L1PcInstance member in Members)
                {
                    sendLeftMessage(member, pc);
                }
                sendLeftMessage(pc, pc);
                // パーティーリーダーでない場合
                /*
				 * if (getNumOfMembers() == 2) { // パーティーメンバーが自分とリーダーのみ
				 * removeMember(pc); L1PcInstance leader = getLeader();
				 * removeMember(leader); sendLeftMessage(pc, pc);
				 * sendLeftMessage(leader, pc); } else { // 残りのパーティーメンバーが２人以上いる
				 * removeMember(pc); for (L1PcInstance member : members) {
				 * sendLeftMessage(member, pc); } sendLeftMessage(pc, pc); }
				 */
            }
        }

        public virtual void kickMember(L1PcInstance pc)
        {
            if (NumOfMembers == 2)
            {
                // パーティーメンバーが自分とリーダーのみ
                breakup();
            }
            else
            {
                removeMember(pc);
                foreach (L1PcInstance member in Members)
                {
                    sendLeftMessage(member, pc);
                }
                sendKickMessage(pc);
            }
        }

        private void showAddPartyInfo(L1PcInstance pc)
        {
            foreach (L1PcInstance member in Members)
            {
                if ((pc.Id == Leader.Id) && (NumOfMembers == 1))
                {
                    continue;
                }
                // 發送給隊長的封包
                if (pc.Id == member.Id)
                {
                    pc.sendPackets(new S_Party(0x68, pc));
                }
                else
                { // 其他成員封包
                    member.sendPackets(new S_Party(0x69, pc));
                }
                member.sendPackets(new S_Party(0x6e, member));
                createMiniHp(member);
            }
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

        private void sendKickMessage(L1PcInstance kickpc)
        {
            kickpc.sendPackets(new S_ServerMessage(419));
        }

        private void sendLeftMessage(L1PcInstance sendTo, L1PcInstance left)
        {
            // %0がパーティーから去りました。
            sendTo.sendPackets(new S_ServerMessage(420, left.Name));
        }

    }

}