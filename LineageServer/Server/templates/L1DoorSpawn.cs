using LineageServer.Server.Model;
using System.Collections.Generic;

namespace LineageServer.Server.Templates
{
    class L1DoorSpawn
    {
        private readonly int _id;
        private readonly L1DoorGfx _gfx;
        private readonly int _x;
        private readonly int _y;
        private readonly int _mapId;
        private readonly L1Location _loc;
        private readonly int _hp;
        private readonly int _keeper;
        private readonly bool _isOpening;

        public L1DoorSpawn(int id, L1DoorGfx gfx, int x, int y, int mapId, int hp, int keeper, bool isOpening) : base()
        {
            _id = id;
            _gfx = gfx;
            _x = x;
            _y = y;
            _mapId = mapId;
            _loc = new L1Location(_x, _y, _mapId);
            _hp = hp;
            _keeper = keeper;
            _isOpening = isOpening;
        }

        public virtual int Id
        {
            get
            {
                return _id;
            }
        }

        public virtual L1DoorGfx Gfx
        {
            get
            {
                return _gfx;
            }
        }

        public virtual int X
        {
            get
            {
                return _x;
            }
        }

        public virtual int Y
        {
            get
            {
                return _y;
            }
        }

        public virtual int MapId
        {
            get
            {
                return _mapId;
            }
        }

        public virtual L1Location Location
        {
            get
            {
                return _loc;
            }
        }

        public virtual int Hp
        {
            get
            {
                return _hp;
            }
        }

        public virtual int Keeper
        {
            get
            {
                return _keeper;
            }
        }

        public virtual bool Opening
        {
            get
            {
                return _isOpening;
            }
        }
    }
}