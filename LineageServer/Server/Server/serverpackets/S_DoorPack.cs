using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
    class S_DoorPack : ServerBasePacket
    {

        private const string S_DOOR_PACK = "[S] S_DoorPack";

        private const int STATUS_POISON = 1;

        private byte[] _byte = null;

        public S_DoorPack(L1DoorInstance door)
        {
            buildPacket(door);
        }

        private void buildPacket(L1DoorInstance door)
        {
            writeC(Opcodes.S_OPCODE_CHARPACK);
            writeH(door.X);
            writeH(door.Y);
            writeD(door.Id);
            writeH(door.GfxId);
            int doorStatus = door.Status;
            int openStatus = door.OpenStatus;
            if (door.Dead)
            {
                writeC(doorStatus);
            }
            else if (openStatus == ActionCodes.ACTION_Open)
            {
                writeC(openStatus);
            }
            else if ((door.MaxHp > 1) && (doorStatus != 0))
            {
                writeC(doorStatus);
            }
            else
            {
                writeC(openStatus);
            }
            writeC(0);
            writeC(0);
            writeC(0);
            writeD(1);
            writeH(0);
            writeS(null);
            writeS(null);
            int status = 0;
            if (door.Poison != null)
            { // 毒状態
                if (door.Poison.EffectId == 1)
                {
                    status |= STATUS_POISON;
                }
            }
            writeC(status);
            writeD(0);
            writeS(null);
            writeS(null);
            writeC(0);
            writeC(0xFF);
            writeC(0);
            writeC(0);
            writeC(0);
            writeC(0xFF);
            writeC(0xFF);
        }

        public override string Type
        {
            get
            {
                return S_DOOR_PACK;
            }
        }

    }

}