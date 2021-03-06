using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LineageServer.Server.Model
{
    class L1War
    {
        private string _param1 = null;

        private string _param2 = null;

        private readonly IList<string> _attackClanList = ListFactory.NewList<string>();

        private string _defenceClanName = null;

        private int _warType = 0;

        private L1Castle _castle = null;

        private DateTime _warEndTime;

        private bool _isWarTimerDelete = false;

        public L1War()
        {
        }

        internal class CastleWarTimer : IRunnable
        {
            private readonly L1War outerInstance;

            public CastleWarTimer(L1War outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void run()
            {
                while (true)
                {
                    Thread.Sleep(1000);

                    if (outerInstance._warEndTime < DateTime.Now)
                    {
                        break;
                    }

                    if (outerInstance._isWarTimerDelete)
                    { // 戦争が終結していたらタイマー終了
                        return;
                    }
                }
                outerInstance.CeaseCastleWar(); // 攻城戦終結処理
                outerInstance.delete();
            }
        }

        internal class SimWarTimer : IRunnable
        {
            private readonly L1War outerInstance;

            public SimWarTimer(L1War outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void run()
            {
                for (int loop = 0; loop < 240; loop++)
                {
                    // 240分
                    Thread.Sleep(60 * 1000);
                    if (outerInstance._isWarTimerDelete)
                    { // 戦争が終結していたらタイマー終了
                        return;
                    }
                }
                outerInstance.CeaseWar(outerInstance._param1, outerInstance._param2); // 終結
                outerInstance.delete();
            }
        }

        public virtual void handleCommands(int war_type, string attack_clan_name, string defence_clan_name)
        {
            // war_type - 1:攻城戦 2:模擬戦
            // attack_clan_name - 布告したクラン名
            // defence_clan_name - 布告されたクラン名（攻城戦時は、城主クラン）

            SetWarType(war_type);

            DeclareWar(attack_clan_name, defence_clan_name);

            _param1 = attack_clan_name;
            _param2 = defence_clan_name;
            InitAttackClan();
            AddAttackClan(attack_clan_name);
            SetDefenceClanName(defence_clan_name);

            if (war_type == 1)
            {
                // 攻城戦
                GetCastleId();
                _castle = GetCastle();
                if (_castle != null)
                {
                    _warEndTime = _castle.WarTime.Add(Config.ALT_WAR_TIME);
                }

                CastleWarTimer castle_war_timer = new CastleWarTimer(this);
                Container.Instance.Resolve<ITaskController>().execute(castle_war_timer); // タイマー開始
            }
            else if (war_type == 2)
            { // 模擬戦
                SimWarTimer sim_war_timer = new SimWarTimer(this);
                Container.Instance.Resolve<ITaskController>().execute(sim_war_timer); // タイマー開始
            }
            Container.Instance.Resolve<IGameWorld>().addWar(this); // 戦争リストに追加
        }

        private void RequestCastleWar(int type, string clan1_name, string clan2_name)
        {
            if (string.IsNullOrEmpty(clan1_name) || string.IsNullOrEmpty(clan2_name))
            {
                return;
            }

            L1Clan clan1 = Container.Instance.Resolve<IGameWorld>().getClan(clan1_name);
            if (clan1 != null)
            {
                L1PcInstance[] clan1_member = clan1.OnlineClanMember;
                foreach (L1PcInstance element in clan1_member)
                {
                    element.sendPackets(new S_War(type, clan1_name, clan2_name));
                }
            }

            int attack_clan_num = GetAttackClanListSize();

            if ((type == 1) || (type == 2) || (type == 3))
            { // 宣戦布告、降伏、終結
                L1Clan clan2 = Container.Instance.Resolve<IGameWorld>().getClan(clan2_name);
                if (clan2 != null)
                {
                    L1PcInstance[] clan2_member = clan2.OnlineClanMember;
                    foreach (L1PcInstance element in clan2_member)
                    {
                        if (type == 1)
                        { // 宣戦布告
                            element.sendPackets(new S_War(type, clan1_name, clan2_name));
                        }
                        else if (type == 2)
                        { // 降伏
                            element.sendPackets(new S_War(type, clan1_name, clan2_name));
                            if (attack_clan_num == 1)
                            { // 攻撃側クランが一つ
                                element.sendPackets(new S_War(4, clan2_name, clan1_name));
                            }
                            else
                            {
                                element.sendPackets(new S_ServerMessage(228, clan1_name, clan2_name));
                                RemoveAttackClan(clan1_name);
                            }
                        }
                        else if (type == 3)
                        { // 終結
                            element.sendPackets(new S_War(type, clan1_name, clan2_name));
                            if (attack_clan_num == 1)
                            { // 攻撃側クランが一つ
                                element.sendPackets(new S_War(4, clan2_name, clan1_name));
                            }
                            else
                            {
                                element.sendPackets(new S_ServerMessage(227, clan1_name, clan2_name));
                                RemoveAttackClan(clan1_name);
                            }
                        }
                    }
                }
            }

            if (((type == 2) || (type == 3)) && (attack_clan_num >= 1))
            { // 投降、終止後攻擊方大於或等於一
                _isWarTimerDelete = true;
                delete();
            }
        }

        private void RequestSimWar(int type, string clan1_name, string clan2_name)
        {
            if ((string.ReferenceEquals(clan1_name, null)) || (string.ReferenceEquals(clan2_name, null)))
            {
                return;
            }

            L1Clan clan1 = Container.Instance.Resolve<IGameWorld>().getClan(clan1_name);
            if (clan1 != null)
            {
                L1PcInstance[] clan1_member = clan1.OnlineClanMember;
                foreach (L1PcInstance element in clan1_member)
                {
                    element.sendPackets(new S_War(type, clan1_name, clan2_name));
                }
            }

            if ((type == 1) || (type == 2) || (type == 3))
            { // 宣戦布告、降伏、終結
                L1Clan clan2 = Container.Instance.Resolve<IGameWorld>().getClan(clan2_name);
                if (clan2 != null)
                {
                    L1PcInstance[] clan2_member = clan2.OnlineClanMember;
                    foreach (L1PcInstance element in clan2_member)
                    {
                        if (type == 1)
                        { // 宣戦布告
                            element.sendPackets(new S_War(type, clan1_name, clan2_name));
                        }
                        else if ((type == 2) || (type == 3))
                        { // 降伏、終結
                            element.sendPackets(new S_War(type, clan1_name, clan2_name));
                            element.sendPackets(new S_War(4, clan2_name, clan1_name));
                        }
                    }
                }
            }

            if ((type == 2) || (type == 3))
            { // 降伏、終結
                _isWarTimerDelete = true;
                delete();
            }
        }

        public virtual void WinCastleWar(string clan_name)
        { // クラウンを奪取して、攻撃側クランが勝利
            string defence_clan_name = GetDefenceClanName();
            // %0血盟が%1血盟との戦争で勝利しました。
            Container.Instance.Resolve<IGameWorld>().broadcastPacketToAll(new S_ServerMessage(231, clan_name, defence_clan_name));

            L1Clan defence_clan = Container.Instance.Resolve<IGameWorld>().getClan(defence_clan_name);
            if (defence_clan != null)
            {
                L1PcInstance[] defence_clan_member = defence_clan.OnlineClanMember;
                foreach (L1PcInstance element in defence_clan_member)
                {
                    foreach (string clanName in GetAttackClanList())
                    {
                        element.sendPackets(new S_War(3, defence_clan_name, clanName));
                    }
                }
            }

            string[] clanList = GetAttackClanList();
            foreach (string element in clanList)
            {
                if (!string.ReferenceEquals(element, null))
                {
                    Container.Instance.Resolve<IGameWorld>().broadcastPacketToAll(new S_ServerMessage(227, defence_clan_name, element));
                    L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(element);
                    if (clan != null)
                    {
                        L1PcInstance[] clan_member = clan.OnlineClanMember;
                        foreach (L1PcInstance element2 in clan_member)
                        {
                            element2.sendPackets(new S_War(3, element, defence_clan_name));
                        }
                    }
                }
            }

            _isWarTimerDelete = true;
            delete();
        }

        public virtual void CeaseCastleWar()
        { // 戦争時間満了し、防衛側クランが勝利
            string defence_clan_name = GetDefenceClanName();
            string[] clanList = GetAttackClanList();
            if (!string.ReferenceEquals(defence_clan_name, null))
            {
                Container.Instance.Resolve<IGameWorld>().broadcastPacketToAll(new S_ServerMessage(231, defence_clan_name, clanList[0]));
            }

            L1Clan defence_clan = Container.Instance.Resolve<IGameWorld>().getClan(defence_clan_name);
            if (defence_clan != null)
            {
                L1PcInstance[] defence_clan_member = defence_clan.OnlineClanMember;
                foreach (L1PcInstance element in defence_clan_member)
                {
                    element.sendPackets(new S_War(4, defence_clan_name, clanList[0]));
                }
            }

            foreach (string element in clanList)
            {
                if (!string.ReferenceEquals(element, null))
                {
                    Container.Instance.Resolve<IGameWorld>().broadcastPacketToAll(new S_ServerMessage(227, defence_clan_name, element));
                    L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(element);
                    if (clan != null)
                    {
                        L1PcInstance[] clan_member = clan.OnlineClanMember;
                        foreach (L1PcInstance element2 in clan_member)
                        {
                            element2.sendPackets(new S_War(3, element, defence_clan_name));
                        }
                    }
                }
            }

            _isWarTimerDelete = true;
            delete();
        }

        public virtual void DeclareWar(string clan1_name, string clan2_name)
        { // _血盟が_血盟に宣戦布告しました。
            if (GetWarType() == 1)
            { // 攻城戦
                RequestCastleWar(1, clan1_name, clan2_name);
            }
            else
            { // 模擬戦
                RequestSimWar(1, clan1_name, clan2_name);
            }
        }

        public virtual void SurrenderWar(string clan1_name, string clan2_name)
        { // _血盟が_血盟に降伏しました。
            if (GetWarType() == 1)
            {
                RequestCastleWar(2, clan1_name, clan2_name);
            }
            else
            {
                RequestSimWar(2, clan1_name, clan2_name);
            }
        }

        public virtual void CeaseWar(string clan1_name, string clan2_name)
        { // _血盟と_血盟との戦争が終結しました。
            if (GetWarType() == 1)
            {
                RequestCastleWar(3, clan1_name, clan2_name);
            }
            else
            {
                RequestSimWar(3, clan1_name, clan2_name);
            }
        }

        public virtual void WinWar(string clan1_name, string clan2_name)
        { // _血盟が_血盟との戦争で勝利しました。
            if (GetWarType() == 1)
            {
                RequestCastleWar(4, clan1_name, clan2_name);
            }
            else
            {
                RequestSimWar(4, clan1_name, clan2_name);
            }
        }

        public virtual bool CheckClanInWar(string clan_name)
        { // クランが戦争に参加しているかチェックする
            bool ret;
            if (GetDefenceClanName().ToLower().Equals(clan_name.ToLower()))
            { // 防衛側クランをチェック
                ret = true;
            }
            else
            {
                ret = CheckAttackClan(clan_name); // 攻撃側クランをチェック
            }
            return ret;
        }

        public virtual bool CheckClanInSameWar(string player_clan_name, string target_clan_name)
        { // 自クランと相手クランが同じ戦争に参加しているかチェックする（同じクランの場合も含む）
            bool player_clan_flag;
            bool target_clan_flag;

            if (GetDefenceClanName().ToLower().Equals(player_clan_name.ToLower()))
            { // 自クランに対して防衛側クランをチェック
                player_clan_flag = true;
            }
            else
            {
                player_clan_flag = CheckAttackClan(player_clan_name); // 自クランに対して攻撃側クランをチェック
            }

            if (GetDefenceClanName().ToLower().Equals(target_clan_name.ToLower()))
            { // 相手クランに対して防衛側クランをチェック
                target_clan_flag = true;
            }
            else
            {
                target_clan_flag = CheckAttackClan(target_clan_name); // 相手クランに対して攻撃側クランをチェック
            }

            if ((player_clan_flag == true) && (target_clan_flag == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual string GetEnemyClanName(string player_clan_name)
        { // 相手のクラン名を取得する
            string enemy_clan_name = null;
            if (GetDefenceClanName().ToLower().Equals(player_clan_name.ToLower()))
            { // 自クランが防衛側
                string[] clanList = GetAttackClanList();
                foreach (string element in clanList)
                {
                    if (!string.ReferenceEquals(element, null))
                    {
                        enemy_clan_name = element;
                        return enemy_clan_name; // リストの先頭のクラン名を返す
                    }
                }
            }
            else
            { // 自クランが攻撃側
                enemy_clan_name = GetDefenceClanName();
                return enemy_clan_name;
            }
            return enemy_clan_name;
        }

        public virtual void delete()
        {
            Container.Instance.Resolve<IGameWorld>().removeWar(this); // 戦争リストから削除
        }

        public virtual int GetWarType()
        {
            return _warType;
        }

        public virtual void SetWarType(int war_type)
        {
            _warType = war_type;
        }

        public virtual string GetDefenceClanName()
        {
            return _defenceClanName;
        }

        public virtual void SetDefenceClanName(string defence_clan_name)
        {
            _defenceClanName = defence_clan_name;
        }

        public virtual void InitAttackClan()
        {
            _attackClanList.Clear();
        }

        public virtual void AddAttackClan(string attack_clan_name)
        {
            if (!_attackClanList.Contains(attack_clan_name))
            {
                _attackClanList.Add(attack_clan_name);
            }
        }

        public virtual void RemoveAttackClan(string attack_clan_name)
        {
            if (_attackClanList.Contains(attack_clan_name))
            {
                _attackClanList.Remove(attack_clan_name);
            }
        }

        public virtual bool CheckAttackClan(string attack_clan_name)
        {
            if (_attackClanList.Contains(attack_clan_name))
            {
                return true;
            }
            return false;
        }

        public virtual string[] GetAttackClanList()
        {
            return ((List<string>)_attackClanList).ToArray();
        }

        public virtual int GetAttackClanListSize()
        {
            return _attackClanList.Count;
        }

        public virtual int GetCastleId()
        {
            int castle_id = 0;
            if (GetWarType() == 1)
            { // 攻城戦
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(GetDefenceClanName());
                if (clan != null)
                {
                    castle_id = clan.CastleId;
                }
            }
            return castle_id;
        }

        public virtual L1Castle GetCastle()
        {
            L1Castle l1castle = null;
            if (GetWarType() == 1)
            { // 攻城戦
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(GetDefenceClanName());
                if (clan != null)
                {
                    int castle_id = clan.CastleId;
                    l1castle = CastleTable.Instance.getCastleTable(castle_id);
                }
            }
            return l1castle;
        }
    }

}