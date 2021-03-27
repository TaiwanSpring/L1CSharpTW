using LineageServer.Server.Model.Map;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Model.trap;
using LineageServer.Server.Types;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Model.Instance
{
    class L1TrapInstance : GameObject
    {
        /// 
        private const long serialVersionUID = 1L;

        private readonly L1Trap _trap;

        private readonly Point _baseLoc = new Point();

        private readonly Point _rndPt = new Point();

        private readonly int _span;

        private bool _isEnable = true;

        private readonly string _nameForView;

        private IList<L1PcInstance> _knownPlayers = ListFactory.NewConcurrentList<L1PcInstance>();

        public L1TrapInstance(int id, L1Trap trap, L1Location loc, Point rndPt, int span)
        {
            Id = id;
            _trap = trap;
            Location.set(loc);
            _baseLoc.set(loc);
            _rndPt.set(rndPt);
            _span = span;
            _nameForView = "trap";

            resetLocation();
        }

        public L1TrapInstance(int id, L1Location loc)
        {
            Id = id;
            _trap = L1Trap.newNull();
            Location.set(loc);
            _span = 0;
            _nameForView = "trap base";
        }

        public virtual void resetLocation()
        {
            if ((_rndPt.X == 0) && (_rndPt.Y == 0))
            {
                return;
            }

            for (int i = 0; i < 50; i++)
            {
                int rndX = RandomHelper.Next(_rndPt.X + 1) * (RandomHelper.Next(2) == 1 ? 1 : -1); // 1/2の確率でマイナスにする
                int rndY = RandomHelper.Next(_rndPt.Y + 1) * (RandomHelper.Next(2) == 1 ? 1 : -1);

                rndX += _baseLoc.X;
                rndY += _baseLoc.Y;

                L1Map map = Location.getMap();
                if (map.isInMap(rndX, rndY) && map.isPassable(rndX, rndY))
                {
                    Location.set(rndX, rndY);
                    break;
                }
            }
            // ループ内で位置が確定しない場合、前回と同じ位置になる。
        }

        public virtual void enableTrap()
        {
            _isEnable = true;
        }

        public virtual void disableTrap()
        {
            _isEnable = false;

            foreach (L1PcInstance pc in _knownPlayers)
            {
                pc.removeKnownObject(this);
                pc.sendPackets(new S_RemoveObject(this));
            }
            _knownPlayers.Clear();
        }

        public virtual bool Enable
        {
            get
            {
                return _isEnable;
            }
        }

        public virtual int Span
        {
            get
            {
                return _span;
            }
        }

        public virtual void onTrod(L1PcInstance trodFrom)
        {
            _trap.onTrod(trodFrom, this);
        }

        public virtual void onDetection(L1PcInstance caster)
        {
            _trap.onDetection(caster, this);
        }

        public override void onPerceive(L1PcInstance perceivedFrom)
        {
            if (perceivedFrom.hasSkillEffect(L1SkillId.GMSTATUS_SHOWTRAPS))
            {
                perceivedFrom.addKnownObject(this);
                perceivedFrom.sendPackets(new S_Trap(this, _nameForView));
                _knownPlayers.Add(perceivedFrom);
            }
        }
    }

}