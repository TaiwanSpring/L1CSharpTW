using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using System;
using System.Threading;
namespace LineageServer.Server.Model
{
    class L1PetMatch
    {
        public const int STATUS_NONE = 0;

        public const int STATUS_READY1 = 1;

        public const int STATUS_READY2 = 2;

        public const int STATUS_PLAYING = 3;

        public const int MAX_PET_MATCH = 1;

        private static readonly short[] PET_MATCH_MAPID = new short[] { 5125, 5131, 5132, 5133, 5134 };

        private string[] _pc1Name = new string[MAX_PET_MATCH];

        private string[] _pc2Name = new string[MAX_PET_MATCH];

        private L1PetInstance[] _pet1 = new L1PetInstance[MAX_PET_MATCH];

        private L1PetInstance[] _pet2 = new L1PetInstance[MAX_PET_MATCH];

        private static L1PetMatch _instance;

        public static L1PetMatch Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new L1PetMatch();
                }
                return _instance;
            }
        }

        public virtual int setPetMatchPc(int petMatchNo, L1PcInstance pc, L1PetInstance pet)
        {
            int status = getPetMatchStatus(petMatchNo);
            if (status == STATUS_NONE)
            {
                _pc1Name[petMatchNo] = pc.Name;
                _pet1[petMatchNo] = pet;
                return STATUS_READY1;
            }
            else if (status == STATUS_READY1)
            {
                _pc2Name[petMatchNo] = pc.Name;
                _pet2[petMatchNo] = pet;
                return STATUS_PLAYING;
            }
            else if (status == STATUS_READY2)
            {
                _pc1Name[petMatchNo] = pc.Name;
                _pet1[petMatchNo] = pet;
                return STATUS_PLAYING;
            }
            return STATUS_NONE;
        }

        private int getPetMatchStatus(int petMatchNo)
        {
            lock (this)
            {
                L1PcInstance pc1 = null;
                if (!string.ReferenceEquals(_pc1Name[petMatchNo], null))
                {
                    pc1 = Container.Instance.Resolve<IGameWorld>().getPlayer(_pc1Name[petMatchNo]);
                }
                L1PcInstance pc2 = null;
                if (!string.ReferenceEquals(_pc2Name[petMatchNo], null))
                {
                    pc2 = Container.Instance.Resolve<IGameWorld>().getPlayer(_pc2Name[petMatchNo]);
                }

                if ((pc1 == null) && (pc2 == null))
                {
                    return STATUS_NONE;
                }
                if ((pc1 == null) && (pc2 != null))
                {
                    if (pc2.MapId == PET_MATCH_MAPID[petMatchNo])
                    {
                        return STATUS_READY2;
                    }
                    else
                    {
                        _pc2Name[petMatchNo] = null;
                        _pet2[petMatchNo] = null;
                        return STATUS_NONE;
                    }
                }
                if ((pc1 != null) && (pc2 == null))
                {
                    if (pc1.MapId == PET_MATCH_MAPID[petMatchNo])
                    {
                        return STATUS_READY1;
                    }
                    else
                    {
                        _pc1Name[petMatchNo] = null;
                        _pet1[petMatchNo] = null;
                        return STATUS_NONE;
                    }
                }

                // PCが試合場に2人いる場合
                if ((pc1.MapId == PET_MATCH_MAPID[petMatchNo]) && (pc2.MapId == PET_MATCH_MAPID[petMatchNo]))
                {
                    return STATUS_PLAYING;
                }

                // PCが試合場に1人いる場合
                if (pc1.MapId == PET_MATCH_MAPID[petMatchNo])
                {
                    _pc2Name[petMatchNo] = null;
                    _pet2[petMatchNo] = null;
                    return STATUS_READY1;
                }
                if (pc2.MapId == PET_MATCH_MAPID[petMatchNo])
                {
                    _pc1Name[petMatchNo] = null;
                    _pet1[petMatchNo] = null;
                    return STATUS_READY2;
                }
                return STATUS_NONE;
            }
        }

        private int decidePetMatchNo()
        {
            // 相手が待機中の試合を探す
            for (int i = 0; i < MAX_PET_MATCH; i++)
            {
                int status = getPetMatchStatus(i);
                if ((status == STATUS_READY1) || (status == STATUS_READY2))
                {
                    return i;
                }
            }
            // 待機中の試合がなければ空いている試合を探す
            for (int i = 0; i < MAX_PET_MATCH; i++)
            {
                int status = getPetMatchStatus(i);
                if (status == STATUS_NONE)
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual bool enterPetMatch(L1PcInstance pc, int amuletId)
        {
            lock (this)
            {
                int petMatchNo = decidePetMatchNo();
                if (petMatchNo == -1)
                {
                    return false;
                }

                L1PetInstance pet = withdrawPet(pc, amuletId);
                L1Teleport.teleport(pc, 32799, 32868, PET_MATCH_MAPID[petMatchNo], 0, true);

                L1PetMatchReadyTimer timer = new L1PetMatchReadyTimer(this, petMatchNo, pc, pet);
                timer.begin();
                return true;
            }
        }

        private L1PetInstance withdrawPet(L1PcInstance pc, int amuletId)
        {
            L1Pet l1pet = PetTable.Instance.getTemplate(amuletId);
            if (l1pet == null)
            {
                return null;
            }
            L1Npc npcTemp = Container.Instance.Resolve<INpcController>().getTemplate(l1pet.get_npcid());
            L1PetInstance pet = new L1PetInstance(npcTemp, pc, l1pet);
            pet.Petcost = 6;
            return pet;
        }

        public virtual void startPetMatch(int petMatchNo)
        {
            _pet1[petMatchNo].CurrentPetStatus = 1;
            _pet1[petMatchNo].Target = _pet2[petMatchNo];

            _pet2[petMatchNo].CurrentPetStatus = 1;
            _pet2[petMatchNo].Target = _pet1[petMatchNo];

            L1PetMatchTimer timer = new L1PetMatchTimer(this, _pet1[petMatchNo], _pet2[petMatchNo], petMatchNo);
            timer.begin();
        }

        public virtual void endPetMatch(int petMatchNo, int winNo)
        {
            L1PcInstance pc1 = Container.Instance.Resolve<IGameWorld>().getPlayer(_pc1Name[petMatchNo]);
            L1PcInstance pc2 = Container.Instance.Resolve<IGameWorld>().getPlayer(_pc2Name[petMatchNo]);
            if (winNo == 1)
            {
                giveMedal(pc1, petMatchNo, true);
                giveMedal(pc2, petMatchNo, false);
            }
            else if (winNo == 2)
            {
                giveMedal(pc1, petMatchNo, false);
                giveMedal(pc2, petMatchNo, true);
            }
            else if (winNo == 3)
            { // 引き分け
                giveMedal(pc1, petMatchNo, false);
                giveMedal(pc2, petMatchNo, false);
            }
            qiutPetMatch(petMatchNo);
        }

        private void giveMedal(L1PcInstance pc, int petMatchNo, bool isWin)
        {
            if (pc == null)
            {
                return;
            }
            if (pc.MapId != PET_MATCH_MAPID[petMatchNo])
            {
                return;
            }
            if (isWin)
            {
                pc.sendPackets(new S_ServerMessage(1166, pc.Name)); // %0%sペットマッチで勝利を収めました。
                L1ItemInstance item = ItemTable.Instance.createItem(41309);
                int count = 3;
                if (item != null)
                {
                    if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
                    {
                        item.Count = count;
                        pc.Inventory.storeItem(item);
                        pc.sendPackets(new S_ServerMessage(403, item.LogName)); // %0を手に入れました。
                    }
                }
            }
            else
            {
                L1ItemInstance item = ItemTable.Instance.createItem(41309);
                int count = 1;
                if (item != null)
                {
                    if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
                    {
                        item.Count = count;
                        pc.Inventory.storeItem(item);
                        pc.sendPackets(new S_ServerMessage(403, item.LogName)); // %0を手に入れました。
                    }
                }
            }
        }

        private void qiutPetMatch(int petMatchNo)
        {
            L1PcInstance pc1 = Container.Instance.Resolve<IGameWorld>().getPlayer(_pc1Name[petMatchNo]);
            if ((pc1 != null) && (pc1.MapId == PET_MATCH_MAPID[petMatchNo]))
            {
                foreach (object item in pc1.PetList.Values)
                {
                    if (item is L1PetInstance pet)
                    {
                        pet.dropItem();
                        pc1.PetList.Remove(pet.Id);
                        pet.deleteMe();
                    }
                }
                L1Teleport.teleport(pc1, 32630, 32744, (short)4, 4, true);
            }
            _pc1Name[petMatchNo] = null;
            _pet1[petMatchNo] = null;

            L1PcInstance pc2 = Container.Instance.Resolve<IGameWorld>().getPlayer(_pc2Name[petMatchNo]);
            if ((pc2 != null) && (pc2.MapId == PET_MATCH_MAPID[petMatchNo]))
            {
                foreach (object item in pc2.PetList.Values)
                {
                    if (item is L1PetInstance pet)
                    {
                        pet.dropItem();
                        pc2.PetList.Remove(pet.Id);
                        pet.deleteMe();
                    }
                }
                L1Teleport.teleport(pc2, 32630, 32744, (short)4, 4, true);
            }
            _pc2Name[petMatchNo] = null;
            _pet2[petMatchNo] = null;
        }

        public class L1PetMatchReadyTimer : TimerTask
        {
            private readonly L1PetMatch outerInstance;

            internal readonly int _petMatchNo;

            internal readonly L1PcInstance _pc;

            internal readonly L1PetInstance _pet;

            public L1PetMatchReadyTimer(L1PetMatch outerInstance, int petMatchNo, L1PcInstance pc, L1PetInstance pet)
            {
                this.outerInstance = outerInstance;
                _petMatchNo = petMatchNo;
                _pc = pc;
                _pet = pet;
            }

            public virtual void begin()
            {
                Container.Instance.Resolve<ITaskController>().execute(this, 3 * 1000);
            }

            public override void run()
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if ((_pc == null) || (_pet == null))
                    {
                        cancel();
                        return;
                    }

                    if (_pc.Teleport)
                    {
                        continue;
                    }
                    if (L1PetMatch.Instance.setPetMatchPc(_petMatchNo, _pc, _pet) == L1PetMatch.STATUS_PLAYING)
                    {
                        L1PetMatch.Instance.startPetMatch(_petMatchNo);
                    }
                    cancel();
                    return;
                }
            }
        }

        public class L1PetMatchTimer : TimerTask
        {
            private readonly L1PetMatch outerInstance;

            internal readonly L1PetInstance _pet1;

            internal readonly L1PetInstance _pet2;

            internal readonly int _petMatchNo;

            internal int _counter = 0;

            public L1PetMatchTimer(L1PetMatch outerInstance, L1PetInstance pet1, L1PetInstance pet2, int petMatchNo)
            {
                this.outerInstance = outerInstance;
                _pet1 = pet1;
                _pet2 = pet2;
                _petMatchNo = petMatchNo;
            }

            public virtual void begin()
            {
                Container.Instance.Resolve<ITaskController>().execute(this, 0);
            }

            public override void run()
            {
                while (true)
                {
                    Thread.Sleep(3000);
                    _counter++;
                    if ((_pet1 == null) || (_pet2 == null))
                    {
                        cancel();
                        return;
                    }

                    if (_pet1.Dead || _pet2.Dead)
                    {
                        int winner = 0;
                        if (!_pet1.Dead && _pet2.Dead)
                        {
                            winner = 1;
                        }
                        else if (_pet1.Dead && !_pet2.Dead)
                        {
                            winner = 2;
                        }
                        else
                        {
                            winner = 3;
                        }
                        L1PetMatch.Instance.endPetMatch(_petMatchNo, winner);
                        cancel();
                        return;
                    }

                    if (_counter == 100)
                    { // 5分経っても終わらない場合は引き分け
                        L1PetMatch.Instance.endPetMatch(_petMatchNo, 3);
                        cancel();
                        return;
                    }
                }
            }

        }

    }

}