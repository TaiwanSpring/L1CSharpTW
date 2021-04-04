using LineageServer.Interfaces;
using LineageServer.Utils;

namespace LineageServer.Server.Templates
{
    class L1PetType
    {
        private readonly int _baseNpcId;

        private readonly L1Npc _baseNpcTemplate;

        private readonly string _name;

        private readonly int _itemIdForTaming;

        private readonly IntRange _hpUpRange;

        private readonly IntRange _mpUpRange;

        private readonly int _npcIdForEvolving;

        private readonly int[] _msgIds;

        private readonly int _defyMsgId;

        private readonly int _evolvItemId;

        private readonly bool _canUseEquipment;

        public L1PetType(int baseNpcId, string name, int itemIdForTaming, IntRange hpUpRange, IntRange mpUpRange, int evolvItemId, int npcIdForEvolving, int[] msgIds, int defyMsgId, bool canUseEquipment)
        {
            _baseNpcId = baseNpcId;
            _baseNpcTemplate = Container.Instance.Resolve<INpcController>().getTemplate(baseNpcId);
            _name = name;
            _itemIdForTaming = itemIdForTaming;
            _hpUpRange = hpUpRange;
            _mpUpRange = mpUpRange;
            _evolvItemId = evolvItemId;
            _npcIdForEvolving = npcIdForEvolving;
            _msgIds = msgIds;
            _defyMsgId = defyMsgId;
            _canUseEquipment = canUseEquipment;

        }

        public virtual int BaseNpcId
        {
            get
            {
                return _baseNpcId;
            }
        }

        public virtual L1Npc BaseNpcTemplate
        {
            get
            {
                return _baseNpcTemplate;
            }
        }

        public virtual string Name
        {
            get
            {
                return _name;
            }
        }

        public virtual int ItemIdForTaming
        {
            get
            {
                return _itemIdForTaming;
            }
        }

        public virtual bool canTame()
        {
            return _itemIdForTaming != 0;
        }

        public virtual IntRange HpUpRange
        {
            get
            {
                return _hpUpRange;
            }
        }

        public virtual IntRange MpUpRange
        {
            get
            {
                return _mpUpRange;
            }
        }

        public virtual int NpcIdForEvolving
        {
            get
            {
                return _npcIdForEvolving;
            }
        }

        public virtual bool canEvolve()
        {
            return _npcIdForEvolving != 0;
        }

        public virtual int getMessageId(int num)
        {
            if (num == 0)
            {
                return 0;
            }
            return _msgIds[num - 1];
        }

        public static int getMessageNumber(int level)
        {
            if (50 <= level)
            {
                return 5;
            }
            if (48 <= level)
            {
                return 4;
            }
            if (36 <= level)
            {
                return 3;
            }
            if (24 <= level)
            {
                return 2;
            }
            if (12 <= level)
            {
                return 1;
            }
            return 0;
        }

        public virtual int DefyMessageId
        {
            get
            {
                return _defyMsgId;
            }
        }

        // 進化道具
        public virtual int EvolvItemId
        {
            get
            {
                return _evolvItemId;
            }
        }

        // 可使用寵物裝備
        public virtual bool canUseEquipment()
        {
            return _canUseEquipment;
        }

    }

}