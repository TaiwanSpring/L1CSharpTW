using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.Model
{
    class L1HauntedHouse
    {
        public const int STATUS_NONE = 0;

        public const int STATUS_READY = 1;

        public const int STATUS_PLAYING = 2;

        private readonly IList<L1PcInstance> _members = ListFactory.NewList<L1PcInstance>();

        private int _hauntedHouseStatus = STATUS_NONE;

        private int _winnersCount = 0;

        private int _goalCount = 0;

        private static L1HauntedHouse _instance;

        public static L1HauntedHouse Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new L1HauntedHouse();
                }
                return _instance;
            }
        }

        private void readyHauntedHouse()
        {
            HauntedHouseStatus = STATUS_READY;
            L1HauntedHouseReadyTimer hhrTimer = new L1HauntedHouseReadyTimer(this);
            hhrTimer.begin();
        }

        private void startHauntedHouse()
        {
            HauntedHouseStatus = STATUS_PLAYING;
            int membersCount = MembersCount;
            if (membersCount <= 4)
            {
                WinnersCount = 1;
            }
            else if ((5 >= membersCount) && (membersCount <= 7))
            {
                WinnersCount = 2;
            }
            else if ((8 >= membersCount) && (membersCount <= 10))
            {
                WinnersCount = 3;
            }
            foreach (L1PcInstance pc in MembersArray)
            {
                L1SkillUse l1skilluse = new L1SkillUse();
                l1skilluse.handleCommands(pc, L1SkillId.CANCELLATION, pc.Id, pc.X, pc.Y, null, 0, L1SkillUse.TYPE_LOGIN);
                L1PolyMorph.doPoly(pc, 6284, 300, L1PolyMorph.MORPH_BY_NPC);
            }

            foreach (GameObject @object in Container.Instance.Resolve<IGameWorld>().Object)
            {
                if (@object is L1DoorInstance)
                {
                    L1DoorInstance door = (L1DoorInstance)@object;
                    if (door.MapId == 5140)
                    {
                        door.open();
                    }
                }
            }
        }

        public virtual void endHauntedHouse()
        {
            HauntedHouseStatus = STATUS_NONE;
            WinnersCount = 0;
            GoalCount = 0;
            foreach (L1PcInstance pc in MembersArray)
            {
                if (pc.MapId == 5140)
                {
                    L1SkillUse l1skilluse = new L1SkillUse();
                    l1skilluse.handleCommands(pc, L1SkillId.CANCELLATION, pc.Id, pc.X, pc.Y, null, 0, L1SkillUse.TYPE_LOGIN);
                    L1Teleport.teleport(pc, 32624, 32813, (short)4, 5, true);
                }
            }
            clearMembers();
            foreach (GameObject @object in Container.Instance.Resolve<IGameWorld>().Object)
            {
                if (@object is L1DoorInstance)
                {
                    L1DoorInstance door = (L1DoorInstance)@object;
                    if (door.MapId == 5140)
                    {
                        door.close();
                    }
                }
            }
        }

        public virtual void removeRetiredMembers()
        {
            L1PcInstance[] temp = MembersArray;
            foreach (L1PcInstance element in temp)
            {
                if (element.MapId != 5140)
                {
                    removeMember(element);
                }
            }
        }

        public virtual void sendMessage(int type, string msg)
        {
            foreach (L1PcInstance pc in MembersArray)
            {
                pc.sendPackets(new S_ServerMessage(type, msg));
            }
        }

        public virtual void addMember(L1PcInstance pc)
        {
            if (!_members.Contains(pc))
            {
                _members.Add(pc);
            }
            if ((MembersCount == 1) && (HauntedHouseStatus == STATUS_NONE))
            {
                readyHauntedHouse();
            }
        }

        public virtual void removeMember(L1PcInstance pc)
        {
            _members.Remove(pc);
        }

        public virtual void clearMembers()
        {
            _members.Clear();
        }

        public virtual bool isMember(L1PcInstance pc)
        {
            return _members.Contains(pc);
        }

        public virtual L1PcInstance[] MembersArray
        {
            get
            {
                return ((List<L1PcInstance>)_members).ToArray();
            }
        }

        public virtual int MembersCount
        {
            get
            {
                return _members.Count;
            }
        }

        public int HauntedHouseStatus
        {
            set
            {
                _hauntedHouseStatus = value;
            }
            get
            {
                return _hauntedHouseStatus;
            }
        }


        public int WinnersCount
        {
            set
            {
                _winnersCount = value;
            }
            get
            {
                return _winnersCount;
            }
        }


        public virtual int GoalCount
        {
            set
            {
                _goalCount = value;
            }
            get
            {
                return _goalCount;
            }
        }


        class L1HauntedHouseReadyTimer : TimerTask
        {
            private readonly L1HauntedHouse outerInstance;


            public L1HauntedHouseReadyTimer(L1HauntedHouse outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public override void run()
            {
                outerInstance.startHauntedHouse();
                L1HauntedHouseTimer hhTimer = new L1HauntedHouseTimer(outerInstance);
                hhTimer.begin();
            }

            public virtual void begin()
            {
                Container.Instance.Resolve<ITaskController>().execute(this, 90 * 1000); // 90秒くらい？
            }

        }

        class L1HauntedHouseTimer : TimerTask
        {
            private readonly L1HauntedHouse outerInstance;


            public L1HauntedHouseTimer(L1HauntedHouse outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public override void run()
            {
                outerInstance.endHauntedHouse();
                cancel();
            }

            public virtual void begin()
            {
                Container.Instance.Resolve<ITaskController>().execute(this, 300 * 1000); // 5分
            }

        }

    }

}